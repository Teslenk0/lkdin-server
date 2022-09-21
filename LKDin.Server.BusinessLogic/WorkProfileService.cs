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

        public void CreateWorkProfile(WorkProfileDTO workProfileDTO)
        {
            this._userService.ValidateUserCredentials(workProfileDTO.UserId, workProfileDTO.UserPassword);

            var exists = this._workProfileRepository.ExistsByUserId(workProfileDTO.UserId);

            if (exists)
            {
                throw new WorkProfileAlreadyExistsException(workProfileDTO.UserId);
            }

            var workProfile = WorkProfileDTO.DTOToEntity(workProfileDTO);

            workProfile.Id = workProfileDTO.UserId;

            this._workProfileRepository.Create(workProfile);

            List<Skill> skills = new();

            foreach (SkillDTO skillDTO in workProfileDTO.Skills)
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

        public List<WorkProfileDTO> GetWorkProfilesBySkills(List<SkillDTO> skillsToSearchFor)
        {
            var skillsThatMatch = this._skillRepository.GetByName(skillsToSearchFor.Select(skill => skill.Name).ToList());

            var workProfilesIds = skillsThatMatch
                .Select(s => s.WorkProfileId)
                .Distinct()
                .ToList();

            var workProfiles = this._workProfileRepository.GetByIds(workProfilesIds);

            var skills = this._skillRepository.GetByWorkProfileIds(workProfilesIds);

            var result = new List<WorkProfileDTO>();

            workProfiles.ForEach(wp =>
            {
                var wpDTO = WorkProfileDTO.EntityToDTO(wp);

                var userDTO = this._userService.GetUser(wp.UserId);

                wpDTO.User = userDTO;

                var wpSkills = skills.Where(skill => skill.WorkProfileId.Equals(wp.Id)).ToList();

                wpSkills.ForEach(skill =>
                {
                    var skillDTO = SkillDTO.EntityToDTO(skill);

                    wpDTO.Skills.Add(skillDTO);
                });

                result.Add(wpDTO);
            });

            return result;
        }

        public List<WorkProfileDTO> GetWorkProfilesByDescription(string description)
        {
            var workProfilesThatMatch = this._workProfileRepository.GetByDescription(description);

            var workProfilesIds = workProfilesThatMatch
                .Select(s => s.Id)
                .Distinct()
                .ToList();

            var skills = this._skillRepository.GetByWorkProfileIds(workProfilesIds);

            var result = new List<WorkProfileDTO>();

            workProfilesThatMatch.ForEach(wp =>
            {
                var wpDTO = WorkProfileDTO.EntityToDTO(wp);

                var userDTO = this._userService.GetUser(wp.UserId);

                wpDTO.User = userDTO;

                var wpSkills = skills.Where(skill => skill.WorkProfileId.Equals(wp.Id)).ToList();

                wpSkills.ForEach(skill =>
                {
                    var skillDTO = SkillDTO.EntityToDTO(skill);

                    wpDTO.Skills.Add(skillDTO);
                });

                result.Add(wpDTO);
            });

            return result;
        }

        public WorkProfileDTO GetWorkProfileByUserId(string userId)
        {
            var userDTO = this._userService.GetUser(userId);

            if (userDTO == null)
            {
                throw new UserDoesNotExistException(userId);
            }

            var wp = this._workProfileRepository.GetByUserId(userId);

            if (wp == null)
            {
                throw new WorkProfileDoesNotExistException(userId);
            }

            var resultWP = WorkProfileDTO.EntityToDTO(wp);

            var skills = this._skillRepository.GetByWorkProfileIds(new List<string>() { resultWP.Id });

            skills.ForEach(skill =>
            {
                var skillDTO = SkillDTO.EntityToDTO(skill);

                resultWP.Skills.Add(skillDTO);
            });

            resultWP.User = userDTO;

            return resultWP;
        }
    }
}