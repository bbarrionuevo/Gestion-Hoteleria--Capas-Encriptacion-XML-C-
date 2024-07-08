using Abstraccion;
using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLClsPago : IGestor<BEClsPago>
    {
        DALClsPago DALPago;
        BEClsPago BEPago;
        public BLLClsPago() 
        {
            DALPago = new DALClsPago();
            BEPago = new BEClsPago();

        }
        public bool Baja(BEClsPago Objeto)
        {
            return DALPago.Baja(Objeto);
        }

        public bool Guardar(BEClsPago Objeto)
        {
            return DALPago.Guardar(Objeto);
        }

        public BEClsPago ListarObjeto(BEClsPago Objeto)
        {
            return DALPago.ListarObjeto(Objeto);
        }

        public List<BEClsPago> ListarTodo()
        {
            return DALPago.ListarTodo();
        }
        public List<BEClsPago> ObtenerPagosPorCliente(BEClsCliente cliente)
        {

            return DALPago.ObtenerPagosPorCliente(cliente);
        }
    }
}
