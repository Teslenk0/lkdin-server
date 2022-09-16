using LKDin.DTOs;
using LKDin.Helpers;
using LKDin.IBusinessLogic;
using LKDin.Server.Domain;

namespace LKDin.UI.ConsoleMenu.AvailableOptions
{
    public class AssignImageToWorkProfile : UserProtectedConsoleMenuOption
    {
        private readonly IWorkProfileService _workProfileService;

        public AssignImageToWorkProfile(string messageToPrint, IWorkProfileService workProfileService) : base(messageToPrint)
        {
            this._workProfileService = workProfileService;
        }

        public override void Execute()
        {
            try
            {
                this.PrintHeader(this.MessageToPrint);

                WorkProfileDTO workProfileDTO = new()
                {
                    UserId = this.RequestUserId(),
                    UserPassword = this.RequestPassword(),
                    ImagePath = this.RequestImagePath()
                };

                this._workProfileService.AssignImageToWorkProfile(workProfileDTO);

                this.PrintFinishedExecutionMessage("Se asigno la imagen al perfil de trabajo exitosamente");
            }
            catch (Exception e)
            {
                this.PrintError(e.Message);

                this.PrintFinishedExecutionMessage(null, false);
            }
        }

        private string RequestImagePath()
        {
            Console.Write("Ruta a la imagen (absoluta): ");

            string imagePath;

            do
            {
                imagePath = Console.ReadLine();

                if (imagePath == null || imagePath.Length < 5)
                {
                    this.PrintError("Valor incorrecto");
                    Console.Write("Ruta a la imagen (absoluta): ");
                }
                else if (!LKDinAssetManager.DoesFileExist(imagePath))
                {
                    this.PrintError("Archivo no existe");
                    Console.Write("Ruta a la imagen (absoluta): ");
                }
            }
            while (
                    imagePath == null
                || imagePath.Length < 5
                || !LKDinAssetManager.DoesFileExist(imagePath));

            return imagePath;
        }
    }
}
