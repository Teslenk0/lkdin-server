using LKDin.IBusinessLogic;

namespace LKDin.UI.ConsoleMenu.AvailableOptions
{
    public class DownloadWorkProfileImageOption : UserProtectedConsoleMenuOption
    {
        private readonly IWorkProfileService _workProfileService;

        public DownloadWorkProfileImageOption(string messageToPrint, IWorkProfileService workProfileService) : base(messageToPrint)
        {
            this._workProfileService = workProfileService;
        }

        protected override void PerformExecution()
        {
            var userId = this.RequestUserId();

            var path = this._workProfileService.DownloadWorkProfileImage(userId);

            this.PrintFinishedExecutionMessage($"Se descargó la imagen correctamente en {path}");
        }
    }
}
