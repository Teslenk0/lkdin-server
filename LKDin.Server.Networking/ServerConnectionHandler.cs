using System.Net.Sockets;
using LKDin.DTOs;
using LKDin.Exceptions;
using LKDin.Helpers.Serialization;
using LKDin.Networking;
using LKDin.Server.BusinessLogic;

namespace LKDin.Server.Networking
{
    public delegate void SocketConnectionHandler(Socket socket);

    public static class ServerConnectionHandler
    {
        public static void HandleConnection(Socket clientSocket)
        {
            bool clientIsConnected = true;

            NetworkDataHelper networkDataHelper = new(clientSocket);

            while (clientIsConnected)
            {
                try
                {
                    var data = networkDataHelper.ReceiveMessage();

                    var messagePayload = data[NetworkDataHelper.MSG_NAME];

                    var cmd = int.Parse(data[NetworkDataHelper.CMD_HEADER_NAME] ?? "01");

                    switch ((AvailableOperation)cmd)
                    {
                        case AvailableOperation.CREATE_USER:
                            {
                                var userService = new UserService();

                                userService.CreateUser(SerializationManager.Deserialize<UserDTO>(messagePayload));

                                networkDataHelper.SendMessage("", AvailableOperation.ACK);
                            }
                            break;
                        case AvailableOperation.CREATE_WORK_PROFILE:
                            {
                                var workProfileService = new WorkProfileService(new UserService());

                                workProfileService.CreateWorkProfile(SerializationManager.Deserialize<WorkProfileDTO>(messagePayload));

                                networkDataHelper.SendMessage("", AvailableOperation.ACK);
                            }
                            break;
                        case AvailableOperation.ASSIGN_IMAGE_TO_WORK_PROFILE:
                            {
                                var workProfileService = new WorkProfileService(new UserService());

                                workProfileService.AssignImageToWorkProfile(SerializationManager.Deserialize<WorkProfileDTO>(messagePayload));

                                networkDataHelper.SendMessage("", AvailableOperation.ACK);
                            }
                            break;
                        case AvailableOperation.SEARCH_PROFILES_BY_DESCRIPTION:
                            {
                                var workProfileService = new WorkProfileService(new UserService());

                                var profiles = workProfileService.GetWorkProfilesByDescription(messagePayload);

                                var serializedProfiles = SerializationManager.Serialize<List<WorkProfileDTO>>(profiles);

                                networkDataHelper.SendMessage(serializedProfiles, AvailableOperation.ACK);
                            }
                            break;
                        case AvailableOperation.SEARCH_PROFILES_BY_SKILLS:
                            {
                                var workProfileService = new WorkProfileService(new UserService());

                                var profiles = workProfileService.GetWorkProfilesBySkills(SerializationManager.Deserialize<List<SkillDTO>>(messagePayload));

                                var serializedProfiles = SerializationManager.Serialize<List<WorkProfileDTO>>(profiles);

                                networkDataHelper.SendMessage(serializedProfiles, AvailableOperation.ACK);
                            }
                            break;
                        case AvailableOperation.SHOW_WORK_PROFILE_BY_ID:
                            {
                                var workProfileService = new WorkProfileService(new UserService());

                                var profile = workProfileService.GetWorkProfileByUserId(messagePayload);

                                var serializedProfile = SerializationManager.Serialize<WorkProfileDTO>(profile);

                                networkDataHelper.SendMessage(serializedProfile, AvailableOperation.ACK);
                            }
                            break;
                        case AvailableOperation.CHECK_CHAT_MESSAGES_BY_SENDER_ID:
                            {
                                var chatMessageService = new ChatMessageService(new UserService());

                                var chatMessages = chatMessageService.GetBySenderId(SerializationManager.Deserialize<UserDTO>(messagePayload));

                                var serializedChatMessages = SerializationManager.Serialize<List<ChatMessageDTO>>(chatMessages);

                                networkDataHelper.SendMessage(serializedChatMessages, AvailableOperation.ACK);
                            }
                            break;
                        case AvailableOperation.CHECK_CHAT_MESSAGES_BY_RECEIVER_ID:
                            {
                                var chatMessageService = new ChatMessageService(new UserService());

                                var chatMessages = chatMessageService.GetByReceiverId(SerializationManager.Deserialize<UserDTO>(messagePayload));

                                var serializedChatMessages = SerializationManager.Serialize<List<ChatMessageDTO>>(chatMessages);

                                networkDataHelper.SendMessage(serializedChatMessages, AvailableOperation.ACK);
                            }
                            break;
                        case AvailableOperation.SEND_CHAT_MESSAGE:
                            {
                                var chatMessageService = new ChatMessageService(new UserService());

                                chatMessageService.CreateChatMessage(SerializationManager.Deserialize<ChatMessageDTO>(messagePayload));

                                networkDataHelper.SendMessage("", AvailableOperation.ACK);
                            }
                            break;
                        case AvailableOperation.MARK_MESSAGE_AS_READ:
                            {
                                var chatMessageService = new ChatMessageService(new UserService());

                                chatMessageService.MarkMessageAsRead(messagePayload);

                                networkDataHelper.SendMessage("", AvailableOperation.ACK);
                            }
                            break;
                        default:
                            throw new CommandNotSupportedException(cmd.ToString());
                    }
                }
                catch (SocketException)
                {
                    Console.WriteLine("Client disconnected");
                    clientIsConnected = false;
                }
                catch (Exception e)
                {
                    networkDataHelper.SendException(e);
                }
            }
        }
    }
}

