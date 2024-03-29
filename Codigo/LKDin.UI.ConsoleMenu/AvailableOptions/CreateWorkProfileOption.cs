﻿using LKDin.DTOs;
using LKDin.IBusinessLogic;
using LKDin.Server.Domain;
using System.Xml.Linq;

namespace LKDin.UI.ConsoleMenu.AvailableOptions
{
    public class CreateWorkProfileOption : UserProtectedConsoleMenuOption
    {
        private readonly IWorkProfileLogic _workProfileService;

        public CreateWorkProfileOption(string messageToPrint, IWorkProfileLogic workProfileService) : base(messageToPrint)
        {
            this._workProfileService = workProfileService;
        }

        protected override async Task PerformExecution()
        {
            WorkProfileDTO workProfileDTO = new()
            {
                UserId = this.RequestUserId(),
                UserPassword = this.RequestPassword(),
                Description = this.RequestDescription(),
                Skills = this.RequestSkills()
            };

            await this._workProfileService.CreateWorkProfile(workProfileDTO);

            this.PrintFinishedExecutionMessage("Se creo el perfil de trabajo exitosamente");
        }

        private string RequestDescription()
        {
            Console.Write("Descripcion: ");

            string description;

            do
            {
                description = this.CancelableReadLine();

                if (string.IsNullOrWhiteSpace(description) || description.Length < 2)
                {
                    this.PrintError("Valor incorrecto");
                    Console.Write("Descripcion: ");
                }
            }
            while (string.IsNullOrWhiteSpace(description) || description.Length < 2);

            return description;
        }

        private List<SkillDTO> RequestSkills()
        {
            List<SkillDTO> resultantSkills = new();

            Console.Write("Habilidades (separadas por coma): ");

            string skills;

            do
            {
                skills = this.CancelableReadLine();

                if (skills.Length > 2)
                {
                    string[] words = skills
                        .Split(',')
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Distinct()
                        .ToArray();

                    foreach (var skill in words)
                    {
                        resultantSkills.Add(new SkillDTO() { Name = skill });
                    }
                }

                if (resultantSkills.Count < 3)
                {
                    resultantSkills.Clear();
                    this.PrintError("Valor incorrecto (debes ingresar al menos 3, no pueden repetirse ni estar vacias)");
                    Console.Write("Habilidades (separadas por coma): ");
                }
            }
            while (resultantSkills.Count < 3);

            return resultantSkills;
        }
    }
}
