using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormExito.Model
{
    public class ControlVehiculo
    {
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public string tecnico_encargado { get; set; }
        public string? condicion_mecanica { get; set; }
        public int kilometraje { get; set; }
        public int nivel { get; set; }
        public string? trabajo_realizar { get; set; }
        public string? observacion { get; set; }
        public int? dias_garantia { get; set; }
        public DateTime? fecha_salida { get; set; }

        public int Vehiculoid { get; set; }
        public Vehiculo? Vehiculo { get; set; }

        public bool EspejoDerecho { get; set; }
        public bool EspejoIzquierdo { get; set; }
        public bool EspejoRetro { get; set; }
        public bool RejillaAire { get; set; }
        public bool Tapete { get; set; }
        public bool Plumillas { get; set; }
        public bool MemoriaUsb { get; set; }
        public bool TapaGasolian { get; set; }
        public bool Bateria { get; set; }
        public bool Radio { get; set; }
        public bool VidriosPuertas { get; set; }
        public bool PanoramicoDel { get; set; }
        public bool PanoramicoTra { get; set; }
        public bool LlantaRep { get; set; }
        public bool PlacaDel { get; set; }
        public bool PlacaTra { get; set; }
        public bool MedidorAceite { get; set; }
        public bool TapasLlanta { get; set; }
        public bool LuzDelDer { get; set; }
        public bool LuzDelIz1 { get; set; }
        public bool LuzTrasDer { get; set; }
        public bool LuzTrasIz1 { get; set; }
        public bool Rayones { get; set; }
        public bool Pangones { get; set; }
        public bool KitCarrera { get; set; }
        public bool TapaRadiador { get; set; }
        public bool MarquillaCromada { get; set; }
        public string? OtrosAccesorios { get; set; }
    }
}
