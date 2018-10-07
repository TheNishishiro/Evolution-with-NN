using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NFramework.NDrawing;

namespace NFramework
{
    class NGraphics
    {
        public static Texture2D Texture_CreatePixel(GraphicsDevice gd, Color colour)
        {
            Texture2D texture;

            texture = new Texture2D(gd, 1, 1);
            texture.SetData<Color>(
                new Color[] { colour });

            return texture;
        }
    }
}
