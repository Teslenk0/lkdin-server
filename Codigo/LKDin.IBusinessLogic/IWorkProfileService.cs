﻿using LKDin.DTOs;

namespace LKDin.IBusinessLogic
{
    public interface IWorkProfileService
    {
        public Task CreateWorkProfile(WorkProfileDTO workProfileDTO);

        public Task AssignImageToWorkProfile(WorkProfileDTO partialWorkProfileDTO);

        public Task<List<WorkProfileDTO>> GetWorkProfilesBySkills(List<SkillDTO> skillsToSearchFor);

        public Task<List<WorkProfileDTO>> GetWorkProfilesByDescription(string description);

        public Task<WorkProfileDTO> GetWorkProfileByUserId(string userId);

        public Task<string> DownloadWorkProfileImage(string userId);
    }
}
