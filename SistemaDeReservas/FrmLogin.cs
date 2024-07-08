using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using BE;
using Servicios;

namespace SistemaDeReservas
{
    public partial class FrmLogin : Form
    {
        BLLClsHotel BLLhotel;
        BEUsuario BEUsu;
        BLLUsuario BLLUsu;
        BLLSesion oBLLSesion;
        BLLPermisos oBLLPermisos;
      
        public FrmLogin()
        {
            InitializeComponent();
            
            BLLhotel = new BLLClsHotel();
            BEUsu = new BEUsuario();
            BLLUsu = new BLLUsuario();
            oBLLSesion = new BLLSesion();
            oBLLPermisos = new BLLPermisos();
            txtContraseña.UseSystemPasswordChar = true;
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }
        //Inicia sesion de usuario
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string usuario = txtUsuario.Text;
                string contraseña = txtContraseña.Text;
                BEUsu = new BEUsuario();
                BEUsu.Usuario = usuario;
                BEUsu = BLLUsu.ListarObjeto(BEUsu);
                if (BEUsu == null) throw new Exception("El usuario ingresado no es valido");
                BEUsu.Contraseña = Encriptacion.EncriptarClave(contraseña);


                if (BLLUsu.ValidarCredenciales(BEUsu))
                {
                    oBLLPermisos.FillUserComponents(BEUsu);

                    // Credenciales válidas, habilita los menús
                    Menu menu = (Menu)MdiParent;

                    oBLLSesion.Login(BEUsu);




                    menu.ValidarPermisos();
                    MessageBox.Show("Inicio de sesión exitoso.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Credenciales incorrectas. Inténtalo nuevamente.");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        

        private void txtContraseña_Leave(object sender, EventArgs e)
        {
            // Limpiar listBox1
            listBox1.Items.Clear();

            // Agregar la contraseña escrita a listBox1
            listBox1.Items.Add("Contraseña: " + txtContraseña.Text);

            // Encriptar la contraseña
            string contraseñaEncriptada = Encriptacion.EncriptarClave(txtContraseña.Text);

            // Agregar la contraseña encriptada a listBox1
            listBox1.Items.Add("Contraseña encriptada: " + contraseñaEncriptada);
        }
    }
   }