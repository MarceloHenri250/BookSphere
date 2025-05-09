using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BookSphere.Views.Perfil
{
    public class MinhaBiblioteca : PageModel
    {
        private readonly ILogger<MinhaBiblioteca> _logger;

        public MinhaBiblioteca(ILogger<MinhaBiblioteca> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}