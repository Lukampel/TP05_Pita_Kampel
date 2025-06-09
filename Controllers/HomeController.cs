using Microsoft.AspNetCore.Mvc;
using TP05_Kampel_Pita.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace ElOrfanatoOlvidado.Controllers
{
    public class HomeController : Controller
    {
        private const string SessionKey = "SalaDeEscape";

        public IActionResult Historia()
        {
            return View();
        }

        public IActionResult Reglas()
        {
            return View();
        }
        public IActionResult Traductor()
        {
            return View();
        }

        public IActionResult Habitacion1()
        {
            return RedirectToAction("Habitacion");
        }
        public IActionResult Integrantes()
        {
            return View();
        }
        public IActionResult Carta()
{
    return View();
}

        public IActionResult Index()
        {
            var salaDeEscape = new SalaDeEscape();
            var jsonString = JsonConvert.SerializeObject(salaDeEscape);
            HttpContext.Session.SetString(SessionKey, jsonString);
            return View("Index");
        }

        public IActionResult Habitacion()
        {
            var jsonString = HttpContext.Session.GetString(SessionKey);
            var salaDeEscape = JsonConvert.DeserializeObject<SalaDeEscape>(jsonString);

            if (salaDeEscape == null)
                return RedirectToAction("Index");

            if (salaDeEscape.tiempoTerminado())
                return RedirectToAction("Perdio");

            switch (salaDeEscape.numHabitacion)
            {
                case 1:
                    var habitacion1 = GenerarDatosHabitacion1();
                    return View("Habitacion1", habitacion1);
                case 2:
                    var habitacion2 = GenerarDatosHabitacion2();
                    return View("Habitacion2", habitacion2);
                case 3:
                    var habitacion3 = GenerarDatosHabitacion3();
                    return View("Habitacion3", habitacion3);
                case 4:
                    var habitacion4 = GenerarDatosHabitacion4();
                    return View("Habitacion4", habitacion4);
                default:
                    return RedirectToAction("Gano");
            }
        }

        [HttpPost]
        public IActionResult ResolverHabitacion(int habitacionNumero, string respuesta)
        {
            var jsonString = HttpContext.Session.GetString(SessionKey);
            var salaDeEscape = JsonConvert.DeserializeObject<SalaDeEscape>(jsonString);

            if (salaDeEscape == null)
                return RedirectToAction("Index");

            if (salaDeEscape.tiempoTerminado())
                return RedirectToAction("Perdio");

            bool correcta = false;

            switch (habitacionNumero)
            {
                case 1:
                    correcta = respuesta?.ToLower() == "humo";
                    if (correcta) salaDeEscape.CodigoHabitacion1 = respuesta;
                    break;
                case 2:
                    correcta = respuesta?.ToLower() == "3313";
                    if (correcta) salaDeEscape.CodigoHabitacion2 = respuesta;
                    break;
                case 3:
                    correcta = respuesta?.ToLower() == "fuego";
                    if (correcta) salaDeEscape.CodigoHabitacion3 = respuesta;
                    break;
                case 4:
                    correcta = respuesta?.ToLower() == "melodia";
                    if (correcta) salaDeEscape.CodigoHabitacion4 = respuesta;
                    break;
            }

            if (correcta)
            {
                salaDeEscape.AvanzarHabitacion();
                HttpContext.Session.SetString(SessionKey, JsonConvert.SerializeObject(salaDeEscape));

                if (salaDeEscape.Gano)
                    return RedirectToAction("Gano");

                return RedirectToAction("Habitacion");
            }

            ViewBag.Error = "Respuesta incorrecta, intenta nuevamente.";
            HttpContext.Session.SetString(SessionKey, JsonConvert.SerializeObject(salaDeEscape));

            switch (habitacionNumero)
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
                    return RedirectToAction("Index");
            }
        }

        public IActionResult Gano()
        {
            return View("Gano");
        }

        public IActionResult Perdio()
        {
            return View("Perdio");
        }

        private Habitacion GenerarDatosHabitacion1()
        {
            return new Habitacion
            {
                Numero = 1,
                Descripcion = "¡Llegaste a la recepción del orfanato!",
                Adivinanza = "Soy hijo de un banquete ardiente, nací sin carne, fui transparente. Subo al cielo sin tener alas, me disuelvo donde no hay balas. No dejo huella donde reposo, y aunque me ves, no soy hermoso. ¿Qué soy ? ",
                Imagenes = new[] { "/images/habitacion1a.jpg", "/images/habitacion1b.jpg" },
                Pista = "Encuentra el codigo y decifra la palabra clave para escapar...",
                RespuestaCorrecta = "humo"
            };
        }

        private Habitacion GenerarDatosHabitacion2()
        {
            return new Habitacion
            {
                Numero = 2,
                Descripcion = "Adivina los objetos según las pistas y obtén la palabra clave.",
                Pista = "Cada objeto vale una letra según su posición en el abecedario.",
                RespuestaCorrecta = "3313"
            };
        }

        public IActionResult Habitacion3()
        {
            Habitacion h3 = GenerarDatosHabitacion3();
            return View(h3);
        }

        private Habitacion GenerarDatosHabitacion3()
        {
            return new Habitacion
            {
                Numero = 3,
                Descripcion = "El silencio es pesado en esta habitación. Entre las sombras, un viejo escritorio parece guardar secretos de tiempos olvidados...",
                Pista = "A veces, las respuestas más importantes están escritas en tinta invisible...",
                Adivinanza = "¿Qué historias ocultas aguardan ser descubiertas?",
                RespuestaCorrecta = "fuego"
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
                Pista = "Solo una canción coincide con la original.",
                RespuestaCorrecta = "melodia"
            };
        }
    }
}


