using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using PositronNova.Class.Unit;
using PositronNova.Class;

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
        static List<Unit> unitList = new List<Unit>();
        static List<Bullet> bulletList = new List<Bullet>();

        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;

        GameScreen activeScreen;
        StartScreen startScreen;
        ActionScreen actionScreen;

        Camera2d _camera;

        Random rand = new Random();

        private SpriteFont chat;
        private Chat text;

        public PositronNova()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            text = new Chat(this);

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

            backgroundTexture = Content.Load<Texture2D>("Background");

            genUnit(20, true);
            genUnit(20, false);

            vidPlayer = new VideoPlayer();

            _thEcoute = new Thread(new ParameterizedThreadStart(Ecouter));
            _thEcoute.Start(text);
            _thEcoute.IsBackground = true;

            UdpClient udpClient = new UdpClient();
            byte[] msg = Encoding.Default.GetBytes("nick:trog");
            udpClient.Send(msg, msg.Length, "10.3.140.222", 1234);
            udpClient.Close();

            base.Initialize();
        }
        private static void Ecouter(Object txt)
        {
            //On crée le serveur en lui spécifiant le port sur lequel il devra écouter.
            UdpClient serveur = new UdpClient(1234);
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
            TextureManager.ContentLoad(Content);

            foreach (Unit unit in unitList)
                unit.LoadContent(Content);

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
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            //    this.Exit();
            
            //Vector2 movement = Vector2.Zero;

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

                        for (int i = 0; i < unitList.Count; i++)
                        {
                            unitList[i].HandleInput(keyboardState, mouse);
                            unitList[i].Update(gameTime);
                            if (unitList[i].Destruction())
                            {
                                unitList.RemoveAt(i);
                                if (i > 0)
                                    i--;
                            }
                            for (int j = 0; j < unitList.Count - 1; j++)
                                if (i != j && unitList[i].CollisionInterVaisseau(unitList[j]))
                                    unitList[i].moving = false;

                        }

                        for (int i = 0; i < bulletList.Count; i++)
                        {
                            bulletList[i].Update(gameTime);
                            if (bulletList[i].destruc)
                            {
                                bulletList.RemoveAt(i);
                                i--;
                            }
                        }

                        // TOUT LE CODE CONCERNANT LE LOGIQUE DU JEU DOIT ETRE MIS ICIIIIIIIIII !!!!!!!!!
                        
                        //UdpClient udpClient = new UdpClient();
                        //udpClient.Send(msg, msg.Length, "10.3.140.222", 1234);
                        //udpClient.Close();
                        //foreach (var unit in unitList)
                        //{
                        //    if (unit.Friendly)
                        //    {
                        //        BinaryFormatter bin = new BinaryFormatter();
                        //        Stream stream = new NetworkStream(new Socket());
                        //        bin.Serialize(stream, unit.position);
                        //        udpClient.Send(()bin, 1024, "10.3.140.222", 1234);
                        //    }
                        //}

                        text.KBInput(Keyboard.GetState());
                    }
                    break;
            }

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

            switch (CurrentGameState)
            {
                case GameState.Video:
                    vidTexture = vidPlayer.GetTexture();
                    spriteBatch.Draw(vidTexture, vidRectangle, Color.White);
                    break;
                case GameState.Game:
                    if (activeScreen == startScreen)
                    {
                        startScreen.Draw(gameTime);
                    }
                    if (activeScreen == actionScreen)
                    {
                        actionScreen.Draw(gameTime);
                        spriteBatch.DrawString(chat, text.ReturnString(Keyboard.GetState()), text.GetPosition(), Color.AntiqueWhite);

                        foreach (var unit in unitList) //Affichage des unites si vous n'aviez pas compris
                            unit.Draw(spriteBatch);
                        foreach (Bullet bullet in bulletList) // Affichage des bullets :p
                            bullet.Draw(spriteBatch);
                    }
                    break;
            }


            spriteBatch.End();
            //base.Draw(gameTime);
        }

        //////////////////////////////////////////////////////METHODES/////////////////////////////////////////////////////

        private bool CheckKey(Keys theKey)
        {
            return
            keyboardState.IsKeyUp(theKey) &&
            oldKeyboardState.IsKeyDown(theKey);
        }

        void genUnit(int nombre, bool friendly)
        {
            for (int i = 0; i < nombre; i++)
            {
                unitList.Add(new Unit("RandomName", new Vector2(rand.Next(0, BackgroundTexture.Width - 300), rand.Next(0, BackgroundTexture.Height - 200)), (UnitType)rand.Next((int)UnitType.Chasseur, (int)UnitType.Cuirasse + 1), friendly));
                //if (unitList.Count > 1)    
                //for (int j = 0; j < i; j++)
                //        while (Physique.IntersectPixel(unitList[j].texture, unitList[j].position, unitList[j].textureData, unitList[i].texture, unitList[i].position, unitList[i].textureData))
                //            unitList[j].position = new Vector2(rand.Next(0, BackgroundTexture.Width - 300), rand.Next(0, BackgroundTexture.Height - 200));
            }
            foreach (Unit unit in unitList)
                unit.Init();
        }

        static public Unit GetEnnemy()
        {
            foreach (Unit unit in unitList)
                if (Math.Abs(Mouse.GetState().X - (unit.position.X + unit.texture.Width / 2) + Camera2d.Origine.X) <= unit.texture.Width / 2 & Math.Abs(Mouse.GetState().Y - (unit.position.Y + unit.texture.Height / 2) + Camera2d.Origine.Y) <= unit.texture.Height / 2)
                    return unit;
            return null;
        }

        static public void AddBullet(Bullet bullet)
        {
            bulletList.Add(bullet);
        }
    }
}