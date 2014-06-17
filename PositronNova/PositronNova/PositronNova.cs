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
using Microsoft.Xna.Framework.Audio;

namespace PositronNova
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PositronNova : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        TimeSpan compt;
        TimeSpan last;

        static public int winWidth = 1366, winHeight = 768; // Accessible pour les autres classes...
        //Gestion des images...
        //Pour la gestion du serveur
        private static Thread _thEcoute;

        static private Texture2D backgroundTexture;
        static public Texture2D BackgroundTexture
        {
            get { return backgroundTexture; }
        }
        Texture2D image;
        Texture2D image_lost;

        // WORLD ELEMENTS
        static List<Unit> unitList = new List<Unit>();
        static public  List<Unit> UnitList 
        { 
            get { return unitList; }
            set { unitList = value; }
        }
        static List<Bullet> bulletList = new List<Bullet>();
        static List<EffectBullet> effectBulletList = new List<EffectBullet>();

        static public List<Planete> planeteList = new List<Planete>();

        static public Ressources ressources;

        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;
        MouseState oldMouse;

        GameScreen activeScreen;
        StartScreen startScreen;
        ActionScreen actionScreen;
        OptionScreen optionScreen;
        PauseScreen pauseScreen;
        FinalScreen finalScreenWon;
        bool action = false;
        static bool difficulte_easy;
        static public bool Difficulte_easy
        {
            get { return difficulte_easy; }
        }
        static bool difficulte_medium;
        static public bool Difficulte_medium
        {
            get { return difficulte_medium; }
        }
        static bool difficulte_hard;
        static public bool Difficulte_hard
        {
            get { return difficulte_hard; }
        }

        //private Song _song_jeu;
        //private Song _song_menu;

        AudioEngine engine;
        SoundBank soundBank;
        WaveBank waveBank;
        Cue cue;
        Cue cue1;
        float musicVolume = 1.0f;
        Texture2D ui;

        //Interface joueur
        IU interfaceJoueur;

        Camera2d _camera;

        // Brouillard
        BrouillardDeGuerre fog;
        bool enableFog = true;

        static Random rand = new Random();
        static public Random Rand { get { return rand; } }

        private SpriteFont chat;
        private Chat text;
        private static string ennemyName = "";

        public PositronNova()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            text = new Chat(this);

            graphics.PreferredBackBufferWidth = winWidth; // Definition de la taille de l'écran...
            graphics.PreferredBackBufferHeight = winHeight;
            graphics.IsFullScreen = true;
        }

        Video vid;
        VideoPlayer vidPlayer;
        Texture2D vidTexture;
        Rectangle vidRectangle;

        enum GameState
        {
            Video,
            Game,
            Final
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

            vidPlayer = new VideoPlayer();

            _thEcoute = new Thread(new ParameterizedThreadStart(Ecouter));
            _thEcoute.Start(text);
            _thEcoute.IsBackground = true;
            compt = new TimeSpan(0, 0, 10);

            difficulte_easy = true;
            difficulte_medium = false;
            difficulte_hard = false;

            UdpClient udpClient = new UdpClient();
            byte[] msg = Encoding.Default.GetBytes("nick:Biatch");
            udpClient.Send(msg, msg.Length, "10.45.3.241", 1234);
            udpClient.Close();

            engine = new AudioEngine("Content\\sounds\\Playsong.xgs");
            soundBank = new SoundBank(engine, "Content\\sounds\\Sound Bank.xsb");
            waveBank = new WaveBank(engine, "Content\\sounds\\Wave Bank.xwb");

            cue = soundBank.GetCue("Menu");
            cue.Play();
            cue.Pause();

            cue1 = soundBank.GetCue("Espace");
            cue1.Play();
            cue1.Pause();

            ressources = Ressources.getStartRessources();

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
                if (message.StartsWith("nick:"))
                    ennemyName = message.Substring(5);
                else
                {
                    ((Chat) txt).addString(ennemyName + " dit: " + message);
                }
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
            Manager.ContentLoad(Content);

            _camera = new Camera2d(GraphicsDevice.Viewport);
            image = Content.Load<Texture2D>("Final_screen_won");
            image_lost = Content.Load<Texture2D>("Final_screen_lost");

            #region LoadScreen
            startScreen = new StartScreen(
                this,
                spriteBatch,
                Content.Load<SpriteFont>("menufont"),
                Content.Load<SpriteFont>("titlefont"),
                Content.Load<Texture2D>("BackgroudMenu"));
            Components.Add(startScreen);
            startScreen.Hide();

            actionScreen = new ActionScreen(
                this,
                spriteBatch,
                Content.Load<Texture2D>("Background"));
            Components.Add(actionScreen);
            actionScreen.Hide();

            optionScreen = new OptionScreen(
                this,
                spriteBatch,
                Content.Load<SpriteFont>("optionfont"),
                Content.Load<Texture2D>("BackgroudMenu"),
                graphics);
            Components.Add(optionScreen);
            optionScreen.Hide();

            pauseScreen = new PauseScreen(
                this,
                spriteBatch,
                Content.Load<SpriteFont>("optionfont"),
                Content.Load<Texture2D>("PauseScreen"));
            Components.Add(pauseScreen);
            pauseScreen.Hide();

            finalScreenWon = new FinalScreen(
                this,
                spriteBatch,
                Content.Load<Texture2D>("Final_screen_won"));
            #endregion

            activeScreen = startScreen;
            startScreen.SelectedIndex = 1;
            activeScreen.Show();

            // Planete humaine
            planeteList.Add(new Planete(this,
                Content.Load<Texture2D>("img\\Planete_1"),
                Content.Load<Texture2D>("img\\Plus"),
                Content.Load<Texture2D>("img\\centrale"),
                Content.Load<Texture2D>("img\\Extracteur2"),
                Content.Load<Texture2D>("img\\Tick"),
                Content.Load<Texture2D>("img\\Caserne"),
                Content.Load<Texture2D>("img\\recrutement"),
                Content.Load<Texture2D>("img\\recrutement_grise"),
                Content.Load<Texture2D>("img\\universite"),
                Content.Load<Texture2D>("img\\Croix"),
                ressources,
                Content.Load<SpriteFont>("Planete"),
                Content.Load<SpriteFont>("progress"),
                text));

            // Planete Alien
            planeteList.Add(new Planete(this, Content.Load<Texture2D>("img\\PlaneteAlien")));

            planeteList[0].LoadContent(Content);

            chat = Content.Load<SpriteFont>("chat");

            vid = Content.Load<Video>("Video\\Vid");
            vidRectangle = new Rectangle(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            vidPlayer.Play(vid);

            ui = Content.Load<Texture2D>(@"newUI");

            interfaceJoueur = new IU();
            interfaceJoueur.ContentLoad(Content);

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
            Vector2 movement = Vector2.Zero;
            last = last.Add(gameTime.ElapsedGameTime);
            engine.Update();
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            //    this.Exit();
            
            //Vector2 movement = Vector2.Zero;
            engine.Update();
            switch (CurrentGameState)
            {
                case GameState.Video:
                    if (vidPlayer.State == MediaState.Stopped || (keyboardState.IsKeyDown(Keys.Space)))
                        CurrentGameState = GameState.Game;
                    _camera.Update2(keyboardState, mouse);
                    break;
                case GameState.Game:
                    vidPlayer.Stop();

#region StartScreen
                    if (activeScreen == startScreen)
                    {
                        cue.Resume();
                        if (cue.IsStopped)
                        {
                            cue = soundBank.GetCue("Menu");
                            cue.Play();
                        }
                        if (enableFog)
                        {
                            fog = new BrouillardDeGuerre(100, 100, BackgroundTexture.Width, BackgroundTexture.Height);
                            fog.ContentLoad(Content);
                        }

                        _camera.Update2(keyboardState, mouse);
                        if (keyboardState.IsKeyDown(Keys.Enter) && oldKeyboardState.IsKeyUp(Keys.Enter))
                        {
                            if (startScreen.SelectedIndex == 1)
                            {
                                activeScreen.Hide();
                                activeScreen = actionScreen;
                                activeScreen.Show();

                                for (int i = 0; i < unitList.Count; i++)
                                {
                                    unitList.RemoveAt(i);
                                    i--;
                                }
                                for (int i = 0; i < bulletList.Count; i++)
                                {
                                    bulletList.RemoveAt(i);
                                    i--;
                                }
                                for (int i = 0; i < effectBulletList.Count; i++)
                                {
                                    effectBulletList.RemoveAt(i);
                                    i--;
                                }
                                for (int i = 0; i < planeteList.Count; i++)
                                {
                                    planeteList[i].Pv = planeteList[i].Pv_max;
                                    planeteList[i].LifeBar.Regenerer();
                                }
                                //genHumain(10);
                                //genAlien(10);
                                if (difficulte_easy)
                                    genHumain(10);
                                else if (difficulte_medium)
                                    genHumain(5);
                                else if (difficulte_hard)
                                    genHumain(1);
                                planeteList[0].Niveau_centrale = 1;
                                planeteList[0].Niveau_extracteur = 1;
                                Planete.Niveau_caserne = 0;
                                Universite.Changement_moteur = false;
                                Universite.Changement_precision = false;
                                planeteList[0].Start();
                                ressources = Ressources.getStartRessources();

                                foreach (Unit unit in unitList)
                                    unit.Init();
                            }
                            if (startScreen.SelectedIndex == 3)
                            {
                                action = false;
                                activeScreen.Hide();
                                activeScreen = optionScreen;
                                activeScreen.Show();
                            }
                            if (startScreen.SelectedIndex == 4)
                                this.Exit();
                        }
                    }
#endregion

                    keyboardState = Keyboard.GetState();

#region OptionScreen
                    if (activeScreen == optionScreen)
                    {
                        if (action)
                        {
                            cue.Pause();
                            cue1.Resume();
                            if (cue1.IsStopped)
                            {
                                cue1 = soundBank.GetCue("Espace");
                                cue1.Play();
                            }
                        }
                        if (!action)
                        {
                            cue1.Pause();
                            cue.Resume();
                            if (cue.IsStopped)
                            {
                                cue = soundBank.GetCue("Menu");
                                cue.Play();
                            }
                        }
                        _camera.Update2(keyboardState, mouse);
                        if (CheckKey(Keys.Escape) && action == false)
                        {
                            activeScreen.Hide();
                            activeScreen = startScreen;
                            activeScreen.Show();
                        }
                        if (CheckKey(Keys.Escape) && action == true)
                        {
                            activeScreen.Hide();
                            activeScreen = pauseScreen;
                            activeScreen.Show();
                        }
                        if (optionScreen.SelectedIndex == 0)
                        {
                            if (optionScreen.SselectedIndex == 0 && keyboardState.IsKeyDown(Keys.Space) && oldKeyboardState.IsKeyUp(Keys.Space))
                                    {
                                        
                                        Window.BeginScreenDeviceChange(true);
                                        graphics.IsFullScreen = true;
                                        graphics.SynchronizeWithVerticalRetrace = true;
                                        Window.EndScreenDeviceChange(Window.ScreenDeviceName);
                                    }
                                    if (optionScreen.SselectedIndex == 1 && CheckKey(Keys.Space))
                                    {
                                        Window.BeginScreenDeviceChange(false);
                                        graphics.PreferredBackBufferWidth = winWidth;
                                        graphics.PreferredBackBufferHeight = winHeight;
                                        graphics.IsFullScreen = false;
                                        graphics.SynchronizeWithVerticalRetrace = true;
                                        Window.EndScreenDeviceChange(Window.ScreenDeviceName);
                                    }
                                
                            
                        }
                        if (optionScreen.SelectedIndex == 1)
                        {
                            if (optionScreen.SselectedIndex == 2 && keyboardState.IsKeyDown(Keys.Space) && oldKeyboardState.IsKeyUp(Keys.Space))
                            {
                                musicVolume -= 0.1f;
                                if (musicVolume < 0f)
                                    musicVolume = 0f;
                                engine.GetCategory("Default").SetVolume(musicVolume);
                            }
                            if(optionScreen.SselectedIndex == 3 && keyboardState.IsKeyDown(Keys.Space) && oldKeyboardState.IsKeyUp(Keys.Space))
                            {
                                musicVolume += 0.1f;
                                if (musicVolume > 1.0f)
                                    musicVolume = 1.0f;
                                engine.GetCategory("Default").SetVolume(musicVolume);
                            }

                        }
                        if (optionScreen.SelectedIndex == 2 && !action)
                        {
                            if (optionScreen.SselectedIndex == 4 && keyboardState.IsKeyDown(Keys.Space) && oldKeyboardState.IsKeyUp(Keys.Space))
                            {
                                difficulte_easy = true;
                                difficulte_medium = false;
                                difficulte_hard = false;
                            }
                            if (optionScreen.SselectedIndex == 5 && keyboardState.IsKeyDown(Keys.Space) && oldKeyboardState.IsKeyUp(Keys.Space))
                            {
                                difficulte_medium = true;
                                difficulte_hard = false;
                                difficulte_easy = false;
                            }
                            if (optionScreen.SselectedIndex == 6 && keyboardState.IsKeyDown(Keys.Space) && oldKeyboardState.IsKeyUp(Keys.Space))
                            {
                                difficulte_easy = false;
                                difficulte_medium = false;
                                difficulte_hard = true;
                            }
                        }

                        if (optionScreen.SelectedIndex == 3 && !action)
                        {
                            if (optionScreen.SselectedIndex == 7 && keyboardState.IsKeyDown(Keys.Space) && oldKeyboardState.IsKeyUp(Keys.Space))
                            {
                                enableFog = true;
                            }
                            if (optionScreen.SselectedIndex == 8 && keyboardState.IsKeyDown(Keys.Space) && oldKeyboardState.IsKeyUp(Keys.Space))
                            {
                                enableFog = false;
                            }
                        }
                        if (optionScreen.SelectedIndex == 4 && CheckKey(Keys.Space) && !action)
                        {
                            activeScreen.Hide();
                            activeScreen = startScreen;
                            activeScreen.Show();
                        }
                        if (optionScreen.SelectedIndex == 4 && keyboardState.IsKeyDown(Keys.Space) && oldKeyboardState.IsKeyUp(Keys.Enter) && action)
                        {
                            activeScreen.Hide();
                            activeScreen = actionScreen;
                            activeScreen.Show();
                        }

                    }
#endregion
                    keyboardState = Keyboard.GetState();
                    
#region PauseScreen
                    if (activeScreen == pauseScreen)
                    {
                        cue.Pause();
                        cue1.Resume();
                        if (enableFog)
                            fog.Update();
                        _camera.Update2(keyboardState, mouse);
                        if (CheckKey(Keys.Escape) || (pauseScreen.SelectedIndex == 0 && CheckKey(Keys.Enter)))
                        {
                            activeScreen.Hide();
                            activeScreen = actionScreen;
                            activeScreen.Show();
                        }
                        if (CheckKey(Keys.Enter))
                        {
                            if (pauseScreen.SelectedIndex == 1)
                            {
                                Camera2d.Origine = new Vector2(0, 0);
                                action = true;
                                activeScreen.Hide();
                                activeScreen = optionScreen;
                                activeScreen.Show();
                            }
                            if (pauseScreen.SelectedIndex == 2)
                            {
                                activeScreen.Hide();
                                Camera2d.Origine = new Vector2(0, 0);
                                cue1.Pause();
                                cue.Resume();
                                activeScreen = startScreen;
                                activeScreen.Show();

                                ressources = Ressources.getStartRessources();
                            }
                        }
                            
                        
                    }
#endregion

                    if (activeScreen == actionScreen)
                    {
                        cue.Pause();
                        cue1.Resume();
                        if (cue1.IsStopped)
                        {
                            cue1 = soundBank.GetCue("Espace");
                            cue1.Play();
                        }

                        _camera.Update1(keyboardState, mouse);
                        _camera.Update2(keyboardState, mouse);

                        planeteList[0].Update(gameTime, mouse, oldMouse, keyboardState, oldKeyboardState);
                        planeteList[1].Update();

                        ressources = planeteList[0].setRessource();
                        ressources.Update(gameTime, planeteList[0].Niveau_centrale, planeteList[0].Niveau_extracteur);

                        genAlienTime();

                        if (CheckKey(Keys.Escape))
                        {
                            action = true;
                            activeScreen.Hide();
                            activeScreen = pauseScreen;
                            activeScreen.Show();
                        }

                        // L'ordinateur évalue la situation
                        IAStrategie();

                        for (int i = 0; i < unitList.Count; i++)
                        {
                            unitList[i].Update(gameTime);
                            if (unitList[i].Destruction()) // Destruction des vaisseaux
                            {
                                effectBulletList.Add(new EffectBullet(unitList[i].position + unitList[i].centre, EffectType.GrosseExplosion));
                                if (unitList[i].Side == UnitSide.Humain)
                                {
                                    text.addString("f:" + unitList[i].Name + " a été détruit !");
                                    Manager.unitePerdu_s.Play();
                                }
                                else
                                {
                                    text.addString("Un Alien a été détruit !");
                                }
                                unitList.RemoveAt(i);
                                if (i > 0)
                                    i--;
                            }
                        }

                        interfaceJoueur.HandleInput(keyboardState, mouse);

                        for (int i = 0; i < bulletList.Count; i++)
                        {
                            bulletList[i].Update(gameTime);
                            if (bulletList[i].destruc)
                            {
                                if (bulletList[i].BulletType == BulletType.Missile)
                                    effectBulletList.Add(new EffectBullet(bulletList[i].position + bulletList[i].centre, EffectType.ExplosionMissile));
                                else if(bulletList[i].BulletType == BulletType.Laser)
                                    effectBulletList.Add(new EffectBullet(bulletList[i].position + bulletList[i].centre, EffectType.ExplosionLaser));
                                else if (bulletList[i].BulletType == BulletType.Plasma)
                                    effectBulletList.Add(new EffectBullet(bulletList[i].position + bulletList[i].centre, EffectType.ExplosionPlasma));
                                else
                                    effectBulletList.Add(new EffectBullet(bulletList[i].position + bulletList[i].centre, EffectType.Explosion));
                                bulletList.RemoveAt(i);
                                i--;
                            }
                        }

                        for (int i = 0; i < effectBulletList.Count; i++)
                        {
                            effectBulletList[i].Update(gameTime);
                            if (effectBulletList[i].destruc)
                            {
                                effectBulletList.RemoveAt(i);
                                i--;
                            }
                        }

                        if (enableFog)
                            fog.Update();

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

                        if (planeteList[1].Pv < 0)
                        {
                            CurrentGameState = GameState.Final;
                        }
                        if (planeteList[0].Pv < 0)
                        {
                            CurrentGameState = GameState.Final;
                        }
                    }
                    break;
                case GameState.Final:
                    if (CheckKey(Keys.Enter))
                    {
                        CurrentGameState = GameState.Game;
                        Camera2d.Origine = new Vector2(0, 0);
                        activeScreen.Hide();
                        activeScreen = startScreen;
                        activeScreen.Show();
                    }

                    break;
            }

            base.Update(gameTime);

            oldKeyboardState = keyboardState;
            oldMouse = mouse;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, _camera.transforme);
            MouseState mouse = Mouse.GetState();

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
                    if (activeScreen == optionScreen)
                    {
                        optionScreen.Draw(gameTime);
                        spriteBatch.DrawString(Content.Load<SpriteFont>("optionfont"), (Math.Round(musicVolume * 100, 0)).ToString() + "%",
                            new Vector2((PositronNova.winWidth) / 2 + 60, PositronNova.winHeight / 2 -42), Color.Red);
                    }
                    if (activeScreen == pauseScreen)
                    {
                        actionScreen.Draw(gameTime);

                        foreach (var unit in unitList) //Affichage des unites si vous n'aviez pas compris
                            unit.Draw(spriteBatch);
                        foreach (Bullet bullet in bulletList) // Affichage des bullets :p
                            bullet.Draw(spriteBatch);
                        foreach (EffectBullet effect in effectBulletList)
                            effect.Draw(spriteBatch);
                        if (enableFog)
                            fog.Draw(spriteBatch);
                        planeteList[0].Draw2(spriteBatch);

                        spriteBatch.DrawString(chat, text.ReturnString(Keyboard.GetState()), text.GetPosition(), Color.AntiqueWhite);
                        pauseScreen.Draw(gameTime);
                    }
                    if (activeScreen == actionScreen)
                    {
                        actionScreen.Draw(gameTime);

                        foreach (Planete p in planeteList)
                        {
                            if (p.champDeVision.Top < Camera2d.Origine.Y + PositronNova.winHeight && // On dessine que ce qu'il y a dans le scroll (performance) 
                                p.champDeVision.Bottom > Camera2d.Origine.Y &&
                                p.champDeVision.Left < Camera2d.Origine.X + PositronNova.winWidth &&
                                p.champDeVision.Right > Camera2d.Origine.X)
                            {
                                p.Draw1(spriteBatch);
                            }
                        }

                        foreach (Unit unit in unitList) // On utilise ChampdeVision ici c'est normal ! 
                        {
                            if (unit.champDeVision.Top < Camera2d.Origine.Y + PositronNova.winHeight && // On dessine que ce qu'il y a dans le scroll (performance) 
                                unit.champDeVision.Bottom > Camera2d.Origine.Y &&
                                unit.champDeVision.Left < Camera2d.Origine.X + PositronNova.winWidth &&
                                unit.champDeVision.Right > Camera2d.Origine.X)
                            {
                                unit.Draw(spriteBatch);
                            }
                        }
                        foreach (Bullet bullet in bulletList) // Affichage des bullets :p
                            if (bullet.hitBoxes[0].Top < Camera2d.Origine.Y + PositronNova.winHeight && // On dessine que ce qu'il y a dans le scroll (performance) 
                                bullet.hitBoxes[0].Bottom > Camera2d.Origine.Y &&
                                bullet.hitBoxes[0].Left < Camera2d.Origine.X + PositronNova.winWidth &&
                                bullet.hitBoxes[0].Right > Camera2d.Origine.X)
                            {
                                bullet.Draw(spriteBatch);
                            }
                        foreach (EffectBullet effect in effectBulletList)
                            if (effect.hitBoxes[0].Top < Camera2d.Origine.Y + PositronNova.winHeight && // On dessine que ce qu'il y a dans le scroll (performance) 
                                effect.hitBoxes[0].Bottom > Camera2d.Origine.Y &&
                                effect.hitBoxes[0].Left < Camera2d.Origine.X + PositronNova.winWidth &&
                                effect.hitBoxes[0].Right > Camera2d.Origine.X)
                            {
                                effect.Draw(spriteBatch);
                            }

                        interfaceJoueur.Draw(spriteBatch);

                        if (enableFog)
                            fog.Draw(spriteBatch);

                        spriteBatch.Draw(ui, new Rectangle((int)Camera2d.Origine.X, (int)Camera2d.Origine.Y, winWidth, winHeight), Color.White);
                        //Affiche le chat
                        //Les élément de la planète sélectionné pour l'iu
                        planeteList[0].Draw2(spriteBatch);
                        spriteBatch.DrawString(chat, text.ReturnString(Keyboard.GetState()), text.GetPosition(), Color.AntiqueWhite);
                        spriteBatch.DrawString(chat, ressources.ToString(), new Vector2(Camera2d.Origine.X, Camera2d.Origine.Y), Color.White);

                    }
                    break;
                case GameState.Final:
                    if (planeteList[1].Pv < 0)
                        spriteBatch.Draw(image, new Rectangle((int)Camera2d.Origine.X, (int)Camera2d.Origine.Y, winWidth, winHeight), Color.White);
                    else
                        spriteBatch.Draw(image_lost, new Rectangle((int)Camera2d.Origine.X, (int)Camera2d.Origine.Y, winWidth, winHeight), Color.White);
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

        void genHumain(int nombre)
        {
            Unit localUnit;
            string[] names = new string[] {"Roger", "Gerard", "Patrick", "Mouloud", "Dede", "Jean-Claude", "Herve", "Gertrude", "Germaine", "Gisele", "Frenegonde", "JacquesArt", "JacquesOuille", "Riton", "Korben", "Jonathan", "Sebastien", "Paul", "Ilan", "Baptiste"};
            for (int i = 0; i < nombre; i++)
            {
                localUnit = new Unit(names[i], new Vector2(rand.Next(0, 600), rand.Next(0, BackgroundTexture.Height - 200)), (UnitType)rand.Next((int)UnitType.Chasseur, (int)UnitType.Cuirasse + 1));
                localUnit.Init();
                unitList.Add(localUnit);
            }
        }

        void genAlien(int nombre)
        {
            Unit localUnit;
            for (int i = 0; i < nombre; i++)
            {
                localUnit = new Unit(new Vector2(rand.Next(BackgroundTexture.Width / 2, BackgroundTexture.Width - 300), rand.Next(BackgroundTexture.Height / 2, BackgroundTexture.Height - 200)), (UnitType)rand.Next((int)UnitType.Bacterie, (int)UnitType.Kraken + 1));
                localUnit.Init();
                unitList.Add(localUnit);
            }
        }

        void genAlienTime()
        {
            if (last > compt)
            {
                int comptalien = 0;
                Unit localUnit;
                for (int i =0; i < unitList.Count; i++)
                {
                    if (unitList[i].Side == UnitSide.Alien)
                        comptalien++;
                }

                if (Planete.Niveau_caserne != 0 && comptalien < 10)
                {
                    if (Planete.Niveau_caserne == 5 && Universite.Changement_blindage && Universite.Changement_moteur)
                    {
                        localUnit = new Unit(new Vector2(rand.Next(BackgroundTexture.Width - 300, BackgroundTexture.Width), rand.Next(BackgroundTexture.Height / 2, BackgroundTexture.Height)), (UnitType)rand.Next((int)UnitType.Bacterie, (int)UnitType.Kraken + 1));
                    }
                    else if (Planete.Niveau_caserne >= 4 && Universite.Changement_blindage)
                    {
                        localUnit = new Unit(new Vector2(rand.Next(BackgroundTexture.Width - 300, BackgroundTexture.Width), rand.Next(BackgroundTexture.Height / 2, BackgroundTexture.Height)), (UnitType)rand.Next((int)UnitType.Bacterie, (int)UnitType.Phagosome + 1));
                    }
                    else if (Planete.Niveau_caserne >= 2)
                    {
                        localUnit = new Unit(new Vector2(rand.Next(BackgroundTexture.Width - 300, BackgroundTexture.Width), rand.Next(BackgroundTexture.Height / 2, BackgroundTexture.Height)), (UnitType)rand.Next((int)UnitType.Bacterie, (int)UnitType.Neurone + 1));
                    }
                    else
                    {
                        localUnit = new Unit(new Vector2(rand.Next(BackgroundTexture.Width - 300, BackgroundTexture.Width), rand.Next(BackgroundTexture.Height / 2, BackgroundTexture.Height)), UnitType.Bacterie);
                    }
                    localUnit.Init();
                    unitList.Add(localUnit);
                }
                last = new TimeSpan(0);
            }
        }

        static public Unit GetEnnemy()
        {
            foreach (Unit unit in unitList)
                if (unit.Side == UnitSide.Alien)
                    if ((Math.Abs(Mouse.GetState().X - (unit.position.X + unit.texture.Width / 2) + Camera2d.Origine.X) <= unit.texture.Width / 2 && Math.Abs(Mouse.GetState().Y - (unit.position.Y + unit.texture.Height / 2) + Camera2d.Origine.Y) <= unit.texture.Height / 2))
                        return unit;
            return null;
        }

        static public Unit GetHumain()
        {
            foreach (Unit unit in unitList)
                if (unit.Side == UnitSide.Humain)
                    if (Math.Abs(Mouse.GetState().X - (unit.position.X + unit.texture.Width / 2) + Camera2d.Origine.X) <= unit.texture.Width / 2 && Math.Abs(Mouse.GetState().Y - (unit.position.Y + unit.texture.Height / 2) + Camera2d.Origine.Y) <= unit.texture.Height / 2)
                        return unit;
            return null;
        }

        static public Planete GetPlanete()
        {
            foreach (Planete planete in planeteList)
                if (Math.Abs(Mouse.GetState().X - (planete.Position.X + planete.Image_planete.Width) + Camera2d.Origine.X) <= planete.Image_planete.Width && Math.Abs(Mouse.GetState().Y - (planete.Position.Y + planete.Image_planete.Height) + Camera2d.Origine.Y) <= planete.Image_planete.Height)
                    return planete;
            return null;
        }

        static public void AddBullet(Bullet bullet)
        {
            bulletList.Add(bullet);
        }

        static public void AddEffect(EffectBullet effect)
        {
            effectBulletList.Add(effect);
        }

        void IAStrategie()
        {
            // Sinon si il y a plus de 5 vaisseaux humain sur la carte, les aliens se dirigent vers la planète

            int countHumain = 0;
            int countAlien = 0;
            for (int i = 0; i < PositronNova.UnitList.Count; i++)
            {
                if (PositronNova.UnitList[i].Side == UnitSide.Humain)
                    countHumain++;
                else if (PositronNova.UnitList[i].Side == UnitSide.Alien)
                    countAlien++;
            }

            if (countHumain >= 5 && countAlien >= 5) // il faut que les aliens ait au moins 5 vaisseaux pour attaqué la planète adversaire, sinon pas assez nombreux et donc ils ne prennent pas le risque de perdre leur défense
            {
                countAlien = 0;
                for (int i = 0; i < PositronNova.UnitList.Count; i++)
                {
                    if (PositronNova.UnitList[i].Side == UnitSide.Alien && countAlien <= 3)
                    {
                        countAlien++;
                        PositronNova.UnitList[i].Destination = PositronNova.planeteList[0].Position + new Vector2(150, 100);
                        PositronNova.UnitList[i].moving = true;
                    }
                }
            }
        }
    }
}