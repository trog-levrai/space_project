using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PositronNova.Class;
using PositronNova.Class.Unit;
using Microsoft.Xna.Framework.Audio;

namespace PositronNova
{
    public enum BulletType
    {
        LittleCinetique, Cinetique, Laser, Ion, Plasma, Missile
    };

    public class Bullet : sprite
    {
        System.TimeSpan frequenceSpawnFumee;
        System.TimeSpan spawnLast;

        SoundEffect hitNoise;
        BulletType bulletType;
        Unit target;
        public BulletType BulletType
        {
            get { return bulletType; }
        }

        public bool destruc = false; //
        bool hitTarget = false; // Degeulasse mais c'est en attendant
        int damage;

        ///////////////////////////////// CONSTRUCTEURS

        public Bullet(Vector2 origine, Unit target, BulletType bulletType)
            : base(origine, target.position + target.centre)
        {
            this.target = target;
            this.bulletType = bulletType;
            switch (bulletType)
            {
                case BulletType.LittleCinetique:
                    texture = Manager.littleCinetique_t;
                    speed = 4;
                    damage = 1;
                    break;
                case BulletType.Cinetique:
                    texture = Manager.cinetique_t;
                    speed = 4;
                    damage = 5;
                    break;
                case BulletType.Laser:
                    texture = Manager.laser_t;
                    //hitNoise = Manager.laserHit_s;
                    speed = 6;
                    damage = 10;
                    break;
                case BulletType.Ion:
                    texture = Manager.ion_t;
                    speed = 8;
                    damage = 15;
                    break;
                case BulletType.Plasma:
                    texture = Manager.plasma_t;
                    speed = 8;
                    damage = 20;
                    break;
                case BulletType.Missile:
                    texture = Manager.missile_t;
                    hitNoise = Manager.missileHit_s;
                    frequenceSpawnFumee = new TimeSpan(0, 0, 0, 0, 2);
                    speed = 12;
                    damage = 50;
                    break;
            }
            centre = new Vector2(texture.Width / 2, texture.Height / 2);
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
        }

        //UPDATE & DRAW

        public override void Update(GameTime gameTime)
        {
            if (bulletType == BulletType.Missile)
            {
                spawnLast = spawnLast.Add(gameTime.ElapsedGameTime);

                if (frequenceSpawnFumee <= spawnLast)
                {
                    spawnLast = new TimeSpan(0);
                    PositronNova.AddEffect(new EffectBullet(position + centre, EffectType.MissileFumee));
                }
            }

            Deplacement();
            Destruction();
            HitTarget();
            if (hitTarget && hitNoise != null)
                hitNoise.Play();
            
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position + centre, null, Color.White, textureRotation, centre, 1f, SpriteEffects.None, 0);
        }

        // METHODES

        void Deplacement()
        {
            position += direction * speed;
        }

        void Destruction()
        {
            if (position.X < 0 || position.X + texture.Width > PositronNova.BackgroundTexture.Width || position.Y < 0 || position.Y + texture.Height > PositronNova.BackgroundTexture.Height)
                destruc = true;
        }

        void HitTarget()
        {
            if (target != null && target.Pv > 0 && Physique.IntersectPixel(texture, position, textureData, target.texture, target.position, target.textureData))
            {
                target.Pv -= damage;
                destruc = true;
                hitTarget = true;
            }
        }
    }
}
