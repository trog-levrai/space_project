using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PositronNova
{

    class FinalScreen : GameScreen
    {
        KeyboardState keyboardState;
        Texture2D image;
        Rectangle imageRectangle;

        public FinalScreen(Game game, SpriteBatch spriteBatch, Texture2D image)
            :base(game, spriteBatch)
        {
            this.image= image;
            imageRectangle = new Rectangle(0 + (int)Camera2d.Origine.X, 0 + (int)Camera2d.Origine.Y, PositronNova.winWidth, PositronNova.winHeight);
        }

        public override void  Draw(GameTime gameTime)
        {
            spriteBatch.Draw(image, new Rectangle(0, 0, 500, 500), Color.White);
 	         base.Draw(gameTime);
        }
    }
}
