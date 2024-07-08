using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using Abstraccion;
using DAL;


namespace BLL
{
    public class BLLClsCliente : IGestor<BEClsCliente>
    {

        public BLLClsCliente() 
        {
            DALCliente = new DALClsCliente();
            BLLReserva = new BLLClsReserva();
        }
        DALClsCliente DALCliente;
        BLLClsReserva BLLReserva;





        

        public bool Guardar(BEClsCliente BEcliente)
        {
            return DALCliente.Guardar(BEcliente);
        }

        public bool Baja(BEClsCliente BECliente)
        {
            // Obtener la lista de reservas del cliente
            List<BEClsReserva> reservas = BLLReserva.RetornaListaReservasDeClientes(BECliente);

            // Verificar si alguna de las reservas está en estado "Pendiente" o "En Curso"
            foreach (BEClsReserva reserva in reservas)
            {
                if (reserva.Estado == "Pendiente" || reserva.Estado == "En Curso")
                {
                    throw new Exception("El Cliente cuenta con reservas pendientes o en curso, Cancele las mismas antes de realizar Baja de cliente");
                }
            }

            // Si ninguna de las reservas está en estado "Pendiente" o "En Curso", proceder a eliminar el cliente
            if (!DALCliente.Baja(BECliente))
            {
                throw new Exception("Error al intentar eliminar el cliente");
            }

            return true;
        }

        public List<BEClsCliente> ListarTodo()
        {
            return DALCliente.ListarTodo();
        }

        public BEClsCliente ListarObjeto(BEClsCliente Objeto)
        {
            return DALCliente.ListarObjeto(Objeto);
        }
        public BEClsCliente BuscarCliente_DNI(BEClsCliente Objeto)
        {
            return DALCliente.BuscarCliente_DNI(Objeto);
        }

        
    }
}
