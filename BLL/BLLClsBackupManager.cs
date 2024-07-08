using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLClsBackupManager
    {
        private DALClsBackupManager _dalBackupManager;

        public BLLClsBackupManager()
        {
            _dalBackupManager = new DALClsBackupManager();
        }

        //Crear una copia del archivo xml
        public bool RealizarBackup(BEClsBackupManager backup)
        {
           return _dalBackupManager.RealizarBackup(backup);
        }
        //restaurar el archivo xml copiado
        public bool RestaurarBackup(BEClsBackupManager backup)
        {
           return _dalBackupManager.RestaurarBackup(backup);
        }
        public string[] ObtenerEventosBackup()
        {
            // Leer el archivo de historial
            string[] lineas = _dalBackupManager.LeerHistorialBackup();

            // Filtrar los eventos de backup
            return lineas.Where(l => l.Contains("Backup realizado")).ToArray();
        }

        public string[] ObtenerEventosRestauracion()
        {
            // Leer el archivo de historial
            string[] lineas = _dalBackupManager.LeerHistorialRestauracion();

            // Filtrar los eventos de restauración
            return lineas.Where(l => l.Contains("Backup restaurado")).ToArray();
        }
    }
}
