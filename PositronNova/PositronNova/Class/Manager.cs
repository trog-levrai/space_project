using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace PositronNova.Class
{
    static public class Manager
    {
        static public Texture2D littleCinetique_t, cinetique_t, laser_t, ion_t, plasma_t, missile_t;
        public static Texture2D fumee_t, explosion_t, grosseExplosion_t;

        public static SoundEffect missileLaunch_s, missileHit_s, laserFire_s, laserHit_s;

        static public void ContentLoad(ContentManager content)
        {
            littleCinetique_t = content.Load<Texture2D>("img\\LittleBullet");
            cinetique_t = content.Load<Texture2D>("img\\Bullet");
            laser_t = content.Load<Texture2D>("img\\LaserBullet"); // Pour l'instant...
            ion_t = content.Load<Texture2D>("img\\IonBullet");
            plasma_t = content.Load<Texture2D>("img\\PlasmaBullet");
            missile_t = content.Load<Texture2D>("img\\Missile");

            fumee_t = content.Load<Texture2D>("img\\MissileFumee");
            explosion_t = content.Load<Texture2D>("img\\Explosion");
            grosseExplosion_t = content.Load<Texture2D>("img\\boom");

            laserFire_s = content.Load<SoundEffect>("sounds\\laserFire2");
            laserHit_s = content.Load<SoundEffect>("sounds\\laserHit");
            missileLaunch_s = content.Load<SoundEffect>("sounds\\MissileLaunch");
            missileHit_s = content.Load<SoundEffect>("sounds\\missileHit");
        }
    }
}
