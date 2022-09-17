using LKDin.DTOs;
using LKDin.Helpers;
using LKDin.IBusinessLogic;
using LKDin.Server.Domain;

namespace LKDin.UI.ConsoleMenu.AvailableOptions
{
    public class SearchWorkProfilesBySkillsOption : ConsoleMenuOption
    {
        private readonly IWorkProfileService _workProfileService;

        public SearchWorkProfilesBySkillsOption(string messageToPrint, IWorkProfileService workProfileService) : base(messageToPrint)
        {
            this._workProfileService = workProfileService;
        }

        protected override void PerformExecution()
        {
            var searchCriteria = this.RequestSearchCriteria();

            var data = this._workProfileService.GetWorkProfilesBySkills(NormalizeSearchCriteriaIntoSkills(searchCriteria));

            this.PrintResultsInTable(data);

            this.PrintFinishedExecutionMessage(null);
        }

        private string RequestSearchCriteria()
        {
            Console.Write("Ingrese las habilidades (separadas por coma): ");

            string searchCriteria = this.CancelableReadLine();

            return searchCriteria;
        }

        private List<SkillDTO> NormalizeSearchCriteriaIntoSkills(string searchCriteria)
        {
            List<SkillDTO> resultantSkills = new();

            string[] words = searchCriteria.Split(',');

            foreach (var skill in words)
            {
                resultantSkills.Add(new SkillDTO() { Name = skill.Trim() });
            }

            return resultantSkills;
        }

        private void PrintResultsInTable(List<WorkProfileDTO> results)
        {
            var columnNames = new[]
            {
                "ID Usuario",
                "Nombre",
                "Descripcion",
                "Skills"
            };

            this.PrintDataInTable<WorkProfileDTO>(results, columnNames,
                wp => wp.User.Id,
                wp => wp.User.Name,
                wp => wp.Description,
                wp => String.Join<string>(',', wp.Skills.Select(s => s.Name).ToArray()));

        }
    }
}
