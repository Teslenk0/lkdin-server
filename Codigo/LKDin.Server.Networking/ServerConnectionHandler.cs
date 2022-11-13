using System.Net.Sockets;
using LKDin.DTOs;
using LKDin.Exceptions;
using LKDin.Helpers.Serialization;
using LKDin.Networking;
using LKDin.Server.BusinessLogic;

namespace LKDin.Server.Networking
{
    public delegate Task TCPConnectionHandler(TcpClient tcpClient);

    public static class ServerConnectionHandler
    {
        public static async Task HandleConnection(TcpClient tcpClient)
        {
            bool clientIsConnected = true;

            NetworkDataHelper networkDataHelper = new(tcpClient);

            while (clientIsConnected)
            {
                try
                {
                    var data = await networkDataHelper.ReceiveMessage();

                    var messagePayload = data[Protocol.MSG_NAME];

                    var cmd = int.Parse(data[Protocol.CMD_HEADER_NAME] ?? "01");

                    switch ((AvailableOperation)cmd)
                    {
                        case AvailableOperation.CREATE_USER:
                            {
                                var userService = new UserLogic();

                                await userService.CreateUser(SerializationManager.Deserialize<UserDTO>(messagePayload));

                                await networkDataHelper.SendMessage("", AvailableOperation.ACK);
                            }
                            break;
                        case AvailableOperation.CREATE_WORK_PROFILE:
                            {
                                var workProfileService = new WorkProfileLogic(new UserLogic());

                                await workProfileService.CreateWorkProfile(SerializationManager.Deserialize<WorkProfileDTO>(messagePayload));

                                await networkDataHelper.SendMessage("", AvailableOperation.ACK);
                            }
                            break;
                        case AvailableOperation.ASSIGN_IMAGE_TO_WORK_PROFILE:
                            {
                                var tmpFilePath = await networkDataHelper.ReceiveFile();

                                var workProfileDTO = SerializationManager.Deserialize<WorkProfileDTO>(messagePayload);

                                workProfileDTO.ImagePath = tmpFilePath;

                                var workProfileService = new WorkProfileLogic(new UserLogic());

                                await workProfileService.AssignImageToWorkProfile(workProfileDTO);

                                await networkDataHelper.SendMessage("", AvailableOperation.ACK);
                            }
                            break;
                        case AvailableOperation.SEARCH_PROFILES_BY_DESCRIPTION:
                            {
                                var workProfileService = new WorkProfileLogic(new UserLogic());

                                var profiles = await workProfileService.GetWorkProfilesByDescription(messagePayload);

                                var serializedProfiles = SerializationManager.Serialize<List<WorkProfileDTO>>(profiles);

                                await networkDataHelper.SendMessage(serializedProfiles, AvailableOperation.ACK);
                            }
                            break;
                        case AvailableOperation.SEARCH_PROFILES_BY_SKILLS:
                            {
                                var workProfileService = new WorkProfileLogic(new UserLogic());

                                var profiles = await workProfileService.GetWorkProfilesBySkills(SerializationManager.Deserialize<List<SkillDTO>>(messagePayload));

                                var serializedProfiles = SerializationManager.Serialize<List<WorkProfileDTO>>(profiles);

                                await networkDataHelper.SendMessage(serializedProfiles, AvailableOperation.ACK);
                            }
                            break;
                        case AvailableOperation.SHOW_WORK_PROFILE_BY_ID:
                            {
                                var workProfileService = new WorkProfileLogic(new UserLogic());

                                var profile = await workProfileService.GetWorkProfileByUserId(messagePayload);

                                var serializedProfile = SerializationManager.Serialize<WorkProfileDTO>(profile);

                                await networkDataHelper.SendMessage(serializedProfile, AvailableOperation.ACK);
                            }
                            break;
                        case AvailableOperation.CHECK_CHAT_MESSAGES_BY_SENDER_ID:
                            {
                                var chatMessageService = new ChatMessageLogic(new UserLogic());

                                var chatMessages = await chatMessageService.GetBySenderId(SerializationManager.Deserialize<UserDTO>(messagePayload));

                                var serializedChatMessages = SerializationManager.Serialize<List<ChatMessageDTO>>(chatMessages);

                                await networkDataHelper.SendMessage(serializedChatMessages, AvailableOperation.ACK);
                            }
                            break;
                        case AvailableOperation.CHECK_CHAT_MESSAGES_BY_RECEIVER_ID:
                            {
                                var chatMessageService = new ChatMessageLogic(new UserLogic());

                                var chatMessages = await chatMessageService.GetByReceiverId(SerializationManager.Deserialize<UserDTO>(messagePayload));

                                var serializedChatMessages = SerializationManager.Serialize<List<ChatMessageDTO>>(chatMessages);

                                await networkDataHelper.SendMessage(serializedChatMessages, AvailableOperation.ACK);
                            }
                            break;
                        case AvailableOperation.SEND_CHAT_MESSAGE:
                            {
                                var chatMessageService = new ChatMessageLogic(new UserLogic());

                                await chatMessageService.CreateChatMessage(SerializationManager.Deserialize<ChatMessageDTO>(messagePayload));

                                await networkDataHelper.SendMessage("", AvailableOperation.ACK);
                            }
                            break;
                        case AvailableOperation.MARK_MESSAGE_AS_READ:
                            {
                                var chatMessageService = new ChatMessageLogic(new UserLogic());

                                await chatMessageService.MarkMessageAsRead(messagePayload);

                                await networkDataHelper.SendMessage("", AvailableOperation.ACK);
                            }
                            break;
                        case AvailableOperation.DOWNLOAD_PROFILE_IMAGE_BY_ID:
                            {
                                var workProfileService = new WorkProfileLogic(new UserLogic());

                                var workProfileDTO = await workProfileService.GetWorkProfileByUserId(messagePayload);

                                if (string.IsNullOrWhiteSpace(workProfileDTO.ImagePath))
                                {
                                    throw new NoImageAssignedException(messagePayload);
                                }

                                // Send ACK
                                await networkDataHelper.SendMessage("", AvailableOperation.ACK);

                                await networkDataHelper.SendFile(workProfileDTO.ImagePath);
                            }
                            break;
                        default:
                            throw new CommandNotSupportedException(cmd.ToString());
                    }
                }
                catch (SocketException)
                {
                    clientIsConnected = false;
                }
                catch (Exception e)
                {
                    await networkDataHelper.SendException(e);
                }
            }
        }
    }
}

