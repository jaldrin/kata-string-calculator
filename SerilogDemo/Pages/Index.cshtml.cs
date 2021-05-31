using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;

namespace SerilogDemo.Pages
{
    public class IndexModel : PageModel
    {
        #region Constructor
        private readonly ILogger<IndexModel> _logger;
        public IndexModel(ILogger<IndexModel> logger)
            => _logger = logger;
        #endregion

        public void OnGet()
        {
            _logger.LogInformation(100, "You requested the Index page.");

            try
            {
                for (int i = 0; i < 100; i++)
                {
                    if (i == 6)
                        throw new Exception("This is our demo exception");
                    else
                        _logger.LogInformation(200 + i, "The value of i is {LoopCountValue}", i);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(300, ex, "We caught this exception in the Index Get call.");
            }
        }
    }
}
