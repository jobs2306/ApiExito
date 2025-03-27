namespace ApiExito.Model
{
    public class Vehiculo
    {
        public int id {  get; set; }
        public string placa { get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }
        public string color { get; set; }
        public string tipo { get; set; }
        public string diesel_gasolina { get; set; }
        public int Clienteid { get; set; }
        public Cliente? Cliente { get; set; }
    }
}
