using BE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALClsBackupManager
    {
        private string backupFolder = AppDomain.CurrentDomain.BaseDirectory + "\\Backup\\"; // Define la carpeta donde se guardarán los backups

        public bool RealizarBackup(BEClsBackupManager backup)
        {
            try
            {
                // Crear la carpeta de backups si no existe
                if (!Directory.Exists(backupFolder))
                {
                    Directory.CreateDirectory(backupFolder);
                }

                // Definir el nombre del archivo de backup
                string backupFileName = "Backup_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xml";

                // Definir la ruta completa del archivo de backup
                string backupFile = backupFolder + backupFileName;

                // Copiar el archivo original al archivo de backup
                File.Copy("Archivo.xml", backupFile, true);

                // Guardar la información del backup
                
                backup.RutaDestino = backupFile;

                // Guardar la información del backup en el historial
                using (StreamWriter sw = File.AppendText("HistorialBackups.txt"))
                {
                    sw.WriteLine($"Backup realizado el {backup.Fecha} en {backupFileName}");
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RestaurarBackup(BEClsBackupManager backup)
        {
            try
            {
                // Leer el archivo de historial
                string[] lineas = File.ReadAllLines("HistorialBackups.txt");

                // Filtrar los eventos de backup
                string[] eventosBackup = lineas.Where(l => l.Contains("Backup realizado")).ToArray();

                // Obtener la última entrada de backup realizado
                string ultimoBackup = eventosBackup.LastOrDefault();

                // Extraer el nombre del último backup realizado
                string nombreUltimoBackup = ultimoBackup.Split(' ').Last();

                // Definir la ruta completa del último backup realizado
                string rutaUltimoBackup = backupFolder + nombreUltimoBackup;

                // Copiar el archivo de backup al archivo original
                File.Copy(rutaUltimoBackup, "Archivo.xml", true);

                // Guardar la información de la restauración
                
                backup.RutaBackup = rutaUltimoBackup;

                // Guardar la información de la restauración en el historial
                using (StreamWriter sw = File.AppendText("HistorialBackups.txt"))
                {
                    sw.WriteLine($"Backup restaurado el {backup.Fecha} desde {nombreUltimoBackup}");
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string[] LeerHistorialBackup()
        {
            // Leer el archivo de historial
            string[] lineas = File.ReadAllLines("HistorialBackups.txt");

            // Filtrar los eventos de backup
            return lineas.Where(l => l.Contains("Backup realizado")).ToArray();
        }

        public string[] LeerHistorialRestauracion()
        {
            // Leer el archivo de historial
            string[] lineas = File.ReadAllLines("HistorialBackups.txt");

            // Filtrar los eventos de restauración
            return lineas.Where(l => l.Contains("Backup restaurado")).ToArray();
        }
    }
}