using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PositronNova.Class
{
    static public class TextureManager
    {
        static public Texture2D littleCinetique_t, cinetique_t, laser_t, ion_t, plasma_t, missile_t;

        static public void ContentLoad(ContentManager content)
        {
            littleCinetique_t = content.Load<Texture2D>("img\\LittleBullet");
            cinetique_t = content.Load<Texture2D>("img\\Bullet");
            laser_t = content.Load<Texture2D>("img\\LaserBullet"); // Pour l'instant...
            ion_t = content.Load<Texture2D>("img\\IonBullet");
            plasma_t = content.Load<Texture2D>("img\\PlasmaBullet");
            missile_t = content.Load<Texture2D>("img\\Missile");
        }
    }
}
