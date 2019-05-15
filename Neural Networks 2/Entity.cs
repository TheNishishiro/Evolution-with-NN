using Microsoft.Xna.Framework;
using Neural_Networks_2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Neural_Networks_2.settings;

namespace Neural_Networks_2
{
    class Entity
    {
        public Vector2 position;    // position of a fly
        public Vector2 sensor1, sensor2;    // positions of each sensor

        public Rectangle bounds;    // hitbox

        public float rotation = 0;
        public float life = initialHP;
        public float velocity;

        float rotationMultiplier = 4;
        float speedMultiplier = 10;

        public bool dead = false;
        public bool locked = false;

        public int eaten = 0;


        Color color = Color.White;

        private float input1, input2;

        public float fitness = 0;

        public NeuralNetworkv2 Brain;

        private Food closestFoodToLeft, closestFoodToRight; 
        private static int[] array = { 2, 5, 9, 5, 3 }; // Neural network architecture
        float distanceToLeft = 99999;
        float distanceToRight = 99999;

        // Debug information for when you hold cursor above an entity
        public void Draw()
        {
            NFramework.NDrawing.Draw("fly", position, color, 1, rotation, true);
            NFramework.NDrawing.Draw("small", sensor1, Color.White, 1, rotation, true);
            NFramework.NDrawing.Draw("small", sensor2, Color.Black, 1, rotation, true);
        }

        public string GetInfoString(int entityID)
        {
            return "ID:" + entityID +
                "\nAlive: " + !dead +
                "\nDistance L: " + distanceToLeft +
                "\nDistance R: " + distanceToRight +
                "\nrotation: " + rotation +
                "\nvelocity: " + velocity +
                "\neaten: " + eaten +
                "\n\nNeural network:" +
                "\nNin_1: " + input1 +
                "\nNin_2: " + input2 +
                "\nNout_1: " + Brain.GetOutput(0) +
                "\nNout_2: " + Brain.GetOutput(1) +
                "\nNout_3: " + Brain.GetOutput(2);

        }

        private void InitBrain()
        {
            bool useOutput = settings.UseLinearOutput;

            // Create new neural network with randomized weigths 
            Brain = new NeuralNetworkv2(array.Length, array);
            Brain.SetLearningRate(0.2); // useless but can stay
            Brain.SetLinearOutput(useOutput); // changes NN output to skip activation function on output layer
            Brain.SetMomentum(true, 0.9); // also useless

            if (useOutput)
            {
                rotationMultiplier = 1;
                speedMultiplier = 1;
            }
        }

        public Entity()
        {
            InitBrain();


            position = new Vector2(rnd.Next(1200), rnd.Next(700));
            bounds = new Rectangle((int)position.X, (int)position.Y, 26, 20);
        }

        public Entity(Entity parent1, Entity parent2, int mutationRate)
        {
            InitBrain();

            position = new Vector2(rnd.Next(1200), rnd.Next(700));
            bounds = new Rectangle((int)position.X, (int)position.Y, 26, 20);

            int neuronConnections = 0;

            // Get number of ALL connections inside nerual network
            for(int i = 0; i < Brain.Layers.Count - 1; i++)
            {
                neuronConnections += Brain.Layers[i].NumberOfNodes * Brain.Layers[i + 1].NumberOfNodes;
            }

            List<double> connection1 = new List<double>();
            List<double> connection2 = new List<double>();

            // Get wieghts of each parent
            for (int x = 0; x < Brain.Layers.Count; x++)
            {
                for (int i = 0; i < parent1.Brain.Layers[x].NumberOfNodes; i++)
                {
                    for (int j = 0; j < parent1.Brain.Layers[x].NumberOfChildNodes; j++)
                    {
                        connection1.Add(parent1.Brain.Layers[x].Weights[i][j]);
                    }
                }
                for (int i = 0; i < parent2.Brain.Layers[x].NumberOfNodes; i++)
                {
                    for (int j = 0; j < parent2.Brain.Layers[x].NumberOfChildNodes; j++)
                    {
                        connection2.Add(parent2.Brain.Layers[x].Weights[i][j]);
                    }
                }
            }

            List<double> resultConnection = new List<double>();

            // Create new weigths for out network based off parents weights and slip in some mutation from time to time
            for (int i = 0; i < neuronConnections; i++)
            {
                if (rnd.Next(2) == 0)
                {
                    if (rnd.Next(101) > mutation)
                        resultConnection.Add(connection1[i]);
                    else
                        resultConnection.Add(rnd.Next(-mutationRange, mutationRange) / 100);
                }
                else
                {
                    if (rnd.Next(101) > mutation)
                        resultConnection.Add(connection2[i]);
                    else
                        resultConnection.Add(rnd.Next(-mutationRange, mutationRange) / 100);
                }

            }

            int w = 0;

            // Apply new weights in correct order to neural network
            for (int x = 0; x < Brain.Layers.Count; x++)
            {
                for (int i = 0; i < Brain.Layers[x].NumberOfNodes; i++)
                {
                    for (int j = 0; j < Brain.Layers[x].NumberOfChildNodes; j++)
                    {
                        Brain.Layers[x].Weights[i][j] = resultConnection[w];
                        w++;
                    }
                }
            }
        }

        public void ChangeColor(bool selected = false)
        {
            if (!selected)
            {
                color = Color.White;
                if (life / maxHP > 0.5)
                {
                    color.G = (byte)(255 - (255 * (life / maxHP)));
                    color.B = (byte)(255 - (255 * (life / maxHP)));
                }

                if (life < (maxHP * 0.2))
                    color.A = (byte)life;
            }
            else
            {
                color = Color.Blue;
            }
        }

        public void Update()
        {
            sensor1 = new Vector2(position.X - 10, position.Y);
            sensor2 = new Vector2(position.X + 10, position.Y);

            // Rotate sensors together with entities body
            sensor1 = NFramework.NAction.RotateAboutOrigin(sensor1, position, rotation);
            sensor2 = NFramework.NAction.RotateAboutOrigin(sensor2, position, rotation);

            distanceToLeft = 99999;
            distanceToRight = 99999;

            // Get distance to the closest food
            for (int i = 0; i < food.Count; i++)
            {

                if (distanceToLeft > NFramework.NAction.Get_Distance_Between_Points(sensor1, food[i].position))
                {
                    closestFoodToLeft = food[i];
                    distanceToLeft = NFramework.NAction.Get_Distance_Between_Points(sensor1, food[i].position);
                }
                if (distanceToRight > NFramework.NAction.Get_Distance_Between_Points(sensor2, food[i].position))
                {
                    closestFoodToRight = food[i];
                    distanceToRight = NFramework.NAction.Get_Distance_Between_Points(sensor2, food[i].position);
                }
            }

            bounds.X = (int)position.X-10;
            bounds.Y = (int)position.Y-3;


            // Set 1st input neuron depending on which sensor should be active
            if (distanceToLeft > distanceToRight)
                input1 = 1;
            else
                input1 = -1;

            Brain.SetInput(0, input1);

            // Set 2nd input neuron depending on hunger status 
            input2 = life / maxHP;
            Brain.SetInput(1, input2);
            Brain.FeedForward();

            // Set fly rotation depending on output and increment it since it's bound to 0-1 which is slow AF
            rotation += Math.Abs((float)Brain.GetOutput(0)) * rotationMultiplier;
            rotation -= Math.Abs((float)Brain.GetOutput(1)) * rotationMultiplier;

            // Same for velocity, get NN output and multiply it so it goes WEEEEEEEEEEEEEEEEEEEEEE
            velocity = (float)Brain.GetOutput(2) * speedMultiplier;

            position += NFramework.NAction.Rotation_Get_Velocity(rotation, velocity);

            life -= HPdrain;

            // Increase fitness for each frame in which it's alive 
            CalculateFitness();

            if (life <= 0 || life > maxHP)
            {
                // If it ded then punish it
                dead = true;
                fitness /= 4;
            }
        }

        private void CalculateFitness()
        {
            fitness += 0.05f;
        }


    }
}
