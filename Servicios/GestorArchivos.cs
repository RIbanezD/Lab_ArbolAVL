using System.Text;
using AVL_Ruben_Ibañez.Modelos;

namespace AVL_Ruben_Ibañez.Servicios
{
    public class GestorArchivos
    {
        private const string Encabezado = "NumeroExpediente,NombrePaciente,Edad,TipoSangre";

        // Ruta archivo de guardado
        public static string ObtenerRutaArchivo(string nombreArchivo = "Expedientes.csv")
        {
            return Path.Combine(AppContext.BaseDirectory, nombreArchivo);
        }

        public static bool ExisteArchivo(string nombreArchivo = "Expedientes.csv")
        {
            return File.Exists(ObtenerRutaArchivo(nombreArchivo));
        }

        // Guarda la lista de los expedientes
        public static string Guardar(List<Expediente> expedientes, string nombreArchivo = "Expedientes.csv")
        {
            string ruta = ObtenerRutaArchivo(nombreArchivo);
            var sb = new StringBuilder();
            sb.AppendLine(Encabezado);

            foreach (var exp in expedientes)
            {
                sb.AppendLine(string.Join(",",
                    csv(exp.NumeroExpediente),
                    csv(exp.NombrePaciente),
                    exp.Edad.ToString(),
                    csv(exp.TipoSangre)));
            }
            File.WriteAllText(ruta, sb.ToString(), Encoding.UTF8);
            return ruta;
        }

        // Funcion Importar
        public static List<Expediente> Importar(out List<string> erroresEncontrados, string nombreArchivo = "Expedientes.csv")
        {
            erroresEncontrados = new List<string>();
            var resultado = new List<Expediente>();
            string ruta = ObtenerRutaArchivo(nombreArchivo);

            if (!File.Exists(ruta))
            {
                erroresEncontrados.Add($"No se encontró el archivo '{nombreArchivo}' en la carpeta del programa.");
                return resultado;
            }

            string[] lineas = File.ReadAllLines(ruta, Encoding.UTF8);

            for (int i = 0; i < lineas.Length; i++)
            {
                string linea = lineas[i];
                if (string.IsNullOrWhiteSpace(linea)) continue;
                if (i == 0 && linea.StartsWith("NumeroExpediente", StringComparison.OrdinalIgnoreCase))
                    continue; // Encabezado

                string[] campos = linea.Split(',');

                if (campos.Length != 4)
                {
                    erroresEncontrados.Add($"Línea {i + 1}: formato inválido (\"{linea}\").");
                    continue;
                }

                string numeroExpediente = campos[0].Trim();
                string nombre = campos[1].Trim();
                string edadTexto = campos[2].Trim();
                string tipoSangre = campos[3].Trim();

                if (!int.TryParse(edadTexto, out int edad))
                {
                    erroresEncontrados.Add($"Línea {i + 1}: edad inválida (\"{edadTexto}\").");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(numeroExpediente) || string.IsNullOrWhiteSpace(nombre))
                {
                    erroresEncontrados.Add($"Línea {i + 1}: datos incompletos.");
                    continue;
                }

                resultado.Add(new Expediente(numeroExpediente, nombre, edad, tipoSangre));
            }

            return resultado;
        }

        private static string csv(string campo)
        {
            if (campo.Contains(',') || campo.Contains('"'))
            {
                return "\"" + campo.Replace("\"", "\"\"") + "\"";
            }
            return campo;
        }
    }
}