
public class SalaDeEscape
{
    public int numHabitacion { get; set; } = 1;
    public bool Terminado { get; set; } = false;
    public bool Gano { get; set; } = false;

    public DateTime TiempoInicio { get; set; } = DateTime.Now;
    public TimeSpan TiempoLimite { get; set; } = TimeSpan.FromMinutes(60);

        public string CodigoHabitacion1 { get; set; } // palabra clave 1
        public string CodigoHabitacion2 { get; set; } // palabra clave 2
        public string CodigoHabitacion3 { get; set; } // palabra clave 3
        public string CodigoHabitacion4 { get; set; } // palabra clave 4

    public bool tiempoTerminado()
    {
        return (DateTime.Now - TiempoInicio) > TiempoLimite;
    }

    public void AvanzarHabitacion()
    {
        numHabitacion++;
        if (numHabitacion > 4)
            Gano = true;
    }
}
