namespace ApiExito.Model
{
    public class ControlVehiculo
    {
        public int id {  get; set; }
        public DateTime fecha { get; set; }
        public string condicion_mecanica { get; set; }
        public int kilometraje { get; set; }
        public string trabajo_realizar { get; set; }
        public string observacion {  get; set; }
        public int dias_garantia { get; set; }
        public DateTime fecha_salida { get; set; }

        public int Vehiculoid { get; set; }
        public Vehiculo Vehiculo { get; set; }
    }
}
