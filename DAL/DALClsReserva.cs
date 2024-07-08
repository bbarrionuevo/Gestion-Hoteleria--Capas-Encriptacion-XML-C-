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
using System.Xml;
using System.Xml.Linq;

namespace DAL
{
    public class DALClsReserva : IGestor<BEClsReserva>
    {
        private XDocument _xmlDocument;
        private string _filePath;
        DALClsHabitacion DALHabitacion;
        DALClsCliente DALCliente;
        BEClsReserva BEReserva;
        public DALClsReserva(string filePath = "Archivo.xml")
        {
            BEReserva=new BEClsReserva();
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
            

            if (!File.Exists(_filePath))
            {
                _xmlDocument = new XDocument(new XElement("Reservas"));
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
                List<int> ids = _xmlDocument.Descendants("Reserva")
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
        public bool Guardar(BEClsReserva BEReserva)
        {
            try
            {
                // Recargar el documento XML desde el archivo
                _xmlDocument = XDocument.Load(_filePath);

                var reservaElement = _xmlDocument.Descendants("Reserva")
                .FirstOrDefault(r => r.Element("ID") != null && (int)r.Element("ID") == BEReserva.ID);

            if (reservaElement != null)
            {
                // Actualizar la reserva existente
                reservaElement.Element("FechaCheckIn").Value = BEReserva.FechaCheckIn.ToString();
                reservaElement.Element("FechaCheckOut").Value = BEReserva.FechaCheckOut.ToString();
                reservaElement.Element("CantDias").Value = BEReserva.Cant_Dias.ToString();
                reservaElement.Element("Importe").Value = BEReserva.Importe.ToString();
                reservaElement.Element("Estado").Value = BEReserva.Estado.ToString();
                reservaElement.Element("Abonada").Value = BEReserva.Abonada.ToString(); 
                reservaElement.Element("CantInquilinos").Value = BEReserva.Cant_Inquilinos.ToString();
                reservaElement.Element("IdHabitacion").Value = BEReserva.Habitacion.ID.ToString();
                reservaElement.Element("IdCliente").Value = BEReserva.Cliente.ID.ToString();
            }
            else
            {
                // Agregar una nueva reserva
                int nuevoId = ObtenerNuevoId();
                BEReserva.ID = nuevoId;

                XElement nuevaReserva = new XElement("Reserva",
                    new XElement("ID", BEReserva.ID),
                    new XElement("FechaCheckIn", BEReserva.FechaCheckIn),
                    new XElement("FechaCheckOut", BEReserva.FechaCheckOut),
                    new XElement("CantDias", BEReserva.Cant_Dias),
                    new XElement("Importe", BEReserva.Importe),
                    new XElement("Estado", BEReserva.Estado),
                    new XElement("Abonada", BEReserva.Abonada),
                    new XElement("CantInquilinos", BEReserva.Cant_Inquilinos),
                    new XElement("IdHabitacion", BEReserva.Habitacion.ID),
                    new XElement("IdCliente", BEReserva.Cliente.ID)
                    );

                    _xmlDocument.Root.Element("Reservas").Add(nuevaReserva);

                }

                _xmlDocument.Save(_filePath);
                // Recargar el documento XML desde el archivo
                _xmlDocument = XDocument.Load(_filePath);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al guardar el reserva: " + ex.Message);
                return false; // Devolver false si hubo un error al guardar la reserva
            }
        }


        public List<BEClsReserva> ObtenerReservasDelCliente(BEClsCliente Cliente)
        {
            List<BEClsReserva> reservas = new List<BEClsReserva>();
            DALHabitacion = new DALClsHabitacion();
            foreach (var reservaElement in _xmlDocument.Descendants("Reserva"))
            {
                if (reservaElement.Element("IdCliente") != null && int.Parse(reservaElement.Element("IdCliente").Value) == Cliente.ID)
                {
                    BEClsReserva reserva = new BEClsReserva
                    {
                        ID = (int)reservaElement.Element("ID"),
                        FechaCheckIn = DateTime.Parse(reservaElement.Element("FechaCheckIn").Value),
                        FechaCheckOut = DateTime.Parse(reservaElement.Element("FechaCheckOut").Value),
                        Cant_Dias = int.Parse(reservaElement.Element("CantDias").Value),
                        Cant_Inquilinos = int.Parse(reservaElement.Element("CantInquilinos").Value),
                        Importe = double.Parse(reservaElement.Element("Importe").Value),
                        Estado = (reservaElement.Element("Estado").Value),
                        Abonada = bool.Parse(reservaElement.Element("Abonada").Value),
                        // obtener el objeto Cliente correspondiente
                        Cliente = new BEClsCliente(int.Parse(reservaElement.Element("IdCliente").Value)),
                        // obtener el objeto Habitacion correspondiente
                        Habitacion = DALHabitacion.ListarObjeto(new BEClsHabitacion((int)reservaElement.Element("IdHabitacion")))
                    };

                    reservas.Add(reserva);
                }
            }

            return reservas;
        }

        public List<BEClsReserva> ObtenerReservasDeHabitacion(BEClsHabitacion Habitacion)
        {
            List<BEClsReserva> reservas = new List<BEClsReserva>();

            foreach (var reservaElement in _xmlDocument.Descendants("Reserva"))
            {
                if (reservaElement.Element("IdHabitacion") != null && (int)reservaElement.Element("IdHabitacion") == Habitacion.ID)
                {
                    BEClsReserva reserva = new BEClsReserva
                    {
                        ID = (int)reservaElement.Element("ID"),
                        FechaCheckIn = DateTime.Parse(reservaElement.Element("FechaCheckIn").Value),
                        FechaCheckOut = DateTime.Parse(reservaElement.Element("FechaCheckOut").Value),
                        Cant_Dias = int.Parse(reservaElement.Element("CantDias").Value),
                        Cant_Inquilinos = int.Parse(reservaElement.Element("CantInquilinos").Value),
                        Importe = double.Parse(reservaElement.Element("Importe").Value),
                        Estado = (reservaElement.Element("Estado").Value),
                        Abonada = bool.Parse(reservaElement.Element("Abonada").Value),
                        Habitacion = Habitacion
                    };

                    reservas.Add(reserva);
                }
            }

            return reservas;
        }
        public BEClsReserva ListarObjeto(BEClsReserva Objeto)
        {
            var reservaElement = _xmlDocument.Descendants("Reserva")
                .FirstOrDefault(r => r.Element("ID") != null && (int)r.Element("ID") == Objeto.ID);

            DALCliente = new DALClsCliente();
            DALHabitacion = new DALClsHabitacion();
            if (reservaElement != null)
            {
                BEClsReserva reserva = new BEClsReserva
                {
                    ID= Objeto.ID,
                    FechaCheckIn = DateTime.Parse(reservaElement.Element("FechaCheckIn").Value),
                    FechaCheckOut = DateTime.Parse(reservaElement.Element("FechaCheckOut").Value),
                    Cant_Dias = int.Parse(reservaElement.Element("CantDias").Value),
                    Cant_Inquilinos = int.Parse(reservaElement.Element("CantInquilinos").Value),
                    Importe = double.Parse(reservaElement.Element("Importe").Value),
                    Estado = (reservaElement.Element("Estado").Value),
                    Abonada = bool.Parse(reservaElement.Element("Abonada").Value),
                    Cliente = DALCliente.ListarObjeto(new BEClsCliente(int.Parse(reservaElement.Element("IdCliente").Value))),
                    Habitacion = DALHabitacion.ListarObjeto(new BEClsHabitacion((int)reservaElement.Element("IdHabitacion")))
                };

                return reserva;
            }

            return null;
        }
        public List<BEClsReserva> ListarTodo()
        {
            try
            {
                if (_xmlDocument == null)
                {
                    return new List<BEClsReserva>(); // Devolver una lista vacía si el documento es nulo
                }
                DALCliente = new DALClsCliente();
                DALHabitacion = new DALClsHabitacion();
                List<BEClsReserva> reservas = _xmlDocument.Descendants("Reserva")
                    .Select(reserva => new BEClsReserva
                    {
                        ID = (int)reserva.Element("ID"),
                        FechaCheckIn = DateTime.Parse(reserva.Element("FechaCheckIn").Value),
                        FechaCheckOut = DateTime.Parse(reserva.Element("FechaCheckOut").Value),
                        Cant_Dias = int.Parse(reserva.Element("CantDias").Value),
                        Cant_Inquilinos = int.Parse(reserva.Element("CantInquilinos").Value),
                        Importe = double.Parse(reserva.Element("Importe").Value),
                        Estado = (reserva.Element("Estado").Value),
                        Abonada = bool.Parse(reserva.Element("Abonada").Value),
                        Cliente = DALCliente.ListarObjeto(new BEClsCliente(int.Parse(reserva.Element("IdCliente").Value))),
                        Habitacion = DALHabitacion.ListarObjeto(new BEClsHabitacion((int)reserva.Element("IdHabitacion")))
                    }).ToList();
                return reservas;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener las reservas: " + ex.Message);
                return new List<BEClsReserva>(); // Devolver una lista vacía en caso de error
            }
        }

        public bool Baja(BEClsReserva BEReserva)
        {
            // Recargar el documento XML desde el archivo
            _xmlDocument = XDocument.Load(_filePath);

            var reservaElement = _xmlDocument.Descendants("Reserva")
                .FirstOrDefault(r => r.Element("ID") != null && (int)r.Element("ID") == BEReserva.ID);

            if (reservaElement != null)
            {

                reservaElement.Element("Estado").Value = BEReserva.Estado.ToString();
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
    }
}
