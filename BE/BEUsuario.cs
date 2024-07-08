using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class BEUsuario : BEClsPersona
    {
        public string Usuario { get; set; }
        public string Contraseña { get; set; }

        public BEComponente Rol { get; set; }

        public BEUsuario()
        {

        }
        public BEUsuario(int id)
        {
            ID = id;
        }


        public override string ToString()
        {
            return Nombre;
        }
    }
}
