namespace LKDin.Networking
{
    public static class FileTransmissionProtocol
    {
        public const int FIXED_FILE_SIZE = 8;

        public const int FIXED_DATA_SIZE = 4;

        public const int MAX_PACKET_SIZE = 32768;

        public static long CalculateFileParts(long fileSize)
        {
            var fileParts = fileSize / MAX_PACKET_SIZE;

            return fileParts * MAX_PACKET_SIZE == fileSize ? fileParts : fileParts + 1;
        }
    }
}
