﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PositronNova
{
    class ActionScreen : GameScreen
    {
        KeyboardState keyboardState;
        Texture2D image;
        Rectangle imageRectangle;

        public ActionScreen(Game game, SpriteBatch spriteBatch, Texture2D image)
            :base(game, spriteBatch)
        {
            this.image = image;
            imageRectangle = new Rectangle(0,0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            keyboardState = Keyboard.GetState();


            // A decommenter si on veut quitter le jeu directement avec echap
            //if (keyboardState.IsKeyDown(Keys.Escape))
            //{
            //    game.Exit();
            //}
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(image, Vector2.Zero, image.Bounds, Color.White);
            base.Draw(gameTime);
        }
    }
}
