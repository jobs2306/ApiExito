using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormExito
{
    internal class PaqVehiculos
    {
        public Panel Contenedor = new Panel();
        public ConfButton button = new ConfButton();
        public Panel Espacio = new Panel();
        public int idSeg = 0;
        public List<CheckBox> Checks = new List<CheckBox>();
        public Panel PanelFases = new Panel();
        public Panel PFases = new Panel();
        public CheckBox CheckMaterial = new CheckBox();
        public ConfButton ButPieza = new ConfButton();

        public int IdPieza;
        public int IdPiezaF;
        public List<int> IdFaseF = new List<int>();
    }
}
