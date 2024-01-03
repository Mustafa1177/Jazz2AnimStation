using Jazz2AnimStation.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Jazz2AnimStation.CustomControls
{
    internal class AnimationBox : PictureBox
    {
        public ProjectAnimation Source { get; set; }

        public bool ImageCentered { get; set; }

        public bool ConsiderFrameOffset { get; set; } //hotspot

        private int currentFrameIndex = 0;

        private ProjectFrame currentFrame = null;

        private DateTime lastAnimationUpdate = DateTime.Now;

        public int CurrentFrameIndex 
        { get { return currentFrameIndex; }
            set 
            {
                if (Source != null)
                    if(value < Source.FrameCount)
                        currentFrameIndex = value;
                    else if(Source.FrameCount > 0) 
                        currentFrameIndex = 0;
                    else
                        CurrentFrameIndex = -1;
                else
                    CurrentFrameIndex = -1;           
            }
        }

        public ProjectFrame GetCurrentFrame
        {
            get { return currentFrame; }
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


        public AnimationBox() 
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // AnimationBox
            // 
            this.SetStyle(ControlStyles.Selectable, true);
            this.TabStop = true;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BackgroundImageLayout = ImageLayout.Center;
            this.Name = "AnimationBox";
            this.Size = new System.Drawing.Size(118, 118);
            this.Text = "";
            this.ImageCentered = true;
            this.ConsiderFrameOffset = true;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.AnimationBox_Paint);
            this.Validating += AnimationBox_Validating;
            this.Invalidated += AnimationBox_Invalidated;
            this.Cursor = Cursors.Hand;
            this.ResumeLayout(false);
            //SetStyle(ControlStyles.UserPaint, true);
            
         

        }
        public void NextFrame()
        {
            CurrentFrameIndex++;
        }

        public void UpdateAnimation()
        {
            if (Source != null)
            {
                double elapsed = (DateTime.Now - lastAnimationUpdate).TotalSeconds;
                double frameInterval = 1.00 / Source.Fps;
                int totFrameSteps = (int)(elapsed / frameInterval);
                if (totFrameSteps > 0)
                {
                    int nextFrameIndex = CurrentFrameIndex + totFrameSteps;
                    if (nextFrameIndex > Source.FrameCount)
                        nextFrameIndex %= Source.FrameCount;
                    CurrentFrameIndex = nextFrameIndex;
                    lastAnimationUpdate = DateTime.Now;
                    if (CurrentFrameIndex >= 0)
                    {
                        currentFrame = Source.Frames[CurrentFrameIndex];
                        this.Image = Source.Frames[CurrentFrameIndex].Image;
                    }
                    else
                    {
                        currentFrame = null;
                        this.Image = null;                
                    }
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
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

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
        }




        protected override void OnPaint(PaintEventArgs pe)
        {
            ///////////////base.OnPaint(pe);
            if(this.Image != null)
            {
                var drawBounds = this.ClientRectangle;
                drawBounds.Inflate(-1, -1);
                Rectangle srcRect = new Rectangle(0, 0, Image.Width, Image.Height);
                Rectangle destRect = srcRect;
                if(destRect.Height > drawBounds.Height)
                {
                    int diffY = destRect.Height - drawBounds.Height;
                    destRect.Height = drawBounds.Height;
                    if(destRect.Width > diffY) //keep aspect ratio
                    {
                        destRect.Width = destRect.Width - diffY;
                    }
                }

                if(this.ImageCentered)
                {
                    if (this.ConsiderFrameOffset && currentFrame != null)
                    {
                        int posX = drawBounds.X + drawBounds.Width / 2;
                        int posY = drawBounds.Y + drawBounds.Height / 2;
                        destRect.X = posX + currentFrame.OffsetX;
                        destRect.Y = posY + currentFrame.OffsetY;
                        //destRect.X += currentFrame.OffsetX;
                        //destRect.Y += currentFrame.OffsetY;
                        //destRect.X = drawBounds.Width / 2 - destRect.Width / 2;
                        //destRect.Y = drawBounds.Height / 2 - destRect.Height / 2;
                    }
                    else
                    {
                        destRect.X = drawBounds.Width / 2 - destRect.Width / 2;
                        destRect.Y = drawBounds.Height / 2 - destRect.Height / 2;
                    }
                      
                }

                if(this.ConsiderFrameOffset && currentFrame != null)
                {      
                    
                }
                //destRect()
                pe.Graphics.DrawImage(Image, destRect, srcRect, GraphicsUnit.Pixel);
            }
  
            if (this.Focused)
            {
                var rc = this.ClientRectangle;
                rc.Inflate(-2, -2);
                ControlPaint.DrawFocusRectangle(pe.Graphics, rc);
                Rectangle rc2 = new Rectangle(0,0,0,0);
                //rc2.Inflate(-15, -15);
                ControlPaint.DrawSelectionFrame(pe.Graphics, true, this.ClientRectangle, rc2, Color.Black);
                //ControlPaint.FillReversibleRectangle(this.ClientRectangle, Color.Blue);
                //ControlPaint.DrawLockedFrame(pe.Graphics, rc2, false) ;
               
                    //ControlPaint.DrawGrid(pe.Graphics,this.ClientRectangle,new Size(16,16),Color.Black);
            }
            pe.Graphics.DrawString(Text, this.Font, Brushes.Green, new Point(2, 2));

        }


        private void AnimationBox_Invalidated(object sender, InvalidateEventArgs e)
        {
            //UpdateAnimation();
        }

        private void AnimationBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //UpdateAnimation();

        }

        private void AnimationBox_Paint(object sender, PaintEventArgs e)
        {
            //UpdateAnimation();
            //e.Graphics.DrawString(Text, this.Font, Brushes.Green, new Point(2, 2));

        }

    }
}
