using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class BEClsHabitacion : Entidad
    {
        #region Propiedades del Habitacion
        public string Numero_Habitacion { get; set; }
        public int Piso { get; set; }

        public double ValorNoche { get; set; }
        
        public string Estado { get; set; }
        public int Capacidad { get; set; }
        public bool CamaDoble { get; set; }


        #endregion
        public BEClsHabitacion()
        { }
        public BEClsHabitacion(int id)
        { ID = id; }

        public BEClsHabitacion(string numeroHabitacion, int piso = 0, int capacidad = 0, double valorNoche = 0)
        {
            ValorNoche = valorNoche;
            Numero_Habitacion = numeroHabitacion;
            Piso = piso;
            Capacidad = capacidad;
           
        }



    }
}
