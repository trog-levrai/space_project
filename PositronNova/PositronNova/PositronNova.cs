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

        static public int winWidth = 800, winHeight = 600; // Accessible pour les autres classes...
        //Gestion des images...
        //Pour la gestion du serveur
        private static Thread _thEcoute;

        static private Texture2D backgroundTexture;
        static public Texture2D BackgroundTexture
        {
            get { return backgroundTexture; }
        }


        // WORLD ELEMENTS
        //public List<Unit> unitList = new List<Unit>();
        //public List<Bullet> bulletList = new List<Bullet>();


        private Unit[] units;
        private Fighter nyan;
        private Destroyer ennemy;

        Camera2d _camera;

        private SpriteFont chat;
        private Chat text;

        public PositronNova()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = winWidth; // Definition de la taille de l'écran...
            graphics.PreferredBackBufferHeight = winHeight;
        }



        Video vid;
        VideoPlayer vidPlayer;

        Texture2D vidTexture;
        Rectangle vidRectangle;

        enum GameState
        {
            Video,
            MainMenu,
            Option,
            Playing
        }

        private GameState CurrentGameState = GameState.Video;

        cButton btnPlay;


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


            nyan = new Fighter("Chasseur", Content, new Vector2(500,300), true);
            ennemy = new Destroyer("Mechant", Content, new Vector2(300,300), false);
            units = new Unit[2];
            units[0] = nyan;
            units[1] = ennemy;

            text = new Chat();

            vidPlayer = new VideoPlayer();

            _thEcoute = new Thread(new ParameterizedThreadStart(Ecouter));
            _thEcoute.Start(text);
            _thEcoute.IsBackground = true;

            base.Initialize();
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

            backgroundTexture = Content.Load<Texture2D>("Background");

            _camera = new Camera2d(GraphicsDevice.Viewport);

            chat = Content.Load<SpriteFont>("chat");

            vid = Content.Load<Video>("Video\\Vid");
            vidRectangle = new Rectangle(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            vidPlayer.Play(vid);

            btnPlay = new cButton(Content.Load<Texture2D>("img\\Bouton"), graphics.GraphicsDevice);
            btnPlay.setPosition(new Vector2(150, 150));
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            Vector2 movement = Vector2.Zero;

            switch (CurrentGameState)
            {
                case GameState.Video:
                    if (vidPlayer.State == MediaState.Stopped || (keyboardState.IsKeyDown(Keys.Space)))
                        CurrentGameState = GameState.MainMenu;
                    _camera.Update2(gameTime, keyboardState, mouse);
                    break;
                case GameState.MainMenu:
                    vidPlayer.Stop();
                    if (btnPlay.IsCliked) CurrentGameState = GameState.Playing;
                    btnPlay.Update(mouse);
                    _camera.Update2(gameTime, keyboardState, mouse);
                    break;

                case GameState.Playing:
                    _camera.Update1(gameTime, keyboardState, mouse);
                    _camera.Update2(gameTime, keyboardState, mouse);
                    break;
            }


            Unit selected = null;
            foreach (var unit in units)
            {
                if (unit.Friendly)
                {
                    if (unit.sprite.Selected)
                    {
                        selected = unit;
                    }
                }
            }
            foreach (var unit in units)
            {
                unit.Update(gameTime);
                if (unit.Friendly)
                {
                    unit.sprite.HandleInput(Keyboard.GetState(), Mouse.GetState());
                }
                else
                {
                    if (unit.sprite.GetEnnemy(Mouse.GetState()))
                    {
                        selected.Ennemy = unit;
                    }
                    else
                    {
                        if (nyan.Ennemy == unit && Mouse.GetState().LeftButton == ButtonState.Pressed && selected != null)
                        {
                            selected.Ennemy = null;
                        }
                    }
                }
            }



            /*if (keyboardState.IsKeyDown(Keys.Left))

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

                _camera.Zoom += 0.05f;*/
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
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, _camera.transforme);

            vidTexture = vidPlayer.GetTexture();
            spriteBatch.Draw(vidTexture, vidRectangle, Color.White);
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Draw(Content.Load<Texture2D>("BackgroudMenu"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    btnPlay.Draw(spriteBatch);
                    break;

                case GameState.Playing:
                    spriteBatch.Draw(backgroundTexture, Vector2.Zero, backgroundTexture.Bounds, Color.White);
                    spriteBatch.DrawString(chat, text.ReturnString(Keyboard.GetState()), text.GetPosition(), Color.AntiqueWhite);
                    //Affichage des unites si vous n'aviez pas compris
                     foreach (var unit in units)
                    {
                        unit.Draw(spriteBatch, gameTime);
                    }
                    break;
            }



            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}