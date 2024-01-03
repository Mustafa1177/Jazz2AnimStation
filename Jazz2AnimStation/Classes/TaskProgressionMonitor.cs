using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jazz2AnimStation.Classes
{
    public class TaskProgressionMonitor
    {

        private int percentage = 0;

        private bool completed = false;

        private bool successful = false;

        public object UserParam { get; set; } = null;

        public string Text { get; set; } = "";

        public string ErrorMsg { get; set; } = "";

        public int Percentage => percentage;

        public bool Completed => completed;

        public bool Successful => successful;

        public void SetPercentage(int value)
        {
            if(value < 0)
                percentage = 0;
            else if(value > 100)
                percentage = 100;
            else
                percentage = value;
        }

        public void FinishTask(bool succeed = true)
        {
            successful = succeed;
            completed = true;
        }
    }
}
