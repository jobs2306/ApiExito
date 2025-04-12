using FormExito.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using static System.Windows.Forms.LinkLabel;
using System.Reflection.Metadata;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FormExito
{
    public partial class FormEntradaVehiculo : Form
    {
        private bool _isUpdate = false;

        Vehiculo vehiculo1 = new Vehiculo();
        Cliente cliente1 = new Cliente();
        ControlVehiculo control1 = new ControlVehiculo();

        public FormEntradaVehiculo()
        {
            InitializeComponent();
            this.Size = new Size(1280, 1080);
            panelUpdate.Visible = false;
            // panelss.Size = new Size(1280, 5);
            _isUpdate = false;
        }

        public FormEntradaVehiculo(Vehiculo vehiculo, ControlVehiculo control, Cliente cliente, bool History) : this()
        {
            //InitializeComponent();
            this.Size = new Size(1280, 1080);
            panelUpdate.Visible = true;
            ButGenerarIngreso.Text = "Actualizar";
            LoadFormVehiculo(vehiculo, control, cliente);
            vehiculo1 = vehiculo;
            cliente1 = cliente;

            if(History)
            {
                if (control.fecha_salida != null)
                    dateTimeSalida.Value = control.fecha_salida.Value;
                else
                    dateTimeSalida.Visible = false;
            }
            
            
            control1 = control;
            _isUpdate = true;
            
        }

        private void LoadFormVehiculo(Vehiculo vehiculo, ControlVehiculo control, Cliente cliente)
        {
            textTecnico.Text = control.tecnico_encargado;
            textName.Text = cliente.nombre;
            textCc.Text = cliente.cc.ToString();
            textNit.Text = cliente.nit;
            textCelular.Text = cliente.celular;
            textPlaca.Text = vehiculo.placa;
            textMarca.Text = vehiculo.marca;
            textModelo.Text = vehiculo.modelo;
            textColor.Text = vehiculo.color;
            textTipo.Text = vehiculo.tipo;
            comboBoxGas.Text = vehiculo.diesel_gasolina;
            dateTimeIngreso.Value = control.fecha;
            richTextCondicionMec.Text = control.condicion_mecanica;
            textKm.Text = control.kilometraje.ToString();
            trackBarNivel.Value = control.nivel;
            richTextTrabajoRealizar.Text = control.trabajo_realizar;
            richTextObservaciones.Text = control.observacion;
            EspLatDer.Checked = control.EspejoDerecho;
            EspLatIzq.Checked = control.EspejoIzquierdo;
            EspRetro.Checked = control.EspejoRetro;
            RejillaAir.Checked = control.RejillaAire;
            Tapete.Checked = control.Tapete;
            Plumillas.Checked = control.Plumillas;
            MemUsb.Checked = control.MemoriaUsb;
            TapGas.Checked = control.TapaGasolian;
            Bateria.Checked = control.Bateria;
            Radio.Checked = control.Radio;
            VidriosPuerta.Checked = control.VidriosPuertas;
            PanorDel.Checked = control.PanoramicoDel;
            PanorTras.Checked = control.PanoramicoTra;
            LlantaRep.Checked = control.LlantaRep;
            PlacaDel.Checked = control.PlacaDel;
            PlacaTras.Checked = control.PlacaTra;
            MedAceite.Checked = control.MedidorAceite;
            TapaLlanta.Checked = control.TapasLlanta;
            LuzDelDer.Checked = control.LuzDelDer;
            LuzDelIzq.Checked = control.LuzDelIz1;
            LuzTrasDer.Checked = control.LuzTrasDer;
            LuzTrasIzq.Checked = control.LuzTrasIz1;
            Rayones.Checked = control.Rayones;

            Pangones.Checked = control.Pangones;
            KitCarretera.Checked = control.KitCarrera;
            TapaRadiador.Checked = control.TapaRadiador;
            MarqCromada.Checked = control.MarquillaCromada;
            richTextOtrosAccesorios.Text = control.OtrosAccesorios;
        }

        private async void ButGenerarIngreso_Click(object sender, EventArgs e)
        {
            //Datos cliente
            string Name = textName.Text;

            int CC = 0;
            if (!string.IsNullOrEmpty(textCc.Text))
                CC = Int32.Parse(textCc.Text);

            string Nit = textNit.Text;
            string Celular = textCelular.Text;

            //Datos Vehiculo
            string Placa = textPlaca.Text;
            string Marca = textMarca.Text;
            string Modelo = textModelo.Text;
            string Color = textColor.Text;
            string Tipo = textTipo.Text;
            string GasDiesel = comboBoxGas.Text;

            //Datos Control Vehiculo
            DateTime FechaIngreso = dateTimeIngreso.Value;
            string Tecnico = textTecnico.Text;
            string CondicionMec = richTextCondicionMec.Text;
            int Kilometraje = 0;
            if (!string.IsNullOrEmpty(textKm.Text))
                Kilometraje = Int32.Parse(textKm.Text);

            int Nivel = trackBarNivel.Value;
            string TrabajoRealizar = richTextTrabajoRealizar.Text;
            string Observacion = richTextObservaciones.Text;

            bool EspejoDer = EspLatDer.Checked;
            bool EspejoIzq = EspLatIzq.Checked;
            bool EspejoRetro = EspRetro.Checked;
            bool RejillaAire = RejillaAir.Checked;
            bool tapete = Tapete.Checked;
            bool plumillas = Plumillas.Checked;
            bool USB = MemUsb.Checked;
            bool TapaGasolina = TapGas.Checked;
            bool bateria = Bateria.Checked;
            bool radio = Radio.Checked;
            bool vidrioPuertas = VidriosPuerta.Checked;
            bool PanoramicoDel = PanorDel.Checked;
            bool PanoramicoTras = PanorTras.Checked;
            bool llantaRep = LlantaRep.Checked;
            bool placaDel = PlacaDel.Checked;
            bool placaTras = PlacaTras.Checked;
            bool MedidorAceite = MedAceite.Checked;
            bool TapasLlantas = TapaLlanta.Checked;
            bool LucesDelDer = LuzDelDer.Checked;
            bool LucesDelIzq = LuzDelIzq.Checked;
            bool LucesTrasDer = LuzTrasDer.Checked;
            bool LucesTrasIzq = LuzTrasIzq.Checked;
            bool Rayon = Rayones.Checked;
            bool Pangon = Pangones.Checked;
            bool Kit = KitCarretera.Checked;
            bool TapRad = TapaRadiador.Checked;
            bool MarqCrom = MarqCromada.Checked;

            string OtrosAccesorios = richTextOtrosAccesorios.Text;

            //Verificacion de datos
            if (string.IsNullOrEmpty(Tecnico))
            { MessageBox.Show("Debe indicar el Técnico encargado"); return; }

            if (string.IsNullOrEmpty(Nit) && CC == 0)
            { MessageBox.Show("Debe indicar NIT o CC"); return; }

            if (string.IsNullOrEmpty(Name))
            { MessageBox.Show("Debe indicar un nombre de cliente"); return; }

            if (string.IsNullOrEmpty(Placa) || string.IsNullOrEmpty(Marca) || string.IsNullOrEmpty(Modelo))
            { MessageBox.Show("Debe indicar Placa, Marca y Modelo del vehiculo"); return; }

            var Metodos = new MetodosHttp();

            if (Kilometraje == 0)
            {
                DialogResult result = MessageBox.Show(
                "Indicó kilometraje de 0 KM ¿Estás seguro de continuar?",
                "Confirmación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

                if (result == DialogResult.No)
                    return;
            }

            if (Nivel == 0)
            {
                DialogResult result = MessageBox.Show(
                    "Indicó nivel de combustible de 0, ¿Está seguro de continuar?",
                    "Confirmación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                    return;
            }

            //Verificar si el cliente existe o no
            string link = "";
            if (CC != 0)
            {
                link = Program.UrlApi + "cliente/cc/" + CC;
            }
            else
            {
                link = Program.UrlApi + "cliente/nit/" + Nit;
            }

            var cliente = new Cliente();

            cliente = await Metodos.GetObject<Cliente>(link);

            //Cliente no existe, se debe crear
            if (cliente == null)
            {
                cliente = new Cliente
                {
                    nombre = Name,
                    cc = CC,
                    nit = Nit,
                    celular = Celular
                };

                link = Program.UrlApi + "cliente";
                cliente = await Metodos.PostObject(link, cliente);

                //No se pudo crear el cliente
                if (cliente == null)
                {
                    MessageBox.Show("No se realizó el registro, verifique la información del cliente");
                    return;
                }
            }

            //Cliente existe o creado, se avanza a la segunda parte, vehiculo
            //Verificar si el vehiculo existe o no
            link = Program.UrlApi + "vehiculo/placa/" + Placa;
            var vehiculo = new Vehiculo();
            vehiculo = await Metodos.GetObject<Vehiculo>(link);

            //vehiculo no existe, se debe crear
            if (vehiculo == null)
            {
                vehiculo = new Vehiculo
                {
                    placa = Placa,
                    marca = Marca,
                    modelo = Modelo,
                    color = Color,
                    tipo = Tipo,
                    diesel_gasolina = GasDiesel,
                    Clienteid = cliente.id
                };

                link = Program.UrlApi + "vehiculo";
                vehiculo = await Metodos.PostObject(link, vehiculo);

                //No se pudo crear el vehiculo
                if (vehiculo == null)
                {
                    MessageBox.Show("No se puedo realizar el registro, verifique la información del vehiculo");
                    return;
                }

            }
            else
            {
                //En caso de que el vehiculo ya exista se debe verificar la información del cliente
                //la información se debe poder actualizar dinamicamente
                link = Program.UrlApi + "cliente/" + vehiculo.Clienteid;

                //Se debe tener si o si el mismo id, en caso de que no sea el mismo entonces se toma el correcto
                //y se actualiza el que se encuentre relacionado
                cliente.id = vehiculo.Clienteid;
                bool Update = await Metodos.PutObject(link, cliente);

                if (Update)
                {
                    link = Program.UrlApi + "cliente/" + cliente.id;
                    cliente = await Metodos.GetObject<Cliente>(link);
                }

                if (!Update)
                {
                    MessageBox.Show("Error, verifique la información del registro");
                    return;
                }

                //Se actualizan los datos en caso tal que el cliente para verificar
                textName.Text = cliente.nombre;
                textCc.Text = cliente.cc.ToString();
                textNit.Text = cliente.nit;
                textCelular.Text = cliente.celular;
            }

            //Vehiculo creado paso a control de entrada
            var Control = new ControlVehiculo
            {
                fecha = FechaIngreso,
                tecnico_encargado = Tecnico,
                condicion_mecanica = CondicionMec,
                kilometraje = Kilometraje,
                nivel = Nivel,
                trabajo_realizar = TrabajoRealizar,
                observacion = Observacion,

                Vehiculoid = vehiculo.id,

                fecha_salida = control1.fecha_salida,

                EspejoDerecho = EspejoDer,
                EspejoIzquierdo = EspejoIzq,
                EspejoRetro = EspejoRetro,
                RejillaAire = RejillaAire,
                Tapete = tapete,
                Plumillas = plumillas,
                MemoriaUsb = USB,
                TapaGasolian = TapaGasolina,
                Bateria = bateria,
                Radio = radio,
                VidriosPuertas = vidrioPuertas,
                PanoramicoDel = PanoramicoDel,
                PanoramicoTra = PanoramicoTras,
                LlantaRep = llantaRep,
                PlacaDel = placaDel,
                PlacaTra = placaTras,
                MedidorAceite = MedidorAceite,
                TapasLlanta = TapasLlantas,
                LuzDelDer = LucesDelDer,
                LuzDelIz1 = LucesDelIzq,
                LuzTrasDer = LucesTrasDer,
                LuzTrasIz1 = LucesTrasIzq,
                Rayones = Rayon,
                Pangones = Pangon,
                KitCarrera = Kit,
                TapaRadiador = TapRad,
                MarquillaCromada = MarqCrom,
                OtrosAccesorios = OtrosAccesorios
            };

            //Verificar si se va a crear o actualizar
            if (!_isUpdate)
            {
                link = Program.UrlApi + "controlvehiculo";
                Control = await Metodos.PostObject<ControlVehiculo>(link, Control);

                if (Control == null)
                {
                    MessageBox.Show("Error, no se pudo crear el registro, verifique la información");
                    return;
                }

                MessageBox.Show("Entrada generada satisfactoriamente");
                ResetFormControls(this);
            }
            //Actualizar estado
            else
            {
                link = Program.UrlApi + "controlvehiculo/" + control1.id;
                Control.id = control1.id;
                control1 = Control;
                bool Update = await Metodos.PutObject<ControlVehiculo>(link, Control);

                if (Update == false)
                {
                    MessageBox.Show("Error, no se pudo actualizar el registro, verifique la información");
                    return;
                }

                MessageBox.Show("Entrada actualizada satisfactoriamente");
            }
        }

        private void textNit_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir números
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != '-')
                e.Handled = true; // Bloquear cualquier otra tecla

            // Solo permitir un -
            if (e.KeyChar == '-' && textNit.Text.Contains("-"))
                e.Handled = true; // Bloquear si ya hay -

        }

        private void textCc_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir números
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                e.Handled = true; // Bloquear cualquier otra tecla

        }

        private void textCelular_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir números
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                e.Handled = true; // Bloquear cualquier otra tecla

        }

        private void textKm_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir números
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != '-')
                e.Handled = true; // Bloquear cualquier otra tecla
        }

        //Evento para resetear los controladores
        private void ResetFormControls(Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (c is TextBox textBox)
                    textBox.Clear(); // Limpiar los TextBox

                else if (c is ComboBox comboBox)
                    comboBox.SelectedIndex = -1; // Reiniciar ComboBox

                else if (c is CheckBox checkBox)
                    checkBox.Checked = false; // Desmarcar CheckBox

                else if (c is RadioButton radioButton)
                    radioButton.Checked = false; // Desmarcar RadioButton

                else if (c is NumericUpDown numericUpDown)
                    numericUpDown.Value = numericUpDown.Minimum; // Resetear NumericUpDown

                else if (c is DateTimePicker dateTimePicker)
                    dateTimePicker.Value = DateTime.Now; // Reiniciar DateTimePicker

                else if (c is TrackBar trackbar)
                    trackbar.Value = trackbar.Minimum; // Resetear TrackBar

                else if (c is RichTextBox richTextBox)
                    richTextBox.Clear(); // Limpiar RichTextBox

                else if (c.HasChildren)
                    ResetFormControls(c); // Llamada recursiva para contenedores como Panel, GroupBox, etc.
            }
        }

        private void ButSalida_Click(object sender, EventArgs e)
        {
            control1.fecha_salida = dateTimeSalida.Value;
            ButGenerarIngreso_Click(sender, e);
            this.Close();
        }
    }
}
