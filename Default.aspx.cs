using System;
using System.Web;
using System.Web.UI;
using JuegoMemoria_DANIEL_INCHE.Helpers;

namespace JuegoMemoria_DANIEL_INCHE
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
              
                    var juego = JuegoMemoriaHelper.ObtenerEstadoJuego();

            }
        }

    }
}