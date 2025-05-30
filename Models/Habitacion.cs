namespace TP05_Kampel_Pita.Models
{
    public class Habitacion
    {
        public int Numero { get; set; }
        public string Descripcion { get; set; }
        public string Adivinanza { get; set; }
        public string[] Imagenes { get; set; }
        public string[] Audios { get; set; }  // Urls o paths relativos
        public string Pista { get; set; }
        public string RespuestaCorrecta { get; set; }
    }
}
