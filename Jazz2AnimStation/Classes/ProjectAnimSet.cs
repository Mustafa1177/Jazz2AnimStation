using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JJ2AnimLib;
using JJ2AnimLib.JJ2AnimSections;

namespace Jazz2AnimStation.Classes
{
    public class ProjectAnimSet
    {
        public string Name { get; set; } = "";
  
        public List<ProjectAnimation> Animations { get; set; }

        public List<object> Samples { get; set; }

        public int AnimationCount => Animations.Count; //byte

        public int SampleCount => Samples.Count; //byte

        public ProjectAnimSet() 
        {
            Animations = new List<ProjectAnimation>();            
            Samples = new List<object>();
        }

        public ProjectAnimSet(int initAnimationListCapacity, int initSampleListCapacity = 0)
        {
            Animations = new List<ProjectAnimation>(initAnimationListCapacity);
            Samples = new List<object>(initSampleListCapacity);
        }

        public static ProjectAnimSet FromJ2aSet(AnimSet src)
        {
            ProjectAnimSet res = new ProjectAnimSet(src.Animations.Length, src.Samples.Length);
            res.Animations.AddRange(ProjectAnimation.FromJ2aSet(src));
            return res;
        }

        public static ProjectAnimSet[] FromJ2aLib(JJ2AnimFile src)
        {
            ProjectAnimSet[] res = new ProjectAnimSet[src.Sets.Length];

            for (int i = 0; i < src.Sets.Length; i++)
            {
                res[i] = ProjectAnimSet.FromJ2aSet(src.Sets[i]);
            }

            return res;
        }
    }
}
