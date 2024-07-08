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
    public partial class FrmBuscarCliente : Form
    {
        Regex re;
        BLLClsCliente BLLClsCliente;
        BEClsCliente BEcliente;
        public FrmBuscarCliente()
        {
            InitializeComponent();

            BLLClsCliente = new BLLClsCliente();
            BEcliente=new BEClsCliente();
        }
        public FrmBuscarCliente(BEClsCliente cliente)
        {
            InitializeComponent();
            BLLClsCliente = new BLLClsCliente();
            this.BEcliente = cliente; // Asigna el cliente pasado al cliente de esta clase
        }
        public event Action ClienteSeleccionado;
        public void Mostrar(DataGridView pDGV, object p0)
        {
            pDGV.DataSource = null; pDGV.DataSource = p0;
        }

        //Busqueda de cliente por DNI
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                re = new Regex(@"\d{8}");
                string dni = TextBox1.Text;
                if (!(re.IsMatch(dni) && dni.Length == 8)) throw new Exception("El DNI no posee el formato correcto !!!");
                BEClsCliente x = new BEClsCliente();
                x.DNI = dni;
                x = BLLClsCliente.BuscarCliente_DNI(x);
                if (x == null) throw new Exception("Cliente no encontrado");
                // Actualiza el DataGridView
                List<BEClsCliente> lista = new List<BEClsCliente> { x };
                Mostrar(dataGridView2, lista);
                MessageBox.Show("Cliente Encontrado.");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        //Selecciona el cliente
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView2.SelectedRows.Count == 0)
                    throw new Exception("Selecciona un cliente");
                // Obtiene el objeto de la fila seleccionada
                BEClsCliente Cliente = (BEClsCliente)dataGridView2.SelectedRows[0].DataBoundItem;
                // Modifica el cliente compartido
                BEcliente.ID = Cliente.ID;
                BEcliente.Apellido = Cliente.Apellido;
                BEcliente.Nombre = Cliente.Nombre;
                BEcliente.Nacionalidad = Cliente.Nacionalidad;
                BEcliente.Mail = Cliente.Mail;
                BEcliente.Telefono = Cliente.Telefono;
                BEcliente.DNI = Cliente.DNI;
               

                // Dispara el evento
                ClienteSeleccionado?.Invoke();
                MessageBox.Show("Cliente Seleccionado");

                this.Close();
            }

            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }
    }
}