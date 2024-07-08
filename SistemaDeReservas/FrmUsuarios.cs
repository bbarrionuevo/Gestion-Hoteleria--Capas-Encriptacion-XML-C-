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
using BE;
using BLL;
using System.Security.Cryptography;
using Servicios;


namespace SistemaDeReservas
{
    public partial class FrmUsuarios : Form
    {
        Regex re;
        BEUsuario BEUsu;
        BLLUsuario BLLUsu;
        public FrmUsuarios()
        {
            InitializeComponent();
            BLLUsu = new BLLUsuario();
        }
       
        public void Mostrar(DataGridView pDGV, object p0)
        {
            pDGV.DataSource = null; pDGV.DataSource = p0;


        }
        //Boton Alta Usuario
        private void Button2_Click(object sender, EventArgs e)
        {


            try
            {

                re = new Regex(@"\d{8}");
                string dni = textBox6.Text;
                if (!(re.IsMatch(dni) && dni.Length == 8)) throw new Exception("El DNI no posee el formato correcto !!!");
                BEUsuario BEUsu = new BEUsuario();
                BEUsu.DNI = dni;
                BEUsu.Nombre = TextBox1.Text;
                BEUsu.Apellido = textBox5.Text;
                BEUsu.Nacionalidad = textBox4.Text;
                BEUsu.Mail = textBox3.Text;
                BEUsu.Usuario = textBox8.Text;
                BEUsu.Contraseña = Encriptacion.EncriptarClave(textBox7.Text);
                BEUsu.Telefono = int.Parse(textBox2.Text);
                BLLUsu.Guardar(BEUsu);
                Mostrar(dataGridUsuarios, BLLUsu.ListarTodo());

                MessageBox.Show("Alta de Usuario realizada con éxito.");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
       

        
        private void FrmUsuarios_Load(object sender, EventArgs e)
        {
            Mostrar(dataGridUsuarios, BLLUsu.ListarTodo());
        }

        

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }


        //Modificar Usuario
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridUsuarios.Rows.Count == 0) throw new Exception("No hay nada para modificar !!!");
                DataGridViewRow f = dataGridUsuarios.SelectedRows[0];
                BEUsuario BEUsu = new BEUsuario(Convert.ToInt32(f.Cells[9].Value));
                re = new Regex(@"\d{8}");
                string dni = textBox6.Text;
                if (!(re.IsMatch(dni) && dni.Length == 8)) throw new Exception("El DNI no posee el formato correcto !!!");

                BEUsu.DNI = dni;
                BEUsu.Nombre = TextBox1.Text;
                BEUsu.Apellido = textBox5.Text;
                BEUsu.Nacionalidad = textBox4.Text;
                BEUsu.Mail = textBox3.Text;
                BEUsu.Usuario = textBox8.Text;
                BEUsu.Contraseña = Encriptacion.EncriptarClave(textBox7.Text);
                BEUsu.Telefono = int.Parse(textBox2.Text);
                BLLUsu.Guardar(BEUsu);
                Mostrar(dataGridUsuarios, BLLUsu.ListarTodo());

                MessageBox.Show("Modificación de Usuario realizada con éxito.");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        //Baja Usuario
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridUsuarios.Rows.Count == 0) throw new Exception("No hay nada para borrar !!!");
                DataGridViewRow f = dataGridUsuarios.SelectedRows[0];
                BEUsuario BEUsu = new BEUsuario(Convert.ToInt32(f.Cells[9].Value));
                BLLUsu.Baja(BEUsu);
                Mostrar(dataGridUsuarios, BLLUsu.ListarTodo());
                MessageBox.Show("Baja de Usuario realizada con éxito.");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void dataGridUsuarios_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow f = dataGridUsuarios.SelectedRows[0];


                textBox6.Text = f.Cells[5].Value.ToString();
                TextBox1.Text = f.Cells[3].Value.ToString();
                textBox5.Text = f.Cells[4].Value.ToString();
                textBox4.Text = f.Cells[6].Value.ToString();
                textBox2.Text = f.Cells[5].Value.ToString();
                textBox3.Text = f.Cells[7].Value.ToString();
                textBox2.Text = f.Cells[8].Value.ToString();
                textBox8.Text = f.Cells[0].Value.ToString();
                textBox7.Text = f.Cells[1].Value.ToString();




            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        
   
    }
}


        