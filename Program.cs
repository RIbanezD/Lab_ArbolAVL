using AVL_Ruben_Ibañez.Estructuras;
using AVL_Ruben_Ibañez.Modelos;
using AVL_Ruben_Ibañez.Servicios;
using AVL_Ruben_Ibañez.Utilidades;
using Spectre.Console;

namespace AVL_Ruben_Ibañez
{
    public class Program
    {
        private static readonly ArbolAVL arbol = new ArbolAVL();

        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            MostrarBienvenida();
            AutoImportar();

            bool salir = false;
            while (!salir)
            {
                MostrarEncabezado();
                MostrarMenuPrincipal();

                string opcion = Validaciones.LeerTexto("Seleccione una opción:", permitirVacio: true);

                switch (opcion)
                {
                    case "1": RegistrarExpediente(); break;
                    case "2": BuscarExpediente(); break;
                    case "3": EditarExpediente(); break;
                    case "4": EliminarExpediente(); break;
                    case "5": MenuRecorridos(); break;
                    case "6": MostrarAlturaArbol(); break;
                    case "7": GuardarDatos(); break;
                    case "8": ImportarDatos(); break;
                    case "9":
                        salir = true;
                        break;
                    default:
                        AnsiConsole.MarkupLine("[red]Opción inválida. Presione una tecla para continuar...[/]");
                        Console.ReadKey(true);
                        break;
                }
            }

            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Gracias por preferirnos!").Centered().Color(Color.SkyBlue1));
            AnsiConsole.MarkupLine("[grey]Cerrando programa...[/]");
        }

        // Menú
        private static void MostrarBienvenida()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Hospital San Gabriel").Centered().Color(Color.Red3_1));
            AnsiConsole.MarkupLine("[bold grey]Sistema de Gestión de Expedientes Médicos — Árbol AVL[/]");
            AnsiConsole.WriteLine();
            Validaciones.PausarYRegresarAlMenu();
        }

        private static void MostrarEncabezado()
        {
            AnsiConsole.Clear();
            var regla = new Rule("[bold cyan]SISTEMA DE GESTIÓN DE EXPEDIENTES MÉDICOS[/]")
            {
                Justification = Justify.Center,
                Style = Style.Parse("cyan")
            };
            AnsiConsole.Write(regla);
            AnsiConsole.WriteLine();
        }

        private static void MostrarMenuPrincipal()
        {
            var panel = new Panel(
                "[green][[1]][/] Registrar Expediente\n" +
                "[green][[2]][/] Buscar Expediente\n" +
                "[green][[3]][/] Editar Expediente\n" +
                "[green][[4]][/] Eliminar Expediente\n" +
                "[green][[5]][/] Recorridos\n" +
                "[green][[6]][/] Mostrar Altura del Árbol\n" +
                "[green][[7]][/] Guardar Datos\n" +
                "[green][[8]][/] Importar Datos\n" +
                "[green][[9]][/] Salir")
            {
                Header = new PanelHeader(" Menú Principal "),
                Border = BoxBorder.Rounded,
                BorderStyle = Style.Parse("cyan")
            };
            AnsiConsole.Write(panel);
            AnsiConsole.WriteLine();
        }

        // Registrar Expediente
        private static void RegistrarExpediente()
        {
            MostrarEncabezado();
            AnsiConsole.Write(new Rule("[yellow]Registro de Expediente[/]").LeftJustified());
            AnsiConsole.WriteLine();

            string codigoSugerido = arbol.SiguienteCorrelativo();
            AnsiConsole.MarkupLine($"[grey]Código sugerido: [bold]{codigoSugerido}[/][/]");
            bool usarSugerido = Validaciones.Confirmar("¿Desea usar el código sugerido?");

            string numeroExpediente = usarSugerido
                ? codigoSugerido
                : Validaciones.LeerCodigoExpediente("Número de Expediente:");

            if (arbol.Buscar(numeroExpediente) != null)
            {
                AnsiConsole.MarkupLine($"[red]Ya existe un expediente con el código {numeroExpediente}.[/]");
                Validaciones.PausarYRegresarAlMenu();
                return;
            }

            string nombre = Validaciones.LeerTexto("Nombre del Paciente:");
            int edad = Validaciones.LeerEntero("Edad:", 0, 100);
            string tipoSangre = Validaciones.LeerTipoSangre("Tipo de Sangre:");

            var expediente = new Expediente(numeroExpediente, nombre, edad, tipoSangre);
            bool insertado = arbol.Insertar(expediente);

            AnsiConsole.WriteLine();
            if (insertado)
            {
                var tabla = new Table().Border(TableBorder.Rounded).BorderColor(Color.Green);
                tabla.AddColumn("Campo");
                tabla.AddColumn("Valor");
                tabla.AddRow("Número de Expediente", numeroExpediente);
                tabla.AddRow("Nombre del Paciente", nombre);
                tabla.AddRow("Edad", $"{edad} años");
                tabla.AddRow("Tipo de Sangre", tipoSangre);
                AnsiConsole.MarkupLine("[bold green]Expediente registrado exitosamente.[/]");
                AnsiConsole.Write(tabla);
            }
            else
            {
                AnsiConsole.MarkupLine("[red]No se pudo registrar el expediente (código duplicado).[/]");
            }

            Validaciones.PausarYRegresarAlMenu();
        }

        // Buscar Expediente
        private static void BuscarExpediente()
        {
            bool volver = false;
            while (!volver)
            {
                MostrarEncabezado();
                var panel = new Panel(
                    "[green][[1]][/] Por Número de Expediente\n" +
                    "[green][[2]][/] Por Tipo de Sangre\n" +
                    "[green][[3]][/] Por Edad (o rango de edad)\n" +
                    "[green][[4]][/] Por Nombre y/o Apellido\n" +
                    "[green][[5]][/] Volver al Menú Principal")
                {
                    Header = new PanelHeader(" Búsqueda de Expediente "),
                    Border = BoxBorder.Rounded,
                    BorderStyle = Style.Parse("cyan")
                };
                AnsiConsole.Write(panel);
                AnsiConsole.WriteLine();

                string opcion = Validaciones.LeerTexto("Seleccione un criterio de búsqueda:", permitirVacio: true);

                switch (opcion)
                {
                    case "1": BuscarPorNumero(); break;
                    case "2": BuscarPorTipoSangre(); break;
                    case "3": BuscarPorEdad(); break;
                    case "4": BuscarPorNombre(); break;
                    case "5": volver = true; break;
                    default:
                        AnsiConsole.MarkupLine("[red]Opción inválida.[/]");
                        Console.ReadKey(true);
                        break;
                }
            }
        }

        private static void BuscarPorNumero()
        {
            MostrarEncabezado();
            AnsiConsole.Write(new Rule("[yellow]Búsqueda por Número de Expediente[/]").LeftJustified());
            AnsiConsole.WriteLine();

            string numeroExpediente = Validaciones.LeerCodigoExpediente("Ingrese el número de expediente:");
            Expediente? resultado = arbol.Buscar(numeroExpediente);

            AnsiConsole.WriteLine();
            if (resultado != null)
                MostrarTablaExpedientes(new List<Expediente> { resultado }, "Resultado de la Búsqueda");
            else
                AnsiConsole.MarkupLine($"[red]No se encontró ningún expediente con el número {numeroExpediente}.[/]");

            Validaciones.PausarYRegresarAlMenu();
        }

        private static void BuscarPorTipoSangre()
        {
            MostrarEncabezado();
            AnsiConsole.Write(new Rule("[yellow]Búsqueda por Tipo de Sangre[/]").LeftJustified());
            AnsiConsole.WriteLine();

            string tipoSangre = Validaciones.LeerTipoSangre("Tipo de sangre a buscar:");
            List<Expediente> resultados = arbol.BuscarPorTipoSangre(tipoSangre);

            AnsiConsole.WriteLine();
            MostrarTablaExpedientes(resultados, $"Pacientes con Tipo de Sangre {tipoSangre}");
            Validaciones.PausarYRegresarAlMenu();
        }

        private static void BuscarPorEdad()
        {
            MostrarEncabezado();
            AnsiConsole.Write(new Rule("[yellow]Búsqueda por Edad[/]").LeftJustified());
            AnsiConsole.WriteLine();

            bool esRango = Validaciones.Confirmar("¿Desea buscar por un rango de edades?");

            int edadMin, edadMax;
            if (esRango)
            {
                edadMin = Validaciones.LeerEntero("Edad mínima:", 0, 100);
                edadMax = Validaciones.LeerEntero("Edad máxima:", edadMin, 100);
            }
            else
            {
                edadMin = edadMax = Validaciones.LeerEntero("Edad exacta:", 0, 100);
            }

            List<Expediente> resultados = arbol.BuscarPorEdad(edadMin, edadMax);

            AnsiConsole.WriteLine();
            string titulo = esRango ? $"Pacientes entre {edadMin} y {edadMax} años" : $"Pacientes de {edadMin} años";
            MostrarTablaExpedientes(resultados, titulo);
            Validaciones.PausarYRegresarAlMenu();
        }

        private static void BuscarPorNombre()
        {
            MostrarEncabezado();
            AnsiConsole.Write(new Rule("[yellow]Búsqueda por Nombre y/o Apellido[/]").LeftJustified());
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[grey]Puede ingresar el nombre completo, solo el nombre, solo el apellido,\no la letra inicial para ver coincidencias.[/]");
            AnsiConsole.WriteLine();

            string texto = Validaciones.LeerTexto("Nombre/apellido o letra inicial a buscar:");
            List<Expediente> resultados = arbol.BuscarPorNombre(texto);

            AnsiConsole.WriteLine();
            MostrarTablaExpedientes(resultados, $"Coincidencias para \"{texto}\"");
            Validaciones.PausarYRegresarAlMenu();
        }

        private static void MostrarTablaExpedientes(List<Expediente> expedientes, string titulo)
        {
            if (expedientes.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No se encontraron expedientes que coincidan con el criterio ingresado.[/]");
                return;
            }

            var tabla = new Table().Border(TableBorder.Rounded).BorderColor(Color.Green);
            tabla.Title = new TableTitle($"[bold]{titulo}[/]");
            tabla.AddColumn("Expediente");
            tabla.AddColumn("Nombre del Paciente");
            tabla.AddColumn("Edad");
            tabla.AddColumn("Tipo de Sangre");

            foreach (var exp in expedientes)
                tabla.AddRow(exp.NumeroExpediente, exp.NombrePaciente, exp.Edad.ToString(), exp.TipoSangre);

            AnsiConsole.Write(tabla);
            AnsiConsole.MarkupLine($"[grey]Total de resultados: {expedientes.Count}[/]");
        }

        // Editar Expediente
        private static void EditarExpediente()
        {
            MostrarEncabezado();
            AnsiConsole.Write(new Rule("[yellow]Editar Expediente[/]").LeftJustified());
            AnsiConsole.WriteLine();

            string numeroExpediente = Validaciones.LeerCodigoExpediente("Ingrese el número de expediente a editar:");
            Expediente? existente = arbol.Buscar(numeroExpediente);

            if (existente == null)
            {
                AnsiConsole.MarkupLine($"[red]No existe ningún expediente con el número {numeroExpediente}.[/]");
                Validaciones.PausarYRegresarAlMenu();
                return;
            }

            AnsiConsole.WriteLine();
            MostrarTablaExpedientes(new List<Expediente> { existente }, "Datos Actuales");
            AnsiConsole.WriteLine();

            var panel = new Panel(
                "[green][[1]][/] Nombre del Paciente\n" +
                "[green][[2]][/] Edad\n" +
                "[green][[3]][/] Tipo de Sangre\n" +
                "[green][[4]][/] Cancelar")
            {
                Header = new PanelHeader(" ¿Qué desea editar? "),
                Border = BoxBorder.Rounded,
                BorderStyle = Style.Parse("cyan")
            };
            AnsiConsole.Write(panel);

            string opcion = Validaciones.LeerTexto("Seleccione una opción:", permitirVacio: true);

            switch (opcion)
            {
                case "1":
                    string nuevoNombre = Validaciones.LeerTexto("Nuevo nombre del paciente:");
                    arbol.Editar(numeroExpediente, nuevoNombre, null, null);
                    AnsiConsole.MarkupLine("[green]Nombre actualizado correctamente.[/]");
                    break;
                case "2":
                    int nuevaEdad = Validaciones.LeerEntero("Nueva edad:", 0, 100);
                    arbol.Editar(numeroExpediente, null, nuevaEdad, null);
                    AnsiConsole.MarkupLine("[green]Edad actualizada correctamente.[/]");
                    break;
                case "3":
                    string nuevoTipo = Validaciones.LeerTipoSangre("Nuevo tipo de sangre:");
                    arbol.Editar(numeroExpediente, null, null, nuevoTipo);
                    AnsiConsole.MarkupLine("[green]Tipo de sangre actualizado correctamente.[/]");
                    break;
                case "4":
                    AnsiConsole.MarkupLine("[grey]Edición cancelada.[/]");
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Opción inválida.[/]");
                    break;
            }

            Validaciones.PausarYRegresarAlMenu();
        }

        // Eliminar Expediente
        private static void EliminarExpediente()
        {
            MostrarEncabezado();
            AnsiConsole.Write(new Rule("[yellow]Eliminar Expediente[/]").LeftJustified());
            AnsiConsole.WriteLine();

            string numeroExpediente = Validaciones.LeerCodigoExpediente("Ingrese el número de expediente a eliminar:");
            Expediente? existente = arbol.Buscar(numeroExpediente);

            if (existente == null)
            {
                AnsiConsole.MarkupLine($"[red]No existe ningún expediente con el número {numeroExpediente}.[/]");
                Validaciones.PausarYRegresarAlMenu();
                return;
            }

            AnsiConsole.WriteLine();
            MostrarTablaExpedientes(new List<Expediente> { existente }, "Expediente a Eliminar");
            AnsiConsole.WriteLine();

            bool confirmar = Validaciones.Confirmar($"¿Está seguro que desea eliminar el expediente {numeroExpediente}?");

            if (confirmar)
            {
                arbol.Eliminar(numeroExpediente);
                AnsiConsole.MarkupLine("[green]Expediente eliminado exitosamente. El árbol se ha rebalanceado automáticamente.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[grey]Eliminación cancelada.[/]");
            }

            Validaciones.PausarYRegresarAlMenu();
        }

        // Recorridos
        private static void MenuRecorridos()
        {
            bool volver = false;
            while (!volver)
            {
                MostrarEncabezado();
                var panel = new Panel(
                    "[green][[1]][/] Recorrido Inorden (Izquierda - Raíz - Derecha)\n" +
                    "[green][[2]][/] Recorrido Preorden (Raíz - Izquierda - Derecha)\n" +
                    "[green][[3]][/] Recorrido Postorden (Izquierda - Derecha - Raíz)\n" +
                    "[green][[4]][/] Volver al Menú Principal")
                {
                    Header = new PanelHeader(" Recorridos del Árbol "),
                    Border = BoxBorder.Rounded,
                    BorderStyle = Style.Parse("cyan")
                };
                AnsiConsole.Write(panel);
                AnsiConsole.WriteLine();

                string opcion = Validaciones.LeerTexto("Seleccione un recorrido:", permitirVacio: true);

                switch (opcion)
                {
                    case "1": MostrarRecorrido("RECORRIDO INORDEN", arbol.RecorridoInorden()); break;
                    case "2": MostrarRecorrido("RECORRIDO PREORDEN", arbol.RecorridoPreorden()); break;
                    case "3": MostrarRecorrido("RECORRIDO POSTORDEN", arbol.RecorridoPostorden()); break;
                    case "4": volver = true; break;
                    default:
                        AnsiConsole.MarkupLine("[red]Opción inválida.[/]");
                        Console.ReadKey(true);
                        break;
                }
            }
        }

        private static void MostrarRecorrido(string titulo, List<Expediente> resultado)
        {
            MostrarEncabezado();
            AnsiConsole.Write(new Rule($"[yellow]{titulo}[/]").LeftJustified());
            AnsiConsole.WriteLine();

            if (arbol.EstaVacio())
            {
                AnsiConsole.MarkupLine("[red]El árbol se encuentra vacío. No hay expedientes registrados.[/]");
            }
            else
            {
                var tabla = new Table().Border(TableBorder.Rounded).BorderColor(Color.Cyan1);
                tabla.AddColumn("#");
                tabla.AddColumn("Expediente");
                tabla.AddColumn("Nombre del Paciente");
                tabla.AddColumn("Edad");
                tabla.AddColumn("Tipo de Sangre");

                for (int i = 0; i < resultado.Count; i++)
                {
                    var exp = resultado[i];
                    tabla.AddRow((i + 1).ToString(), exp.NumeroExpediente, exp.NombrePaciente, exp.Edad.ToString(), exp.TipoSangre);
                }

                AnsiConsole.Write(tabla);
            }

            Validaciones.PausarYRegresarAlMenu();
        }

        // Altura
        private static void MostrarAlturaArbol()
        {
            MostrarEncabezado();
            AnsiConsole.Write(new Rule("[yellow]Altura del Árbol[/]").LeftJustified());
            AnsiConsole.WriteLine();

            int altura = arbol.ObtenerAltura();
            AnsiConsole.MarkupLine($"[bold]La altura actual del árbol es:[/] [bold cyan]{altura}[/]");
            AnsiConsole.MarkupLine($"[grey]Total de expedientes almacenados: {arbol.Contar()}[/]");

            Validaciones.PausarYRegresarAlMenu();
        }

        // Exportar Datos
        private static void GuardarDatos()
        {
            MostrarEncabezado();
            AnsiConsole.Write(new Rule("[yellow]Guardar Datos[/]").LeftJustified());
            AnsiConsole.WriteLine();

            if (arbol.EstaVacio())
            {
                AnsiConsole.MarkupLine("[red]No hay expedientes registrados para guardar.[/]");
                Validaciones.PausarYRegresarAlMenu();
                return;
            }

            List<Expediente> todos = arbol.RecorridoInorden();
            string ruta = GestorArchivos.Guardar(todos);

            AnsiConsole.MarkupLine($"[green]Se guardaron {todos.Count} expedientes exitosamente.[/]");
            AnsiConsole.MarkupLine($"[grey]Archivo: {ruta}[/]");

            Validaciones.PausarYRegresarAlMenu();
        }

        // Importar datos
        private static void ImportarDatos()
        {
            MostrarEncabezado();
            AnsiConsole.Write(new Rule("[yellow]Importar Datos[/]").LeftJustified());
            AnsiConsole.WriteLine();

            if (!GestorArchivos.ExisteArchivo())
            {
                AnsiConsole.MarkupLine("[red]No se encontró el archivo 'expedientes.csv' en la carpeta del programa.[/]");
                Validaciones.PausarYRegresarAlMenu();
                return;
            }

            List<Expediente> importados = GestorArchivos.Importar(out List<string> errores);

            int agregados = 0, duplicados = 0;
            foreach (var exp in importados)
            {
                if (arbol.Insertar(exp))
                    agregados++;
                else
                    duplicados++;
            }

            AnsiConsole.MarkupLine($"[green]Importación finalizada: {agregados} expedientes agregados.[/]");
            if (duplicados > 0)
                AnsiConsole.MarkupLine($"[yellow]{duplicados} expedientes fueron omitidos por tener un código ya existente.[/]");
            if (errores.Count > 0)
            {
                AnsiConsole.MarkupLine($"[red]{errores.Count} líneas no se pudieron procesar:[/]");
                foreach (var error in errores.Take(10))
                    AnsiConsole.MarkupLine($"  [red]- {error}[/]");
            }

            Validaciones.PausarYRegresarAlMenu();
        }

        // Autolectura de archivo exportado
        private static void AutoImportar()
        {
            if (!GestorArchivos.ExisteArchivo()) return;

            MostrarEncabezado();
            AnsiConsole.MarkupLine("[yellow]Se detectó un archivo de datos guardado anteriormente (expedientes.csv).[/]");
            bool importar = Validaciones.Confirmar("¿Desea importar los expedientes guardados antes de continuar?");

            if (!importar) return;

            List<Expediente> importados = GestorArchivos.Importar(out List<string> errores);
            int agregados = importados.Count(arbol.Insertar);

            AnsiConsole.MarkupLine($"[green]Se importaron {agregados} expedientes automáticamente.[/]");
            Validaciones.PausarYRegresarAlMenu();
        }
    }
}
