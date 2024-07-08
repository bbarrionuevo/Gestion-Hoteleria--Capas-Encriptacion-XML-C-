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
    public class DALClsMedioPago : IGestor<BEClsMedioPago>
    {
        private XDocument _xmlDocument;
        private string _filePath;
        BEClsMedioPago BEMedioPago;

        public DALClsMedioPago(string filePath = "Archivo.xml")
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);

            if (!File.Exists(_filePath))
            {
                _xmlDocument = new XDocument(new XElement("MediosPago"));
                _xmlDocument.Save(_filePath);
            }
            else
            {
                _xmlDocument = XDocument.Load(_filePath);
            }

            BEMedioPago = new BEClsMedioPago();
        }
        private int ObtenerNuevoId()
        {
            try
            {
                List<int> ids = _xmlDocument.Descendants("MedioPago")
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
        public bool Baja(BEClsMedioPago Objeto)
        {
            // Recargar el documento XML desde el archivo
            _xmlDocument = XDocument.Load(_filePath);

            var medioPagoElement = _xmlDocument.Descendants("MedioPago")
                .FirstOrDefault(mp => mp.Element("ID") != null && (int)mp.Element("ID") == Objeto.ID);

            if (medioPagoElement != null)
            {
                medioPagoElement.Remove();
                _xmlDocument.Save(_filePath);
                // Recargar el documento XML desde el archivo
                _xmlDocument = XDocument.Load(_filePath);

                return true;
            }

            return false;
        }

        public bool Guardar(BEClsMedioPago Objeto)
        {
            // Recargar el documento XML desde el archivo
            _xmlDocument = XDocument.Load(_filePath);

            var medioPagoElement = _xmlDocument.Descendants("MedioPago")
                .FirstOrDefault(mp => mp.Element("ID") != null && (int)mp.Element("ID") == Objeto.ID);

            if (medioPagoElement != null)
            {
                // Actualizar el medio de pago existente

                medioPagoElement.Element("tipo").Value = Objeto.tipo;
                medioPagoElement.Element("titular").Value = Objeto.titular;
                medioPagoElement.Element("numero").Value = Objeto.numero;
                medioPagoElement.Element("vencimiento").Value = Objeto.vencimiento.ToString();
                medioPagoElement.Element("cuotas").Value = Objeto.cuotas.ToString();
            }
            else
            {
                // Agregar un nuevo medio de pago
                int nuevoId = ObtenerNuevoId();
                Objeto.ID = nuevoId;
                XElement nuevoMedioPago = new XElement("MedioPago",
                    new XElement("ID", Objeto.ID),
                    new XElement("tipo", Objeto.tipo),
                    new XElement("titular", Objeto.titular),
                    new XElement("numero", Objeto.numero),
                    new XElement("vencimiento", Objeto.vencimiento),
                    new XElement("cuotas", Objeto.cuotas)
                );

                _xmlDocument.Root.Element("MediosPago").Add(nuevoMedioPago);
                
            }

            _xmlDocument.Save(_filePath);
            // Recargar el documento XML desde el archivo
            _xmlDocument = XDocument.Load(_filePath);

            return true;
        }

        public BEClsMedioPago ListarObjeto(BEClsMedioPago Objeto)
        {
            var medioPagoElement = _xmlDocument.Descendants("MedioPago")
                .FirstOrDefault(mp => mp.Element("ID") != null && (int)mp.Element("ID") == Objeto.ID);

            if (medioPagoElement != null)
            {
                Objeto.tipo = medioPagoElement.Element("tipo").Value;
                Objeto.titular = medioPagoElement.Element("titular").Value;
                Objeto.numero = medioPagoElement.Element("numero").Value;
                Objeto.vencimiento = int.Parse(medioPagoElement.Element("vencimiento").Value);
                Objeto.cuotas = int.Parse(medioPagoElement.Element("cuotas").Value);
            }

            return Objeto;
        }

        public List<BEClsMedioPago> ListarTodo()
        {
            List<BEClsMedioPago> mediosPago = new List<BEClsMedioPago>();

            foreach (var medioPagoElement in _xmlDocument.Descendants("MedioPago"))
            {
                BEClsMedioPago medioPago = new BEClsMedioPago
                {
                    ID = (int)medioPagoElement.Element("ID"),
                    tipo = medioPagoElement.Element("tipo").Value,
                    titular = medioPagoElement.Element("titular").Value,
                    numero = medioPagoElement.Element("numero").Value,
                    vencimiento = int.Parse(medioPagoElement.Element("vencimiento").Value),
                    cuotas = int.Parse(medioPagoElement.Element("cuotas").Value)
                };

                mediosPago.Add(medioPago);
            }

            return mediosPago;
        }
        
    }

}
