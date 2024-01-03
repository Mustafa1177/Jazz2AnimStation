using JJ2AnimLib.JJ2AnimSections;
using System;
using System.Collections.Generic;

namespace Jazz2AnimStation.Classes
{
    public class ProjectAnimation
    {
        public string Name { get; set; } = "";
 
        public Int16 Fps { get; set; }

        public Int32 Reserved { get; set;}

        public List <ProjectFrame> Frames { get; set; }

        public int FrameCount => Frames.Count;

       
        public ProjectAnimation()
        {
            Frames = new List<ProjectFrame> ();
        }

        public ProjectAnimation(int initAnimationListCapacity)
        {
            Frames = new List<ProjectFrame>(initAnimationListCapacity);
        }

        public static ProjectAnimation FromJ2aAnimation(JJ2AnimLib.JJ2AnimSections.AnimInfo anim, JJ2AnimLib.JJ2AnimSections.AnimSet container)
        {
            ProjectAnimation res = new ProjectAnimation(anim.FrameCount) { Fps = anim.FPS, Reserved = anim.Reserved };
       
            int nextAnimFirstFrame = anim.FramesStartIndex + anim.FrameCount;
            for (int i = anim.FramesStartIndex; i < nextAnimFirstFrame; i++)
            {
                ProjectFrame f = ProjectFrame.FromPreparedJ2aFrame(container.Frames[i]);
                res.Frames.Add(f);
            }

            return res;
        }

        public static ProjectAnimation[] FromJ2aSet(JJ2AnimLib.JJ2AnimSections.AnimSet src)
        {
            ProjectAnimation[] res = new ProjectAnimation[src.Animations.Length];

            for(int i = 0; i < src.Animations.Length; i++)
            {
                //res[i] = new ProjectAnimation() { Fps = src.Animations[i].FPS, Reserved = src.Animations[i].Reserved };
                res[i] = ProjectAnimation.FromJ2aAnimation(src.Animations[i], src);
            }

            return res;
        }
    }
}
