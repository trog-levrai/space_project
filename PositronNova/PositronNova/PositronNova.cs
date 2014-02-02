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
using PositronNova.Class.Unit;

namespace PositronNova
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PositronNova : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //Gestion des images...
        private Texture2D background;
        private Unit nyan;
        private SpriteFont _font;
        Camera2d _camera;
        public PositronNova()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //Les deux lignes suivantes sont là pour une histoire de FPS. A modifier quand on saura faire donc...
            IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = false;
            this.IsMouseVisible = true;
            nyan = new Unit("Nyan avec classe :-)", 1, Content);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("img\\background");
            _camera = new Camera2d(background.Width, background.Height, GraphicsDevice);
            _font = Content.Load<SpriteFont>("Affichage_mouse");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            nyan.Update(gameTime);
            KeyboardState keyboardState = Keyboard.GetState();

            Vector2 movement = Vector2.Zero;

            if (keyboardState.IsKeyDown(Keys.Q))

                movement.X--;

            if (keyboardState.IsKeyDown(Keys.D))

                movement.X++;

            if (keyboardState.IsKeyDown(Keys.Z))

                movement.Y--;

            if (keyboardState.IsKeyDown(Keys.S))

                movement.Y++;

            _camera.Pos += movement * 20;

            //PageDown et PageUp pour le zoom

            if (keyboardState.IsKeyDown(Keys.PageDown))

                _camera.Zoom -= 0.05f;

            if (keyboardState.IsKeyDown(Keys.PageUp))

                _camera.Zoom += 0.05f;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGoldenrodYellow);
            spriteBatch.Begin(SpriteSortMode.Immediate,

        BlendState.AlphaBlend, SamplerState.PointClamp,

        null, null, null, _camera.GetTransformation());
            //Laiser cette ligne en première position.
            //spriteBatch.Draw(background, Vector2.Zero, Color.White);
            spriteBatch.Draw(background, Vector2.Zero, background.Bounds, Color.White);
            nyan.sprite.Draw(spriteBatch, gameTime);
            spriteBatch.DrawString(_font, nyan.Name, new Vector2(nyan.sprite.Position.X - 3,nyan.sprite.Position.Y - 15), Color.Thistle);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}