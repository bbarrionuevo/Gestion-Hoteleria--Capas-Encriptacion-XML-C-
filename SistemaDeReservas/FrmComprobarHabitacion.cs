using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaDeReservas
{
    
    public partial class FrmComprobarHabitacion : Form
    {
        BLLClsReserva BLLreserva;
        BEClsHotel BEhotel;
        BEClsCliente BEcliente;
        BEClsReserva BEreserva; // Esta es la reserva que se compartirá
        BLLClsHabitacion BLLHabitacion;

        public FrmComprobarHabitacion() {
            BLLreserva = new BLLClsReserva();
            BEhotel = new BEClsHotel();
            BLLHabitacion = new BLLClsHabitacion();
            BEreserva=new BEClsReserva();
            InitializeComponent(); }
        // constructor acepta una reserva
        public FrmComprobarHabitacion(BEClsReserva reserva)
        {
            InitializeComponent();

            BLLreserva = new BLLClsReserva();
            BEhotel = new BEClsHotel();
            BLLHabitacion = new BLLClsHabitacion();
            this.BEreserva = reserva; // Asigna la reserva pasada a la reserva de esta clase
        }
        public event Action HabitacionSeleccionada;

        public void Mostrar(DataGridView pDGV, object p0)
        {
            pDGV.DataSource = null; pDGV.DataSource = p0;
        }


        //Selecciona habitacion
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView2.SelectedRows.Count == 0)
                    throw new Exception("Selecciona una Habitacion");
                // Obtiene el objeto BEClsHabitacion de la fila seleccionada
                BEClsHabitacion Habitacion = (BEClsHabitacion)dataGridView2.SelectedRows[0].DataBoundItem;
                BEreserva.Habitacion = Habitacion; // Modifica la reserva compartida
            
                // Dispara el evento
                HabitacionSeleccionada?.Invoke();
                MessageBox.Show("Habitacion Seleccionada");

                this.Close();

            }

            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        //Establece las fechas de entrada y salida y busca habitaciones disponibles
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                // Obtiene las fechas de check-in y check-out desde los controles DateTimePicker
                DateTime fechaCheckIn = dtpCheckIn.Value;
                DateTime fechaCheckOut = dtpCheckOut.Value;
                int cantidadDias = Convert.ToInt32((fechaCheckOut - fechaCheckIn).TotalDays); // Calcula la cantidad de días
                if (cantidadDias <= 0) throw new Exception("la reserva debe ser de minimo 1 noche");
                BEreserva.FechaCheckIn = fechaCheckIn;
                BEreserva.FechaCheckOut = fechaCheckOut;
                BEreserva.Cant_Dias = cantidadDias;
                Mostrar(dataGridView2, BLLHabitacion.ListarHabitacionesDisponibles(BEreserva));
            }

            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    
    }
}
