using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Neural_Networks_2.settings;

namespace Neural_Networks_2
{
    class Food
    {
        public Vector2 position;
        public Rectangle bounds;

        public Food()
        {
            position = new Vector2(settings.rnd.Next(100, 1200), settings.rnd.Next(100, 600));
            bounds = new Rectangle((int)position.X, (int)position.Y, NFramework.NDrawing.Textures["food"].Width, NFramework.NDrawing.Textures["food"].Height);
        }

        public void Draw()
        {
            NFramework.NDrawing.Draw("food", position);
        }

        public void NewPosition(List<Entity> eList, List<Food> fList)
        {
            bool AcceptPosition = true;

            position = new Vector2(settings.rnd.Next(100, 1200), settings.rnd.Next(100, 600));
            bounds = new Rectangle((int)position.X, (int)position.Y, 
                NFramework.NDrawing.Textures["food"].Width, NFramework.NDrawing.Textures["food"].Height);

            foreach (Entity e in eList)
            {
                foreach (Food f in fList)
                {
                    if (f != this)
                    {
                        if (e.bounds.Intersects(bounds) || f.bounds.Intersects(bounds))
                        {
                            AcceptPosition = false;
                        }
                    }
                }
            }

            if (!AcceptPosition)
                NewPosition(eList, fList);
        }

        public bool isEaten(Entity entity)
        {
            if (entity.bounds.Intersects(bounds))
            {
                entity.life += HPrecover;
                entity.eaten++;

                return true;
            }
            return false;
        }
    }
}
