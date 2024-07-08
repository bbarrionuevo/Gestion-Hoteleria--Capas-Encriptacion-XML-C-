using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLFamilia : BLLComponente
    {
        BEFamilia oBEFamilia;
        public BLLFamilia()
        {
            oBEFamilia=new BEFamilia();
        }
        //Traigo al listta de componentes.
        public override IList<BEComponente> Hijos
        {
            get
            {
                return oBEFamilia._hijos.ToArray();
            }

        }

        //método agregar hijo, le agrego el componente a la lista

        public override void AgregarHijo(BEComponente c)
        {
            oBEFamilia._hijos.Add(c);
        }
        //método Quitar hijo, le agrego el componente a la lista

        public void QuitarHijo(BEComponente c)
        {
            var itemToRemove = oBEFamilia._hijos.FirstOrDefault(item => item.ID == c.ID);
            if (itemToRemove != null)
            {
                oBEFamilia._hijos.Remove(itemToRemove);
            }
        }


        //vaciar  
        public override void VaciarHijos()
        {
             oBEFamilia._hijos = new List<BEComponente>();
        }
    }
}
