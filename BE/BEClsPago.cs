using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class BEClsPago : Entidad
    {

       
        public BEClsCliente cliente { get; set; }
        public BEClsReserva reserva { get; set; }
        public BEClsMedioPago medioPago { get; set; }
        public DateTime fecha { get; set; }
        public double importe { get; set; }

        public BEClsPago() { }
        public BEClsPago(int id)
        {
            ID = id;

        }
    }
}
