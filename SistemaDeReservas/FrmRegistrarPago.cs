using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SistemaDeReservas
{
    public partial class FrmRegistrarPago : Form
    {
        BLLClsReserva BLLreserva;
        Regex re;
        BEClsReserva BEreserva;
        BLLClsMedioPago BLLMedioPago;
        BLLClsPago BLLPago;
        BEClsCliente BEcliente;

        public FrmRegistrarPago()
        {
            InitializeComponent();
            BEcliente=new BEClsCliente();
            BLLMedioPago =new BLLClsMedioPago();
            BLLPago=new BLLClsPago();   
            BEreserva = new BEClsReserva();
            BLLreserva=new BLLClsReserva();


        }
        public void Mostrar(DataGridView pDGV, object p0)
        {
            pDGV.DataSource = null; pDGV.DataSource = p0;


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "Efectivo")
            {
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
            }
            else if (comboBox1.SelectedItem.ToString() == "Tarjeta")
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
            }
        }

        //Alta de Pago de la reserva seleccionada
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Verifica si se ha seleccionado un CLIENTE    
                if (BEcliente == null)
                    throw new Exception("Selecciona un cliente");
                // Verifica si se ha seleccionado una reserva
                if (dataGridView2.Rows.Count == 0) throw new Exception("Seleccione una Reserva !!!");
                BEClsReserva reserva = new BEClsReserva();
                reserva.ID = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[0].Value);
                reserva = BLLreserva.ListarObjeto(reserva);
                if (comboBox1.SelectedIndex == -1)
                {
                    throw new Exception("Seleccione un Metodo de Pago !!!");
                }
                BEClsMedioPago BEMedio = new BEClsMedioPago();
                if (comboBox1.SelectedItem.ToString() == "Tarjeta")
                {
                    re = new Regex(@"\d{16}");
                    string numero = textBox2.Text;
                    if (!(re.IsMatch(numero) && numero.Length == 16)) throw new Exception("El Numero no posee el formato correcto !!!");
                    BEMedio.tipo = comboBox1.SelectedItem.ToString();
                    BEMedio.numero = numero;
                    BEMedio.titular = textBox1.Text;
                    BEMedio.vencimiento = int.Parse(textBox3.Text);
                    BEMedio.cuotas = int.Parse(textBox4.Text);
                }
                BEMedio.tipo = comboBox1.SelectedItem.ToString();
                BLLMedioPago.Guardar(BEMedio);
                BEClsPago BEPago = new BEClsPago();
                BEPago.medioPago= BEMedio;
                BEPago.cliente = BEcliente;
                BEPago.reserva = reserva;
                BEPago.fecha = DateTime.Now;
                BEPago.importe= Double.Parse(textBox5.Text);
                if (BLLPago.Guardar(BEPago)){ MessageBox.Show("Pago Realizado con Exito!!!");}
                reserva.Abonada = true;
                BLLreserva.Guardar(reserva);


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        //inicia formulario buscar cliente
        private void button3_Click(object sender, EventArgs e)
        {



            BEClsCliente cliente = BEcliente;

            FrmBuscarCliente FrmBuscarCliente = new FrmBuscarCliente(cliente);

            // Suscríbete al evento
            FrmBuscarCliente.ClienteSeleccionado += () =>
            {
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

            List<BEClsReserva> reservas = BLLreserva.RetornaListaReservasDeClientes(cliente);
            if (reservas != null)
            {
                    var reservasFiltradas = reservas.Where(r => r.Estado != "Cancelada" & r.Abonada == false)
                    .Select(r => new
                        {
                        ReservaID = r.ID,
                        Estado = r.Estado,
                        ClienteID = r.Cliente.ID,
                        HabitacionID = r.Habitacion.ID,
                        CheckIn = r.FechaCheckIn,
                        CheckOut = r.FechaCheckOut,
                        Importe = r.Importe
                        })
                     .ToList();

                    Mostrar(dataGridView2, reservasFiltradas);

                    var pagos = BLLPago.ObtenerPagosPorCliente(cliente)
                    .Select(p => new
                     {
                         ReservaID = p.reserva.ID,
                         ClienteID = p.cliente.ID,
                         Fecha = p.fecha,
                         Importe = p.importe
                     })
                    .ToList();

                    Mostrar(dataGridView3, pagos);
                }
            else
            {
                throw new Exception("No se encontraron Reservas pendientes de Pago!!!");
            }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void dataGridView2_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                textBox5.Text = dataGridView2.SelectedRows[0].Cells[6].Value.ToString();
            }
            catch (FormatException)
            {
                MessageBox.Show("El valor no es un número válido.");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
