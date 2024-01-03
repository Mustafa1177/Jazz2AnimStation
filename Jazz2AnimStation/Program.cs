using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jazz2AnimStation
{

    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var t = new Task(new Action(() =>
            {
                return;
                Jazz2AnimStation.Game1 game = new Game1();
                game.Run();
            }));
           t.Start();

            //System.Net.HttpWebRequest dfdf = new System.Net.HttpWebRequest();
            System.Net.WebClient client = new System.Net.WebClient();
            client.DownloadStringCompleted += Client_DownloadStringCompleted;
            client.DownloadStringAsync(new Uri( @"https://jj2multiplayer.online/api/topplayers"));
          

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormViewer());
        }

        private static void Client_DownloadStringCompleted(object sender, System.Net.DownloadStringCompletedEventArgs e)
        {
            var a = (System.Net.WebClient)sender;
           
           
        }
    }
}
