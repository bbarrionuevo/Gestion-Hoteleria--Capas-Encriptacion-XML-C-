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
    public partial class FrmBackUp : Form
    {
        private BLLClsBackupManager _bllBackupManager;

        public FrmBackUp()
        {
            InitializeComponent();
            _bllBackupManager = new BLLClsBackupManager();
        }

        private void FrmBackUp_Load(object sender, EventArgs e)
        {
            MostrarEventos();
        }
        private void MostrarEventos()
        {
            // Obtener los eventos de backup y restauración
            List<string> eventosBackup = _bllBackupManager.ObtenerEventosBackup().ToList();
            List<string> eventosRestauracion = _bllBackupManager.ObtenerEventosRestauracion().ToList();

            // Convertir las listas de cadenas a listas de objetos anónimos
            var eventosBackupParaMostrar = eventosBackup.Select(e => new { Evento = e }).ToList();
            var eventosRestauracionParaMostrar = eventosRestauracion.Select(e => new { Evento = e }).ToList();

            // Mostrar los eventos de backup en dataGridView1
            listBox1.DataSource = eventosBackupParaMostrar;

            // Mostrar los eventos de restauración en dataGridView2
            listBox2.DataSource = eventosRestauracionParaMostrar;
        }



        private void btnRealizarBackup_Click_1(object sender, EventArgs e)
        {
            // Crear un nuevo objeto BEClsBackupManager
            BEClsBackupManager backup = new BEClsBackupManager();
            backup.Fecha = DateTime.Now;

            // Realizar el backup
            bool exito = _bllBackupManager.RealizarBackup(backup);

            // Mostrar un mensaje según si el backup fue exitoso o no
            MessageBox.Show(exito ? "Backup realizado con éxito" : "Error al realizar el backup");
            MostrarEventos();
        }

        private void btnRestaurarBackup_Click(object sender, EventArgs e)
        {
            // Crear un nuevo objeto BEClsBackupManager
            BEClsBackupManager backup = new BEClsBackupManager();
            backup.Fecha = DateTime.Now;
            // Preguntar al usuario si está seguro de que quiere restaurar el backup
            DialogResult dialogResult = MessageBox.Show("¿Estás seguro de que quieres restaurar el backup?", "Confirmar restauración", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                // Restaurar el backup
                bool exito = _bllBackupManager.RestaurarBackup(backup);

                // Mostrar un mensaje según si la restauración fue exitosa o no
                MessageBox.Show(exito ? "Backup restaurado con éxito" : "Error al restaurar el backup");
                MostrarEventos();
            }
            else
            {
                // El usuario decidió no restaurar el backup
            }
        }
    }

}
