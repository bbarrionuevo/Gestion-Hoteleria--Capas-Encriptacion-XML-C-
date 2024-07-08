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
    public partial class FrmGestionPermisos : Form
    {
        BEUsuario BEUsu;
        BEUsuario seleccion;
        BLLPermisos oBLLPermiso;
        BLLUsuario oBLLUsu;
        public FrmGestionPermisos()
        {
            InitializeComponent();
            BEUsu = new BEUsuario();
            oBLLPermiso = new BLLPermisos();
            oBLLUsu = new BLLUsuario();
            this.listBox1.DataSource = oBLLUsu.ListarTodo();
            this.listBox2.DataSource = oBLLPermiso.GetAllFamilias();
            //this.listBox3.DataSource = oBLLPermiso.GetAllPatentes();
        }

        private void Rol_Click(object sender, EventArgs e)
        {

        }
        //Agrega Rol a Usuario
        private void button1_Click(object sender, EventArgs e)
        {

            if (BEUsu != null)
            {
                var flia = (BEFamilia)listBox2.SelectedItem;
                if (flia != null)
                {
                    var esta = false;
                    //verifico que ya no tenga el permiso. TODO: Esto debe ser parte de otra capa.
                    if(BEUsu.Rol != null)
                    if (oBLLPermiso.Existe(BEUsu.Rol, flia.ID))
                        {
                            esta = true;
                        }
                    

                    if (esta)
                        MessageBox.Show("El usuario ya tiene la familia indicada");
                    else
                    {
                            oBLLPermiso.FillFamilyComponents(flia);
                            BEUsu.Rol=flia;
                            oBLLUsu.GuardarPermisos(BEUsu);
                            MostrarPermisos(BEUsu);
                        
                    }
                }
            }
            else
                MessageBox.Show("Seleccione un usuario");
        }
        //Quita Rol a usuario
        private void button2_Click(object sender, EventArgs e)
        {

            if (BEUsu != null)
            {
                BEUsu.Rol = null;
                oBLLUsu.BorrarPermisos(BEUsu);
                MostrarPermisos(BEUsu);


            }
            else
                MessageBox.Show("Seleccione un usuario");
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void FrmGestionPermisos_Load(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            seleccion = (BEUsuario)this.listBox1.SelectedItem;
            // Hago una copia del objeto para no modificar el que está en el combo.
            BEUsu = new BEUsuario();
            BEUsu.ID = seleccion.ID;
            BEUsu.Nombre = seleccion.Nombre;
            BEUsu.Usuario = seleccion.Usuario;
            oBLLPermiso.FillUserComponents(BEUsu);

            MostrarPermisos(BEUsu);
        }
        void MostrarPermisos(BEUsuario u)
        {
            // Limpia el TreeView
            this.treeView1.Nodes.Clear();

            // Crea un nuevo nodo de árbol para el usuario
            TreeNode root = new TreeNode(u.Usuario);

            // Llama a la función LlenarTreeView para añadir el permiso y sus hijos al nodo del árbol
            if(u.Rol != null)
            LlenarTreeView(root, u.Rol);
            

            // Añade el nodo del árbol al control TreeView
            this.treeView1.Nodes.Add(root);

            // Expande todos los nodos en el control TreeView
            this.treeView1.ExpandAll();
        }
        void LlenarTreeView(TreeNode padre, BEComponente c)
        {
            
                TreeNode hijo = new TreeNode(c.Nombre);
                hijo.Tag = c;
                padre.Nodes.Add(hijo);

                foreach (var item in c.Hijos)
                {
                    LlenarTreeView(hijo, item);
                }
           

        }
    }
}
