using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLPatente : BLLComponente
    {
        //obtiene todos los permisos patentes

        public override IList<BEComponente> Hijos
        {
            get
            {
                return new List<BEComponente>();
            }

        }

        //no agrega porque las patentes son estaticas
        public override void AgregarHijo(BEComponente c)
        {

        }

        //no se borran porque las patentes son estaticas
        public override void VaciarHijos()
        {

        }
    }
}
