using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PositronNova.Class
{
    enum bulletType
    {
        cinetique, laser, ion, plasma, missile
    };

    class Bullet : sprite
    {
        bulletType bulletType;
        public bool destruc = false;
        int damage;

        static string assetName;

        // CONSTRUCTEURS

        public Bullet(Vector2 position, float rotation, ContentManager content, string assetName)
            : base(position, rotation, content, assetName)
        {
            
        }

        //public Bullet(Vector2 position, float rotation, bulletType bt, ContentManager content)
        //    : base(position, rotation, content, assetName)
        //{
        //    bulletType = bt;
        //    switch (bulletType)
        //    {
        //        case bulletType.cinetique:
        //            damage = 5;
        //            assetName = "Laser";
        //            break;
        //        case bulletType.laser:
        //            damage = 10;
        //            break;
        //        case bulletType.ion:
        //            damage = 15;
        //            break;
        //        case bulletType.plasma:
        //            damage = 20;
        //            break;
        //        case bulletType.missile:
        //            damage = 25;
        //            break;
        //    }
        //}

        // METHODES

        void deplacement()
        {
            Position += Direction * Speed;
        }

        void Destruction()
        {
            if (Position.X < 0 || Position.X + Texture.Width > PositronNova.BackgroundTexture.Width || Position.Y < 0 || Position.Y + Texture.Height > PositronNova.BackgroundTexture.Height)
                destruc = true;
        }

        //UPDATE & DRAW

        public void Update(GameTime gameTime)
        {
            deplacement();
            Destruction();
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Texture, Position, null, Color.White, textureRotation, textureOrigine, 1f, SpriteEffects.None, 0);
        }
    }
}
