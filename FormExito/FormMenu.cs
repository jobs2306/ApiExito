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
    public partial class FormMenu : Form
    {
        public FormMenu()
        {
            InitializeComponent();
        }

        private void ButIngreso_Click(object sender, EventArgs e)
        {
            //Se permiten varios Form para poder ingresar varios vehiculos
            FormEntradaVehiculo Entrada = new FormEntradaVehiculo();
            Entrada.Show();
        }

        private void ButTaller_Click(object sender, EventArgs e)
        {
            // Buscar si ya existe un formulario abierto con el nombre "FormHistorial"
            foreach (Form form in Application.OpenForms)
            {
                if (form is FormTaller)
                {
                    form.BringToFront(); // Traer al frente si ya está abierto
                    return; // Salir del método sin abrir una nueva instancia
                }
            }

            // Si no hay ninguno abierto, creamos una nueva instancia
            FormTaller Taller = new FormTaller();
            Taller.Show();
        }

        private void ButHistorial_Click(object sender, EventArgs e)
        {
            // Buscar si ya existe un formulario abierto con el nombre "FormHistorial"
            foreach (Form form in Application.OpenForms)
            {
                if (form is FormVehiculos)
                {
                    form.BringToFront(); // Traer al frente si ya está abierto
                    return; // Salir del método sin abrir una nueva instancia
                }
            }

            // Si no hay ninguno abierto, creamos una nueva instancia
            FormVehiculos historial = new FormVehiculos();
            historial.Show();
        }

        private void ButLogin_Click(object sender, EventArgs e)
        {
            // Buscar si ya existe un formulario abierto con el nombre "FormHistorial"
            foreach (Form form in Application.OpenForms)
            {
                if (form is LoginForm)
                {
                    form.BringToFront(); // Traer al frente si ya está abierto
                    return; // Salir del método sin abrir una nueva instancia
                }
            }

            // Si no hay ninguno abierto, creamos una nueva instancia
            LoginForm Login = new LoginForm();
            Login.Show();
        }
    }
}
