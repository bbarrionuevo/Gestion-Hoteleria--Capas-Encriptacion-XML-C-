using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class BEClsMedioPago : Entidad
    {
        public string tipo { get; set; }
        public string titular { get; set; }
        public string numero { get; set; }
        public int vencimiento { get; set; }

        public int cuotas { get; set; }

        public BEClsMedioPago() { }
        public BEClsMedioPago(int id)
        {
            ID = id;

        }
    }
}
