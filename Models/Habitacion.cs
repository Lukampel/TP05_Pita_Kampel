namespace TP05_Kampel_Pita.Models
{
    public class Habitacion
    {
        public int Numero { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string Adivinanza { get; set; } = string.Empty;
        public string[]? Imagenes { get; set; }
        public string[]? Audios { get; set; }
        public string Pista { get; set; } = string.Empty;
        public string RespuestaCorrecta { get; set; } = string.Empty;

        public Habitacion()
        {
            Descripcion = string.Empty;
            Adivinanza = string.Empty;
            Pista = string.Empty;
            RespuestaCorrecta = string.Empty;
        }
    }
}
