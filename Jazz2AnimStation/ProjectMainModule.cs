using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Jazz2AnimStation.Classes;
using JJ2AnimLib;
using JJ2AnimLib.JJ2AnimSections;

namespace Jazz2AnimStation
{

    /// <summary>
    /// This class will link everything in Jazz2AnimStation together
    /// </summary>
    internal static class ProjectMainModule 
    {
        public static bool WorkstationLocked = false;

        public static List<ProjectAnimSet> Sets = new List<ProjectAnimSet>();

        public static TaskProgressionMonitor LoadProgressionMonitor;

        public static bool ImportJ2aToProject(JJ2AnimFile j2a)
        {
           
            if(j2a.Success)
            {
                ProjectAnimSet[] loadedSets = ProjectAnimSet.FromJ2aLib(j2a);
                Sets = new List<ProjectAnimSet>(loadedSets);
            }

            return j2a.Success;
        }

        public static bool ImportJ2aToProject(string file)
        {
            var j2a = new JJ2AnimLib.JJ2AnimFile(file);
            return ImportJ2aToProject(j2a);
        }

        public static TaskProgressionMonitor ImportJ2aToProjectAsyncWay1(string file)
        {
            var taskMonitor = new TaskProgressionMonitor();
     
            var t = new Task(new Action(() =>
            {
                var j2a = new JJ2AnimLib.JJ2AnimFile();
                var buff = File.ReadAllBytes(file);
                j2a.LoadAsync(buff);

                while(j2a.Busy)
                {
                    taskMonitor.SetPercentage(j2a.LoadPercentage / 2);
                    Thread.Sleep(5);
                }
                taskMonitor.SetPercentage(50); //completed 50%

                if (j2a.Success)
                {
                   bool Success = ImportJ2aToProject(j2a);
                    taskMonitor.FinishTask(Success);
                    return;
                }
                else
                {
                    taskMonitor.ErrorMsg = "Undefined error";

                }

                taskMonitor.FinishTask(j2a.Success);

            }));
            t.Start();
            LoadProgressionMonitor = taskMonitor;
            return
                taskMonitor;
        }


        public static TaskProgressionMonitor ImportJ2aToProjectAsyncWay2(string file)
        {
            var taskMonitor = new TaskProgressionMonitor();


            var t = new Task(new Action(() =>
            {
                var j2a = new JJ2AnimLib.JJ2AnimFile();
                var buff = File.ReadAllBytes(file);
                j2a.LoadAsync(buff);

                while (j2a.Busy)
                {
                    taskMonitor.SetPercentage(j2a.LoadPercentage / 5);
                    Thread.Sleep(5);
                }
                taskMonitor.SetPercentage(20); //completed 50%

                if (j2a.Success)
                {
                    bool Success = true;
                    //Success = ImportJ2aToProject(j2a); //we can directly use this, but we will do it manually for the sake of progress bar
                    //=================
                    ProjectAnimSet[] loadedSets = new ProjectAnimSet[j2a.Sets.Length];

                    for (int i = 0; i < j2a.Sets.Length; i++)
                    {
                        //loadedSets[i] = ProjectAnimSet.FromJ2aSet(j2a.Sets[i]);
                        loadedSets[i] = new ProjectAnimSet(j2a.Sets[i].Animations.Length, j2a.Sets[i].Samples.Length);
                        ProjectAnimation[] loadedAnimations = new ProjectAnimation[j2a.Sets[i].Animations.Length];

                        for (int i2 = 0; i2 < j2a.Sets[i].Animations.Length; i2++)
                        {

                            loadedAnimations[i2] = new ProjectAnimation(j2a.Sets[i].Animations[i2].FrameCount) { Fps = j2a.Sets[i].Animations[i2].FPS, Reserved = j2a.Sets[i].Animations[i2].Reserved };

                            int nextAnimFirstFrame = j2a.Sets[i].Animations[i2].FramesStartIndex + j2a.Sets[i].Animations[i2].FrameCount;
                            for (int i3 = j2a.Sets[i].Animations[i2].FramesStartIndex; i3 < nextAnimFirstFrame; i3++)
                            {
                                taskMonitor.Text = string.Format("Opening set [{0}/{1}] animation [{2}] frame [{3}]", i + 1, j2a.Sets.Length, i2 + 1, loadedAnimations[i2].Frames.Count + 1);
                                ProjectFrame f = ProjectFrame.FromPreparedJ2aFrame(j2a.Sets[i].Frames[i3]);
                                loadedAnimations[i2].Frames.Add(f);
                            }


                        }
                        loadedSets[i].Animations.AddRange(loadedAnimations);

                        taskMonitor.SetPercentage((sbyte)(20 + (i / (float)j2a.Sets.Length * (100 - 20))));

                    }
                   Sets = new List<ProjectAnimSet>(loadedSets);
                    //=================
                    taskMonitor.SetPercentage(100);
                    taskMonitor.FinishTask(Success);
                    return;
                }
                else
                {
                    taskMonitor.ErrorMsg = "Undefined error";

                }

                taskMonitor.FinishTask(j2a.Success);

            }));
            t.Start();
            LoadProgressionMonitor = taskMonitor;
            return
                taskMonitor;
        }

        //public static async Task Method1()
        //{
        //    await Task.Run(() =>
        //    {
        //        for (int i = 0; i < 100; i++)
        //        {
        //            Console.WriteLine(" Method 1");
        //            // Do something
        //            Task.Delay(100).Wait();
        //        }
        //    });
        //}

    }
}
