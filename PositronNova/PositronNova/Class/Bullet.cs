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
                    speed = 10;
                    damage = 20;
                    break;
                case BulletType.Missile:
                    texture = TextureManager.missile_t;
                    speed = 12;
                    damage = 25;
                    break;
            }

            textureOrigine = new Vector2(texture.Width / 2, texture.Height / 2);
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
        }

        //UPDATE & DRAW

        public void Update(GameTime gameTime)
        {
            Deplacement();
            Destruction();
            HitTarget();
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, null, Color.White, textureRotation, textureOrigine, 1f, SpriteEffects.None, 0);
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
