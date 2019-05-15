using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neural_Networks_2
{
    class SideBar
    {
        private int animate = 0;
        private int maxWidth = 300;
        private int animationStep = 10;
        private bool Visible = false;

        public SideBar()
        {

        }

        public void Toggle()
        {
            Visible = !Visible;
        }

        private void Animate()
        {
            if (Visible)
            {
                if (animate > 0)
                    animate -= animationStep;
            }
            else
            {
                if (animate < maxWidth)
                    animate += animationStep;
            }
        }

        public void Draw(int time, float prevFitness, float avarageFitness, string entityInfo)
        {
            Animate();
            NFramework.NDrawing.Draw("sidebar", new Rectangle(0, 0, animate, 800));
            NFramework.NDrawing.DrawText("Simulation info: \n\n" +
                "Time: " + time + "\n" +
                "Generation: " + settings.Generation + "\n" +
                "Max Fitness: " + prevFitness + "\n" +
                "Avarage Fitness: " + avarageFitness + "\n\n\nEntity info:\n\n" +
                entityInfo, 
                
                new Vector2(animate - (maxWidth - 10), 10));
        }
    }
}
