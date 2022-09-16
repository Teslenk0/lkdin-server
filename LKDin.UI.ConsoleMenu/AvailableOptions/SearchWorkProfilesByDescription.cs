using LKDin.DTOs;
using LKDin.Helpers;
using LKDin.IBusinessLogic;
using LKDin.Server.Domain;
using LKDin.UI.ConsoleMenu.Extensions;

namespace LKDin.UI.ConsoleMenu.AvailableOptions
{
    public class SearchWorkProfilesByDescription : ConsoleMenuOption
    {
        private readonly IWorkProfileService _workProfileService;

        public SearchWorkProfilesByDescription(string messageToPrint, IWorkProfileService workProfileService) : base(messageToPrint)
        {
            this._workProfileService = workProfileService;
        }

        public override void Execute()
        {
            try
            {
                this.PrintHeader(this.MessageToPrint);

                var searchCriteria = this.RequestSearchCriteria();

                this.PrintInfoDiv();

                var data = this._workProfileService.GetWorkProfilesByDescription(searchCriteria);

                this.PrintResultsInTable(data);

                this.PrintFinishedExecutionMessage(null);
            }
            catch (Exception e)
            {
                this.PrintError(e.Message);

                this.PrintFinishedExecutionMessage(null, false);
            }
        }

        private string RequestSearchCriteria()
        {
            Console.Write("Ingrese la descripción a buscar: ");

            string searchCriteria = Console.ReadLine() ?? "";

            return searchCriteria;
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
