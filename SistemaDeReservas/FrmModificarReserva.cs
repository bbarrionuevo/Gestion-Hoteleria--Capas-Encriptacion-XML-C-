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
    public partial class FrmModificarReserva : Form
    {

        BEClsCliente BEcliente;
        BLLClsReserva BLLreserva;
        BEClsReserva BEreserva;
        BLLClsHabitacion BLLHabitacion;
        public FrmModificarReserva()
        {
            InitializeComponent();
            BLLreserva = new BLLClsReserva();
            
            BLLHabitacion = new BLLClsHabitacion();
            BEreserva = new BEClsReserva();

            BEcliente = new BEClsCliente();

        }
        public void Mostrar(DataGridView pDGV, object p0)
        {
            pDGV.DataSource = null; pDGV.DataSource = p0;


        }

        //inicia formulario buscar cliente
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

        //Modificar reserva seleccionada
        private void button5_Click(object sender, EventArgs e)
        {

            try
            {
                
                if (BEreserva.Cliente == null)
                    throw new Exception("Selecciona un cliente");

                if (BEreserva.Habitacion == null)
                    throw new Exception("Selecciona una Habitacion");

                if (dataGridView3.Rows.Count == 0) throw new Exception("No hay nada para modificar !!!");
                BEClsReserva reserva = new BEClsReserva();
                reserva.ID = Convert.ToInt32(dataGridView3.SelectedRows[0].Cells[0].Value);
                reserva = BLLreserva.ListarObjeto(reserva);
                reserva.Habitacion = BEreserva.Habitacion;
                if (BLLreserva.Guardar(reserva))
                {
                    MessageBox.Show("Reserva modificada con éxito.");
                    // Actualiza los DataGridViews
                    BEClsCliente cliente = new BEClsCliente(Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[6].Value));

                    Mostrar(dataGridView3, BLLreserva.RetornaListaReservasDeClientesModificada(cliente));
                    // Crear el informe de modificación
                    string informe = $"Modificación de reserva - ID: {reserva.ID}, Fecha: {DateTime.Now}";

                    // Guardar el informe en el archivo XML
                    //GuardarInformeReserva(informe);

                }
                else

                    MessageBox.Show("La habitación no está disponible para las fechas seleccionadas.");

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }


        //inicia formulario buscar habitacion
        private void button1_Click(object sender, EventArgs e)
        {

            BEClsReserva reserva = BEreserva;

            FrmComprobarHabitacion FrmComprobarHabitacion = new FrmComprobarHabitacion(reserva);

            //FrmComprobarHabitacion.MdiParent = this; // Establece el formulario MDI como padre
            // Suscríbete al evento
            FrmComprobarHabitacion.HabitacionSeleccionada += () =>
            {
                // Actualiza el DataGridView

                List<BEClsHabitacion> lista = new List<BEClsHabitacion> { BEreserva.Habitacion };

                Mostrar(dataGridView2, lista);

            };

            FrmComprobarHabitacion.Show(); // Muestra el formulario hijo
        }

        //baja de reserva
        private void button6_Click(object sender, EventArgs e)
        {

            try
            {
                if (dataGridView3.Rows.Count == 0) throw new Exception("No hay nada para borrar !!!");
                BEClsReserva reserva = new BEClsReserva(Convert.ToInt32(dataGridView3.SelectedRows[0].Cells[0].Value));
                reserva.Estado = "Cancelada";
                BLLreserva.Baja(reserva);
                BEClsCliente cliente = new BEClsCliente(Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[6].Value));
                Mostrar(dataGridView3, BLLreserva.RetornaListaReservasDeClientesModificada(cliente));

                throw new Exception("Baja de Reserva realizada con exito !!!");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Cierra el formulario actual
            this.Close();
        }
    }
}
