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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class PauseComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        string[] pauseItems;
        int selectedIndex;

        Color normal = Color.Red;
        Color hilite = Color.Yellow;

        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;

        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        //GamePadState gamePadState;
        //GamePadState oldGamePadState;

        //public Vector2 Position
        //{
        //    get { return position; }
        //    set { position = value; }
        //}

        //public float Width
        //{
        //    get { return width; }
        //}

        //public float Height
        //{
        //    get { return height; }
        //}

        Vector2 position;
        float width = 0f;
        float height = 0f;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                if (selectedIndex < 0)
                {
                    selectedIndex = 0;
                }
                if (selectedIndex > pauseItems.Length)
                {
                    selectedIndex = pauseItems.Length - 1;
                }
            }
        }

        public PauseComponent(Game game, SpriteBatch spriteBatch, SpriteFont spriteFont, string[] menuItems)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;
            this.pauseItems = menuItems;
            MeasureMenu();
            // TODO: Construct any child components here
        }

        private void MeasureMenu()
        {
            height = 0;
            width = 0;

            foreach (string item in pauseItems)
            {
                Vector2 size = spriteFont.MeasureString(item);
                if (size.X > width)
                {
                    width = size.X;
                }
                height += spriteFont.LineSpacing + 5;
            }

            position = new Vector2((Game.Window.ClientBounds.Width - width) / 2 + Camera2d.Origine.X,
                (Game.Window.ClientBounds.Height - height) / 2);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        private bool checkKey(Keys theKey)
        {
            return keyboardState.IsKeyUp(theKey) && oldKeyboardState.IsKeyDown(theKey);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            keyboardState = Keyboard.GetState();

            if (checkKey(Keys.Right))
            {
                selectedIndex++;
                if (selectedIndex == pauseItems.Length)
                {
                    selectedIndex = pauseItems.Length - 1;
                }
            }
            if (checkKey(Keys.Left))
            {
                selectedIndex--;
                if (selectedIndex < 0)
                {
                    selectedIndex = 0;
                }
            }

            base.Update(gameTime);

            oldKeyboardState = keyboardState;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Vector2 location = position;
            location.X = (Game.Window.ClientBounds.Width / 2 - 100) + Camera2d.Origine.X;
            Color tint;
            location.Y += Camera2d.Origine.Y;
            for (int i = 0; i < pauseItems.Length; i++)
            {
                if (i == selectedIndex)
                {
                    tint = hilite;
                }
                else
                {
                    tint = normal;
                }
                spriteBatch.DrawString(
                spriteFont, pauseItems[i], location, tint);
                location.X += 100;
            }
        }

    }
}
