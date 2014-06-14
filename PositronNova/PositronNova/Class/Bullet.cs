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
        LittleCinetique, Cinetique, Laser, Ion, Plasma, Missile,
        BloodSting
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

        public bool destruc = false; 
        bool hitTarget = false;
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
                    if (Universite.Changement_precision)
                        damage = 6;
                    else
                        damage = 4;
                    break;
                case BulletType.Cinetique:
                    texture = Manager.cinetique_t;
                    speed = 6;
                    if (Universite.Changement_precision)
                        damage = 12;
                    else
                        damage = 8;
                    break;
                case BulletType.Laser:
                    texture = Manager.laser_t;
                    hitNoise = Manager.laserHit_s;
                    speed = 10;
                    if (Universite.Changement_precision)
                        damage = 30;
                    else
                        damage = 20;
                    break;
                case BulletType.Ion:
                    texture = Manager.ion_t;
                    speed = 10;
                    if (Universite.Changement_precision)
                        damage = 42;
                    else
                        damage = 28;
                    break;
                case BulletType.Plasma:
                    texture = Manager.plasma_t;
                    hitNoise = Manager.plasmaHit_s;
                    speed = 10;
                    if (Universite.Changement_precision)
                        damage = 54;
                    else
                        damage = 36;
                    break;
                case BulletType.Missile:
                    texture = Manager.missile_t;
                    hitNoise = Manager.missileHit_s;
                    frequenceSpawnFumee = new TimeSpan(0, 0, 0, 0, 2);
                    speed = 12;
                    if (Universite.Changement_precision)
                        damage = 75;
                    else
                        damage = 50;
                    break;
                case BulletType.BloodSting:
                    texture = Manager.bloodSting_t;
                    speed = 4;
                    damage = 4;
                    break;
            }

            hitBoxes = new Rectangle[1];
            hitBoxes[0] = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            centre = new Vector2(texture.Width / 2, texture.Height / 2);
            //textureData = new Color[texture.Width * texture.Height];
            //texture.GetData(textureData);
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
                if (position.X + centre.X < PositronNova.winWidth + Camera2d.Origine.X &&
                    position.X + centre.X > Camera2d.Origine.X &&
                    position.Y + centre.Y < PositronNova.winWidth + Camera2d.Origine.Y &&
                    position.Y + centre.Y > Camera2d.Origine.Y)
                {
                    hitNoise.Play();
                }

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
            hitBoxes[0].X = (int)position.X;
            hitBoxes[0].Y = (int)position.Y;
        }

        void Destruction()
        {
            if (position.X < 0 || position.X + texture.Width > PositronNova.BackgroundTexture.Width || position.Y < 0 || position.Y + texture.Height > PositronNova.BackgroundTexture.Height)
                destruc = true;
        }

        void HitTarget()
        {
            if (target != null && target.Pv > 0)
            {
                for (int i = 0; i < target.hitBoxes.Length; i++)
                {
                    if (hitBoxes[0].Intersects(target.hitBoxes[i]))
                    {
                        target.Pv -= damage;
                        destruc = true;
                        hitTarget = true;
                    }
                }
            }
        }
    }
}
