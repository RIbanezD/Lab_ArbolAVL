using AVL_Ruben_Ibañez.Modelos;

namespace AVL_Ruben_Ibañez.Estructuras
{
    public class NodoAVL
    {
        public Expediente Dato { get; set; }
        public NodoAVL? Izquierdo { get; set; }
        public NodoAVL? Derecho { get; set; }
        public int Altura { get; set; }

        public NodoAVL(Expediente dato)
        {
            Dato = dato;
            Izquierdo = null;
            Derecho = null;
            Altura = 1;
        }
    }
}
