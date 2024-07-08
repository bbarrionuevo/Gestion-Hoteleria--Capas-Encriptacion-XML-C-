using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstraccion;
using BE;
using DAL;

namespace BLL
{
    public class BLLClsReserva : IGestor<BEClsReserva>
    {
        DALClsReserva DALReserva;
        BEClsHotel bEClsHotel;
        BLLClsHabitacion BLLHabitacion;
        public BLLClsReserva()
        {
            DALReserva = new DALClsReserva();
            bEClsHotel = new BEClsHotel();
            BLLHabitacion = new BLLClsHabitacion(); 
        }


        public bool Guardar(BEClsReserva BEreserva)
        {

            if (BEreserva.Habitacion != null || BEreserva.Cliente != null)
            {
                    
                    BEreserva.CalcularImporte();
                    BEreserva.Cant_Inquilinos = BEreserva.Habitacion.Capacidad;

                    DALReserva.Guardar(BEreserva);

                    // Devuelve true para indicar que la reserva se realizó con éxito
                    return true;
            }
             else { return false; }
           
        }
        public BEClsReserva ListarObjeto(BEClsReserva Objeto)
        { 

            return DALReserva.ListarObjeto(Objeto);    
        }

        public List<BEClsReserva> ListarTodo()
        {

            return DALReserva.ListarTodo() ;
        }

        

        public List<BEClsReserva> RetornaListaReservasDeClientes(BEClsCliente BEcliente)
        {
            return DALReserva.ObtenerReservasDelCliente(BEcliente);
        }
        public object RetornaListaReservasDeClientesModificada(BEClsCliente BEcliente)
        {



            return (from r in DALReserva.ObtenerReservasDelCliente(BEcliente) select new { Num_De_Reserva = r.ID, Estado = r.Estado, ID_De_Habitacion = r.Habitacion.ID, CheckIn = r.FechaCheckIn, CheckOut = r.FechaCheckOut, Importe = r.Importe,Abonada= r.Abonada }).ToArray();

        }

        public bool Baja(BEClsReserva BEreserva)
        {
            

                return DALReserva.Baja(BEreserva);

            
        }

        
    }
}
