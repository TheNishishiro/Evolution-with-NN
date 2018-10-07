using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework
{
    class NAction
    {
        public static Random rnd = new Random();

        public static bool IsCollision(Rectangle object1, Rectangle object2)
        {
            if (object1.Intersects(object2))
                return true;
            return false;
        }

        public static float Get_Distance_Between_Points(Vector2 PositionFrom, Vector2 PositionTo)
        {
            return Vector2.Distance(PositionFrom, PositionTo);
        }

        public static Vector2 Move_To_Point(Vector2 PositionFrom, Vector2 PositionTo, float Velocity, float Distance)
        {
            if (Vector2.Distance(PositionFrom, PositionTo) < Distance)
            {
                Vector2 direction = Vector2.Normalize(PositionTo - PositionFrom);
                PositionFrom += direction * Velocity;
            }
            else
            {
                PositionFrom = PositionTo;
            }

            return PositionFrom;
        }

        public static Vector2 Jump_To_Random_Nearby_Position(Vector2 Position, float Radius)
        {
            Random rnd = new Random();

            return new Vector2(rnd.Next((int)(Position.X - Radius), (int)(Position.X + Radius)), rnd.Next((int)(Position.Y - Radius), (int)(Position.Y + Radius)));
        }
        public static Vector2 Align_To_Grid(Vector2 position, int GridSize)
        {
            return new Vector2((int)(position.X / GridSize) * GridSize, (int)(position.Y / GridSize) * GridSize);
        }

        public static float Rotation_LookAt(Vector2 Position, Vector2 PositionToLookAt)
        {
            float rotation;
            Vector2 distance;

            distance.X = PositionToLookAt.X - Position.X;
            distance.Y = PositionToLookAt.Y - Position.Y;

            rotation = (float)Math.Atan2(distance.Y, distance.X);

            return rotation;
        }

        public static Vector2 Rotation_Get_Velocity(float rotation, float velocity)
        {
            Vector2 spriteVelocity;

            spriteVelocity.X = (float)Math.Cos(rotation) * velocity;
            spriteVelocity.Y = (float)Math.Sin(rotation) * velocity;

            return spriteVelocity;
        }

        public static Vector2 RotateAboutOrigin(Vector2 point, Vector2 origin, float rotation)
        {
            return Vector2.Transform(point - origin, Matrix.CreateRotationZ(rotation)) + origin;
        }

        public static Vector2 Vector_SetLength(float length, float angle)
        {
            Vector2 newVector = new Vector2(0, 0);

            newVector.X = (float)Math.Cos(angle) * length;
            newVector.Y = (float)Math.Sin(angle) * length;

            float GetLength = (float)Math.Sqrt((newVector.X * newVector.X) + (newVector.Y * newVector.Y));

            newVector.X = (float)Math.Cos(angle) * GetLength;
            newVector.Y = (float)Math.Sin(angle) * GetLength;

            return newVector;
        }

        public static double Get_Random_Double(double min, double max)
        {
            return rnd.NextDouble() * (max - min) + min;
        }
    }
}
