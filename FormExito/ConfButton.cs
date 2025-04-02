using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace FormExito
{
    public class ConfButton : Button
    {
        private int borderSize = 0;
        private int borderRadio = 40;
        private Color borderColor = Color.PaleVioletRed;

        [Category("Conf Code")]
        public int BorderSize
        {
            get
            {
                return borderSize;
            }
            set
            {
                borderSize = value;
                this.Invalidate();
            }
        }
        [Category("Conf Code")]
        public int BorderRadio
        {
            get
            {
                return borderRadio;
            }
            set
            {
                borderRadio = value;
                this.Invalidate();
            }
        }
        [Category("Conf Code")]
        public Color BorderColor
        {
            get
            {
                return borderColor;
            }
            set
            {
                borderColor = value;
                this.Invalidate();
            }
        }
        [Category("Conf Code")]
        public Color BackGroundColor
        {
            get
            {
                return this.BackColor;
            }
            set
            {
                this.BackColor = value;
            }
        }
        [Category("Conf Code")]
        public Color TextColor
        {
            get
            {
                return this.ForeColor;
            }
            set
            {
                this.ForeColor = value;
            }
        }

        //Consutructor
        public ConfButton()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Size = new Size(150, 40);
            this.BackColor = Color.MediumSlateBlue;
            this.ForeColor = Color.White;
            //this.MouseDownBackColor = Color.Black;
        }

        //metodos

        private GraphicsPath GetFigurePath(RectangleF rect, float radius)
        {
            GraphicsPath Path = new GraphicsPath();
            Path.StartFigure();
            Path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            Path.AddArc(rect.Width - radius, rect.Y, radius, radius, 270, 90);
            Path.AddArc(rect.Width - radius, rect.Height - radius, radius, radius, 0, 90);
            Path.AddArc(rect.X, rect.Height - radius, radius, radius, 90, 90);
            Path.CloseFigure();

            return Path;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            RectangleF rectSurface = new RectangleF(0, 0, this.Width, this.Height);
            RectangleF rectBorder = new RectangleF(1, 1, this.Width - 0.8F, this.Height - 1);

            if (BorderRadio > 2)
            {
                using (GraphicsPath pathSurface = GetFigurePath(rectSurface, BorderRadio))
                using (GraphicsPath pathBorder = GetFigurePath(rectBorder, BorderRadio - 1F))
                using (Pen penSurface = new Pen(this.Parent.BackColor, 2))
                using (Pen penBorder = new Pen(BorderColor, BorderSize))
                {
                    penBorder.Alignment = PenAlignment.Inset;
                    this.Region = new Region(pathSurface);

                    e.Graphics.DrawPath(penSurface, pathSurface);

                    if (BorderSize >= 1)
                    {
                        e.Graphics.DrawPath(penBorder, pathBorder);
                    }
                }
            }
            else
            {
                this.Region = new Region(rectSurface);
                if (BorderSize >= 1)
                {
                    using (Pen penBorder = new Pen(BorderColor, BorderSize))
                    {
                        penBorder.Alignment = PenAlignment.Inset;
                        e.Graphics.DrawRectangle(penBorder, 0, 0, this.Width - 1, this.Height - 1);
                    }
                }
            }
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.Parent.BackColorChanged += new EventHandler(Container_BackColorChanged);
        }
        private void Container_BackColorChanged(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                this.Invalidate();
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
    }
}