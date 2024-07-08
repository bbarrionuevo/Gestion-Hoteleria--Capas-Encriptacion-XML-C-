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
    public class DALClsCliente : IGestor<BEClsCliente>
    {

        // Defino el objeto datos
        private XDocument _xmlDocument;
        private string _filePath;
        BEClsCliente BECliente;
        BEClsHotel BEHotel;
        ArrayList AP;
        DALClsReserva DALReserva;
        


        public DALClsCliente(string filePath = "Archivo.xml")
        {
            BECliente = new BEClsCliente();
            BEHotel = new BEClsHotel();
            DALReserva = new DALClsReserva();
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);

            if (!File.Exists(_filePath))
            {
                _xmlDocument = new XDocument(new XElement("Clientes"));
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
                List<int> ids = _xmlDocument.Descendants("Cliente")
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



        // Agregar Cliente en archivo de texto

        public bool Guardar(BEClsCliente BECliente)
        {
            try
            {
                // Recargar el documento XML desde el archivo
                _xmlDocument = XDocument.Load(_filePath);

                if (BECliente.ID != 0)
                {
                    

                    // Si el ID del cliente existe, actualizar el cliente
                    XElement clienteExistente = _xmlDocument.Descendants("Cliente")
                        .Where(c => c.Element("ID") != null && (int)c.Element("ID") == BECliente.ID)
                    .FirstOrDefault();

                    if (clienteExistente != null)
                    {
                        clienteExistente.Element("Nombre").Value = BECliente.Nombre;
                        clienteExistente.Element("Apellido").Value = BECliente.Apellido;
                        clienteExistente.Element("DNI").Value = BECliente.DNI.ToString();
                        clienteExistente.Element("Mail").Value = BECliente.Mail;
                        clienteExistente.Element("Nacionalidad").Value = BECliente.Nacionalidad;
                        clienteExistente.Element("Telefono").Value = BECliente.Telefono.ToString();


                    }
                    else
                    {
                        Console.WriteLine("Cliente con ID " + BECliente.ID + " no encontrado.");
                        return false; // Devolver false si el cliente no fue encontrado
                    }
                }
                else
                {
                    // Si el ID del cliente es nulo, agregar un nuevo cliente
                    int nuevoId = ObtenerNuevoId(); // Método para obtener un nuevo ID autoincremental
                    BECliente.ID = nuevoId;

                    XElement nuevoCliente = new XElement("Cliente",
                        new XElement("ID", BECliente.ID),
                        new XElement("Nombre", BECliente.Nombre),
                        new XElement("Apellido", BECliente.Apellido),
                        new XElement("DNI", BECliente.DNI.ToString()),
                        new XElement("Mail", BECliente.Mail),
                        new XElement("Nacionalidad", BECliente.Nacionalidad),
                        new XElement("Telefono", BECliente.Telefono.ToString())
                    ); ;
                    _xmlDocument.Root.Element("Clientes").Add(nuevoCliente);
                }

                _xmlDocument.Save(_filePath);
                // Recargar el documento XML desde el archivo
                _xmlDocument = XDocument.Load(_filePath);

                return true; // Devolver true si el cliente se guardó correctamente
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al guardar el cliente: " + ex.Message);
                return false; // Devolver false si hubo un error al guardar el cliente
            }
        }


        public bool Baja(BEClsCliente oBECliente)
        {
            
                try
                {
                    // Recargar el documento XML desde el archivo
                    _xmlDocument = XDocument.Load(_filePath);

                    XElement clienteAEliminar = _xmlDocument.Descendants("Cliente")
                        .Where(c => c.Element("ID") != null && (int)c.Element("ID") == oBECliente.ID)
                    .FirstOrDefault();

                if (clienteAEliminar != null)
                    {
                        clienteAEliminar.Remove();
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
                catch (Exception ex)
                {
                    // Manejar la excepción
                    Console.WriteLine("Error al dar de baja al cliente: " + ex.Message);
                    return false; // Devolver false si hubo un error al dar de baja al cliente
                }
        }


       public List<BEClsCliente> ListarTodo()
        {
            try
            {
                if (_xmlDocument == null)
                {
                    return new List<BEClsCliente>(); // Devolver una lista vacía si el documento es nulo
                }

                List<BEClsCliente> clientes = _xmlDocument.Descendants("Cliente")
                    .Select(cliente => new BEClsCliente
                    {
                        ID = (int)cliente.Element("ID"),
                        Nombre = (string)cliente.Element("Nombre"),
                        Apellido = (string)cliente.Element("Apellido"),
                        DNI = (string)cliente.Element("DNI"),
                        Nacionalidad = (string)cliente.Element("Nacionalidad"),
                        Mail = (string)cliente.Element("Mail"),
                        Telefono = (int)cliente.Element("Telefono"),
                        //Reservas = MPPReserva.ObtenerReservasDelCliente((string)cliente.Element("ID"))
                    }).ToList();
                return clientes;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener los clientes: " + ex.Message);
                return new List<BEClsCliente>(); // Devolver una lista vacía en caso de error
            }
        }

        public BEClsCliente ListarObjeto(BEClsCliente Objeto)
        {
            try
            {
                // Busca el cliente en el XML
                var clienteElement = _xmlDocument.Descendants("Cliente")
                    .FirstOrDefault(c => c.Element("ID") != null && (int)c.Element("ID") == Objeto.ID);

                // Si el cliente no existe, retorna null
                if (clienteElement == null)
                    return null;

                // Si el cliente existe, crea un nuevo objeto BEClsCliente y lo retorna
                BEClsCliente clienteEncontrado = new BEClsCliente
                {
                    ID = (int)clienteElement.Element("ID"),
                    Nombre = (string)clienteElement.Element("Nombre"),
                    Apellido = (string)clienteElement.Element("Apellido"),
                    DNI = (string)clienteElement.Element("DNI"),
                    Nacionalidad = (string)clienteElement.Element("Nacionalidad"),
                    Mail = (string)clienteElement.Element("Mail"),
                    Telefono = (int)clienteElement.Element("Telefono"),
                    
                };

                return clienteEncontrado;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al buscar el cliente: " + ex.Message);
                return null; // Devolver null en caso de error
            }
        }
    
        public BEClsCliente BuscarCliente_DNI(BEClsCliente cliente)
        {
            try
            {
                // Busca el cliente en el XML
                var clienteElement = _xmlDocument.Descendants("Cliente")
                    .FirstOrDefault(c => c.Element("DNI") != null && (string)c.Element("DNI") == cliente.DNI);

                // Si el cliente no existe, retorna null
                if (clienteElement == null)
                    return null;

                // Si el cliente existe, crea un nuevo objeto BEClsCliente y lo retorna
                BEClsCliente clienteEncontrado = new BEClsCliente
                {
                    ID = (int)clienteElement.Element("ID"),
                    Nombre = (string)clienteElement.Element("Nombre"),
                    Apellido = (string)clienteElement.Element("Apellido"),
                    DNI = (string)clienteElement.Element("DNI"),
                    Nacionalidad = (string)clienteElement.Element("Nacionalidad"),
                    Mail = (string)clienteElement.Element("Mail"),
                    Telefono = (int)clienteElement.Element("Telefono"),
                    
                };

                return clienteEncontrado;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al buscar el cliente: " + ex.Message);
                return null; // Devolver null en caso de error
            }
        }


    }
}
