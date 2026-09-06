using Spectre.Console;

namespace AVL_Ruben_Ibañez.Utilidades
{
    public static class Validaciones
    {
        private static readonly string[] TiposSangreValidos =
            { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };

        public static string LeerTexto(string mensaje, bool permitirVacio = false)
        {
            while (true)
            {
                AnsiConsole.Markup($"[bold cyan]{mensaje}[/] ");
                string? entrada = Console.ReadLine();

                if (!permitirVacio && string.IsNullOrWhiteSpace(entrada))
                {
                    AnsiConsole.MarkupLine("[red]El valor no puede estar vacío. Intente nuevamente.[/]");
                    continue;
                }
                return entrada?.Trim() ?? string.Empty;
            }
        }

        public static int LeerEntero(string mensaje, int minimo = 0, int maximo = 100)
        {
            while (true)
            {
                AnsiConsole.Markup($"[bold cyan]{mensaje}[/] ");
                string? entrada = Console.ReadLine();

                if (int.TryParse(entrada, out int valor) && valor >= minimo && valor <= maximo)
                    return valor;

                AnsiConsole.MarkupLine($"[red]Entrada inválida. Ingrese un número entero entre {minimo} y {maximo}.[/]");
            }
        }

        public static string LeerTipoSangre(string mensaje)
        {
            while (true)
            {
                AnsiConsole.Markup($"[bold cyan]{mensaje}[/] (A+, A-, B+, B-, AB+, AB-, O+, O-): ");
                string? entrada = Console.ReadLine()?.Trim().ToUpperInvariant();

                if (entrada != null && TiposSangreValidos.Contains(entrada))
                    return entrada;

                AnsiConsole.MarkupLine("[red]Tipo de sangre inválido. Valores permitidos: A+, A-, B+, B-, AB+, AB-, O+, O-.[/]");
            }
        }

        public static string LeerCodigoExpediente(string mensaje)
        {
            while (true)
            {
                AnsiConsole.Markup($"[bold cyan]{mensaje}[/] (formato EXP####): ");
                string? entrada = Console.ReadLine()?.Trim().ToUpperInvariant();

                if (entrada != null &&
                    entrada.Length == 7 &&
                    entrada.StartsWith("EXP") &&
                    entrada.Substring(3).All(char.IsDigit))
                {
                    return entrada;
                }

                AnsiConsole.MarkupLine("[red]Formato inválido. Ejemplo válido: EXP0001.[/]");
            }
        }

        public static bool Confirmar(string mensaje)
        {
            AnsiConsole.Markup($"[bold yellow]{mensaje}[/] (S/N): ");
            string? entrada = Console.ReadLine()?.Trim().ToUpperInvariant();
            return entrada == "S" || entrada == "SI";
        }

        public static void PausarYRegresarAlMenu()
        {
            AnsiConsole.MarkupLine("\n[grey]Presione una tecla para continuar...[/]");
            Console.ReadKey(true);
        }
    }
}
