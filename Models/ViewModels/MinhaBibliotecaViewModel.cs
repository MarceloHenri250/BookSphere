using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookSphere.Models.ViewModels
{
    public class MinhaBibliotecaViewModel
    {
        public string NomeUsuario { get; set; }
        public List<Favorito> Favoritos { get; set; } = new List<Favorito>();
    }
}