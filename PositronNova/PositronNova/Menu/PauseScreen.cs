using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PositronNova
{
    class PauseScreen : GameScreen
    {
        PauseComponent pauseComponent;
        Texture2D image;
        Rectangle imageRectangle;

        public int SelectedIndex
        {
            get { return pauseComponent.SelectedIndex; }
            set { pauseComponent.SelectedIndex = value; }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public PauseScreen(Game game, SpriteBatch spriteBatch, SpriteFont spriteFont, Texture2D image)
            : base(game, spriteBatch)
        {
            string[] pauseItems = { "Resume", "Settings", "Quit" };
            pauseComponent = new PauseComponent(game, spriteBatch, spriteFont, pauseItems);

            Components.Add(pauseComponent);
            this.image = image;
            //imageRectangle = new Rectangle((Game.Window.ClientBounds.Width - image.Width) / 2,
            //    (Game.Window.ClientBounds.Height - image.Height) / 2,
            //    image.Width,
            //    image.Height);

            imageRectangle = new Rectangle(((Game.Window.ClientBounds.Width - image.Width) / 2), ((Game.Window.ClientBounds.Height - image.Height) / 2), image.Width, image.Height);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(image, new Vector2(((Game.Window.ClientBounds.Width - image.Width) / 2) + Camera2d.Origine.X, ((Game.Window.ClientBounds.Height - image.Height) / 2) + Camera2d.Origine.Y), Color.White);
            base.Draw(gameTime);
        }
    }
}
