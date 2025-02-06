using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JuegoMemoria_DANIEL_INCHE.Models
{
    public class JuegoMemoria
    {
        public List<string> CartasBarajadas { get; set; }
        public List<int> CartasEmparejadas { get; set; }
        public List<int> CartasSeleccionadas { get; set; }
        public int Intentos { get; set; }
    }
}