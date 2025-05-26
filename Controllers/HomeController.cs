using Microsoft.AspNetCore.Mvc;
using TP05_Kampel_Pita.Models;

namespace ElOrfanatoOlvidado.Controllers
{
    public class JuegoController : Controller
    {
      
        public IActionResult PantallaInicio()
        {
            // Iniciar juego nuevo
            var SalaDeEscape = new SalaDeEscape();
            var jsonString = Objeto.ObjectToString(SalaDeEscape);
            HttpContext.Session.SetString("Juego", jsonString);
            return View("PantallaInicioView");
        }

        
        public IActionResult Habitacion()
        {
          var jsonString = HttpContext.Session.GetString("Sala de Escape");
          var SalaDeEscape = Objeto.StringToObject<SalaDeEscape>(jsonString);


            if (SalaDeEscape == null)
                return RedirectToAction("PantallaInicio");

            if (SalaDeEscape.TiempoCumplido())
                return RedirectToAction("PantallaFinal", new { perdio = true });

            // Según habitación, mostrar la vista y datos
            switch (SalaDeEscape.HabitacionActual)
            {
                case 1:
                    return View("Habitacion1", GenerarDatosHabitacion1());
                case 2:
                    return View("Habitacion2", GenerarDatosHabitacion2());
                case 3:
                    return View("Habitacion3", GenerarDatosHabitacion3());
                case 4:
                    return View("Habitacion4", GenerarDatosHabitacion4());
                default:
                    return RedirectToAction("PantallaFinal", new { perdio = false });
            }
        }

        [HttpPost]
        public IActionResult ResolverHabitacion(int habitacionNumero, string respuesta)
        {
            var jsonString = HttpContext.Session.GetString("Sala de Escape");
          var SalaDeEscape = Objeto.StringToObject<SalaDeEscape>(jsonString);
            if (SalaDeEscape == null)
                return RedirectToAction("PantallaInicio");

            if (SalaDeEscape.TiempoCumplido())
                return RedirectToAction("PantallaFinal", new { perdio = true });

            // Validar respuesta según habitación
            bool correcta = false;

            switch (habitacionNumero)
            {
                case 1:
                    correcta = respuesta?.ToLower() == "juguetes";  // Ejemplo palabra clave de diferencias
                    if (correcta) SalaDeEscape.CodigoHabitacion1 = respuesta;
                    break;

                case 2:
                    correcta = respuesta?.ToLower() == "abc"; // Ejemplo letras objeto (pista 2)
                    if (correcta) SalaDeEscape.CodigoHabitacion2 = respuesta;
                    break;

                case 3:
                    correcta = respuesta?.ToLower() == "clave"; // Palabra clave carta
                    if (correcta) SalaDeEscape.CodigoHabitacion3 = respuesta;
                    break;

                case 4:
                    correcta = respuesta?.ToLower() == "melodia"; // Palabra clave canción correcta
                    if (correcta) SalaDeEscape.CodigoHabitacion4 = respuesta;
                    break;
            }

            if (correcta)
            {
                SalaDeEscape.Avanzar();
                HttpContext.Session.SetString("Sala de escape", Objeto.ObjectToString(SalaDeEscape));


                if (SalaDeEscape.Gano)
                    return RedirectToAction("PantallaFinal", new { perdio = false });
            }

            // Si no acertó, vuelve a la misma habitación con mensaje de error
            ViewBag.Error = correcta ? null : "Respuesta incorrecta, intenta nuevamente.";
            HttpContext.Session.SetObjectAsJson("Juego", juego);

            return RedirectToAction("Habitacion");
        }

        public IActionResult PantallaFinal(bool perdio)
        {
            ViewBag.Perdio = perdio;
            return View("PantallaFinalView");
        }

        // Métodos para generar datos de habitaciones

        private Habitacion GenerarDatosHabitacion1()
        {
            return new Habitacion
            {
                Numero = 1,
                Descripcion = "Encuentra las diferencias entre estas dos imágenes y forma la palabra clave.",
                Imagenes = new[] { "/images/habitacion1a.jpg", "/images/habitacion1b.jpg" },
                Pista = "Las diferencias forman la palabra clave.",
                RespuestaCorrecta = "juguetes"
            };
        }

        private Habitacion GenerarDatosHabitacion2()
        {
            return new Habitacion
            {
                Numero = 2,
                Descripcion = "Adivina los objetos según las pistas y obtén la palabra clave.",
                Pista = "Cada objeto vale una letra según su posición en el abecedario.",
                RespuestaCorrecta = "abc"
            };
        }

        private Habitacion GenerarDatosHabitacion3()
        {
            return new Habitacion
            {
                Numero = 3,
                Descripcion = "Descifra la palabra clave en la carta del exdirector.",
                Pista = "Observa las letras en negrita y rojo.",
                RespuestaCorrecta = "clave"
            };
        }

        private Habitacion GenerarDatosHabitacion4()
        {
            return new Habitacion
            {
                Numero = 4,
                Descripcion = "Escucha la canción de cuna correcta para obtener la clave.",
                Audios = new[]
                {
                    "/audio/cancion1.mp3",
                    "/audio/cancion2.mp3",
                    "/audio/cancion3.mp3",
                    "/audio/cancion4.mp3"
                },
                Pista = "Solo una canción coincide con la original."
            };
        }
    }
}

