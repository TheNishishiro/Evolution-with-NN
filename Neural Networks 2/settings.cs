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
        public static int Population = 20;      // Number of flies in generation
        public static int timeLimit = 2500;     // simulation time in frames 
        public static int maxFood = 50;         // Number of food spawns per map
        public static int mutation = 1;         // Mutation rate in %
        public static float initialHP = 255;    // HP at which flies start life with, decreases if fly doesn't eat 
        public static float maxHP = 500;        // HP threshold at which it dies from eating too much 
        public static float HPdrain = 0.9f;     // HP drop for each frame of life
        public static float HPrecover = 30;     // HP recovered per food eaten
        public static int mutationRange = 100;  // How much can neural networks weights change if mutation happens (divided by 100)

        public static Rectangle cursor;

        public static Random rnd = new Random();
    }
}
