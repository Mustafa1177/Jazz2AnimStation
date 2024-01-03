using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace Jazz2AnimStation.Classes
{
    /*
     [Browsable(bool)] – to show property or not
     [ReadOnly(bool)] – possibility to edit property
     [Category(string)] – groups of property
     [Description(string)] – property description. It is something like a hint.
     [DisplayName(string)] – display property
     [Range(int, int)]
     */

    /// <summary>
    /// Contains wrapper classes with properties to be shown GUI to edit other classes variables
    /// </summary>
    public class ProjectGuiWrappers
    {

        #region ProjectFrameEditorWrapper class
        public class ProjectFrameEditorWrapper
        {

            [Browsable(false)]
            public ProjectFrame Source;

            [Description("Frame width in pixels")]
            public int Width
            {
                get
                {
                    return Source.Width;
                }
            }

            [Description("Frame height in pixels")]
            public int Height
            {
                get
                {
                    return Source.Height;
                }
            }

            [Description("X value for the point in the sprite that will stand on the ground")]
            public Int16 ColdspotX
            {
                get
                {
                    return Source.ColdspotX;
                }
                set
                {
                    Source.ColdspotX = value;
                }
            }    // Relative to hotspot

            [Description("Y value for the point in the sprite that will stand on the ground")]
            public Int16 ColdspotY
            {
                get
                {
                    return Source.ColdspotY;
                }
                set
                {
                    Source.ColdspotY = value;
                }
            }    // Relative to hotspot

            [Description("-X value of sprite pivot point")]
            public Int16 OffsetX
            {
                get
                {
                    return Source.OffsetX;
                }
                set
                {
                    Source.OffsetX = value;
                }
            } //Pivot point


            [Description("-Y value of sprite pivot point")]
            public Int16 OffsetY
            {
                get
                {
                    return Source.OffsetY;
                }
                set
                {
                    Source.OffsetY = value;
                }
            } //Pivot point


            [ReadOnly(true)]
            [Description("")]
            public Int16 PivotX
            {
                get
                {
                    return (Int16)(-this.OffsetX);
                }
                set
                {
                    this.OffsetX = (Int16)(-value);
                }
            }

            [ReadOnly(true)]
            [Description("")]
            public Int16 PivotY
            {
                get
                {
                    return (Int16)(-this.OffsetY);
                }
                set
                {
                    this.OffsetY = (Int16)(-value);
                }
            }

            [Description("Relative X position of the bullets fired by this sprite")]
            public Int16 GunspotX
            {
                get
                {
                    return Source.GunspotX;
                }
                set
                {
                    Source.GunspotX = value;
                }
            }     // Relative to hotspot

            [Description("Relative Y position of the bullets fired by this sprite")]
            public Int16 GunspotY
            {
                get
                {
                    return Source.GunspotY;
                }
                set
                {
                    Source.GunspotY = value;
                }
            }     // Relative to hotspot

            public Bitmap TempImage
            {
                get
                {
                    return Source.Image;
                }
                set
                {
                    Source.Image = value;
                }
            }


            public ProjectFrameEditorWrapper(ProjectFrame source)
            {
                Source = source;
            }
        }
        #endregion

        #region ProjectAnimationEditorWrapper class
        public class ProjectAnimationEditorWrapper
        {

            private ProjectAnimation source;

            private List<ProjectFrameEditorWrapper> frames;

            private bool importFrames;

            [Browsable(false)]
            public ProjectAnimation Source
            {
                get 
                {
                    return source; 
                }
                set
                {
                    source = value;
                    if (importFrames) //is it really needed?
                    {
                        frames.Clear();
                        foreach (var frame in value.Frames)
                            frames.Add(new ProjectFrameEditorWrapper(frame));
                    }
                }
            }


            [Description("Animation name")]
            public string Name
            {
                get { return Source.Name; }
                set { Source.Name = value; }
            }

            [Description("Frames per second (FPS)")]
            public Int16 FrameRate
            {
                get { return Source.Fps; }
                set { Source.Fps = value; }
            }

            [Description("")]
            public Int32 Reserved
            {
                get { return Source.Reserved; }
                set { Source.Reserved = value; }
            }

            [Description("List of frames included in this animation")]

            [ReadOnly(true)]
            public List<ProjectFrameEditorWrapper> Frames => frames;

            [Description("Total number of frames included this animation")]
            public int FrameCount => Source.Frames.Count;

            public ProjectAnimationEditorWrapper(ProjectAnimation source, bool importFrames = false)
            {
                this.importFrames = importFrames;
                frames = new List<ProjectFrameEditorWrapper>(importFrames ? source.FrameCount : 0);
                Source = source;
             }
        }
        #endregion
    }
}
