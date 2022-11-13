using AutoMapper;
using Grpc.Core;
using LKDin.DTOs;
using LKDin.IBusinessLogic;
using LKDin.Logging.Client;

namespace LKDin.Server.V2.Services
{
    public class WorkProfileService : WorkProfile.WorkProfileBase
    {
        private readonly Logger _logger;

        private readonly IWorkProfileLogic _workProfileLogic;

        private readonly IMapper _mapper;

        public WorkProfileService(IWorkProfileLogic workProfileLogic, IMapper mapper)
        {
            _logger = new Logger("server-v2:grpc-services:work-profile");

            _workProfileLogic = workProfileLogic;

            _mapper = mapper;
        }

        public override async Task<ActionReply> CreateWorkProfile(UpsertWorkProfileRequest request, ServerCallContext context)
        {
            _logger.Info("Recibida nueva llamada gRPC para crear perfil de trabajo");

            var workProfileDTO = _mapper.Map<WorkProfileDTO>(request);

            await _workProfileLogic.CreateWorkProfile(workProfileDTO);

            return ConfirmOperation();
        }

        public override async Task<ActionReply> UpdateWorkProfile(UpsertWorkProfileRequest request, ServerCallContext context)
        {
            _logger.Info("Recibida nueva llamada gRPC para actualizar perfil de trabajo");

            var workProfileDTO = _mapper.Map<WorkProfileDTO>(request);

            workProfileDTO.Id = workProfileDTO.UserId;

            await _workProfileLogic.UpdateWorkProfile(workProfileDTO);

            return ConfirmOperation();
        }

        public override async Task<ActionReply> DeleteWorkProfile(DeleteActionRequest request, ServerCallContext context)
        {
            _logger.Info("Recibida nueva llamada gRPC para eliminar perfil de trabajo");

            var workProfileDTO = _mapper.Map<WorkProfileDTO>(request);

            workProfileDTO.UserId = workProfileDTO.Id;

            await _workProfileLogic.DeleteWorkProfile(workProfileDTO);

            return ConfirmOperation();
        }

        public override async Task<ActionReply> DeleteWorkProfileImage(DeleteActionRequest request, ServerCallContext context)
        {
            _logger.Info("Recibida nueva llamada gRPC para eliminar imagen de perfil de trabajo");

            var workProfileDTO = _mapper.Map<WorkProfileDTO>(request);

            workProfileDTO.UserId = workProfileDTO.Id;

            await _workProfileLogic.DeleteWorkProfileImage(workProfileDTO);

            return ConfirmOperation();
        }

        private ActionReply ConfirmOperation()
        {
            return new ActionReply() { Message = "Operación realizada exitosamente", Status = StatusCode.OK.ToString(), Success = true };
        }
    }
}