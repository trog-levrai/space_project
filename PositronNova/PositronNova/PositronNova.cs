 using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
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
        Game game;
        private Client client;
        static public int winWidth = 800, winHeight = 600; // Accessible pour les autres classes...
        //Gestion des images...
        //Pour la gestion du serveur

        static private Texture2D backgroundTexture;
        static public Texture2D BackgroundTexture
        {
            get { return backgroundTexture; }
        }

        // WORLD ELEMENTS
        static List<Unit> unitList = new List<Unit>();
        static List<Bullet> bulletList = new List<Bullet>();
        static List<EffectBullet> effectBulletList = new List<EffectBullet>();
        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;

        GameScreen activeScreen;
        StartScreen startScreen;
        ActionScreen actionScreen;
        ActionScreen actionScreen_;
        OptionScreen optionScreen;
        PauseScreen pauseScreen;
        bool action = false;

        //private Song _song_jeu;
        //private Song _song_menu;

        AudioEngine engine;
        SoundBank soundBank;
        WaveBank waveBank;
        Cue cue;
        Cue cue1;
        float musicVolume = 1.0f;

        Camera2d _camera;

        Random rand = new Random();

        private SpriteFont chat;
        private static string ennemyName = "";
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
            
            backgroundTexture = Content.Load<Texture2D>("Background");

            vidPlayer = new VideoPlayer();

            //_thEcoute = new Thread(new ParameterizedThreadStart(Ecouter));
            //_thEcoute.Start(text);
            //_thEcoute.IsBackground = true;

            engine = new AudioEngine("Content\\sounds\\Playsong.xgs");
            soundBank = new SoundBank(engine, "Content\\sounds\\Sound Bank.xsb");
            waveBank = new WaveBank(engine, "Content\\sounds\\Wave Bank.xwb");

            cue = soundBank.GetCue("Menu");
            cue.Play();
            cue.Pause();

            cue1 = soundBank.GetCue("Espace");
            cue1.Play();
            cue1.Pause();


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
            Manager.ContentLoad(Content);

            foreach (Unit unit in unitList)
                unit.LoadContent(Content);

            _camera = new Camera2d(GraphicsDevice.Viewport);

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
            actionScreen_ = new ActionScreen(
                this,
                spriteBatch,
                Content.Load<Texture2D>("Background"));
            Components.Add(actionScreen_);
            actionScreen_.Hide();
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
            #endregion

            activeScreen = startScreen;
            startScreen.SelectedIndex = 1;
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
            Vector2 movement = Vector2.Zero;
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            //    this.Exit();
            
            //Vector2 movement = Vector2.Zero;
            engine.Update();
            switch (CurrentGameState)
            {
                case GameState.Video:
                    if (vidPlayer.State == MediaState.Stopped || (keyboardState.IsKeyDown(Keys.Space)))
                        CurrentGameState = GameState.Game;
                    _camera.Update2(gameTime, keyboardState, mouse);
                    break;
                case GameState.Game:
                    vidPlayer.Stop();
                    cue.Resume();

#region StartScreen
                    if (activeScreen == startScreen)
                    {
                        _camera.Update2(gameTime, keyboardState, mouse);
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
                                genUnit(20, true);
                                genUnit(20, false);

                                foreach (Unit unit in unitList)
                                    unit.LoadContent(Content);
                            }
                            if (startScreen.SelectedIndex == 2)
                            {
<<<<<<< HEAD
                                client = new Client("Polo", "10.3.141.70", 1234, this);
=======
                                client = new Client("trog", "10.3.141.70", 1234, this);
>>>>>>> a76dc1b13f83234dcc97d83cd0b8d2e348fc8078
                                client.Connect();
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
                                genUnit(20, true);
                                genUnit(20, false);
                                foreach (Unit unit in unitList)
                                    unit.LoadContent(Content);
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
                        }
                        if (!action)
                        {
                            cue1.Pause();
                            cue.Resume();
                        }
                        _camera.Update2(gameTime, keyboardState, mouse);
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
                        if (optionScreen.SelectedIndex == 2 && CheckKey(Keys.Space) && !action)
                        {
                            activeScreen.Hide();
                            activeScreen = startScreen;
                            activeScreen.Show();
                        }
                        if (optionScreen.SelectedIndex == 2 && keyboardState.IsKeyDown(Keys.Space) && oldKeyboardState.IsKeyUp(Keys.Enter) && action)
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
                        _camera.Update2(gameTime, keyboardState, mouse);
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
                                client.Close();
                            }
                        }
                            
                        
                    }
#endregion

                    if (activeScreen == actionScreen || activeScreen == actionScreen_)
                    {
                        if (activeScreen == actionScreen_)
                        {
                            List<Unit> send = new List<Unit>();
                            foreach (var unit in unitList)
                            {
                                if (unit.Friendly)
                                {
                                    send.Add(unit);
                                }
                            }
                            //client.SendUnit(send);
                            List<Unit> aux = unitList;
                            unitList.Clear();
                            foreach (var unit in aux)
                            {
                                if (unit.Friendly)
                                    unitList.Add(unit);
                            }
                            foreach (var unit in client.Ennemies)
                            {
                                unitList.Add(unit);
                            }
                        }
                        cue.Pause();
                        cue1.Resume();
                        _camera.Update1(gameTime, keyboardState, mouse);
                        _camera.Update2(gameTime, keyboardState, mouse);

                        if (CheckKey(Keys.P))
                        {
                            action = true;
                            activeScreen.Hide();
                            activeScreen = pauseScreen;
                            activeScreen.Show();
                        }

                        for (int i = 0; i < unitList.Count; i++)
                        {
                            unitList[i].HandleInput(keyboardState, mouse);
                            unitList[i].Update(gameTime);
                            if (!unitList[i].Friendly && unitList[0].Friendly)
                            {
                                unitList[i].Ennemy = unitList[0];
                                unitList[i].HasTarget = true;
                            }
                            if (unitList[i].Destruction()) // Destruction des vaisseaux
                            {
                                effectBulletList.Add(new EffectBullet(unitList[i].position, EffectType.GrosseExplosion));
                                if (unitList[i].Friendly)
                                    client.chat.addString("f:" + unitList[i].Name + " has been destroyed !");
                                else
                                {
                                    client.chat.addString("e:" + unitList[i].Name + " has been destroyed !");
                                }
                                unitList.RemoveAt(i);
                                if (i > 0)
                                    i--;
                            }
                            //for (int j = 0; j < unitList.Count - 1; j++)
                            //    if (i != j && unitList[i].CollisionInterVaisseau(unitList[j]))
                            //        unitList[i].moving = false;

                        }

                        for (int i = 0; i < bulletList.Count; i++)
                        {
                            bulletList[i].Update(gameTime);
                            if (bulletList[i].destruc)
                            {
                                //if (bulletList[i].BulletType == BulletType.Missile)
                                    effectBulletList.Add(new EffectBullet(bulletList[i].position, EffectType.Explosion));
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

                        // TOUT LE CODE CONCERNANT LE LOGIQUE DU JEU DOIT ETRE MIS ICIIIIIIIIII !!!!!!!
                        client.KBInput(keyboardState);
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
                    if (activeScreen == optionScreen)
                    {
                        optionScreen.Draw(gameTime);
                        spriteBatch.DrawString(Content.Load<SpriteFont>("optionfont"), (Math.Round(musicVolume * 100, 0)).ToString() + "%",
                            new Vector2((PositronNova.winWidth) / 2 + 85, PositronNova.winHeight / 2 - 12), Color.Red);
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

                        spriteBatch.DrawString(chat, client.chat.ReturnString(Keyboard.GetState()), client.chat.GetPosition(), Color.AntiqueWhite);
                        pauseScreen.Draw(gameTime);
                    }
                    if (activeScreen == actionScreen)
                    {
                        actionScreen.Draw(gameTime);

                        foreach (var unit in unitList) //Affichage des unites si vous n'aviez pas compris
                            unit.Draw(spriteBatch);
                        foreach (Bullet bullet in bulletList) // Affichage des bullets :p
                            bullet.Draw(spriteBatch);
                        foreach (EffectBullet effect in effectBulletList)
                            effect.Draw(spriteBatch);
                        //Affiche le chat
                        spriteBatch.DrawString(chat, client.chat.ReturnString(Keyboard.GetState()), client.chat.GetPosition(), Color.AntiqueWhite);
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
            string[] names = new string[] {"Roger", "Gerard", "Patrick", "Mouloud", "Dede", "Jean-Claude", "Herve", "Gertrude", "Germaine", "Gisele", "Frenegonde", "JacquesArt", "JacquesOuille", "Riton", "Korben", "Jonathan", "Sebastien", "Paul", "Ilan", "Baptiste"};
            for (int i = 0; i < nombre; i++)
            {
                unitList.Add(new Unit(names[i], new Vector2(rand.Next(0, BackgroundTexture.Width - 300), rand.Next(0, BackgroundTexture.Height - 200)), (UnitType)rand.Next((int)UnitType.Chasseur, (int)UnitType.Cuirasse + 1), friendly));
                //if (unitList.Count > 1)    
                //for (int j = 0; j < i; j++)
                //        while (Physique.IntersectPixel(unitList[j].texture, unitList[j].position, unitList[j].textureData, unitList[i].texture, unitList[i].position, unitList[i].textureData))
                //            unitList[j].position = new Vector2(rand.Next(0, BackgroundTexture.Width - 300), rand.Next(0, BackgroundTexture.Height - 200));
            }
            foreach (Unit unit in unitList)
                unit.Init();
        }

        static public Unit GetEnnemy(bool friend)
        {
            foreach (Unit unit in unitList)
                if (Math.Abs(Mouse.GetState().X - (unit.position.X + unit.texture.Width / 2) + Camera2d.Origine.X) <= unit.texture.Width / 2 && Math.Abs(Mouse.GetState().Y - (unit.position.Y + unit.texture.Height / 2) + Camera2d.Origine.Y) <= unit.texture.Height / 2)
                    if (unit.Friendly != friend)
                        return unit;
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
    }
}