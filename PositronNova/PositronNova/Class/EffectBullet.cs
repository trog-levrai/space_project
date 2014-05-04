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
        MissileFumee
    };

    public class EffectBullet : sprite
    {
        EffectType effect;
        public bool destruc = false;
        int frameSquare;

        System.TimeSpan tempsDeVie;
        System.TimeSpan duree;

        public EffectBullet(Vector2 position, EffectType effect)
            : base(position)
        {
            this.effect = effect;
            switch (effect)
            {
                case EffectType.MissileFumee:
                    texture = TextureManager.fumee_t;
                    tempsDeVie = new TimeSpan(0, 0, 0, 0, 40);
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            duree = duree.Add(gameTime.ElapsedGameTime);

            if (effect == EffectType.MissileFumee)
            {
                if (tempsDeVie <= duree)
                {
                    duree = new TimeSpan(0);
                    frameSquare++;
                }

                if (frameSquare == (texture.Width / texture.Height))
                    destruc = true;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, new Rectangle(frameSquare * texture.Height, 0, texture.Height, texture.Height), Color.White);
        }

    }
}
