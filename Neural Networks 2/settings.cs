using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neural_Networks_2
{
    class settings
    {
        public static List<Food> food = new List<Food>();
        public static List<Entity> _entity = new List<Entity>();
        public static List<float> fitnessHistory = new List<float>();
        public static List<float> avaragefitnessHistory = new List<float>();

        public static int Generation = 0;
        public static int Population = 20;
        public static int timeLimit = 2500;
        public static int maxFood = 50;
        public static int mutation = 1;
        public static float initialHP = 255;
        public static float maxHP = 500;
        public static float HPdrain = 0.9f;
        public static float HPrecover = 30;
        public static int mutationRange = 100;
        public static Rectangle cursor;

        public static Random rnd = new Random();
    }
}
