using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;
using Abstraccion;

namespace BLL
{
    public class BLLClsHabitacion : IGestor<BEClsHabitacion>
    {

        DALClsHabitacion DALHabitacion;
        DALClsReserva DALreserva;
        BEClsHotel BEHotel;
        public BLLClsHabitacion()
        {
            DALHabitacion = new DALClsHabitacion();
            BEHotel = new BEClsHotel();
            DALreserva = new DALClsReserva();
        }


        
        public List<BEClsHabitacion> ListarHabitacionesDisponibles(BEClsReserva reserva)
        {
            // Obtén todas las habitaciones
            List<BEClsHabitacion> todasLasHabitaciones = DALHabitacion.ListarTodo();

            // Filtra las habitaciones para encontrar las que están disponibles
            List<BEClsHabitacion> habitacionesDisponibles = todasLasHabitaciones
                .Where(h => EsDisponible(h, reserva))
                .ToList();

            return habitacionesDisponibles;
        }
        // Método para verificar la disponibilidad de la habitación para un rango de fechas
        public bool EsDisponible(BEClsHabitacion BEHabitacion, BEClsReserva BEReserva)
        {
            // Verifica si la habitación está ocupada en algún momento durante el rango de fechas
            // y si la reserva no está cancelada
            return !DALreserva.ObtenerReservasDeHabitacion(BEHabitacion).Any(r => ((BEReserva.FechaCheckIn >= r.FechaCheckIn && BEReserva.FechaCheckIn < r.FechaCheckOut) ||
                                       (BEReserva.FechaCheckOut > r.FechaCheckIn && BEReserva.FechaCheckOut <= r.FechaCheckOut)) && r.Estado != "Cancelada");
        }

        public virtual decimal CalcularImporte( int CantidadDias)
        {
            // Implementación genérica para el cálculo del importe 
            return 0;
        }

        public bool Guardar(BEClsHabitacion Objeto)
        {
            return DALHabitacion.Guardar(Objeto);
        }

        public bool Baja(BEClsHabitacion Objeto)
        {
            return DALHabitacion.Baja(Objeto);
        }

        public List<BEClsHabitacion> ListarTodo()
        {
            return DALHabitacion.ListarTodo();
        }

        public BEClsHabitacion ListarObjeto(BEClsHabitacion Objeto)
        {
            throw new NotImplementedException();
        }
    }
}
