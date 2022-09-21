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

