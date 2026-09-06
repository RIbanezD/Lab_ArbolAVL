using AVL_Ruben_Ibañez.Modelos;

namespace AVL_Ruben_Ibañez.Estructuras
{
    public class ArbolAVL
    {
        private NodoAVL? raiz;

        public ArbolAVL()
        {
            raiz = null;
        }

        public bool EstaVacio() => raiz == null;

        // ==================================================================
        //  UTILIDADES DE ALTURA Y BALANCE
        // ==================================================================

        private int Altura(NodoAVL? nodo) => nodo == null ? 0 : nodo.Altura;

        private void ActualizarAltura(NodoAVL nodo)
        {
            nodo.Altura = 1 + Math.Max(Altura(nodo.Izquierdo), Altura(nodo.Derecho));
        }

        private int FactorBalance(NodoAVL? nodo) =>
            nodo == null ? 0 : Altura(nodo.Izquierdo) - Altura(nodo.Derecho);

        public int ObtenerAltura() => Altura(raiz);

        // Rotación simple a la derecha (caso LL)
        private NodoAVL RotarDerecha(NodoAVL y)
        {
            NodoAVL x = y.Izquierdo!;
            NodoAVL? t2 = x.Derecho;

            x.Derecho = y;
            y.Izquierdo = t2;

            ActualizarAltura(y);
            ActualizarAltura(x);

            return x;
        }

        // Rotación simple a la izquierda (caso RR)
        private NodoAVL RotarIzquierda(NodoAVL x)
        {
            NodoAVL y = x.Derecho!;
            NodoAVL? t2 = y.Izquierdo;

            y.Izquierdo = x;
            x.Derecho = t2;

            ActualizarAltura(x);
            ActualizarAltura(y);

            return y;
        }

        // Funcion de balanceo
        private NodoAVL Balancear(NodoAVL nodo)
        {
            ActualizarAltura(nodo);
            int balance = FactorBalance(nodo);

            // Caso Izquierda-Izquierda (LL) -> Rotación simple derecha
            if (balance > 1 && FactorBalance(nodo.Izquierdo) >= 0)
                return RotarDerecha(nodo);

            // Caso Izquierda-Derecha (LR) -> Rotación doble
            if (balance > 1 && FactorBalance(nodo.Izquierdo) < 0)
            {
                nodo.Izquierdo = RotarIzquierda(nodo.Izquierdo!);
                return RotarDerecha(nodo);
            }

            // Caso Derecha-Derecha (RR) -> Rotación simple izquierda
            if (balance < -1 && FactorBalance(nodo.Derecho) <= 0)
                return RotarIzquierda(nodo);

            // Caso Derecha-Izquierda (RL) -> Rotación doble
            if (balance < -1 && FactorBalance(nodo.Derecho) > 0)
            {
                nodo.Derecho = RotarDerecha(nodo.Derecho!);
                return RotarIzquierda(nodo);
            }

            return nodo; // Ya estaba balanceado
        }

        // Inserta un nuevo expediente
        public bool Insertar(Expediente expediente)
        {
            if (Buscar(expediente.NumeroExpediente) != null)
                return false; // Duplicado

            raiz = InsertarRec(raiz, expediente);
            return true;
        }

        private NodoAVL InsertarRec(NodoAVL? nodo, Expediente expediente)
        {
            if (nodo == null)
                return new NodoAVL(expediente);

            int comparacion = string.Compare(expediente.NumeroExpediente, nodo.Dato.NumeroExpediente, StringComparison.OrdinalIgnoreCase);

            if (comparacion < 0)
                nodo.Izquierdo = InsertarRec(nodo.Izquierdo, expediente);
            else if (comparacion > 0)
                nodo.Derecho = InsertarRec(nodo.Derecho, expediente);
            else
                return nodo; // Duplicado (no debería llegar aquí por el chequeo previo)

            return Balancear(nodo);
        }

        // Busquedas
        public Expediente? Buscar(string numeroExpediente)
        {
            return BuscarRec(raiz, numeroExpediente);
        }

        private Expediente? BuscarRec(NodoAVL? nodo, string numeroExpediente)
        {
            if (nodo == null) return null;

            int comparacion = string.Compare(numeroExpediente, nodo.Dato.NumeroExpediente, StringComparison.OrdinalIgnoreCase);

            if (comparacion == 0) return nodo.Dato;
            return comparacion < 0
                ? BuscarRec(nodo.Izquierdo, numeroExpediente)
                : BuscarRec(nodo.Derecho, numeroExpediente);
        }

        public List<Expediente> BuscarPorTipoSangre(string tipoSangre)
        {
            var resultados = new List<Expediente>();
            BuscarPorTipoSangreRec(raiz, tipoSangre.Trim().ToUpperInvariant(), resultados);
            return resultados;
        }

        private void BuscarPorTipoSangreRec(NodoAVL? nodo, string tipoSangre, List<Expediente> resultados)
        {
            if (nodo == null) return;
            BuscarPorTipoSangreRec(nodo.Izquierdo, tipoSangre, resultados);
            if (nodo.Dato.TipoSangre.ToUpperInvariant() == tipoSangre)
                resultados.Add(nodo.Dato);
            BuscarPorTipoSangreRec(nodo.Derecho, tipoSangre, resultados);
        }

        public List<Expediente> BuscarPorEdad(int edadMinima, int edadMaxima)
        {
            var resultados = new List<Expediente>();
            BuscarPorEdadRec(raiz, edadMinima, edadMaxima, resultados);
            return resultados;
        }

        private void BuscarPorEdadRec(NodoAVL? nodo, int edadMin, int edadMax, List<Expediente> resultados)
        {
            if (nodo == null) return;
            BuscarPorEdadRec(nodo.Izquierdo, edadMin, edadMax, resultados);
            if (nodo.Dato.Edad >= edadMin && nodo.Dato.Edad <= edadMax)
                resultados.Add(nodo.Dato);
            BuscarPorEdadRec(nodo.Derecho, edadMin, edadMax, resultados);
        }


        public List<Expediente> BuscarPorNombre(string textoBusqueda)
        {
            var resultados = new List<Expediente>();
            string query = textoBusqueda.Trim().ToLowerInvariant();
            BuscarPorNombreRec(raiz, query, resultados);
            return resultados;
        }

        private void BuscarPorNombreRec(NodoAVL? nodo, string query, List<Expediente> resultados)
        {
            if (nodo == null) return;
            BuscarPorNombreRec(nodo.Izquierdo, query, resultados);

            string nombreCompleto = nodo.Dato.NombrePaciente.ToLowerInvariant();
            bool coincide = nombreCompleto.StartsWith(query) ||
                             nombreCompleto.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                            .Any(palabra => palabra.StartsWith(query));

            if (coincide)
                resultados.Add(nodo.Dato);

            BuscarPorNombreRec(nodo.Derecho, query, resultados);
        }

        // Editar expediente
        public bool Editar(string numeroExpediente, string? nuevoNombre, int? nuevaEdad, string? nuevoTipoSangre)
        {
            Expediente? existente = Buscar(numeroExpediente);
            if (existente == null) return false;

            if (!string.IsNullOrWhiteSpace(nuevoNombre)) existente.NombrePaciente = nuevoNombre;
            if (nuevaEdad.HasValue) existente.Edad = nuevaEdad.Value;
            if (!string.IsNullOrWhiteSpace(nuevoTipoSangre)) existente.TipoSangre = nuevoTipoSangre;

            return true;
        }

        // Eliminar y balancear
        public bool Eliminar(string numeroExpediente)
        {
            if (Buscar(numeroExpediente) == null) return false;
            raiz = EliminarRec(raiz, numeroExpediente);
            return true;
        }

        private NodoAVL? EliminarRec(NodoAVL? nodo, string numeroExpediente)
        {
            if (nodo == null) return null;

            int comparacion = string.Compare(numeroExpediente, nodo.Dato.NumeroExpediente, StringComparison.OrdinalIgnoreCase);

            if (comparacion < 0)
            {
                nodo.Izquierdo = EliminarRec(nodo.Izquierdo, numeroExpediente);
            }
            else if (comparacion > 0)
            {
                nodo.Derecho = EliminarRec(nodo.Derecho, numeroExpediente);
            }
            else
            {
                if (nodo.Izquierdo == null || nodo.Derecho == null)
                {
                    nodo = nodo.Izquierdo ?? nodo.Derecho;
                }
                else
                {
                    // Dos hijos: se reemplaza por el valor mínimo del subárbol derecho
                    NodoAVL sucesor = ObtenerMinimo(nodo.Derecho);
                    nodo.Dato = sucesor.Dato;
                    nodo.Derecho = EliminarRec(nodo.Derecho, sucesor.Dato.NumeroExpediente);
                }
            }

            if (nodo == null) return null; // El árbol quedó vacío en esta rama

            return Balancear(nodo);
        }

        private NodoAVL ObtenerMinimo(NodoAVL nodo)
        {
            NodoAVL actual = nodo;
            while (actual.Izquierdo != null)
                actual = actual.Izquierdo;
            return actual;
        }

        // Recorridos
        public List<Expediente> RecorridoInorden()
        {
            var lista = new List<Expediente>();
            InordenRec(raiz, lista);
            return lista;
        }

        private void InordenRec(NodoAVL? nodo, List<Expediente> lista)
        {
            if (nodo == null) return;
            InordenRec(nodo.Izquierdo, lista);
            lista.Add(nodo.Dato);
            InordenRec(nodo.Derecho, lista);
        }

        public List<Expediente> RecorridoPreorden()
        {
            var lista = new List<Expediente>();
            PreordenRec(raiz, lista);
            return lista;
        }

        private void PreordenRec(NodoAVL? nodo, List<Expediente> lista)
        {
            if (nodo == null) return;
            lista.Add(nodo.Dato);
            PreordenRec(nodo.Izquierdo, lista);
            PreordenRec(nodo.Derecho, lista);
        }

        public List<Expediente> RecorridoPostorden()
        {
            var lista = new List<Expediente>();
            PostordenRec(raiz, lista);
            return lista;
        }

        private void PostordenRec(NodoAVL? nodo, List<Expediente> lista)
        {
            if (nodo == null) return;
            PostordenRec(nodo.Izquierdo, lista);
            PostordenRec(nodo.Derecho, lista);
            lista.Add(nodo.Dato);
        }

        // Generacion automatica de correlativos
        public string GenerarSiguienteCodigo()
        {
            int mayor = 0;
            foreach (var expediente in RecorridoInorden())
            {
                string numeroTexto = expediente.NumeroExpediente.Replace("EXP", "", StringComparison.OrdinalIgnoreCase);
                if (int.TryParse(numeroTexto, out int valor) && valor > mayor)
                    mayor = valor;
            }
            return $"EXP{mayor + 1:D4}";
        }

        public int Contar() => RecorridoInorden().Count;
    }
}
