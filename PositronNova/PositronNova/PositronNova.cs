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

        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;

        GameScreen activeScreen;
        StartScreen startScreen;
        ActionScreen actionScreen;

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
            Game
        }

        private GameState CurrentGameState = GameState.Video;



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

            startScreen = new StartScreen(
                this,
                spriteBatch,
                Content.Load<SpriteFont>("menufont"),
                Content.Load<Texture2D>("BackgroudMenu"));
            Components.Add(startScreen);
            startScreen.Hide();

            actionScreen = new ActionScreen(
                this,
                spriteBatch,
                Content.Load<Texture2D>("Background"));
            Components.Add(actionScreen);
            actionScreen.Hide();

            activeScreen = startScreen;
            activeScreen.Show();

            chat = Content.Load<SpriteFont>("chat");

            vid = Content.Load<Video>("Video\\Vid");
            vidRectangle = new Rectangle(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            vidPlayer.Play(vid);
            UdpClient udpClient = new UdpClient();
            byte[] msg = Encoding.Default.GetBytes("nick:trog");
            udpClient.Send(msg, 9, "10.3.140.222", 1234);
            udpClient.Close();
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
                        CurrentGameState = GameState.Game;
                    _camera.Update2(gameTime, keyboardState, mouse);
                    break;
                case GameState.Game:
                    vidPlayer.Stop();
                    if (activeScreen == startScreen)
                    {
                        _camera.Update2(gameTime, keyboardState, mouse);
                        if (CheckKey(Keys.Enter))
                        {
                            if (startScreen.SelectedIndex == 0)
                            {
                                activeScreen.Hide();
                                activeScreen = actionScreen;
                                activeScreen.Show();
                            }
                            if (startScreen.SelectedIndex == 2)
                                this.Exit();
                        }
                    }
                    if (activeScreen == actionScreen)
                    {
                        _camera.Update1(gameTime, keyboardState, mouse);
                        _camera.Update2(gameTime, keyboardState, mouse);

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
                                if (unit.sprite.GetEnnemy(Mouse.GetState()) && selected != null)
                                {
                                    selected.Ennemy = unit;
                                    selected.sprite.Selected = true;
                                }
                                else
                                {
                                    if (selected != null && selected.Ennemy == unit && Mouse.GetState().LeftButton == ButtonState.Pressed)
                                    {
                                        selected.Ennemy = null;
                                    }
                                }
                            }
                        }

                        // TOUT LE CODE CONCERNANT LE LOGIQUE DU JEU DOIT ETRE MIS ICIIIIIIIIII !!!!!!!!!


                        text.KBInput(Keyboard.GetState());
                    }
                    break;
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

            base.Update(gameTime);

            oldKeyboardState = keyboardState;
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

                case GameState.Game:
                    if (activeScreen == startScreen)
                    {
                        startScreen.Draw(gameTime);
                    }
                    if (activeScreen == actionScreen)
                    {
                        actionScreen.Draw(gameTime);
                        spriteBatch.DrawString(chat, text.ReturnString(Keyboard.GetState()), text.GetPosition(), Color.AntiqueWhite);
                        //Affichage des unites si vous n'aviez pas compris
                        foreach (var unit in units)
                        {
                            unit.Draw(spriteBatch, gameTime);
                        }
                    }
                    break;
            }


            spriteBatch.End();
            //base.Draw(gameTime);
        }

        private bool CheckKey(Keys theKey)
        {
            return
            keyboardState.IsKeyUp(theKey) &&
            oldKeyboardState.IsKeyDown(theKey);
        }
    }
}