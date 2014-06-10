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

        Texture2D boom;
        
        private float elapsed;
        public float Elapsed
                {
            get { return elapsed; }
            set { elapsed = value; }
        }

        private float delay = 150f;
        public float Delay
        {
            get { return delay; }
            set { delay = value; }
        }

        private int frames = 0;
        public int Frames
        {
            get { return frames; }
            set { frames = value; }
        }

        private Rectangle destination;
        public Rectangle Destination
        {
            get { return destination; }
            set { destination = value; }
        }

        private Rectangle source;
        public Rectangle Source
        {
            get { return source; }
            set { source = value; }
        }

        protected override void Initialize()
        {
            destination = new Rectangle(50, 50, 80, 80);

            base.Initialize();
        }


        public void LoadContent()
        {
            boom = Content.Load<Texture2D>("img\\boom80");

            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        public void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsed >= delay)
            {
                frames++;
                elapsed = 0;
            }

            source = new Rectangle(80 * frames, 0, 80, 80);

            base.Update(gameTime);
        }


        public void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(boom, destination, source ,Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
