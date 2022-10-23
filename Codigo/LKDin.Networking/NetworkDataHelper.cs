using LKDin.DTOs;
using LKDin.Exceptions;
using LKDin.Helpers.Assets;
using LKDin.Helpers.Configuration;
using LKDin.Helpers.Serialization;
using System.Net.Sockets;

namespace LKDin.Networking
{
    public class NetworkDataHelper
    {
        private readonly TcpClient _tcpClient;

        public NetworkDataHelper(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
        }

        public async Task<Dictionary<string, string>> ReceiveMessage()
        {
            var resultantData = new Dictionary<string, string>();

            byte[] rawHeaders = await this.PerformReception(Protocol.HEADER_SIZE);

            var parsedHeaders = this.DeserializeHeaders(rawHeaders);

            byte[] data = await this.PerformReception(int.Parse(parsedHeaders[Protocol.LENGTH_HEADER_NAME]));

            var message = ConversionHandler.ConvertBytesToString(data);

            if ((AvailableOperation)int.Parse(parsedHeaders[Protocol.CMD_HEADER_NAME] ?? "01") == AvailableOperation.ERR)
            {
                this.ThrowException(message);
            }

            resultantData.Add(Protocol.MSG_NAME, message);

            resultantData.Add(Protocol.CMD_HEADER_NAME, parsedHeaders[Protocol.CMD_HEADER_NAME]);

            return resultantData;
        }

        public async Task SendMessage(string messageBody, AvailableOperation availableOperation)
        {
            byte[] messageBytes = ConversionHandler.ConvertStringToBytes(messageBody);

            var messageBodySize = messageBytes.Length.ToString();

            while (messageBodySize.Length != Protocol.SIZE_LENGTH_HEADER)
            {
                messageBodySize = $"0{messageBodySize}";
            }

            var operation = ((int)availableOperation).ToString();

            while (operation.Length != Protocol.SIZE_CMD_HEADER)
            {
                operation = $"0{operation}";
            }

            var headers = $"CMD={operation}|LENGTH={messageBodySize}";

            byte[] rawHeaders = ConversionHandler.ConvertStringToBytes(headers);

            await this.PerformTransmission(rawHeaders);

            await this.PerformTransmission(messageBytes);
        }

        public async Task SendException(Exception exception)
        {
            var message = new ExceptionDTO()
            {
                Message = exception.Message,
                ExceptionType = exception.GetType().FullName,
                AssemblyName = exception.GetType().Assembly.FullName.Replace("=", "#")
            };

            var stringifiedMessage = SerializationManager.Serialize<ExceptionDTO>(message);

            byte[] messageBytes = ConversionHandler.ConvertStringToBytes(stringifiedMessage);

            var messageBodySize = messageBytes.Length.ToString();

            while (messageBodySize.Length != Protocol.SIZE_LENGTH_HEADER)
            {
                messageBodySize = $"0{messageBodySize}";
            }

            var operation = ((int)AvailableOperation.ERR).ToString();

            while (operation.Length != Protocol.SIZE_CMD_HEADER)
            {
                operation = $"0{operation}";
            }

            var headers = $"CMD={operation}|LENGTH={messageBodySize}";

            byte[] rawHeaders = ConversionHandler.ConvertStringToBytes(headers);

            await this.PerformTransmission(rawHeaders);

            await this.PerformTransmission(messageBytes);
        }

        public async Task SendFile(string path)
        {
            var fileName = AssetManager.GetFileName(path);

            // Send file name length
            await this.PerformTransmission(ConversionHandler.ConvertIntToBytes(fileName.Length));

            // Send the file name
            await this.PerformTransmission(ConversionHandler.ConvertStringToBytes(fileName));

            // Get the file size
            long fileSize = AssetManager.GetFileSize(path);

            // Send the file size
            await this.PerformTransmission(ConversionHandler.ConvertLongToBytes(fileSize));

            // Send the file
            await SendFileWithStream(fileSize, path);
        }

        public async Task<string> ReceiveFile()
        {
            // Receive file name size
            int fileNameSize = ConversionHandler.ConvertBytesToInt(await this.PerformReception(FileTransmissionProtocol.FIXED_DATA_SIZE));

            // Receive file name
            string fileName = ConversionHandler.ConvertBytesToString(await this.PerformReception(fileNameSize));

            // Receive file size
            long fileSize = ConversionHandler.ConvertBytesToLong(await this.PerformReception(FileTransmissionProtocol.FIXED_FILE_SIZE));

            // Receive the file and return the temporal name
            var tmpFolder = ConfigManager.GetTmpAssetsFolderPath();

            var tmpFilePath = await ReceiveFileWithStreams(fileSize, fileName);

            return Path.Join(tmpFolder, tmpFilePath);
        }

        private async Task SendFileWithStream(long fileSize, string path)
        {
            long fileParts = FileTransmissionProtocol.CalculateFileParts(fileSize);
            long offset = 0;
            long currentPart = 1;

            while (fileSize > offset)
            {
                byte[] data;
                if (currentPart == fileParts)
                {
                    var lastPartSize = (int)(fileSize - offset);
                    data = AssetManager.ReadAsset(path, offset, lastPartSize);
                    offset += lastPartSize;
                }
                else
                {
                    data = AssetManager.ReadAsset(path, offset, FileTransmissionProtocol.MAX_PACKET_SIZE);
                    offset += FileTransmissionProtocol.MAX_PACKET_SIZE;
                }

                await this.PerformTransmission(data);

                currentPart++;
            }
        }

        private async Task<string> ReceiveFileWithStreams(long fileSize, string fileName)
        {
            long fileParts = FileTransmissionProtocol.CalculateFileParts(fileSize);
            long offset = 0;
            long currentPart = 1;

            var finalFileName = fileName;

            while (fileSize > offset)
            {
                byte[] data;

                if (currentPart == fileParts)
                {
                    var lastPartSize = (int)(fileSize - offset);
                    data = await this.PerformReception(lastPartSize);
                    offset += lastPartSize;
                }
                else
                {
                    data = await this.PerformReception(FileTransmissionProtocol.MAX_PACKET_SIZE);
                    offset += FileTransmissionProtocol.MAX_PACKET_SIZE;
                }

                if (finalFileName.Equals(fileName))
                {
                    finalFileName = AssetManager.WriteAssetToTmp(finalFileName, data);
                }
                else
                {
                    AssetManager.WriteAssetToTmp(finalFileName, data);
                }

                currentPart++;
            }

            return finalFileName;
        }

        private void ThrowException(string exceptionMessage)
        {
            var exceptionDTO = SerializationManager.Deserialize<ExceptionDTO>(exceptionMessage);

            var assemblyName = exceptionDTO.AssemblyName.Replace("#", "=");

            Exception e = null;

            var availableTypes = new Dictionary<Type, Action> {
                { typeof(AssetDoesNotExistException), () => { e = new AssetDoesNotExistException(exceptionDTO.Message, true); } },
                { typeof(CommandNotSupportedException), () => { e = new CommandNotSupportedException(exceptionDTO.Message, true); } },
                { typeof(EntityAlreadyExistsException), () => { e = new EntityAlreadyExistsException(exceptionDTO.Message, true); } },
                { typeof(UnauthorizedException), () => { e = new UnauthorizedException(); } },
                { typeof(UserAlreadyExistsException), () => { e = new UserAlreadyExistsException(exceptionDTO.Message, true); } },
                { typeof(UserDoesNotExistException), () => { e = new UserDoesNotExistException(exceptionDTO.Message, true); } },
                { typeof(WorkProfileAlreadyExistsException), () => { e = new WorkProfileAlreadyExistsException(exceptionDTO.Message, true); } },
                { typeof(WorkProfileDoesNotExistException), () => { e = new WorkProfileDoesNotExistException(exceptionDTO.Message, true); } },
            };

            var type = Type.GetType($"{exceptionDTO.ExceptionType},{assemblyName}");

            if (availableTypes.ContainsKey(type))
            {
                availableTypes[type]();
            }
            else
            {
                e = new Exception(exceptionDTO.Message);
            }

            if (e != null)
            {
                throw e;
            }

        }

        private async Task PerformTransmission(byte[] data)
        {
            int size = data.Length;

            var networkStream = _tcpClient.GetStream();

            await networkStream.WriteAsync(data, 0, size).ConfigureAwait(false);
        }

        private async Task<byte[]> PerformReception(int length)
        {
            byte[] response = new byte[length];

            int offset = 0;

            var networkStream = _tcpClient.GetStream();

            while (offset < length)
            {
                int received = await networkStream.ReadAsync(response, offset, length - offset).ConfigureAwait(false);
                
                if (received == 0)
                {
                    throw new SocketException();
                }
                
                offset += received;
            }

            return response;
        }

        private Dictionary<string, string> DeserializeHeaders(byte[] rawHeaders)
        {
            string headers = ConversionHandler.ConvertBytesToString(rawHeaders);

            var results = new Dictionary<string, string>();

            var fields = headers.Split('|');

            foreach (string field in fields)
            {
                var data = field.Split('=');

                results[data[0]] = data[1];
            }

            return results;
        }
    }
}

