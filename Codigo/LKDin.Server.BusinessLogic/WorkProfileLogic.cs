using LKDin.DTOs;
using LKDin.Exceptions;
using LKDin.Helpers.Assets;
using LKDin.IBusinessLogic;
using LKDin.Logging.Client;
using LKDin.Server.DataAccess.Repositories;
using LKDin.Server.Domain;
using LKDin.Server.IDataAccess.Repositories;

namespace LKDin.Server.BusinessLogic
{
    public class WorkProfileLogic : IWorkProfileLogic
    {
        private readonly IWorkProfileRepository _workProfileRepository;

        private readonly ISkillRepository _skillRepository;

        private readonly IUserLogic _userLogic;

        private readonly Logger _logger;

        public WorkProfileLogic(IUserLogic userLogic)
        {
            this._workProfileRepository = new WorkProfileRepository();

            this._skillRepository = new SkillRepository();

            this._userLogic = userLogic;

            this._logger = new Logger("server:business-logic:work-profile-service");
        }

        public async Task CreateWorkProfile(WorkProfileDTO workProfileDTO)
        {
            this._logger.Info($"Creando nuevo perfil de trabajo ID:{workProfileDTO.UserId}");

            await this._userLogic.ValidateUserCredentials(workProfileDTO.UserId, workProfileDTO.UserPassword);

            var exists = this._workProfileRepository.ExistsByUserId(workProfileDTO.UserId);

            if (exists)
            {
                this._logger.Error($"Usuario ID:{workProfileDTO.UserId} ya tiene un perfil de trabajo");

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

            this._logger.Info($"Se creo un perfil de trabajo ID:{workProfileDTO.UserId}");
        }

        public async Task AssignImageToWorkProfile(WorkProfileDTO partialWorkProfileDTO)
        {
            this._logger.Info($"Asignando imagen a perfil de trabajo ID:{partialWorkProfileDTO.UserId}");

            await this._userLogic.ValidateUserCredentials(partialWorkProfileDTO.UserId, partialWorkProfileDTO.UserPassword);

            var workProfile = this._workProfileRepository.GetByUserId(partialWorkProfileDTO.UserId);

            if (workProfile == null)
            {
                this._logger.Error($"Perfil de trabajo ID:{partialWorkProfileDTO.UserId} no existe");

                throw new WorkProfileDoesNotExistException(partialWorkProfileDTO.UserId);
            }

            var assetPath = AssetManager.CopyAssetToAssetsFolder<WorkProfile>(partialWorkProfileDTO.ImagePath, workProfile.Id);

            workProfile.ImagePath = assetPath;

            this._workProfileRepository.AssignImageToWorkProfile(workProfile);

            this._logger.Info($"Se asignó imagen al perfil de trabajo ID:{workProfile.UserId}");
        }

        public async Task<List<WorkProfileDTO>> GetWorkProfilesBySkills(List<SkillDTO> skillsToSearchFor)
        {
            this._logger.Info($"Obteniendo perfiles de trabajo por habilidades");

            var skillsThatMatch = this._skillRepository.GetByName(skillsToSearchFor.Select(skill => skill.Name).ToList());

            var workProfilesIds = skillsThatMatch
                .Select(s => s.WorkProfileId)
                .Distinct()
                .ToList();

            var workProfiles = this._workProfileRepository.GetByIds(workProfilesIds);

            var skills = this._skillRepository.GetByWorkProfileIds(workProfilesIds);

            var result = new List<WorkProfileDTO>();

            workProfiles.ForEach(async wp =>
            {
                var wpDTO = WorkProfileDTO.EntityToDTO(wp);

                var userDTO = await this._userLogic.GetUser(wp.UserId);

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

        public async Task<List<WorkProfileDTO>> GetWorkProfilesByDescription(string description)
        {
            this._logger.Info($"Obteniendo perfiles de trabajo por descripción");

            var workProfilesThatMatch = this._workProfileRepository.GetByDescription(description);

            var workProfilesIds = workProfilesThatMatch
                .Select(s => s.Id)
                .Distinct()
                .ToList();

            var skills = this._skillRepository.GetByWorkProfileIds(workProfilesIds);

            var result = new List<WorkProfileDTO>();

            workProfilesThatMatch.ForEach(async wp =>
            {
                var wpDTO = WorkProfileDTO.EntityToDTO(wp);

                var userDTO = await this._userLogic.GetUser(wp.UserId);

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

        public async Task<WorkProfileDTO> GetWorkProfileByUserId(string userId)
        {
            this._logger.Info($"Obteniendo perfil de trabajo por ID:{userId}");

            var userDTO = await this._userLogic.GetUser(userId);

            if (userDTO == null)
            {
                this._logger.Error($"Usuario ID:{userId} no existe");

                throw new UserDoesNotExistException(userId);
            }

            var wp = this._workProfileRepository.GetByUserId(userId);

            if (wp == null)
            {
                this._logger.Error($"Usuario ID:{userId} no tiene un perfil de trabajo asignado");

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

        public async Task<string> DownloadWorkProfileImage(string userId)
        {
            this._logger.Info($"Descargando imagen de perfil de trabajo ID:{userId}");

            var workProfile = this._workProfileRepository.GetByUserId(userId);

            if (workProfile == null)
            {
                this._logger.Error($"El perfil de trabajo ID:{userId} no existe");

                throw new WorkProfileDoesNotExistException(userId);
            }

            if (string.IsNullOrWhiteSpace(workProfile.ImagePath))
            {
                this._logger.Error($"El perfil de trabajo ID:{userId} no tiene imagen asignada");

                throw new NoImageAssignedException(userId);
            }

            var assetPath = AssetManager.CopyFileToDownloadsFolder<WorkProfileDTO>(workProfile.ImagePath, true);

            return assetPath;
        }

        public async Task DeleteWorkProfileImage(WorkProfileDTO workProfileDTO)
        {
            this._logger.Info($"Eliminando imagen del perfil de trabajo ID:{workProfileDTO.Id}");

            await this._userLogic.ValidateUserCredentials(workProfileDTO.UserId, workProfileDTO.UserPassword);

            var workProfile = this._workProfileRepository.GetByUserId(workProfileDTO.Id);

            if (workProfile == null)
            {
                this._logger.Error($"Perfil de trabajo ID:{workProfileDTO.Id} no existe");

                throw new WorkProfileDoesNotExistException(workProfileDTO.Id);
            }

            if (string.IsNullOrWhiteSpace(workProfile.ImagePath))
            {
                this._logger.Error($"Perfil de trabajo ID:{workProfileDTO.Id} no tiene una imagen asignada");

                throw new NoImageAssignedException(workProfileDTO.Id);
            }

            if (AssetManager.DoesFileExist(workProfile.ImagePath))
            {
                AssetManager.DeleteAssetFromAssetsFolder<WorkProfile>(workProfile.ImagePath);
            }
            else
            {
                this._logger.Warn($"El archivo {workProfile.ImagePath} no existe - Omitiendo eliminacion del archivo");
            }

            this._workProfileRepository.UnAssignImageFromWorkProfile(workProfile);

            this._logger.Info($"Se eliminó imagen del perfil de trabajo ID:{workProfile.Id} exitosamente");
        }

        public async Task DeleteWorkProfile(WorkProfileDTO workProfileDTO)
        {
            this._logger.Info($"Eliminando perfil de trabajo ID:{workProfileDTO.Id}");

            await this._userLogic.ValidateUserCredentials(workProfileDTO.UserId, workProfileDTO.UserPassword);

            var workProfile = this._workProfileRepository.GetByUserId(workProfileDTO.Id);

            if (workProfile == null)
            {
                this._logger.Error($"Perfil de trabajo ID:{workProfileDTO.Id} no existe");

                throw new WorkProfileDoesNotExistException(workProfileDTO.Id);
            }

            if (!string.IsNullOrWhiteSpace(workProfile.ImagePath) && AssetManager.DoesFileExist(workProfile.ImagePath))
            {
                AssetManager.DeleteAssetFromAssetsFolder<WorkProfile>(workProfile.ImagePath);
            }

            this._workProfileRepository.DeleteWorkProfileByUserId(workProfile.Id);

            this._logger.Info($"Se eliminó el perfil de trabajo ID:{workProfile.Id} exitosamente");
        }

        public async Task UpdateWorkProfile(WorkProfileDTO workProfileDTO)
        {
            this._logger.Info($"Actualizando perfil de trabajo ID:{workProfileDTO.Id}");

            await this._userLogic.ValidateUserCredentials(workProfileDTO.UserId, workProfileDTO.UserPassword);

            var workProfile = this._workProfileRepository.GetByUserId(workProfileDTO.Id);

            if (workProfile == null)
            {
                this._logger.Error($"Perfil de trabajo ID:{workProfileDTO.Id} no existe");

                throw new WorkProfileDoesNotExistException(workProfileDTO.Id);
            }

            var updatedWorkProfile = WorkProfileDTO.DTOToEntity(workProfileDTO);

            this._workProfileRepository.UpdateWorkProfile(updatedWorkProfile);

            this._skillRepository.DeleteSkillsByWorkProfileId(workProfile.Id);

            if(workProfileDTO.Skills != null && workProfileDTO.Skills.Count > 0)
            {
                List<Skill> skills = new();

                foreach (SkillDTO skillDTO in workProfileDTO.Skills)
                {
                    skillDTO.WorkProfileId = workProfile.Id.ToString();

                    skills.Add(SkillDTO.DTOToEntity(skillDTO));
                }

                this._skillRepository.CreateMany(skills);
            }

            this._logger.Info($"Se actualizó el perfil de trabajo ID:{updatedWorkProfile.Id} exitosamente");
        }
    }
}