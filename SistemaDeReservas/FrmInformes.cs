using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Windows.Forms.DataVisualization.Charting;
using BLL;
using BE;
using System.Security.Cryptography;
using System.Security.Policy;

namespace SistemaDeReservas
{
    public partial class FrmInformes : Form
    {
        BLLClsCliente BLLcliente;
        BLLClsReserva BLLreserva;
        BLLClsHabitacion BLLHabitacion;
        BLLClsPago BLLPago;

        public FrmInformes()
        {
            InitializeComponent();
            BLLcliente = new BLLClsCliente();
            BLLreserva = new BLLClsReserva();
            BLLPago=new BLLClsPago();
            BLLHabitacion = new BLLClsHabitacion();
        }

        private void ConfigurarChart(DateTime Desde, DateTime Hasta)
        {
            // Configurar el tipo de gráfico
            chart1.Series.Clear();
            chart1.Series.Add("Reservas");
            chart1.Series["Reservas"].ChartType = SeriesChartType.Column;

            // Obtener todas las reservas
            var reservas = BLLreserva.ListarTodo();

            // Filtrar las reservas por las fechas seleccionadas
            var reservasFiltradas = reservas.Where(r => r.FechaCheckIn >= Desde && r.FechaCheckIn <= Hasta);

            // Agrupar las reservas por fecha y contarlas
            var reservasPorDia = reservasFiltradas.GroupBy(r => r.FechaCheckIn.Date).Select(g => new { Fecha = g.Key, Cantidad = g.Count() });

            // Agregar los datos al gráfico
            foreach (var reserva in reservasPorDia)
            {
                chart1.Series["Reservas"].Points.AddXY(reserva.Fecha, reserva.Cantidad);
            }

            // Configurar el título y los ejes del gráfico
            chart1.Titles.Clear();
            chart1.Titles.Add("Informe de Reservas por Día");
            chart1.ChartAreas[0].AxisX.Title = "Fecha";
            chart1.ChartAreas[0].AxisY.Title = "Cantidad de Reservas";

            // Configurar el segundo gráfico
            chart2.Series.Clear();
            chart2.Series.Add("Estados");
            chart2.Series["Estados"].ChartType = SeriesChartType.Pie;

            // Agrupar las reservas por estado y contarlas
            var reservasPorEstado = reservasFiltradas.GroupBy(r => r.Estado).Select(g => new { Estado = g.Key, Cantidad = g.Count() });

            // Agregar los datos al gráfico
            foreach (var reserva in reservasPorEstado)
            {
                chart2.Series["Estados"].Points.AddXY(reserva.Estado, reserva.Cantidad);
            }

            // Configurar el título del gráfico

            chart2.Titles.Clear();
            chart2.Titles.Add("Informe de Reservas por Estado");
        }

        private void MostrarPagos(DateTime Desde, DateTime Hasta)
        {
            // Limpiar el DataGridView
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            // Obtener todos los pagos
            var pagos = BLLPago.ListarTodo();

            // Filtrar los pagos por las fechas seleccionadas
            var pagosFiltrados = pagos.Where(p => p.fecha >= Desde && p.fecha <= Hasta);

            // Crear una tabla para mostrar los pagos
            DataTable tablaPagos = new DataTable();
            tablaPagos.Columns.Add("Cliente");
            tablaPagos.Columns.Add("Reserva");
            tablaPagos.Columns.Add("Medio de Pago");
            tablaPagos.Columns.Add("Fecha");
            tablaPagos.Columns.Add("Importe");

            // Agregar los pagos a la tabla
            foreach (var pago in pagosFiltrados)
            {
                tablaPagos.Rows.Add(pago.cliente.ID, pago.reserva.ID, pago.medioPago.tipo, pago.fecha.ToString("dd/MM/yyyy"), pago.importe.ToString("C"));
            }

            // Mostrar la tabla en el DataGridView
            dataGridView1.DataSource = tablaPagos;
        }
        private void MostrarTotalReservas(DateTime Desde, DateTime Hasta)
        {
            // Limpiar el texto de label
            label7.Text = "";
            // Obtener todas las reservas
            var reservas = BLLreserva.ListarTodo();

            // Filtrar las reservas por las fechas seleccionadas
            var reservasFiltradas = reservas.Where(r => r.FechaCheckIn >= Desde && r.FechaCheckIn <= Hasta);

            // Calcular el número total de reservas
            int totalReservas = reservasFiltradas.Count();

            // Mostrar el total en el label
            label7.Text = "Total de reservas: " + totalReservas;

            // Ajustar el tamaño del texto en label
            label7.Font = new Font(label7.Font.FontFamily, 16);
        }
        private void MostrarTotalPagos(DateTime Desde, DateTime Hasta)
        {
            // Limpiar el texto de label3
            label3.Text = "";

            // Obtener todos los pagos
            var pagos = BLLPago.ListarTodo();

            // Filtrar los pagos por las fechas seleccionadas
            var pagosFiltrados = pagos.Where(p => p.fecha >= Desde && p.fecha <= Hasta);

            // Calcular la suma total de los pagos
            double totalPagos = pagosFiltrados.Sum(p => p.importe);

            // Mostrar el total en el label
            label3.Text = "Total de pagos: " + totalPagos.ToString("C");

            // Ajustar el tamaño del texto en label3
            label3.Font = new Font(label3.Font.FontFamily, 16);
        }




        private void chart1_Click(object sender, EventArgs e)
        {
            
        }

        private void FrmInformes_Load(object sender, EventArgs e)
        {

        }


        //Establese las fechas seleccionadas y genera los informes
        private void button3_Click(object sender, EventArgs e)
        {
            // Obtiene las fechas desde los controles DateTimePicker
            DateTime fechaDesde = dtpDesde.Value;
            DateTime fechaHasta = dtpHasta.Value;

            // Configura los gráficos con los datos filtrados por las fechas seleccionadas
            ConfigurarChart(fechaDesde, fechaHasta);

            // Muestra los pagos realizados en las fechas seleccionadas
            MostrarPagos(fechaDesde, fechaHasta);
            // Muestra el total de pagos en las fechas seleccionadas
            MostrarTotalPagos(fechaDesde, fechaHasta);
            // Muestra el total de reservas 

            MostrarTotalReservas(fechaDesde, fechaHasta);
        }

        //Establese las fechas en 7 dias y genera los informes
        private void button4_Click(object sender, EventArgs e)
        {
            // Establecer "Desde" como 7 días antes del día actual
            DateTime Desde = DateTime.Today.AddDays(-7);

            // Establecer "Hasta" como el final del día actual
            DateTime Hasta = DateTime.Today.AddDays(1).AddTicks(-1);

            // Llamar a las funciones con las fechas seleccionadas
            ConfigurarChart(Desde, Hasta);
            MostrarPagos(Desde, Hasta);
            MostrarTotalPagos(Desde, Hasta);
            MostrarTotalReservas(Desde, Hasta);
        }

        //Establese las fechas en ultimo mes y genera los informes
        private void button5_Click(object sender, EventArgs e)
        {

            // Establecer "Desde" como el inicio del mes anterior
            DateTime Desde = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);

            // Establecer "Hasta" como el final del mes anterior
            DateTime Hasta = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddDays(-1);


            // Llamar a las funciones con las fechas seleccionadas
            ConfigurarChart(Desde, Hasta);
            MostrarPagos(Desde, Hasta);
            MostrarTotalPagos(Desde, Hasta);
            MostrarTotalReservas(Desde, Hasta);
        }

        //Establese las fechas en un año y genera los informes
        private void button6_Click(object sender, EventArgs e)
        {
            // Establecer "Desde" como el inicio del año actual
            DateTime Desde = new DateTime(DateTime.Today.Year, 1, 1);

            // Establecer "Hasta" como el final del día actual
            DateTime Hasta = DateTime.Today.AddDays(1).AddTicks(-1);

            

            // Llamar a las funciones con las fechas seleccionadas
            ConfigurarChart(Desde, Hasta);
            MostrarPagos(Desde, Hasta);
            MostrarTotalPagos(Desde, Hasta);
            MostrarTotalReservas(Desde, Hasta);
        }
    }
}