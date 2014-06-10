﻿using System;
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
        static public Texture2D lifeBrick_t;

        public static SoundEffect missileLaunch_s, missileHit_s, laserFire_s, laserHit_s;

        static public void ContentLoad(ContentManager content)
        {
            littleCinetique_t = content.Load<Texture2D>("img\\bullets\\LittleBullet");
            cinetique_t = content.Load<Texture2D>("img\\bullets\\Bullet");
            laser_t = content.Load<Texture2D>("img\\bullets\\LaserBullet"); // Pour l'instant...
            ion_t = content.Load<Texture2D>("img\\bullets\\IonBullet");
            plasma_t = content.Load<Texture2D>("img\\bullets\\PlasmaBullet");
            missile_t = content.Load<Texture2D>("img\\bullets\\Missile");

            fumee_t = content.Load<Texture2D>("img\\effects\\MissileFumee");
            explosion_t = content.Load<Texture2D>("img\\effects\\Explosion");
            grosseExplosion_t = content.Load<Texture2D>("img\\effects\\ExplosionMk3");

            lifeBrick_t = content.Load<Texture2D>("img\\life");

            laserFire_s = content.Load<SoundEffect>("sounds\\laserFire2");
            laserHit_s = content.Load<SoundEffect>("sounds\\laserHit");
            missileLaunch_s = content.Load<SoundEffect>("sounds\\MissileLaunch");
            missileHit_s = content.Load<SoundEffect>("sounds\\missileHit");
        }
    }
}