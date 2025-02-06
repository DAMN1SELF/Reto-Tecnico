using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JuegoMemoria_DANIEL_INCHE.Models;

namespace JuegoMemoria_DANIEL_INCHE.Helpers
{
    public static class JuegoMemoriaHelper
    {
        public static JuegoMemoria ObtenerEstadoJuego()
        {
            if (HttpContext.Current?.Session == null)
            {
                throw new Exception("La sesión no está disponible. Verifica la configuración de sessionState en Web.config.");
            }

            if (HttpContext.Current.Session["EstadoJuego"] == null)
            {
                InicializarJuego();
            }
            return (JuegoMemoria)HttpContext.Current.Session["EstadoJuego"];
        }

        public static void GuardarEstadoJuego(JuegoMemoria juego)
        {
            HttpContext.Current.Session["EstadoJuego"] = juego;
        }

        public static void InicializarJuego()
        {

            string[] valoresCartas = { 
                "csharp.png", 
                "csharp.png", 
                "html.png", 
                "html.png", 
                "js.png", 
                "js.png", 
                "mssql.png", 
                "mssql.png",
                "mysql.png",
                "mysql.png",
                "net.png",
                "net.png", 
                "oracle.png", 
                "oracle.png", 
                "vbnet.png", 
                "vbnet.png" };

            var juego = new JuegoMemoria
            {
                CartasBarajadas = valoresCartas.OrderBy(x => Guid.NewGuid()).ToList(),
                CartasEmparejadas = new List<int>(),
                CartasSeleccionadas = new List<int>()
            };

            HttpContext.Current.Session["EstadoJuego"] = juego;
        }
    }
}
