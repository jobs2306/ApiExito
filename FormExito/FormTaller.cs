using FormExito.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormExito
{
    public partial class FormTaller : Form
    {
        public FormTaller()
        {
            InitializeComponent();
            CargarVehiculosEnTaller();
        }

        private void confButton1_Click(object sender, EventArgs e)
        {
            
        }

        private async void CargarVehiculosEnTaller()
        {
            var Metodos = new MetodosHttp();

            string link = Program.UrlApi + "controlvehiculo/taller";

            List<ControlVehiculo> ControlVehiculos = await Metodos.GetObject<List<ControlVehiculo>>(link);

            // Limpiar el panel antes de agregar nuevos elementos
            PanelContenedor.Controls.Clear();

            if(ControlVehiculos != null)
            {
                foreach (var controlVehiculo in ControlVehiculos)
                {
                    //Obtener el vehículo y cliente relacionado
                    link = Program.UrlApi + "vehiculo/" + controlVehiculo.Vehiculoid;
                    var vehiculo = await Metodos.GetObject<Vehiculo>(link);

                    link = Program.UrlApi + "cliente/" + vehiculo.Clienteid;
                    var cliente = await Metodos.GetObject<Cliente>(link);

                    //Panel para contener en general
                    Panel panel = new Panel
                    {
                        Height = 50,
                        Dock = DockStyle.Top
                    };

                    // Crear el Panel
                    Panel panelVehiculo = new Panel
                    {
                        Width = 1000,
                        Height = 50,
                        BorderStyle = BorderStyle.FixedSingle,
                        Padding = new Padding(5),
                        Dock = DockStyle.Left
                    };

                    //Crear paneles que almacen los datos
                    Panel panelPlaca = new Panel
                    {
                        Width = 200,
                        BorderStyle = BorderStyle.FixedSingle,
                        Padding = new Padding(5),
                        Dock = DockStyle.Left
                    };

                    Panel panelCliente = new Panel
                    {
                        Width = 200,
                        BorderStyle = BorderStyle.FixedSingle,
                        Padding = new Padding(5),
                        Dock = DockStyle.Left
                    };

                    Panel panelTecnico = new Panel
                    {
                        Width = 200,
                        BorderStyle = BorderStyle.FixedSingle,
                        Padding = new Padding(5),
                        Dock = DockStyle.Left
                    };

                    Panel panelFecha = new Panel
                    {
                        Width = 250,
                        BorderStyle = BorderStyle.FixedSingle,
                        Padding = new Padding(5),
                        Dock = DockStyle.Left
                    };

                    Panel panelButton = new Panel
                    {
                        Width = 135,
                        BorderStyle = BorderStyle.FixedSingle,
                        Padding = new Padding(2),
                        Dock = DockStyle.Left
                    };

                    // Etiqueta para la Placa
                    Label lblPlaca = new Label
                    {
                        Text = $"Placa: {vehiculo.placa}",
                        AutoSize = true,
                        Font = new Font("Arial",14),
                        Location = new Point(10, 10),
                        Dock = DockStyle.Left
                    };
                    
                    // Etiqueta para el Nombre del Cliente
                    Label lblCliente = new Label
                    {
                        Text = $"Cliente: {cliente.nombre}",
                        AutoSize = true,
                        Font = new Font("Arial", 14),
                        Location = new Point(10, 30),
                        Dock = DockStyle.Left
                    };

                    Label lblTecnico = new Label
                    {
                        Text = $"Técnico: {controlVehiculo.tecnico_encargado}",
                        AutoSize = true,
                        Font = new Font("Arial", 14),
                        Location = new Point(10, 30),
                        Dock = DockStyle.Left
                    };

                    Label lblFecha = new Label
                    {
                        Text = $"{controlVehiculo.fecha}",
                        AutoSize = true,
                        Font = new Font("Arial", 14),
                        Location = new Point(10, 30),
                        Dock = DockStyle.Left
                    };

                    // Botón "Ver Vehículo"
                    Button btnVerVehiculo = new Button
                    {
                        Text = "Ver Vehículo",
                        AutoSize = true,
                        //Location = new Point(10, 60),
                        Font = new Font("Arial", 14),
                        Tag = controlVehiculo.id, // Guardar el ID para identificarlo,
                        Dock = DockStyle.Left
                    };

                    panelPlaca.Controls.Add(lblPlaca);
                    panelCliente.Controls.Add(lblCliente);
                    panelTecnico.Controls.Add(lblTecnico);
                    panelButton.Controls.Add(btnVerVehiculo);
                    panelFecha.Controls.Add(lblFecha);

                    btnVerVehiculo.Click += BtnVerVehiculo_Click;

                    // Agregar controles al Panel
                    panelVehiculo.Controls.Add(panelButton);
                    panelVehiculo.Controls.Add(panelCliente);
                    panelVehiculo.Controls.Add(panelTecnico);
                    panelVehiculo.Controls.Add(panelPlaca);
                    panelVehiculo.Controls.Add(panelFecha);

                    // Agregar el Panel al FlowLayoutPanel
                    panel.Controls.Add(panelVehiculo);
                    PanelContenedor.Controls.Add(panel);
                }
            }
        }

        // Evento para abrir el Form con la información del vehículo
        private async void BtnVerVehiculo_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int controlId = (int)btn.Tag;

            var Metodos = new MetodosHttp();
            string link = Program.UrlApi + "controlvehiculo/" + controlId;

            ControlVehiculo controlVehiculo = await Metodos.GetObject<ControlVehiculo>(link);
            link = Program.UrlApi + "vehiculo/" + controlVehiculo.Vehiculoid;
            Vehiculo vehiculo = await Metodos.GetObject<Vehiculo>(link);
            link = Program.UrlApi + "cliente/" + vehiculo.Clienteid;
            Cliente cliente = await Metodos.GetObject<Cliente>(link);
            // Abrir el Form con la información del vehículo
            FormEntradaVehiculo formDetalle = new FormEntradaVehiculo(vehiculo, controlVehiculo, cliente);

            formDetalle.Show();
        }

    }
}
