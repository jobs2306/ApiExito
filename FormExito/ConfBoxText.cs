using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace FormExito
{
    public class ConfBoxText : UserControl
    {
        private TextBox textBox = new TextBox();
        private int borderSize = 2;
        private int borderRadius = 20;
        private Color borderColor = Color.PaleVioletRed;
        private Color backgroundColor = Color.White;

        [Category("Conf Code")]
        public int BorderSize
        {
            get { return borderSize; }
            set { borderSize = Math.Max(0, value); this.Invalidate(); }
        }

        [Category("Conf Code")]
        public int BorderRadius
        {
            get { return borderRadius; }
            set { borderRadius = Math.Max(0, value); this.Invalidate(); }
        }

        [Category("Conf Code")]
        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; this.Invalidate(); }
        }

        [Category("Conf Code")]
        public Color BackGroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; this.Invalidate(); }
        }

        [Category("Conf Code")]
        public Color TextColor
        {
            get { return textBox.ForeColor; }
            set { textBox.ForeColor = value; }
        }

        [Category("Conf Code")]
        public override string Text
        {
            get { return textBox.Text; }
            set { textBox.Text = value; }
        }

        public ConfBoxText()
        {
            this.Size = new Size(200, 40);
            this.Padding = new Padding(8);
            this.BackColor = Color.Transparent;
            this.DoubleBuffered = true; // Evita parpadeos

            textBox.Parent = this;
            textBox.BorderStyle = BorderStyle.None;
            textBox.Font = new Font("Arial", 10);
            textBox.Location = new Point(10, 10);
            textBox.Width = this.Width - 20;
            textBox.BackColor = Color.White;
            textBox.ForeColor = Color.Black;
            textBox.TextAlign = HorizontalAlignment.Left;

            this.Controls.Add(textBox);
            this.Resize += new EventHandler(Control_Resize);

            // Optimiza el renderizado
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void Control_Resize(object sender, EventArgs e)
        {
            textBox.Width = this.Width - 20;
            textBox.Height = this.Height - 20;
        }

        private GraphicsPath GetFigurePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int curveSize = radius * 2;

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();

            return path;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rectBorder = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            using (GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius))
            using (Pen penBorder = new Pen(borderColor, borderSize))
            using (SolidBrush brush = new SolidBrush(backgroundColor))
            {
                // Fondo
                this.Region = new Region(pathBorder);
                e.Graphics.FillPath(brush, pathBorder);

                // Borde
                if (borderSize > 0)
                {
                    penBorder.Alignment = PenAlignment.Center;
                    e.Graphics.DrawPath(penBorder, pathBorder);
                }
            }
        }
    }
}
