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

        private void FrameBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawString(Text, this.Font, Brushes.Green, new Point(2, 2));

        }

        private void FrameBox_Load(object sender, EventArgs e)
        {
            
        }
    }
}
