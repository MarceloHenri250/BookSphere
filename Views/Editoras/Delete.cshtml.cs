using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BookSphere.Views.Editoras
{
    public class Delete : PageModel
    {
        private readonly ILogger<Delete> _logger;

        public Delete(ILogger<Delete> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}