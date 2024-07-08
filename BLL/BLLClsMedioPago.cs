using Abstraccion;
using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class BLLClsMedioPago : IGestor<BEClsMedioPago>
    {
        DALClsMedioPago DALMedioPago;
        public BLLClsMedioPago()
        {
            DALMedioPago = new DALClsMedioPago();


        }

        public bool Baja(BEClsMedioPago Objeto)
        {
            return DALMedioPago.Baja(Objeto);
        }

        public bool Guardar(BEClsMedioPago Objeto)
        {
            return DALMedioPago.Guardar(Objeto);
        }

        public BEClsMedioPago ListarObjeto(BEClsMedioPago Objeto)
        {
            return DALMedioPago.ListarObjeto(Objeto);
        }

        public List<BEClsMedioPago> ListarTodo()
        {
            return DALMedioPago.ListarTodo();
        }
    }
}
