using Jazz2AnimStation.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace Jazz2AnimStation.CustomControls
{
    public partial class FrameBox : UserControl
    {
       
        private HorizontalAlignment textAlign;
        private Image image;
        private bool IsHovered = false;

        public ProjectFrame Source { get; set; }

        [Localizable(true)]
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public Color HoverBackColor { get; set; } = SystemColors.GradientInactiveCaption;

        [Localizable(true)]
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public Color NormalBackColor { get; set; } = SystemColors.Control;

        [Localizable(true)]
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public Color FocusBackColor { get; set; } = SystemColors.GradientActiveCaption;

        public FrameBox()
        {
            InitializeComponent();
            DoubleBuffered = true;
            
        }


        //
        // Summary:
        //     Gets or sets the current text in the frame box.
        //
        // Returns:
        //     The text displayed in the control.
        [Localizable(true)]
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }


        //
        // Summary:
        //     Gets or sets the image that is displayed by System.Windows.Forms.PictureBox.
        //
        // Returns:
        //     The System.Drawing.Image to display.
        [Localizable(true)]
        [Bindable(true)]
        public Image Image
        {
            get
            {
                return base.BackgroundImage;//return image;
            }
            set
            {
                base.BackgroundImage = value;
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            this.Focus();
            base.OnMouseDown(e);
        }

        protected override void OnEnter(EventArgs e)
        {
            this.Invalidate();
            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            this.Invalidate();
            base.OnLeave(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            this.BackColor = this.FocusBackColor;
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            this.BackColor = this.IsHovered ? this.HoverBackColor : this.NormalBackColor;
            base.OnLostFocus(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            IsHovered = true;
            this.BackColor = this.Focused ? this.FocusBackColor : this.HoverBackColor;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            IsHovered = false;
            this.BackColor = this.NormalBackColor;
            base.OnMouseLeave(e);
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.Focused)
            {
                var rc = this.ClientRectangle;
                rc.Inflate(-2, -2);
                ControlPaint.DrawFocusRectangle(e.Graphics, rc);
                Rectangle rc2 = new Rectangle(0, 0, 0, 0);
                //rc2.Inflate(-15, -15);
                ControlPaint.DrawSelectionFrame(e.Graphics, true, this.ClientRectangle, rc2, Color.Black);
            }

            e.Graphics.DrawString(Text, this.Font, Brushes.Green, new Point(2, 2));
        }



        private void FrameBox_Load(object sender, EventArgs e)
        {
            
        }
    }
}
