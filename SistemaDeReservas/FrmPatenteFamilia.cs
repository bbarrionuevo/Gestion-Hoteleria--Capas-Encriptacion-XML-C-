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
    public partial class FrmPatenteFamilia : Form
    {
        BLLPermisos oBLLPer;
        BEFamilia oBEFam;
        public FrmPatenteFamilia()
        {
            InitializeComponent();
            oBLLPer = new BLLPermisos();
            this.cboPermisos.DataSource = oBLLPer.GetAllPermission();
            //cargo todas las patentes con familias
            LlenarPatentesFamilias();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void LlenarPatentesFamilias()
        {

            this.cboPatentes.DataSource = oBLLPer.GetAllPatentes();
            this.cboFamilias.DataSource = oBLLPer.GetAllFamilias();
        }

        //Crear nuevo permiso simple
        private void btnGuardarPatente_Click(object sender, EventArgs e)
        {
            BEPatente oBEPat = new BEPatente()
            {
                Nombre = this.txtNombrePatente.Text,
                Permiso = (BETipoPermiso)this.cboPermisos.SelectedItem

            };

            oBLLPer.GuardarComponente(oBEPat, false);
            //LlenarPatentesFamilias();

            MessageBox.Show("Patente guardada correctamente");
        }


        //Crear nuevo Rol
        private void button1_Click(object sender, EventArgs e)
        {
            BEFamilia oBEFam = new BEFamilia()
            {
                Nombre = this.txtNombreFamilia.Text

            };




            oBLLPer.GuardarComponente(oBEFam, true);
            //LlenarPatentesFamilias();
            MessageBox.Show("Familia guardada correctamente");
        }

        
         private void CmdAgregarPatente_Click(object sender, EventArgs e)
        {
            if (oBEFam != null)
            {
                var patente = (BEPatente)cboPatentes.SelectedItem;
                if (patente != null)
                {
                    var esta = oBLLPer.Existe(oBEFam, patente.ID);
                    if (esta)
                        MessageBox.Show("ya exsite la patente indicada");
                    else
                    {

                        {
                            oBEFam.AgregarHijo(patente);
                            MostrarFamilia(oBEFam);
                        }
                    }
                }
            }
        }
        void MostrarFamilia(BEFamilia oBEFam)
        {
            if (oBEFam == null) return;



            this.treeConfigurarFamilia.Nodes.Clear();

            TreeNode root = new TreeNode(oBEFam.Nombre);
            root.Tag = oBEFam;
            this.treeConfigurarFamilia.Nodes.Add(root);

            foreach (var item in oBEFam.Hijos)
            {
                MostrarEnTreeView(root, item);
            }

            treeConfigurarFamilia.ExpandAll();
        }
        void MostrarEnTreeView(TreeNode tn, BEComponente c)
        {
            // Creo un nuevo nodo con el nombre del componente
            TreeNode n = new TreeNode(c.Nombre);
            // Asigno el componente a la etiqueta del nuevo nodo
            n.Tag = c;
            // Agrego el nuevo nodo al nodo padre
            tn.Nodes.Add(n);
            // Si el componente tiene hijos, los agrego al nuevo nodo
            if (c.Hijos != null)
                foreach (var item in c.Hijos)
                    MostrarEnTreeView(tn, item);  // Llamada recursiva
        }

        private void cmdAgregarFamilia_Click(object sender, EventArgs e)
        {

            if (oBEFam != null)
            {
                var familia = (BEFamilia)cboFamilias.SelectedItem;
                if (familia != null)
                {

                    var esta = oBLLPer.Existe(oBEFam, familia.ID);
                    if (esta)
                        MessageBox.Show("ya exsite la familia indicada");
                    else
                    {

                        oBLLPer.FillFamilyComponents(familia);
                        oBEFam.AgregarHijo(familia);
                        MostrarFamilia(oBEFam);
                    }


                }
            }
        }

        //agrega permiso simple al Rol
        private void cmdAgregarPatente_Click(object sender, EventArgs e)
        {
            if (oBEFam != null)
            {
                var patente = (BEPatente)cboPatentes.SelectedItem;
                if (patente != null)
                {
                    var esta = oBLLPer.Existe(oBEFam, patente.ID);
                    if (esta)
                        MessageBox.Show("ya exsite la patente indicada");
                    else
                    {

                        {
                            oBEFam.AgregarHijo(patente);
                            MostrarFamilia(oBEFam);
                        }
                    }
                }
            }
        }

        //Configurar Rol
        private void cmdSeleccionar_Click(object sender, EventArgs e)
        {
            var tmp = (BEFamilia)this.cboFamilias.SelectedItem;
            oBEFam = new BEFamilia();
            oBEFam.ID = tmp.ID;
            oBEFam.Nombre = tmp.Nombre;
            oBLLPer.FillFamilyComponents(oBEFam);

            MostrarFamilia(oBEFam);
        }

        //Guardar el Rol modificado
        private void cmdGuardarFamilia_Click(object sender, EventArgs e)
        {

            try
            {
                oBLLPer.GuardarFamilia(oBEFam);
                MessageBox.Show("Familia guardada correctamente");
            }
            catch (Exception)
            {

                MessageBox.Show("Error al guardar la familia");
            }
        }

        //Quita permiso simple al Rol
        private void button2_Click(object sender, EventArgs e)
        {
            if (oBEFam != null)
            {
                var patente = (BEPatente)cboPatentes.SelectedItem;
                if (patente != null)
                {
                    var esta = oBLLPer.Existe(oBEFam, patente.ID);
                    if (esta) {
                        oBEFam.QuitarHijo(patente);
                        MostrarFamilia(oBEFam);
                    }

                    else
                    {
                        MessageBox.Show("La patente que desea eliminar no se encuentra asignada");


                    }
                }
            }
        }
    }
}
