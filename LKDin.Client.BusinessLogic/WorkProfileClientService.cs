using LKDin.DTOs;
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

        public void AssignImageToWorkProfile(WorkProfileDTO partialWorkProfileDTO)
        {
            var serializedWorkProfile = SerializationManager.Serialize<WorkProfileDTO>(partialWorkProfileDTO);

            _networkDataHelper.SendMessage(serializedWorkProfile, AvailableOperation.ASSIGN_IMAGE_TO_WORK_PROFILE);

            _networkDataHelper.ReceiveMessage();
        }

        public void CreateWorkProfile(WorkProfileDTO workProfileDTO)
        {
            var serializedWorkProfile = SerializationManager.Serialize<WorkProfileDTO>(workProfileDTO);

            _networkDataHelper.SendMessage(serializedWorkProfile, AvailableOperation.CREATE_WORK_PROFILE);

            _networkDataHelper.ReceiveMessage();
        }

        public WorkProfileDTO GetWorkProfileByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public List<WorkProfileDTO> GetWorkProfilesByDescription(string description)
        {
            throw new NotImplementedException();
        }

        public List<WorkProfileDTO> GetWorkProfilesBySkills(List<SkillDTO> skillsToSearchFor)
        {
            throw new NotImplementedException();
        }
    }
}