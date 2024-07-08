using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    //familia es lo mismo que rol
    public class BEFamilia : BEComponente

    {   //método para traer todas las flia o roles o grupos
        //la relacion 1 a M del componente con Flia
        private IList<BEComponente> _hijos;

        //instancio la IList<BEComponente en el constructor
        public BEFamilia()
        {
            _hijos = new List<BEComponente>();
        }

        //Traigo al listta de componentes.
        public override IList<BEComponente> Hijos
        {
            get
            {
                return _hijos.ToArray();
            }

        }

        //método agregar hijo, le agrego el componente a la lista
        
        public override void AgregarHijo(BEComponente c)
        {
            _hijos.Add(c);
        }
        //método Quitar hijo, le agrego el componente a la lista

        public void QuitarHijo(BEComponente c)
        {
            var itemToRemove = _hijos.FirstOrDefault(item => item.ID == c.ID);
            if (itemToRemove != null)
            {
                _hijos.Remove(itemToRemove);
            }
        }


        //vaciar  
        public override void VaciarHijos()
        {
            _hijos = new List<BEComponente>();
        }


    }
}
