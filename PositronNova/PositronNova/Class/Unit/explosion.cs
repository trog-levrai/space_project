using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace AnimationExplosion
{



    public class explosion : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public explosion()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        private Texture2D boom;
        private float elapsed;
        private float delay = 150f;
        private int frames = 0;
        private Rectangle destination;
        private Rectangle source;

        protected override void Initialize()
        {
            destination = new Rectangle(340, 200, 80, 80);

            base.Initialize();
        }


        protected override void LoadContent()
        {
            boom = Content.Load<Texture2D>("img\\boom80");

            spriteBatch = new SpriteBatch(GraphicsDevice);
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsed >= delay)
            {
                frames++;
                /*if (frames >= 7)
                {
                    frames = 0;
                }
                else
                {
                    frames++;
                }*/
                elapsed = 0;
            }

            source = new Rectangle(80 * frames, 0, 80, 80);

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(boom, destination, source ,Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
