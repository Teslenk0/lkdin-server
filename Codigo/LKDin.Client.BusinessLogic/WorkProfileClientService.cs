using LKDin.DTOs;
using LKDin.Helpers.Assets;
using LKDin.Helpers.Serialization;
using LKDin.IBusinessLogic;
using LKDin.Networking;

namespace LKDin.Client.BusinessLogic
{
    public class WorkProfileClientService : IWorkProfileService
    {
        private readonly NetworkDataHelper _networkDataHelper;

        public WorkProfileClientService(NetworkDataHelper networkDataHelper)
        {
            _networkDataHelper = networkDataHelper;
        }

        public async Task AssignImageToWorkProfile(WorkProfileDTO partialWorkProfileDTO)
        {
            var serializedWorkProfile = SerializationManager.Serialize<WorkProfileDTO>(partialWorkProfileDTO);

            await _networkDataHelper.SendMessage(serializedWorkProfile, AvailableOperation.ASSIGN_IMAGE_TO_WORK_PROFILE);

            await _networkDataHelper.SendFile(partialWorkProfileDTO.ImagePath);

            await _networkDataHelper.ReceiveMessage();
        }

        public async Task CreateWorkProfile(WorkProfileDTO workProfileDTO)
        {
            var serializedWorkProfile = SerializationManager.Serialize<WorkProfileDTO>(workProfileDTO);

            await _networkDataHelper.SendMessage(serializedWorkProfile, AvailableOperation.CREATE_WORK_PROFILE);

            await _networkDataHelper.ReceiveMessage();
        }

        public async Task<string> DownloadWorkProfileImage(string userId)
        {
            await _networkDataHelper.SendMessage(userId, AvailableOperation.DOWNLOAD_PROFILE_IMAGE_BY_ID);

            // Receive ACK or ERR
            await _networkDataHelper.ReceiveMessage();

            // Receive File
            var tmpPath = await _networkDataHelper.ReceiveFile();

            return AssetManager.CopyFileToDownloadsFolder<WorkProfileDTO>(tmpPath, false);
        }

        public async Task<WorkProfileDTO> GetWorkProfileByUserId(string userId)
        {
            await _networkDataHelper.SendMessage(userId, AvailableOperation.SHOW_WORK_PROFILE_BY_ID);

            var data = await _networkDataHelper.ReceiveMessage();

            var messagePayload = data[Protocol.MSG_NAME];

            return SerializationManager.Deserialize<WorkProfileDTO>(messagePayload);
        }

        public async Task<List<WorkProfileDTO>> GetWorkProfilesByDescription(string description)
        {
            await _networkDataHelper.SendMessage(description, AvailableOperation.SEARCH_PROFILES_BY_DESCRIPTION);

            var data = await _networkDataHelper.ReceiveMessage();

            var messagePayload = data[Protocol.MSG_NAME];

            return SerializationManager.DeserializeList<List<WorkProfileDTO>>(messagePayload);
        }

        public async Task<List<WorkProfileDTO>> GetWorkProfilesBySkills(List<SkillDTO> skillsToSearchFor)
        {
            var serializedSkills = SerializationManager.Serialize<List<SkillDTO>>(skillsToSearchFor);

            await _networkDataHelper.SendMessage(serializedSkills, AvailableOperation.SEARCH_PROFILES_BY_SKILLS);

            var data = await _networkDataHelper.ReceiveMessage();

            var messagePayload = data[Protocol.MSG_NAME];

            return SerializationManager.DeserializeList<List<WorkProfileDTO>>(messagePayload);
        }
    }
}