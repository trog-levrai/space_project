using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CollisionPerPixel
{
    static public class Physique
    {
        //UTILISE LES RECTANGLE
        static public bool IntersectPixel(Rectangle rect1, Color[] data1,
                                    Rectangle rect2, Color[] data2)
        {
            int top = Math.Max(rect1.Top, rect2.Top);
            int bottom = Math.Min(rect1.Bottom, rect2.Bottom);
            int left = Math.Max(rect1.Left, rect2.Left);
            int right = Math.Min(rect1.Right, rect2.Right);

            for (int y = top; y < bottom; y++)
                for (int x = left; x < right; x++)
                {
                    Color colour1 = data1[(x - rect1.Left) + (y - rect1.Top) * rect1.Width];
                    Color colour2 = data2[(x - rect2.Left) + (y - rect2.Top) * rect2.Width];

                    if (colour1.A != 0 && colour2.A != 0)
                        return true;
                }

            return false;
        }

        //UTILISE LES TEXTURE2D ET LES VECTOR2, LAQUELLE DES DEUX EST LA MEILLEURE ?... BONNE QUESTION
        static public bool IntersectPixel(Texture2D text1, Vector2 pos1, Color[] data1,
                                    Texture2D text2, Vector2 pos2, Color[] data2)
        {
            int top = Math.Max((int)pos1.Y, (int)pos2.Y);
            int bottom = Math.Min((int)pos1.Y + text1.Height, (int)pos2.Y + text2.Height);
            int left = Math.Max((int)pos1.X, (int)pos2.X);
            int right = Math.Min((int)pos1.X + text1.Width, (int)pos2.X + text2.Width);

            for (int y = top; y < bottom; y++)
                for (int x = left; x < right; x++)
                {
                    Color colour1 = data1[(x - (int)pos1.X) + (y - (int)pos1.Y) * text1.Width];
                    Color colour2 = data2[(x - (int)pos2.X) + (y - (int)pos2.Y) * text2.Width];

                    if (colour1.A != 0 && colour2.A != 0)
                        return true;
                }

            return false;
        }
    }
}
