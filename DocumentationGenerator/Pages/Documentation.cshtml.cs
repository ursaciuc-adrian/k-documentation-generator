using DocumentationGenerator.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DocumentationGenerator.Pages
{
	public class DocumentationModel : PageModel
	{
        private readonly IProcessorService _processorService;

        public Processor Processor;

        public DocumentationModel(IProcessorService processorService)
        {
            _processorService = processorService;
        }

		public void OnGet(string fileContent)
		{
            Processor = _processorService.ProcessData();
		}
	}
}