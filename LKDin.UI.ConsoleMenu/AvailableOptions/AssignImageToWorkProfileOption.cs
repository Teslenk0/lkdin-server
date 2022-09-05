using LKDin.DTOs;
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
            throw new NotImplementedException();

            try
            {
                this.PrintHeader(this.MessageToPrint);

                var userId = this.RequestUserId();

                WorkProfileDTO workProfileDTO = new()
                {
                    
                    User = new UserDTO()
                    {
                        Id = userId,
                        Password = this.RequestPassword()
                    },
                    UserId = userId,
                    Description = this.RequestImagePath()
                };

                this._workProfileService.AssignImageToWorkProfile(workProfileDTO);

                this.PrintFinishedExecutionMessage("Se creo el perfil de trabajo exitosamente");
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

            string description;

            do
            {
                description = Console.ReadLine();

                if (description == null || description.Length < 2)
                {
                    this.PrintError("Valor incorrecto");
                    Console.Write("Descripcion: ");
                }
            }
            while (description == null || description.Length < 2);

            return description;
        }

        private List<SkillDTO> RequestSkills()
        {
            List<SkillDTO> resultantSkills = new();

            Console.Write("Habilidades (separadas por coma): ");

            string skills;

            do
            {
                skills = Console.ReadLine();

                if(skills.Length > 2)
                {
                    string[] words = skills.Split(',');

                    foreach (var skill in words)
                    {
                        resultantSkills.Add(new SkillDTO() { Name = skill.Trim() });
                    }
                }

                if(resultantSkills.Count < 3)
                {
                    this.PrintError("Valor incorrecto (debes ingresar al menos 3)");
                    Console.Write("Habilidades (separadas por coma): ");
                }
            }
            while (resultantSkills.Count < 3);

            return resultantSkills;
        }
    }
}
