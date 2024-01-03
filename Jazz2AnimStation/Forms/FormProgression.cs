using Jazz2AnimStation.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Jazz2AnimStation.Forms
{
    public partial class FormProgression : Form
    {

        private TaskProgressionMonitor progressionStatus = null;

        private int loadTicks = Environment.TickCount;

        private int completeTicks = Environment.TickCount;

        private bool completed = false;

        public int MinimumMilliseconds { get; set; }

        public int CompleteExtraMilliseconds { get; set; } // pause value MS on complete

        public bool PlaySoundOnComplete { get; set; } = true;

        public FormProgression(TaskProgressionMonitor progression, int minimumMilliseconds = 0)
        {
            InitializeComponent();
            Reset(progression, minimumMilliseconds);
        }

        public void Reset(TaskProgressionMonitor progression, int minimumMilliseconds = 0)
        {
            progressionStatus = progression;
            MinimumMilliseconds = minimumMilliseconds;
            completed = false;
            labelPercentage.Text = "0%";
            labelTaskText.Text = "";
        }

        private void FormProgression_Load(object sender, EventArgs e)
        {
            loadTicks = Environment.TickCount;
            Cursor = Cursors.WaitCursor;
            timerRefreh.Start();
        }

        private void timerRefreh_Tick(object sender, EventArgs e)
        {
            
            if(progressionStatus.Percentage != progressBarMain.Value) 
            {
                labelPercentage.Text = progressBarMain.Value.ToString() + "%";
                progressBarMain.Value = progressionStatus.Percentage;
            }

            if(labelTaskText.Text != progressionStatus.Text)
            {
                labelTaskText.Text = progressionStatus.Text;
            }

            if(!completed && progressionStatus.Completed)
            { 
                //on complete
                completed = true;
                completeTicks = Environment.TickCount;
                Cursor = DefaultCursor;
                if (progressionStatus.Successful)
                {
                    if (PlaySoundOnComplete)
                        SystemSounds.Asterisk.Play();
                    labelTaskText.Text = "Completed successfully!";
                }
                else
                {
                    MessageBox.Show(!string.IsNullOrEmpty(progressionStatus.ErrorMsg) ? progressionStatus.ErrorMsg : "Task Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
           
            }

            if(completed && Environment.TickCount - loadTicks >= MinimumMilliseconds && Environment.TickCount - completeTicks >= CompleteExtraMilliseconds)
            {
                timerRefreh.Stop();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }


        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if(completed)
                this.DialogResult = DialogResult.OK;
            else
                this.DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
