namespace FormExito
{
    internal static class Program
    {
        //Cambiar por la URL local donde se ejecuta la API 
        public static string UrlApi { get; set; } = "http://localhost:5005/api/";
        
        //Token de Acceso
        public static string AccessToken { get; set; }

        //usuario y contraseña
        public static string User {  get; set; }
        public static string Password { get; set; }
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            //Application.Run(new FormEntradaVehiculo());

            
            // Mostrar el LoginForm primero
            using (var loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    // Si la autenticación es exitosa, abrir el MainForm
                    Application.Run(new FormMenu());
                }
                else
                {
                    // Si el usuario no se autentica, cerrar la aplicación
                    Application.Exit();
                }
            }
            
        }
    }
}