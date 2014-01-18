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
        private sprite nyan;
        private SpriteFont _font;
        private SpriteFont chat;
        private Chat text;
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
            nyan = new sprite();
            nyan.Direction = Vector2.Zero;
            nyan.Mouse = Vector2.Zero;
            nyan.Initialize();
            text = new Chat();
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
            nyan.LoadContent(Content, "img\\nyan");
            nyan.Speed = (float)0.5;
            _font = Content.Load<SpriteFont>("Affichage_mouse");
            chat = Content.Load<SpriteFont>("chat");
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
            // Allows the game to exit
            nyan.HandleInput(Keyboard.GetState(), Mouse.GetState());
            text.KBInput(Keyboard.GetState());
            nyan.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGoldenrodYellow);
            spriteBatch.Begin();
            //Laiser cette ligne en première position.
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            spriteBatch.DrawString(_font, "Mouse position : " + nyan.Mouse, new Vector2(0,0), Color.Red);
            spriteBatch.DrawString(_font, "Position : " + nyan.Position, new Vector2(0, 10), Color.Red);
            spriteBatch.DrawString(chat, text.ReturnString(Keyboard.GetState()), text.GetPosition(), Color.AntiqueWhite);
            nyan.Draw(spriteBatch, gameTime);
            spriteBatch.DrawString(_font, "NyanCat", new Vector2(nyan.Position.X - 3,nyan.Position.Y - 15), Color.Thistle);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}