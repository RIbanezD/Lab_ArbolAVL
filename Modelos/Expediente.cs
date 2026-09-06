namespace AVL_Ruben_Ibañez.Modelos
{
    public class Expediente
    {
        public string NumeroExpediente { get; set; }
        public string NombrePaciente { get; set; }
        public int Edad { get; set; }
        public string TipoSangre { get; set; }

        public Expediente(string numeroExpediente, string nombrePaciente, int edad, string tipoSangre)
        {
            NumeroExpediente = numeroExpediente;
            NombrePaciente = nombrePaciente;
            Edad = edad;
            TipoSangre = tipoSangre;
        }

        public int CompararCon(string otroNumeroExpediente)
        {
            return string.Compare(NumeroExpediente, otroNumeroExpediente, StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return $"{NumeroExpediente} | {NombrePaciente} | {Edad} años | {TipoSangre}";
        }
    }
}
