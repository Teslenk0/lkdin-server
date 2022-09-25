using LKDin.DTOs;

namespace LKDin.IBusinessLogic
{
    public interface IWorkProfileService
    {
        public void CreateWorkProfile(WorkProfileDTO workProfileDTO);

        public void AssignImageToWorkProfile(WorkProfileDTO partialWorkProfileDTO);

        public List<WorkProfileDTO> GetWorkProfilesBySkills(List<SkillDTO> skillsToSearchFor);

        public List<WorkProfileDTO> GetWorkProfilesByDescription(string description);

        public WorkProfileDTO GetWorkProfileByUserId(string userId);

        public string DownloadWorkProfileImage(string userId);
    }
}
