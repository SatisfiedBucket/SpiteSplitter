using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SpiteSplitter
{
    public class RoundButton : Button
    {
        private const int radius = 5;
        private Color hoverBackColor = Color.LightGray;
        private Color hoverForeColor = Color.Black;
        private Color originalBackColor;
        private Color originalForeColor;

        public RoundButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
            originalBackColor = BackColor;
            originalForeColor = ForeColor;
        }

        public Color HoverBackColor
        {
            get { return hoverBackColor; }
            set { hoverBackColor = value; }
        }

        public Color HoverForeColor
        {
            get { return hoverForeColor; }
            set { hoverForeColor = value; }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            originalBackColor = BackColor;
            originalForeColor = ForeColor; 
            BackColor = hoverBackColor; 
            ForeColor = hoverForeColor; 
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            BackColor = originalBackColor; 
            ForeColor = originalForeColor;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            GraphicsPath roundedRect = GetRoundedRect(ClientRectangle);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (SolidBrush brush = new SolidBrush(BackColor))
            {
                e.Graphics.FillPath(brush, roundedRect);
            }

            // Draw the text
            TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private GraphicsPath GetRoundedRect(Rectangle bounds)
        {
            GraphicsPath roundedRect = new GraphicsPath();

            bounds.Width -= 1;
            bounds.Height -= 1;

            roundedRect.AddArc(bounds.Left, bounds.Top, radius * 2, radius * 2, 180, 90);
            roundedRect.AddArc(bounds.Right - radius * 2, bounds.Top, radius * 2, radius * 2, 270, 90);
            roundedRect.AddArc(bounds.Right - radius * 2, bounds.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            roundedRect.AddArc(bounds.Left, bounds.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);

            roundedRect.CloseFigure();

            return roundedRect;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            SetRegion();
        }

        private void SetRegion()
        {
            Region = new Region(GetRoundedRect(ClientRectangle));
        }
    }
}
