using LKDin.DTOs;
using LKDin.Exceptions;
using LKDin.IBusinessLogic;
using LKDin.Server.DataAccess.Repositories;
using LKDin.Server.Domain;
using LKDin.Server.IDataAccess.Repositories;

namespace LKDin.Server.BusinessLogic
{
    public class WorkProfileService : IWorkProfileService
    {
        private readonly IWorkProfileRepository _workProfileRepository;

        private readonly ISkillRepository _skillRepository;

        private readonly IUserService _userService;

        public WorkProfileService(IUserService userService)
        {
            this._workProfileRepository = new WorkProfileRepository();

            this._skillRepository = new SkillRepository();

            this._userService = userService;
        }

        public void CreateWorkProfile (WorkProfileDTO workProfileDTO)
        {
            this._userService.ValidateUserCredentials(workProfileDTO.UserId, workProfileDTO.UserPassword);

            var exists = this._workProfileRepository.ExistsByUserId(workProfileDTO.UserId);

            if (exists)
            {
                throw new WorkProfileAlreadyExistsException(workProfileDTO.UserId);
            }

            var workProfile = WorkProfileDTO.DTOToEntity(workProfileDTO);

            this._workProfileRepository.Create(workProfile);

            List<Skill> skills = new();

            foreach(SkillDTO skillDTO in workProfileDTO.Skills)
            {
                skillDTO.WorkProfileId = workProfile.Id.ToString();

                skills.Add(SkillDTO.DTOToEntity(skillDTO));
            }

            this._skillRepository.CreateMany(skills);
        }


        public void AssignImageToWorkProfile(WorkProfileDTO partialWorkProfileDTO)
        {
            this._userService.ValidateUserCredentials(partialWorkProfileDTO.UserId, partialWorkProfileDTO.UserPassword);

            var workProfile = this._workProfileRepository.GetByUserId(partialWorkProfileDTO.UserId);

            if (workProfile == null)
            {
                throw new WorkProfileDoesNotExistException(partialWorkProfileDTO.UserId);
            }

            workProfile.ImagePath = partialWorkProfileDTO.ImagePath;

            this._workProfileRepository.AssignImageToWorkProfile(workProfile);
        }
    }
}