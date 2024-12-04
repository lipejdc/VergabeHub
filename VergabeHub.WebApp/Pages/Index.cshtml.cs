using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VergabeHub.Data;
using VergabeHub.WebApp.Pages.Shared;

namespace VergabeHub.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly VergabeHubDbContext _dbContext;

        public TableData? TableData { get; set; }

        public IndexModel(VergabeHubDbContext dBcontext)
        {
            _dbContext = dBcontext;
        }

        public async Task<IActionResult> OnGetNoticesAsync(string noticePlatformCheckboxValues)
        {

            if (string.IsNullOrEmpty(noticePlatformCheckboxValues))
            {
                // Handle case when no checkboxes are selected
                return BadRequest("At least one checkbox value must be provided.");
            }

            var selectedPlatformNames = new List<string>();

            // Map checkbox values to NoticePlatform names
            foreach (var checkboxValue in noticePlatformCheckboxValues.Split(','))
            {
                if (checkboxValue.Equals("vergabe_bw", StringComparison.OrdinalIgnoreCase))
                {
                    selectedPlatformNames.Add("Vergabe BW");
                }
                else if (checkboxValue.Equals("vergabe_nrw", StringComparison.OrdinalIgnoreCase))
                {
                    selectedPlatformNames.Add("Vergabe NRW");
                }
            }

            // Query the database to retrieve NoticePlatform IDs based on names
            var selectedPlatformIds = await _dbContext.NoticePlatforms
                .Where(np => selectedPlatformNames.Contains(np.Name!))
                .Select(np => np.Id)
                .ToListAsync();

            // Query the database to retrieve notices based on selected platform IDs
            var notices = await _dbContext.Notices
                .Include(n => n.NoticePlatform)
                .Include(n => n.ContractingAuthority)
                .Include(n => n.Type)
                .Where(n => selectedPlatformIds.Contains(n.NoticePlatformId))
                .ToListAsync();

            // Set the ViewData to be used in the PartialView
            ViewData["tableData"] = notices;

            // Return the PartialView along with the ViewData
            return new PartialViewResult
            {
                // Sets the PartialView to be TableData (the file named _TableData.cshtml)
                ViewName = "_TableData",
                // Assigns the ViewData from the controller to the ViewData property
                // of the PartialViewResult, allowing any data stored in ViewData to be
                // accessed in the partial view
                ViewData = ViewData
            };
        }
    }
}