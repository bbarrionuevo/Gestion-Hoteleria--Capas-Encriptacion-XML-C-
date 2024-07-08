using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
     public abstract class BLLComponente
    {
        BEComponente oBEComponente;

        
        //método para obtener todos los hijos
        //IList Iterface predefinida de .NET
        public abstract IList<BEComponente> Hijos { get; }

        //método para agregar hijos
        public abstract void AgregarHijo(BEComponente c);

        //método para vacias hijos
        public abstract void VaciarHijos();

        //propiedad Permisos, los permisos simples son estaticos.
        public BETipoPermiso Permiso { get; set; }

        public override string ToString()
        {
            return oBEComponente.Nombre;
        }
    }
}
