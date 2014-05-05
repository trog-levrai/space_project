using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PositronNova.Class;
using PositronNova.Class.Unit;

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

        BulletType bulletType;
        Unit target;
        public BulletType BulletType
        {
            get { return bulletType; }
        }

        public bool destruc = false;
        int damage;

        ///////////////////////////////// CONSTRUCTEURS

        public Bullet(Vector2 origine, Unit target, BulletType bulletType)
            : base(origine, target.centre)
        {
            this.target = target;
            this.bulletType = bulletType;
            switch (bulletType)
            {
                case BulletType.LittleCinetique:
                    texture = TextureManager.littleCinetique_t;
                    speed = 4;
                    damage = 1;
                    break;
                case BulletType.Cinetique:
                    texture = TextureManager.cinetique_t;
                    speed = 4;
                    damage = 5;
                    break;
                case BulletType.Laser:
                    texture = TextureManager.laser_t;
                    speed = 6;
                    damage = 10;
                    break;
                case BulletType.Ion:
                    texture = TextureManager.ion_t;
                    speed = 8;
                    damage = 15;
                    break;
                case BulletType.Plasma:
                    texture = TextureManager.plasma_t;
                    speed = 8;
                    damage = 20;
                    break;
                case BulletType.Missile:
                    texture = TextureManager.missile_t;
                    frequenceSpawnFumee = new TimeSpan(0, 0, 0, 0, 2);
                    speed = 12;
                    damage = 50;
                    break;
            }
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
        }

        //UPDATE & DRAW

        public void Update(GameTime gameTime)
        {
            if (bulletType == BulletType.Missile)
            {
                spawnLast = spawnLast.Add(gameTime.ElapsedGameTime);

                if (frequenceSpawnFumee <= spawnLast)
                {
                    spawnLast = new TimeSpan(0);
                    PositronNova.AddEffect(new EffectBullet(position - new Vector2(texture.Width / 2, texture.Height / 2), EffectType.MissileFumee));
                }
            }

            Deplacement();
            Destruction();
            HitTarget();
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, null, Color.White, textureRotation, centre, 1f, SpriteEffects.None, 0);
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
            }
        }
    }
}
