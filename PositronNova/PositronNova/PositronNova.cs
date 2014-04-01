using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
        //Pour la gestion du serveur
        private static Thread _thEcoute;
        private Unit[] units;
        private Texture2D background;
        private Fighter nyan;
        private Destroyer ennemy;
        Camera2d _camera;
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
            nyan = new Fighter("Chasseur", Content, new Vector2(10,10), true);
            ennemy = new Destroyer("Mechant", Content, new Vector2(300,300), false);
            units = new Unit[2];
            units[0] = nyan;
            units[1] = ennemy;
            text = new Chat();
            base.Initialize();
            _thEcoute = new Thread(new ParameterizedThreadStart(Ecouter));
            _thEcoute.Start(text);
        }
        private static void Ecouter(Object txt)
        {
            //On crée le serveur en lui spécifiant le port sur lequel il devra écouter.
            UdpClient serveur = new UdpClient(5035);
            //Création d'une boucle infinie qui aura pour tâche d'écouter.
            while (true)
            {
                IPEndPoint client = null;
                byte[] data = serveur.Receive(ref client);
                string message = Encoding.Default.GetString(data);
                ((Chat) txt).addString(message);
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("img\\wallpaper2");
            _camera = new Camera2d(background.Width, background.Height, GraphicsDevice);
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
            foreach (var unit in units)
            {
                unit.Update(gameTime);
            }
            KeyboardState keyboardState = Keyboard.GetState();

            Vector2 movement = Vector2.Zero;


            if (keyboardState.IsKeyDown(Keys.Left))

                movement.X--;

            if (keyboardState.IsKeyDown(Keys.Right))

                movement.X++;

            if (keyboardState.IsKeyDown(Keys.Up))

                movement.Y--;

            if (keyboardState.IsKeyDown(Keys.Down))

                movement.Y++;

            _camera.Pos += movement * 20;

            //PageDown et PageUp pour le zoom

            if (keyboardState.IsKeyDown(Keys.PageDown))

                _camera.Zoom -= 0.05f;

            if (keyboardState.IsKeyDown(Keys.PageUp))

                _camera.Zoom += 0.05f;
            text.KBInput(Keyboard.GetState());
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
            spriteBatch.Draw(background, Vector2.Zero, background.Bounds, Color.White);
            spriteBatch.DrawString(chat, text.ReturnString(Keyboard.GetState()), text.GetPosition(), Color.AntiqueWhite);
            //Affichage des unites si vous n'aviez pas compris
            foreach (var unit in units)
            {
                unit.Draw(spriteBatch, gameTime);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}