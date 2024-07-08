using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BLL;
using BE;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace SistemaDeReservas
{
    public partial class FrmGestionarReserva : Form
    {
        BLLClsHotel BLLhotel;
        BLLClsCliente BLLcliente;
        BLLClsReserva BLLreserva;
        BEClsHotel BEhotel;
        BEClsCliente BEcliente;
        BEClsReserva BEreserva;
        BLLClsHabitacion BLLHabitacion;
        Regex re;
        Regex re2;

        public FrmGestionarReserva()
        {
            InitializeComponent();

            BLLhotel = new BLLClsHotel();
            BLLcliente = new BLLClsCliente();
            BLLreserva = new BLLClsReserva();
            BEhotel = new BEClsHotel();
            BLLHabitacion = new BLLClsHabitacion();
            BEreserva = new BEClsReserva();
            BEcliente = new BEClsCliente();


        }
        public void Mostrar(DataGridView pDGV, object p0)
        {
            pDGV.DataSource = null; pDGV.DataSource = p0;


        }
        public void ActualizarListas()
        { 
            //Mostrar(dataGridView1, BLLcliente.ListarTodo());
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            

           
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        //Boton ABM Cliente
        private void button7_Click(object sender, EventArgs e)
        {
            FrmRegistrarCliente frmCliente = new FrmRegistrarCliente();
            frmCliente.Show(); // Muestra el formulario
        }
      
        
        //Alta Reservas
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {

                // Verifica si se ha seleccionado un cliente y una habitación
                BEClsReserva reserva = BEreserva;
                if (reserva.Cliente == null)
                    throw new Exception("Selecciona un cliente");
                
                if (reserva.Habitacion == null)
                    throw new Exception("Selecciona una Habitacion");
                reserva.Estado = "Pendiente";

                BEClsCliente Cliente = new BEClsCliente(Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[6].Value));
                // Agrega la reserva al hotel
                if (BLLreserva.Guardar(reserva))
                {
                    MessageBox.Show("Reserva realizada con éxito.");


                    BEClsCliente cliente = new BEClsCliente(Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[6].Value));

                    Mostrar(dataGridView3, BLLreserva.RetornaListaReservasDeClientesModificada(cliente));
                    // Crear el informe de alta
                    string informe = $"Alta de reserva - ID: {reserva.ID}, Fecha: {DateTime.Now}";

                }
                else

                    MessageBox.Show("La habitación no está disponible para las fechas seleccionadas.");

            }

            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        
        
        private void GuardarInformeReserva(string informe)
        {
            try
            {
                // Crear el archivo XML si no existe
                if (!File.Exists("informes.xml"))
                {
                    using (XmlWriter writer = XmlWriter.Create("informes.xml"))
                    {
                        writer.WriteStartDocument();
                        writer.WriteStartElement("Informes");
                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                    }
                }

                // Agregar el informe al archivo XML
                XDocument doc = XDocument.Load("informes.xml");
                XElement informeElement = new XElement("Informe", informe);
                doc.Element("Informes").Add(informeElement);
                doc.Save("informes.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el informe en el archivo XML: " + ex.Message);
            }
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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






        private void dataGridView2_RowEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView3_RowEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Mostrar(dataGridView2, BLLHabitacion.ListarTodo());
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        //Inicia formulario busqueda de habitacion
        private void button1_Click_1(object sender, EventArgs e)
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

        
            
        

        private void button2_Click(object sender, EventArgs e)
        {

            // Cierra el formulario actual
            this.Close();


        }


        //Inicia formulario buscar Cliente
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
    }
}
