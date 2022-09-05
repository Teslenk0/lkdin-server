using LKDin.DTOs;
using LKDin.Exceptions;
using LKDin.IBusinessLogic;
using LKDin.Server.DataAccess.Repositories;
using LKDin.Server.IDataAccess.Repositories;

namespace LKDin.Server.BusinessLogic
{
    public class WorkProfileService : IWorkProfileService
    {
        private readonly IWorkProfileRepository _workProfileRepository;

        private readonly IUserService _userService;

        public WorkProfileService(IUserService userService)
        {
            this._workProfileRepository = new WorkProfileRepository();

            this._userService = userService;
        }

        public void CreateWorkProfile (WorkProfileDTO workProfileDTO)
        {
            this._userService.ValidateUserCredentials(workProfileDTO.UserId, workProfileDTO.User.Password);

            var exists = this._workProfileRepository.ExistsByUserId(workProfileDTO.UserId);

            if (exists)
            {
                throw new WorkProfileAlreadyExistsException(workProfileDTO.UserId);
            }

            var workProfile = WorkProfileDTO.DTOToEntity(workProfileDTO);

            // Set the User as null so EF does not try to create a new user
            workProfile.User = null;

            this._workProfileRepository.Create(workProfile);
        }


        public void AssignImageToWorkProfile(WorkProfileDTO partialWorkProfileDTO)
        {
            this._userService.ValidateUserCredentials(partialWorkProfileDTO.UserId, partialWorkProfileDTO.User.Id);

            var workProfile = this._workProfileRepository.GetByUserId(partialWorkProfileDTO.UserId);

            if (workProfile == null)
            {
                throw new WorkProfileDoesNotExistException(partialWorkProfileDTO.UserId);
            }

            workProfile.ImagePath = partialWorkProfileDTO.ImagePath;

            this._workProfileRepository.Update(workProfile);
        }
    }
}