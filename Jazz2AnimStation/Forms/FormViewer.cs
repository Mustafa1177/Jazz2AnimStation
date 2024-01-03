using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JJ2AnimLib;
using Jazz2AnimStation.CustomControls;
using static Jazz2AnimStation.ProjectMainModule;
using Jazz2AnimStation.Classes;
using Jazz2AnimStation.Forms;
using Microsoft.Xna.Framework.Input;
using System.Xml.Linq;

namespace Jazz2AnimStation
{
    public partial class FormViewer : Form
    {

        Font frameBoxFont = new Font("Arial", 9);
        public List<Control> LockableControls = new List<Control>(16);
        private List<AnimationBox> animationBoxes = new List<AnimationBox>(24);
        ImageList currentFramesImageList = new ImageList();
        private FormProgression _progressionForm = new FormProgression(null, 0);

        private ProjectGuiWrappers.ProjectAnimationEditorWrapper _animationEditorWrapper = new ProjectGuiWrappers.ProjectAnimationEditorWrapper(new ProjectAnimation(), true);
        private ProjectGuiWrappers.ProjectFrameEditorWrapper _frameEditorWrapper = new ProjectGuiWrappers.ProjectFrameEditorWrapper(new ProjectFrame());

        private int currentOpenSetIndex = -1;
        private int currentOpenAnimIndex = -1;
        //private int selectedFrameIndex = -1;

        public FormViewer()
        {
            InitializeComponent();
            splitContainerOfFramesTab.SplitterDistance = toolStripFrames.Height;
            splitContainerFrameViews.Panel2Collapsed = true; //hide tile view
            listViewFrames.Columns.Add("",150);
            listViewFrames.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);
            currentFramesImageList.ImageSize = new Size(100, 100);
            currentFramesImageList.ColorDepth = ColorDepth.Depth8Bit; //8 bit
           var dummyImg = System.Drawing.Image.FromFile(@"d:\blah2.bmp");
            currentFramesImageList.Images.Add(dummyImg);
            currentFramesImageList.Images.Add(dummyImg);
            currentFramesImageList.Images.Add(dummyImg);
            listViewFrames.LargeImageList = currentFramesImageList;
            listViewFrames.Items.Add("1", 0);
            listViewFrames.Items.Add("2", 1);
            listViewFrames.Items.Add("3", 2);

            
        }

        #region Window UI functions

        public void SelectAnimation(int setIndex, int animIndex) //renaem to OpenAnimation
        {
            currentFramesImageList.Images.Clear();
            if (setIndex < Sets.Count && animIndex < Sets[setIndex].AnimationCount)
            {
                var anim = Sets[setIndex].Animations[animIndex];
                int maxW = 0, maxH = 0;
                currentFramesImageList.ImageSize = new Size(64, 64);
                for (int i = 0; i < anim.FrameCount; i++)
                {
                    if (anim.Frames[i].Width > maxW)
                        maxW = anim.Frames[i].Width;
                    if (anim.Frames[i].Height > maxH)
                        maxH = anim.Frames[i].Height;
                  
                    currentFramesImageList.Images.Add(anim.Frames[i].Image);

                    //test:
                    FrameBox frameBox = new FrameBox();
                    frameBox.Text = (i).ToString(); ;
                    frameBox.Image = anim.Frames[i].Image;
                    frameBox.BorderStyle = BorderStyle.FixedSingle;
                    frameBox.BackgroundImageLayout = ImageLayout.Center;
                    flowLayoutPanel1.Controls.Add(frameBox);

                 
                    
                }

                currentOpenSetIndex = setIndex;
                currentOpenAnimIndex = animIndex;


                DisplayAnimationPropertyEditor(Sets[setIndex].Animations[animIndex]);
                if (Sets[setIndex].Animations[animIndex].FrameCount > 0)
                {
                    /////////DisplayFramePropertyEditor(Sets[setIndex].Animations[animIndex].Frames[0]);
                    //_frameEditorWrapper.Source = Sets[setIndex].Animations[animIndex].Frames[0];
                   // propertyGrid1.SelectedObject = _frameEditorWrapper;

                }



            }
            else
                MessageBox.Show("anim or set does not exist");
        }

        public void RefreshSetsList(ListView dest, IEnumerable<ProjectAnimSet> src)
        {
            dest.Items.Clear();
            string itemText;
            int i = 0;
            foreach (var item in  src) 
            {
                itemText = "Set " + i++ + (!string.IsNullOrEmpty(item.Name) ? " (" + item.Name + ")" : "");
                dest.Items.Add(itemText);
            }
            dest.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);
            dest.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        public void RefreshAnimationsList(Control container, IEnumerable<ProjectAnimation> src, bool disposeOldChildren = false)
        {

            if(disposeOldChildren)
            {
                foreach (AnimationBox ctrl in animationBoxes)
                    if (!ctrl.IsDisposed)
                        ctrl.Dispose();
                foreach (Control ctrl in container.Controls)
                    if (!ctrl.IsDisposed)
                        ctrl.Dispose();
            }
               
            container.Controls.Clear();
            animationBoxes.Clear();

            var animBoxSize = new Size((int)(container.Size.Width * 0.8), (int)(container.Size.Width * 0.8));
            if(animBoxSize.Width < 32 ||  animBoxSize.Height < 32)
                animBoxSize = new Size(32, 32);

            if (src == null)
                return;

            int i = 0;
            foreach (var item in src)
            {
                AnimationBox animBox = new AnimationBox();
                animBox.Size = animBoxSize;
                animBox.BackgroundImageLayout = ImageLayout.Stretch;
                animBox.Source = item;
                animBox.Text = i + (!string.IsNullOrEmpty(item.Name) ? " (" + item.Name + ")" : "");
                animationBoxes.Add(animBox);
                container.Controls.Add(animBox);
                animBox.Click += animationBoxes_Click;
                animBox.DoubleClick += animationBoxes_DoubleClick;
                i++;
            }
        }

        public void RefreshFrameList(ListView dest, IEnumerable<ProjectFrame> src)
        {
            dest.Items.Clear();
            currentFramesImageList.Images.Clear();

            if (src == null)
                return;

            int i = 0;
            foreach (var item in src)
            {
                //listViewFrames.Columns.Add("", 150);
                listViewFrames.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);
                currentFramesImageList.ImageSize = new Size(100, 100);
                //currentFramesImageList.ColorDepth = ColorDepth.Depth8Bit; //8 bit
       
                currentFramesImageList.Images.Add(item.Image);
                //listViewFrames.LargeImageList = currentFramesImageList;
                listViewFrames.Items.Add(i.ToString(), i);
                i++;
            }
            dest.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);
            dest.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }


        void DisplayAnimationPropertyEditor(ProjectAnimation src, bool updateTitleLabel = false, string newTitle = "")
        {
            _animationEditorWrapper.Source = src;
            propertyGrid1.SelectedObject = _animationEditorWrapper;
            if (updateTitleLabel)
                labelPropertyEditorTitle.Text = string.IsNullOrEmpty(newTitle) ? "Animation Properties" : newTitle;
        }

        void DisplayAnimationPropertyEditor(int setIndex, int animIndex, bool updateTitleLabel = true, string newTitle = "")
        {
            if (setIndex >= 0 && setIndex < Sets.Count
                && animIndex >= 0 && animIndex < Sets[setIndex].AnimationCount)
            {
                DisplayAnimationPropertyEditor(Sets[setIndex].Animations[animIndex]);
                if (updateTitleLabel)
                    labelPropertyEditorTitle.Text = string.IsNullOrEmpty(newTitle) ? string.Format("Animation Properties (ID = {0}-{1})", setIndex, animIndex) : newTitle;
            }
        }

        void DisplayFramePropertyEditor(ProjectFrame src)
        {
            _frameEditorWrapper.Source = src;
            propertyGrid1.SelectedObject = _frameEditorWrapper;
        }

        void DisplayFramePropertyEditor(int setIndex, int animIndex, int frameIndex)
        {
            if (setIndex >= 0 && setIndex < Sets.Count 
                && animIndex >= 0 && animIndex < Sets[setIndex].AnimationCount 
                && frameIndex >= 0 && frameIndex < Sets[setIndex].Animations[animIndex].FrameCount)
            {
                DisplayFramePropertyEditor(Sets[setIndex].Animations[animIndex].Frames[0]);
            }
        }

        #endregion


        //==========================================================================================================//
        //==========================================================================================================//
        #region Program Action Functions

        private void UserImportJ2a()
        {
            using (var d = new OpenFileDialog())
            {
                d.Filter = "*.j2a files (*.j2a)|*.j2a|All files (*.*)|*.*";
                if (d.ShowDialog() == DialogResult.OK)
                    if(File.Exists(d.FileName))
                        StartImportJ2a(d.FileName);
            }
        }

        private void StartImportJ2a(string filePath)
        {
            TaskProgressionMonitor monitor = ProjectMainModule.ImportJ2aToProjectAsyncWay2(filePath);
            _progressionForm.Reset(monitor, 500);
            _progressionForm.CompleteExtraMilliseconds = 1000;
            var dialogRes = _progressionForm.ShowDialog();

            RefreshSetsList(listViewSets, Sets);
            RefreshAnimationsList(flowLayoutPanelAnimations, null);
            RefreshFrameList(listViewFrames, null);
            if (monitor.Successful)
                tabControlProjectContent.SelectTab(tabPageSets);
       
            //test
            if (monitor.Successful && Sets.Count > 54 && Sets[54].AnimationCount > 3)
            {
                AnimationBox animBox = new AnimationBox();
                animBox.Source = Sets[54].Animations[3];
                animationBoxes.Add(animBox);
                panelSpritePreview.Controls.Add(animBox);
                animBox.BringToFront();

            }
        }

        #endregion


        //==========================================================================================================//
        //==========================================================================================================//
        #region Form Events

        private void Form1_Load(object sender, EventArgs e)
        {
            //SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.AllPaintingInWmPaint, true);
            propertyGrid1.SelectedObject = _progressionForm;
            //Font = new Font(Font.FontFamily, 11);
            if ( false &&  ProjectMainModule.ImportJ2aToProject(@"D:\M\Jazz2 1.23\Jazz2\Anims.j2a"))
            {
                pictureBox1.Image = Sets[54].Animations[1].Frames[0].Image;
            }

            AnimationBox animBox = new AnimationBox();
           animationBoxes.Add(animBox);
           panelSpritePreview.Controls.Add(animBox);
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawString("1", frameBoxFont, Brushes.Green, new Point(2, 2));
        }

        int _xxcounter = 1;
        private void buttonTest_Click(object sender, EventArgs e)
        {
            FrameBox frameBox = new FrameBox();
            frameBox.Text = (_xxcounter++).ToString(); ;
            //frameBox.BackgroundImage = System.Drawing.Image.FromFile(@"d:\blah2.bmp"); ;
            frameBox.Image = Sets[54].Animations[3].Frames[0].Image;

            frameBox.BorderStyle = BorderStyle.FixedSingle;
            frameBox.BackgroundImageLayout = ImageLayout.Center;
            flowLayoutPanel1.Controls.Add(frameBox);

            SelectAnimation(54, 5);
           
        }

        private void buttonTest2_Click(object sender, EventArgs e)
        {
            ProjectMainModule.ImportJ2aToProject(@"D:\M\Jazz2 1.23\Jazz2\Anims.j2a");
            RefreshSetsList(listViewSets, Sets);
        }

        private void buttonTest3_Click(object sender, EventArgs e)
        {
            StartImportJ2a(@"D:\M\Jazz2 1.23\Jazz2\Anims.j2a");
        }

        private void buttonTemp_Click(object sender, EventArgs e)
        {
            RefreshSetsList(listViewSets, Sets);
        }

        private void listViewSets_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            ListView control = (ListView)sender;
            int i = control.Items.IndexOf(e.Item);
            if(i >= 0 && i < Sets.Count)
            {
                RefreshFrameList(listViewFrames, Sets[i].Animations[0].Frames);

            }
        }

        private void listViewSets_DoubleClick(object sender, EventArgs e)
        {
            ListView control = (ListView)sender;
            int i = control.SelectedIndices.Count > 0? control.SelectedIndices[0] : -1;
            int animIndex = 0;
            if (i >= 0 && i < Sets.Count)
            {
                labelAnimationInfo.Text = i >= 0 ? string.Format("Animations of set {0}", i) : string.Empty;
                RefreshAnimationsList(flowLayoutPanelAnimations, Sets[i].Animations, true);

                currentOpenSetIndex = i;
                currentOpenAnimIndex = animIndex;

                if (animIndex >= 0 && animIndex < Sets[i].AnimationCount)
                    RefreshFrameList(listViewFrames, Sets[i].Animations[animIndex].Frames);
                else
                    RefreshFrameList(listViewFrames, new ProjectFrame[0]);

                tabControlProjectContent.SelectTab(tabPageAnimations);

            }
        }

        private void animationBoxes_Click(object sender, EventArgs e)
        {
            var ctrl = (AnimationBox)sender;
            if (currentOpenSetIndex >= 0 && currentOpenSetIndex < Sets.Count)
            {
                int i = Sets[currentOpenSetIndex].Animations.IndexOf(ctrl.Source);
                if (i >= 0 && i < Sets[currentOpenSetIndex].Animations.Count)
                {
                    DisplayAnimationPropertyEditor(Sets[currentOpenSetIndex].Animations[i], true, string.Format("Animation Properties (ID = {0}-{1})", currentOpenSetIndex, i));

                }
            }
        }

        private void animationBoxes_DoubleClick(object sender, EventArgs e)
        {
            var ctrl = (AnimationBox)sender;
            if (currentOpenSetIndex >= 0 && currentOpenSetIndex < Sets.Count)
            {
                int i = Sets[currentOpenSetIndex].Animations.IndexOf(ctrl.Source);
                if (i >= 0 && i < Sets[currentOpenSetIndex].Animations.Count)
                {
                    SelectAnimation(currentOpenSetIndex, i);
                    tabControlProjectContent.SelectTab(tabPageFrames);
                }
                else
                    MessageBox.Show(string.Format( "Invalid anim ID [{0}] in set [{1}]", i, currentOpenSetIndex));
            }
            else
                MessageBox.Show("Invalid set ID of" + currentOpenSetIndex);
     
        }

        private void FormViewer_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timerTick_Tick(object sender, EventArgs e)
        {
            foreach(var item in animationBoxes) 
            {
                item.UpdateAnimation();
            }
            SetStyle(ControlStyles.UserPaint, true);
            this.Invalidate();
            //this.Refresh();
            //this.Update();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        Size _flowLayoutPanelAnimationsOldSize = Size.Empty;
        private void flowLayoutPanelAnimations_Resize(object sender, EventArgs e)
        {
            const int SIZE_MULTIPLES = 16;
            FlowLayoutPanel ctrl = (FlowLayoutPanel)sender;
            if(ctrl.Size.Width / SIZE_MULTIPLES != _flowLayoutPanelAnimationsOldSize.Width / SIZE_MULTIPLES) //Minimize resizing
            {
                var animBoxSize = new Size((int)(ctrl.Size.Width * 0.8), (int)(ctrl.Size.Width * 0.8));
                if (animBoxSize.Width < 32 || animBoxSize.Height < 32)
                    animBoxSize = new Size(32, 32);

                for (int i = animationBoxes.Count - 1; i >= 0; --i)
                    animationBoxes[i].Size = animBoxSize;

                foreach (var item in animationBoxes)
                {
                    //item.Size = animBoxSize;
                }
            }
            _flowLayoutPanelAnimationsOldSize = ctrl.Size;
        }

        private void originToolStripMenuItemAnimsOriginCentered_CheckedChanged(object sender, EventArgs e)
        {
            var ctrl = (ToolStripMenuItem)sender;
            foreach (var item in animationBoxes)
                item.ConsiderFrameOffset = ctrl.Checked;
        }

        private void toolStripMenuItemCenteredAnims_CheckedChanged(object sender, EventArgs e)
        {
            var ctrl = (ToolStripMenuItem)sender;
            foreach (var item in animationBoxes)
                item.ImageCentered = ctrl.Checked;
        }

        #region Frames tab
        private void toolStripMenuItemFramesTileView_CheckedChanged(object sender, EventArgs e)
        {
            var ctrl = (ToolStripMenuItem)sender;
            splitContainerFrameViews.Panel1Collapsed = ctrl.Checked;
            splitContainerFrameViews.Panel2Collapsed = !ctrl.Checked;
        }

        #endregion


        private void toolStripMenuItemImportJ2a_Click(object sender, EventArgs e)
        {
            UserImportJ2a();
        }


        #endregion

  
    }
}
