using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace FormExito
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            textPassword.UseSystemPasswordChar = true;
        }

        private async void ButSesion_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textUser.Text) && !string.IsNullOrEmpty(textPassword.Text))
            {
                string user = textUser.Text;
                string password = textPassword.Text;

                var login = new LoginDto
                {
                    Email = user,
                    Password = password
                };

                string link = Program.UrlApi + "auth/login";
                var Metodos = new MetodosHttp();

                using (var client = new HttpClient())
                {

                    var response = await client.PostAsJsonAsync(link, login);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var tokenData = JsonSerializer.Deserialize<JsonElement>(responseBody);

                        // Almacenar el token
                        Program.AccessToken = tokenData.GetProperty("token").GetString();

                        //Guardar usuario y contraseña
                        Program.User = user;
                        Program.Password = password;

                        // Autenticación exitosa
                        MessageBox.Show("Inicio de sesión exitoso.");
                        textPassword.Text = "";
                        textUser.Text = "";

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Error al iniciar sesión: {response.StatusCode}\n{error}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe indicar contraseña y usuario");
            }
        }

        private void HidePassword_Click(object sender, EventArgs e)
        {
            bool band = textPassword.UseSystemPasswordChar;

            if (band == false)
            {
                textPassword.UseSystemPasswordChar = true;
            }
            else
            {
                textPassword.UseSystemPasswordChar = false;
            }
        }

        private void textPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ButSesion_Click(sender, e);
            }
        }

        private void textUser_KeyDown(object sender, KeyEventArgs e)
        {
        }
    }
}
