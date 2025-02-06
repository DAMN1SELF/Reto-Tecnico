using System.Linq;
using System.Web.Mvc;
using JuegoMemoria_DANIEL_INCHE.Helpers;
using JuegoMemoria_DANIEL_INCHE.Models;

namespace JuegoMemoria_DANIEL_INCHE.Controllers
{
    public class JuegoMemoriaController : Controller
    {
        [HttpPost]
        public JsonResult VoltearCarta(int indice)
        {
            var estadoJuego = JuegoMemoriaHelper.ObtenerEstadoJuego();

            if (!estadoJuego.CartasSeleccionadas.Contains(indice) && !estadoJuego.CartasEmparejadas.Contains(indice))
            {
                estadoJuego.CartasSeleccionadas.Add(indice);

                if (estadoJuego.CartasSeleccionadas.Count == 2)
                {
                    estadoJuego.Intentos++;
                    bool ExisteMatch = (estadoJuego.CartasBarajadas[estadoJuego.CartasSeleccionadas[0]] == estadoJuego.CartasBarajadas[estadoJuego.CartasSeleccionadas[1]]);

                    if (ExisteMatch)
                    {
                        estadoJuego.CartasEmparejadas.AddRange(estadoJuego.CartasSeleccionadas);
                        estadoJuego.CartasSeleccionadas.Clear();
                    }
                    else
                    {
                        var cartasSeleccionadasPrevias = estadoJuego.CartasSeleccionadas.ToList();
                        estadoJuego.CartasSeleccionadas.Clear();

                        var respuesta = new
                        {
                            success = true,
                            cartasEmparejadas = estadoJuego.CartasEmparejadas,
                            cartasSeleccionadas = cartasSeleccionadasPrevias,
                            cartasBarajadas = estadoJuego.CartasBarajadas,
                            intentos = estadoJuego.Intentos,
                            esPar = false,
                            breveRetraso = 1000 
                        };

                        JuegoMemoriaHelper.GuardarEstadoJuego(estadoJuego);
                        return Json(respuesta);
                    }
                }
            }

            JuegoMemoriaHelper.GuardarEstadoJuego(estadoJuego);

            return Json(new
            {
                success = true,
                cartasEmparejadas = estadoJuego.CartasEmparejadas,
                cartasSeleccionadas = estadoJuego.CartasSeleccionadas,
                cartasBarajadas = estadoJuego.CartasBarajadas,
                intentos = estadoJuego.Intentos,
                esPar = estadoJuego.CartasSeleccionadas.Count != 2,
                breveRetraso = estadoJuego.CartasSeleccionadas.Count == 1 ? 0 : 5000
            });
        }

        [HttpPost]
        public JsonResult ReiniciarJuego()
        {
            JuegoMemoriaHelper.InicializarJuego();
            return Json(new { success = true });
        }
    }
}
