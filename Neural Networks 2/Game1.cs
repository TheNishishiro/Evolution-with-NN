using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

using static Neural_Networks_2.settings;

namespace Neural_Networks_2
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int alive = 1, time = timeLimit;
        float prevFitness = 0, avarageFitness = 0;

        FileStream fs;
        StreamWriter sw;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            

            graphics.PreferredBackBufferHeight = 700;
            graphics.PreferredBackBufferWidth = 1300;

           
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            NFramework.NDrawing.SetFramework(spriteBatch, Content);

            NFramework.NDrawing.AddTexture("small");
            NFramework.NDrawing.AddTexture("food");
            NFramework.NDrawing.AddTexture("dotw");
            NFramework.NDrawing.AddTexture("fly");
            NFramework.NDrawing.AddFont("font", "font");
            NFramework.NDrawing.AddPixelTexture("line", GraphicsDevice);

            for (int i = 0; i < maxFood; i++)
                food.Add(new Food());

            for(int i = 0; i < Population; i++)
                _entity.Add(new Entity());

            cursor = new Rectangle(0, 0, 1, 1);

            fitnessHistory.Add(0);
            avaragefitnessHistory.Add(0);
            fs = new FileStream(".\\evolution.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            sw = new StreamWriter(fs);
            sw.Close();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            cursor.X = Mouse.GetState().Position.X;
            cursor.Y = Mouse.GetState().Position.Y;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);

            alive = 0;
            for (int i = 0; i < _entity.Count; i++)
            {
                if (!_entity[i].dead)
                {
                    _entity[i].Update();
                    _entity[i].Draw();
                    alive = 1;
                }
            }

            for (int i = 0; i < food.Count; i++)
            {
                food[i].Draw();
                for (int j = 0; j < _entity.Count; j++)
                {
                    if (food[i].Collision(_entity[j]))
                    {
                        food.RemoveAt(i);
                        i--;
                        food.Add(new Food());
                    }
                    if (i < 0)
                        break;
                }
            }
            time--;

            if (alive == 0 || time <= 0)
            {
                List<Entity> newGeneration = new List<Entity>();
                float avarageFitness = 0;
                prevFitness = 0;
                for (int i = 0; i < _entity.Count; i++)
                {
                    _entity[i].fitness *= _entity[i].eaten;
                    avarageFitness += _entity[i].fitness;
                    if (prevFitness < _entity[i].fitness)
                        prevFitness = _entity[i].fitness;
                }
                avarageFitness /= _entity.Count;
                this.avarageFitness = avarageFitness;
                fitnessHistory.Add(prevFitness);
                avaragefitnessHistory.Add(avarageFitness);
                fs = new FileStream(".\\evolution.txt", FileMode.Append, FileAccess.Write);
                sw = new StreamWriter(fs);
                sw.WriteLine(Generation.ToString() + " : " + prevFitness.ToString("0.0000000") + " | " + this.avarageFitness);
                sw.Close();

                List<Entity> pool = new List<Entity>();
                for (int i = 0; i < _entity.Count; i++)
                {
                    if (_entity[i].fitness <= avarageFitness)
                    {
                        _entity.RemoveAt(i);
                        i--;
                    }
                }

                for (int i = 0; i < _entity.Count; i++)
                {
                    for (float j = 0; j < _entity[i].fitness; j++)
                        pool.Add(_entity[i]);
                }

                while (newGeneration.Count < Population)
                {
                    int parent1ID, parent2ID;

                    parent1ID = rnd.Next(pool.Count);
                    parent2ID = rnd.Next(pool.Count);
                    newGeneration.Add(new Entity(pool[parent1ID], pool[parent2ID], 0));
                }
                food = new List<Food>();
                for (int i = 0; i < maxFood; i++)
                    food.Add(new Food());

                _entity = new List<Entity>();
                _entity = newGeneration;
                newGeneration = new List<Entity>();
                pool = new List<Entity>();

                alive = 1;
                time = timeLimit;
                Generation++;
            }

            NFramework.NDrawing.DrawText("Time: " + time + "\nGeneration: " + Generation + "\nMax Fitness: " + prevFitness + "\nAvarage Fitness: " + avarageFitness, new Vector2(10, 10));

            int prev = 0, newPrev = 0;
            for(int i = 0, w = 0; i < fitnessHistory.Count-1; i++, w++)
            {
                newPrev = w * 5;
                NFramework.NDrawing.Draw_Line_Between_Points(new Vector2(prev, 680-(fitnessHistory[i]/100)), new Vector2(newPrev, 680 - (fitnessHistory[i+1] / 100)),"line", Color.Red);
                NFramework.NDrawing.Draw_Line_Between_Points(new Vector2(prev, 680 - (avaragefitnessHistory[i] / 100)), new Vector2(newPrev, 680 - (avaragefitnessHistory[i + 1] / 100)), "line", Color.ForestGreen);
                prev = newPrev;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
