using Abstraccion;
using BE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DAL
{
    public class DALClsPago : IGestor<BEClsPago>
    {
        //defino el obejto datos 

        private XDocument _xmlDocument;
        private string _filePath;
        BEClsPago BEPago;
        DALClsCliente DALCliente;
        DALClsMedioPago DALMedioPago;
        DALClsReserva DALReserva;
        public DALClsPago(string filePath = "Archivo.xml")
        {
            BEPago = new BEClsPago();
            DALCliente= new DALClsCliente();
            DALMedioPago= new DALClsMedioPago();
            DALReserva= new DALClsReserva();

            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);

            if (!File.Exists(_filePath))
            {
                _xmlDocument = new XDocument(new XElement("Pagos"));
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
                List<int> ids = _xmlDocument.Descendants("Pago")
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


        public bool Baja(BEClsPago Objeto)
        {
            // Recargar el documento XML desde el archivo
            _xmlDocument = XDocument.Load(_filePath);
            var pagoElement = _xmlDocument.Descendants("Pago")
                .FirstOrDefault(p => p.Element("ID") != null && (int)p.Element("ID") == Objeto.ID);

            if (pagoElement != null)
            {
                pagoElement.Remove();
                _xmlDocument.Save(_filePath);
                // Recargar el documento XML desde el archivo
                _xmlDocument = XDocument.Load(_filePath);

                return true;
            }

            return false;
        }

        public bool Guardar(BEClsPago Objeto)
        {
            // Recargar el documento XML desde el archivo
            _xmlDocument = XDocument.Load(_filePath);
            var pagoElement = _xmlDocument.Descendants("Pago")
                .FirstOrDefault(p => p.Element("ID") != null && (int)p.Element("ID") == Objeto.ID);

            if (pagoElement != null)
            {
                // Actualizar el pago existente
                pagoElement.Element("IdCliente").Value = Objeto.cliente.ToString();
                pagoElement.Element("IdReserva").Value = Objeto.reserva.ToString();
                pagoElement.Element("IdMedioPago").Value = Objeto.medioPago.ToString();
                pagoElement.Element("Fecha").Value = Objeto.fecha.ToString();
                pagoElement.Element("Importe").Value = Objeto.importe.ToString();
            }
            else
            {
                // Agregar un nuevo pago
                int nuevoId = ObtenerNuevoId();
                Objeto.ID = nuevoId;

                XElement nuevoPago = new XElement("Pago",
                    new XElement("ID", Objeto.ID),
                    new XElement("IdCliente", Objeto.cliente.ID),
                    new XElement("IdReserva", Objeto.reserva.ID),
                    new XElement("IdMedioPago", Objeto.medioPago.ID),
                    new XElement("Fecha", Objeto.fecha),
                    new XElement("Importe", Objeto.importe)
                );

                _xmlDocument.Root.Element("Pagos").Add(nuevoPago);
            }

            _xmlDocument.Save(_filePath);
            // Recargar el documento XML desde el archivo
            _xmlDocument = XDocument.Load(_filePath);

            return true;
        }

        public BEClsPago ListarObjeto(BEClsPago Objeto)
        {
            var pagoElement = _xmlDocument.Descendants("Pago")
                .FirstOrDefault(p => p.Element("ID") != null && (int)p.Element("ID") == Objeto.ID);

            if (pagoElement != null)
            {
                Objeto.cliente = DALCliente.ListarObjeto(new BEClsCliente(int.Parse(pagoElement.Element("IdCliente").Value)));
                Objeto.reserva = DALReserva.ListarObjeto(new BEClsReserva(int.Parse(pagoElement.Element("IdReserva").Value)));
                Objeto.medioPago = DALMedioPago.ListarObjeto(new BEClsMedioPago(int.Parse(pagoElement.Element("IdMedioPago").Value)));
                Objeto.fecha = DateTime.Parse(pagoElement.Element("Fecha").Value);
                Objeto.importe = double.Parse(pagoElement.Element("Importe").Value);
            }

            return Objeto;
        }
        public List<BEClsPago> ObtenerPagosPorCliente(BEClsCliente cliente)
        {
            List<BEClsPago> pagos = new List<BEClsPago>();

            var pagoElements = _xmlDocument.Descendants("Pago")
                .Where(p => (int)p.Element("IdCliente") == cliente.ID);

            foreach (var pagoElement in pagoElements)
            {
                BEClsPago pago = new BEClsPago
                {
                    ID = (int)pagoElement.Element("ID"),
                    reserva = DALReserva.ListarObjeto(new BEClsReserva(int.Parse(pagoElement.Element("IdReserva").Value))),
                    medioPago = DALMedioPago.ListarObjeto(new BEClsMedioPago(int.Parse(pagoElement.Element("IdMedioPago").Value))),
                    fecha = DateTime.Parse(pagoElement.Element("Fecha").Value),
                    importe = double.Parse(pagoElement.Element("Importe").Value),
                    cliente = cliente
                };

                pagos.Add(pago);
            }

            return pagos;
        }

        public List<BEClsPago> ListarTodo()
        {
            List<BEClsPago> pagos = new List<BEClsPago>();

            foreach (var pagoElement in _xmlDocument.Descendants("Pago"))
            {
                BEClsPago pago = new BEClsPago();
                pago.cliente = DALCliente.ListarObjeto(new BEClsCliente(int.Parse(pagoElement.Element("IdCliente").Value)));
                pago.reserva = DALReserva.ListarObjeto(new BEClsReserva(int.Parse(pagoElement.Element("IdReserva").Value)));
                pago.medioPago = DALMedioPago.ListarObjeto(new BEClsMedioPago(int.Parse(pagoElement.Element("IdMedioPago").Value)));
                pago.fecha = DateTime.Parse(pagoElement.Element("Fecha").Value);
                pago.importe = double.Parse(pagoElement.Element("Importe").Value);
                pagos.Add(pago);
            }

            return pagos;
        }

    }
}
