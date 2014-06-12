using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PositronNova.Class
{
    public enum EffectType
    {
        MissileFumee, ExplosionMissile, Explosion, GrosseExplosion, ExplosionLaser, ExplosionPlasma
    };

    public class EffectBullet : sprite
    {
        EffectType effect;
        public bool destruc = false;

        System.TimeSpan tempsDeVie;
        System.TimeSpan duree;

        public EffectBullet(Vector2 position, EffectType effect)
            : base(position)
        {
            this.effect = effect;
            switch (effect)
            {
                case EffectType.MissileFumee:
                    texture = Manager.fumee_t;
                    tempsDeVie = new TimeSpan(0, 0, 0, 0, 40);
                    break;
                case EffectType.Explosion:
                    texture = Manager.explosion_t;
                    tempsDeVie = new TimeSpan(0, 0, 0, 0, 40);
                    break;
                case EffectType.GrosseExplosion:
                    texture = Manager.grosseExplosion_t;
                    tempsDeVie = new TimeSpan(0, 0, 0, 0, 40);
                    break;
                case EffectType.ExplosionLaser:
                    texture = Manager.explosionLaser_t;
                    tempsDeVie = new TimeSpan(0, 0, 0, 0, 40);
                    break;
                case EffectType.ExplosionPlasma:
                    texture = Manager.explosionPlasma_t;
                    tempsDeVie = new TimeSpan(0, 0, 0, 0, 40);
                    break;
                case EffectType.ExplosionMissile:
                    texture = Manager.explosionMissile_t;
                    tempsDeVie = new TimeSpan(0, 0, 0, 0, 20);
                    break;
            }

            hitbox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public override void Update(GameTime gameTime)
        {
            duree = duree.Add(gameTime.ElapsedGameTime);

            if (tempsDeVie <= duree)
            {
                duree = new TimeSpan(0);
                frameSquare++;
            }

            if (frameSquare == (texture.Width / texture.Height))
                destruc = true;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position - new Vector2(texture.Height / 2, texture.Height / 2), new Rectangle(frameSquare * texture.Height, 0, texture.Height, texture.Height), Color.White);
        }

    }
}
