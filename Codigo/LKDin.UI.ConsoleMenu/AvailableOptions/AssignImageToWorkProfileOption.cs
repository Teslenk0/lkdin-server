using LKDin.DTOs;
using LKDin.IBusinessLogic;
using LKDin.Helpers.Assets;

namespace LKDin.UI.ConsoleMenu.AvailableOptions
{
    public class AssignImageToWorkProfile : UserProtectedConsoleMenuOption
    {
        private readonly IWorkProfileService _workProfileService;

        public AssignImageToWorkProfile(string messageToPrint, IWorkProfileService workProfileService) : base(messageToPrint)
        {
            this._workProfileService = workProfileService;
        }

        protected override async Task PerformExecution()
        {
            WorkProfileDTO workProfileDTO = new()
            {
                UserId = this.RequestUserId(),
                UserPassword = this.RequestPassword(),
                ImagePath = this.RequestImagePath()
            };

            await this._workProfileService.AssignImageToWorkProfile(workProfileDTO);

            this.PrintFinishedExecutionMessage("Se asigno la imagen al perfil de trabajo exitosamente");
        }

        private string RequestImagePath()
        {
            Console.Write("Ruta a la imagen (absoluta): ");

            string imagePath;

            do
            {
                imagePath = this.CancelableReadLine();

                if (string.IsNullOrWhiteSpace(imagePath) || imagePath.Length < 5)
                {
                    this.PrintError("Valor incorrecto");
                    Console.Write("Ruta a la imagen (absoluta): ");
                }
                else if (!AssetManager.DoesFileExist(imagePath))
                {
                    this.PrintError("Archivo no existe");
                    Console.Write("Ruta a la imagen (absoluta): ");
                }
            }
            while (
                    string.IsNullOrWhiteSpace(imagePath)
                || imagePath.Length < 5
                || !AssetManager.DoesFileExist(imagePath));

            return imagePath;
        }
    }
}
