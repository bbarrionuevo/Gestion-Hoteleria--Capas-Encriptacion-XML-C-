using Abstraccion;
using BE;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DAL
{
    public class DALClsHabitacion : IGestor<BEClsHabitacion>
    {
        private XDocument _xmlDocument;
        private string _filePath;
        DALClsReserva DALReserva;
        public DALClsHabitacion(string filePath = "Archivo.xml")
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
            

            if (!File.Exists(_filePath))
            {
                _xmlDocument = new XDocument(new XElement("Habitaciones"));
                _xmlDocument.Save(_filePath);
            }
            else
            {
                _xmlDocument = XDocument.Load(_filePath);
            }
        }
        private int ObtenerNuevoId()
        {
            try
            {
                List<int> ids = _xmlDocument.Descendants("Habitacion")
                    .Where(c => c.Element("ID") != null)
                    .Select(c => (int)c.Element("ID"))
                    .ToList();

                if (ids.Count == 0)
                {
                    return 1; // Si no hay clientes, el primer ID será 1
                }
                else
                {
                    return ids.Max() + 1; // El nuevo ID será el máximo actual + 1
                }

            }
            catch (Exception ex)
            {
                // Manejar la excepción, por ejemplo, registrar el error
                Console.WriteLine("Error al obtener el nuevo ID: " + ex.Message);
                // También podrías lanzar la excepción nuevamente si es necesario
                throw;
            }
        }

        public bool Baja(BEClsHabitacion Objeto)
        {
            // Recargar el documento XML desde el archivo
            _xmlDocument = XDocument.Load(_filePath);

            var habitacion = _xmlDocument.Descendants("Habitacion")
                .FirstOrDefault(h => h.Element("ID") != null && (int)h.Element("ID") == Objeto.ID);

            if (habitacion != null)
            {
                habitacion.Remove();
                _xmlDocument.Save(_filePath);
                // Recargar el documento XML desde el archivo
                _xmlDocument = XDocument.Load(_filePath);

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Guardar(BEClsHabitacion Objeto)
        {
            try
            {
                // Recargar el documento XML desde el archivo
                _xmlDocument = XDocument.Load(_filePath);

                var habitacion = _xmlDocument.Descendants("Habitacion")
                .FirstOrDefault(h => h.Element("ID") != null && (int)h.Element("ID") == Objeto.ID);

                if (habitacion != null)
                {
                    // Actualizar la habitación existente
                    habitacion.Element("Numero_Habitacion").Value = Objeto.Numero_Habitacion;
                    habitacion.Element("Piso").Value = Objeto.Piso.ToString();
                    habitacion.Element("Capacidad").Value = Objeto.Capacidad.ToString();
                    habitacion.Element("CamaDoble").Value = Objeto.CamaDoble.ToString();
                    habitacion.Element("ValorNoche").Value = Objeto.ValorNoche.ToString();
                    habitacion.Element("Estado").Value = Objeto.Estado.ToString();
                }
                else
                {
                    // Agregar una nueva habitación
                    int nuevoId = ObtenerNuevoId();
                    Objeto.ID = nuevoId;

                    XElement nuevaHabitacion = new XElement("Habitacion",
                        new XElement("ID", Objeto.ID),
                        new XElement("Numero_Habitacion", Objeto.Numero_Habitacion),
                        new XElement("Piso", Objeto.Piso),
                        new XElement("Capacidad", Objeto.Capacidad),
                        new XElement("Estado", Objeto.Estado),
                        new XElement("CamaDoble", Objeto.CamaDoble),
                        new XElement("ValorNoche", Objeto.ValorNoche)
                    );

                    _xmlDocument.Root.Element("Habitaciones").Add(nuevaHabitacion);
                }

                _xmlDocument.Save(_filePath);
                // Recargar el documento XML desde el archivo
                _xmlDocument = XDocument.Load(_filePath);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al guardar el Habitacion: " + ex.Message);
                return false; // Devolver false si hubo un error al guardar la reserva
            }
        }

            public List<BEClsHabitacion> ListarTodo()
        {
            List<BEClsHabitacion> habitaciones = new List<BEClsHabitacion>();
            DALReserva = new DALClsReserva();

            foreach (var habitacionElement in _xmlDocument.Descendants("Habitacion"))
            {
                BEClsHabitacion habitacion = new BEClsHabitacion
                {
                    ID = (int)habitacionElement.Element("ID"),
                    Numero_Habitacion = habitacionElement.Element("Numero_Habitacion").Value,
                    Piso = int.Parse(habitacionElement.Element("Piso").Value),
                    Capacidad = int.Parse(habitacionElement.Element("Capacidad").Value),
                    CamaDoble = bool.Parse(habitacionElement.Element("CamaDoble").Value),
                    Estado = (habitacionElement.Element("Estado").Value),
                    ValorNoche = double.Parse(habitacionElement.Element("ValorNoche").Value),
                   
                };

                habitaciones.Add(habitacion);
            }

            return habitaciones;
        }

        public BEClsHabitacion ListarObjeto(BEClsHabitacion Objeto)
        {
            var habitacionElement = _xmlDocument.Descendants("Habitacion")
                .FirstOrDefault(h => h.Element("ID") != null && (int)h.Element("ID") == Objeto.ID);

            if (habitacionElement != null)
            {
                BEClsHabitacion habitacion = new BEClsHabitacion
                {
                    ID = (int)habitacionElement.Element("ID"),
                    Numero_Habitacion = habitacionElement.Element("Numero_Habitacion").Value,
                    Piso = int.Parse(habitacionElement.Element("Piso").Value),
                    Capacidad = int.Parse(habitacionElement.Element("Capacidad").Value),
                    Estado = (habitacionElement.Element("Estado").Value),
                    CamaDoble = bool.Parse(habitacionElement.Element("CamaDoble").Value),
                    ValorNoche = double.Parse(habitacionElement.Element("ValorNoche").Value)
                };

                return habitacion;
            }
            else
            {
                return null;
            }
        }

        
    }
}
