<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="JuegoMemoria_DANIEL_INCHE._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" type="text/css" href="Scripts/JuegoMemoriaxxx.css" />

    <h2>Juego de Memoria</h2>
     <button id="reiniciarJuego">Reiniciar</button>
    <p><strong>Intentos:</strong> <span id="contadorIntentos">0</span></p>


    <div class="tablero" id="tableroJuego">
        <% 
            var juego = (JuegoMemoria_DANIEL_INCHE.Models.JuegoMemoria)Session["EstadoJuego"];
            if (juego != null) { 
                for (int i = 0; i < juego.CartasBarajadas.Count; i++) { 
        %>
            <div class="carta" data-indice="<%= i %>">
                <img src='<%= juego.CartasEmparejadas.Contains(i) ? "/Images/" + juego.CartasBarajadas[i] : "/Images/joker.png" %>' alt="Carta">
            </div>
        <% 
                }
            } else { 
        %>
            <p>No hay datos cargados. Recarga la página.</p>
        <% } %>
    </div>
    
    <p id="contador" style="text-align: center; font-size: 20px; font-weight: bold; color: red;"></p>
    <h3>Cartas Emparejadas</h3>
    <div id="cartasEmparejadas">
        <% if (juego != null && juego.CartasEmparejadas.Count > 0) { %>
            <% foreach (var index in juego.CartasEmparejadas) { %>
                <span class="carta-emparejada">
                    <img src="/Images/<%= juego.CartasBarajadas[index] %>" alt="Emparejada">
                </span>
            <% } %>
        <% } else { %>
            <p>Aún no hay cartas emparejadas.</p>
        <% } %>
    </div>
    
   

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script>
        $(document).ready(function () {
            let bloqueado = false; // Variable para bloquear la selección de cartas

            $(document).on("click", ".carta", function () {
                if (bloqueado) return; // Si está bloqueado, no permitir selección

                let indice = $(this).data("indice");

                $.ajax({
                    url: "/JuegoMemoria/VoltearCarta",
                    type: "POST",
                    data: { indice: indice },
                    success: function (data) {
                        if (data.success) {
                            $("#contadorIntentos").text(data.intentos); 

                            $(".carta").each(function () {
                                let i = $(this).data("indice");

                                //  mostrar su valor permanentemente
                                if (data.cartasEmparejadas.includes(i)) {
                                    $(this).find("img").attr("src", "/Images/" + data.cartasBarajadas[i]);
                                }
                                // mostrar su valor temporalmente
                                else if (data.cartasSeleccionadas.includes(i)) {
                                    $(this).find("img").attr("src", "/Images/" + data.cartasBarajadas[i]);
                                }
                                // mostrar valor por defecto
                                else {
                                    $(this).find("img").attr("src", "/Images/joker.png");
                                }
                            });

                            // Si las cartas no coinciden
                            if (!data.esPar && data.cartasSeleccionadas.length === 2) {
                                bloqueado = true; 
                                let segundosRestantes = data.breveRetraso / 1000;
                                $("#contador").text(`Las cartas se voltearán en ${segundosRestantes} segundos`);

                                let intervalo = setInterval(() => {
                                    segundosRestantes--;
                                    $("#contador").text(`Las cartas se voltearán en ${segundosRestantes} segundos`);

                                    if (segundosRestantes <= 0) {
                                        clearInterval(intervalo);
                                        $("#contador").text("");
                                        bloqueado = false; 

                                        // Voltear las cartas que no hicieron match
                                        $(".carta").each(function () {
                                            let i = $(this).data("indice");
                                            if (!data.cartasEmparejadas.includes(i)) {
                                                $(this).find("img").attr("src", "/Images/joker.png");
                                            }
                                        });
                                    }
                                }, 1000);
                            }


                            // Actualizar lista de cartas emparejadas
                            let emparejadasHTML = "";
                            if (data.cartasEmparejadas.length > 0) {
                                data.cartasEmparejadas.forEach((index) => {
                                    emparejadasHTML += `<span class="carta-emparejada"><img src="/Images/${data.cartasBarajadas[index]}" alt="Emparejada"></span>`;
                                });
                            } else {
                                emparejadasHTML = "<p>Aún no hay cartas emparejadas.</p>";
                            }
                            $("#cartasEmparejadas").html(emparejadasHTML);
                        }
                    },
                    error: function () {
                        alert("Error al voltear la carta.");
                    }
                });
            });

            $("#reiniciarJuego").click(function () {
                $.ajax({
                    url: "/JuegoMemoria/ReiniciarJuego",
                    type: "POST",
                    success: function () {
                        location.reload();
                    },
                    error: function () {
                        alert("Error al reiniciar el juego.");
                    }
                });
            });
        });
    </script>
</asp:Content>
