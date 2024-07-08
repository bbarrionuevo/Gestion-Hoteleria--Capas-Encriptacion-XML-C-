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
    public partial class FrmCheckInCliente : Form
    {
        BEClsCliente BEcliente;
        BLLClsReserva BLLreserva;
        BEClsReserva BEreserva;
        BLLClsHabitacion BLLHabitacion;
        public FrmCheckInCliente()
        {
            BLLreserva = new BLLClsReserva();
            BLLHabitacion=new BLLClsHabitacion();
            InitializeComponent();
            BEcliente=new BEClsCliente();
            BEreserva = new BEClsReserva();
        }
        public void Mostrar(DataGridView pDGV, object p0)
        {
            pDGV.DataSource = null; pDGV.DataSource = p0;


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        
        private void button3_Click(object sender, EventArgs e)
        {


            BEClsCliente cliente = BEcliente;

            FrmBuscarCliente FrmBuscarCliente = new FrmBuscarCliente(cliente);

            // Suscríbete al evento
            FrmBuscarCliente.ClienteSeleccionado += () =>
            {
                BEreserva.Cliente = BEcliente;
                // Actualiza el DataGridView

                List<BEClsCliente> lista = new List<BEClsCliente> { BEcliente };

                Mostrar(dataGridView1, lista);

            };

            FrmBuscarCliente.Show(); // Muestra el formulario hijo
        }
        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int clienteID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[6].Value);
                BEClsCliente cliente = new BEClsCliente(clienteID);

                Mostrar(dataGridView3, BLLreserva.RetornaListaReservasDeClientesModificada(cliente));

            }
            catch (Exception) { }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView3_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                BEClsReserva reserva = BEreserva;
                reserva.ID= Convert.ToInt32(dataGridView3.SelectedRows[0].Cells[0].Value);

                reserva = BLLreserva.ListarObjeto(reserva);
                List<BEClsHabitacion> lista = new List<BEClsHabitacion> { reserva.Habitacion };

                Mostrar(dataGridView2, lista);

            }
            catch (Exception) { }
        }


        //Check-IN de cliente
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                // Verifica si se ha seleccionado un cliente
                BEClsReserva reserva = BEreserva;
                if (reserva.Cliente == null)
                    throw new Exception("Selecciona un cliente");


                if (dataGridView3.Rows.Count == 0) throw new Exception("Seleccione una Reserva !!!");
                reserva.ID = Convert.ToInt32(dataGridView3.SelectedRows[0].Cells[0].Value);

                reserva = BLLreserva.ListarObjeto(reserva);
                // Establecer FechaCheckIn como la fecha y hora actual de la PC
                DateTime ahora = DateTime.Now;
                // Verificar que el día sea igual o superior al de reserva.FechaCheckIn
                if (ahora.Date < reserva.FechaCheckIn.Date || ahora.Date > reserva.FechaCheckOut)
                {
                    throw new Exception("La fecha de check-in no puede ser anterior ni posterior a la fecha de la reserva.");
                }
                 reserva.FechaCheckIn = ahora;
                 reserva.Habitacion.Estado = "Ocupado";
                 reserva.Estado = "En Curso";
                 BLLHabitacion = new BLLClsHabitacion();
                 BLLHabitacion.Guardar(reserva.Habitacion);
                 BLLreserva = new BLLClsReserva();
                 BLLreserva.Guardar(reserva);
                 MessageBox.Show("Check IN Realizado con exito con éxito.");
                
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Cierra el formulario actual
            this.Close();
        }
    }
    
}
