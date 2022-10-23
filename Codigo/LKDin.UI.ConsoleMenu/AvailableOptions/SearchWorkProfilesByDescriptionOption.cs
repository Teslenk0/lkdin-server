using LKDin.DTOs;
using LKDin.IBusinessLogic;

namespace LKDin.UI.ConsoleMenu.AvailableOptions
{
    public class SearchWorkProfilesByDescriptionOption : ConsoleMenuOption
    {
        private readonly IWorkProfileService _workProfileService;

        public SearchWorkProfilesByDescriptionOption(string messageToPrint, IWorkProfileService workProfileService) : base(messageToPrint)
        {
            this._workProfileService = workProfileService;
        }

        protected override async Task PerformExecution()
        {
            var searchCriteria = this.RequestSearchCriteria();

            var data = await this._workProfileService.GetWorkProfilesByDescription(searchCriteria);

            this.PrintResultsInTable(data);

            this.PrintFinishedExecutionMessage(null);
        }

        private string RequestSearchCriteria()
        {
            Console.Write("Ingrese la descripción a buscar: ");

            string searchCriteria = this.CancelableReadLine();

            return searchCriteria;
        }

        private void PrintResultsInTable(List<WorkProfileDTO> results)
        {
            var sortedResults = results.OrderBy(wp => wp.Id).ToList();

            var columnNames = new[]
            {
                "ID Usuario",
                "Nombre",
                "Descripcion",
                "Skills"
            };

            this.PrintDataInTable<WorkProfileDTO>(sortedResults, columnNames,
                wp => wp.User.Id,
                wp => wp.User.Name,
                wp => wp.Description,
                wp => String.Join<string>(',', wp.Skills.Select(s => s.Name).ToArray()));

        }
    }
}
