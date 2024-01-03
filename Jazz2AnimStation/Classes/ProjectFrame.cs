using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Mail;
using System.Text;
using JJ2AnimLib.JJ2AnimSections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jazz2AnimStation.Classes
{
    public class ProjectFrame
    {

        public int Width { get; set; }

        public int Height { get; set; }

        public Int16 ColdspotX { get; set; }    // Relative to hotspot

        public Int16 ColdspotY { get; set; }    // Relative to hotspot

        public Int16 OffsetX { get; set; }

        public Int16 OffsetY { get; set; }

        public Int16 GunspotX { get; set; }     // Relative to hotspot

        public Int16 GunspotY { get; set; }     // Relative to hotspot

        public Bitmap Image;

        public Texture2D Texture;

        public ProjectFrame()
        {
            
        }

        public ProjectFrame(short w, short h)
        {
            Width = w;
            Height = h;
        }

        public ProjectFrame(short w, short h, short coldspotX, short coldspotY, short offsetX, short offsetY, short gunspotX, short gunspotY)
        {
            Width = w;
            Height = h;
            ColdspotX = coldspotX;
            ColdspotY = coldspotY;
            OffsetX = offsetX;
            OffsetY = offsetY;
            GunspotX = gunspotX;
            GunspotY = gunspotY;
        }


        public static ProjectFrame FromJ2aFrame(JJ2AnimLib.JJ2AnimSections.FrameInfo frame, JJ2AnimLib.JJ2AnimSections.AnimSet container)
        {
            ProjectFrame res = new ProjectFrame(frame.Width, frame.Height, frame.ColdspotX, frame.GunspotY,frame.HotspotX, frame.HotspotY, frame.GunspotX, frame.GunspotY);
            //res.Image = 
            return null;
        }

        public static ProjectFrame FromPreparedJ2aFrame(JJ2AnimLib.JJ2AnimSections.FrameInfo frame)
        {
            ProjectFrame res = new ProjectFrame(frame.Width, frame.Height, frame.ColdspotX, frame.GunspotY, frame.HotspotX, frame.HotspotY, frame.GunspotX, frame.GunspotY);
            res.Image = res.MakeBitmapFromPreparedData(frame);
            return res;
        }

        private Bitmap MakeBitmapFromPreparedData(FrameInfo frame)
        {
            var bm = new Bitmap(frame.Width, frame.Height, PixelFormat.Format32bppArgb);
            for (int i = 0; i < frame.Width; i++)
            {
                for (int j = 0; j < frame.Height; j++)
                {

                    if (frame.TMask[i, j] == 0)
                        bm.SetPixel(i, j, System.Drawing.Color.FromArgb(JJ2AnimLib.JJ2ColorPalettes.DefaultPaletteRGB[frame.Img8Bit[i, j], 0], JJ2AnimLib.JJ2ColorPalettes.DefaultPaletteRGB[frame.Img8Bit[i, j], 1], JJ2AnimLib.JJ2ColorPalettes.DefaultPaletteRGB[frame.Img8Bit[i, j], 2]));
                    else
                        bm.SetPixel(i, j, System.Drawing.Color.Transparent);

                    // bm.SetPixel(i, j, frame.TMask[i, j] == 0 ? Color.DodgerBlue : Color.Transparent);    
                }
            }
            return bm;
        }

        private void MakeBitmap(byte[] buff, int imageAddress)
        {

        }

        private void MakeXnaTexture2D(byte[] buff, int imageAddress)
        {

        }

    }
}
