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
    public class MenuComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        string[] menuItems;
        int selectedIndex;

        Color normal = Color.Red;
        Color hilite = Color.Yellow;

        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;

        SpriteBatch spriteBatch;
        SpriteFont spriteFont1;
        SpriteFont spriteFont2;

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

        Vector2 position1;
        float width1 = 0f;
        float height1 = 0f;

        Vector2 position2;
        float width2 = 0f;
        float height2 = 0f;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value;
                if (selectedIndex < 0)
                {
                    selectedIndex = 0;
                }
                if (selectedIndex > menuItems.Length)
                {
                    selectedIndex = menuItems.Length - 1;
                }
            }
        }

        public MenuComponent(Game game, SpriteBatch spriteBatch, SpriteFont spriteFont1, SpriteFont spriteFont2, string[] menuItems)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.spriteFont1 = spriteFont1;
            this.spriteFont2 = spriteFont2;
            this.menuItems = menuItems;
            MeasureMenu();
            // TODO: Construct any child components here
        }

        private void MeasureMenu()
        {
            height1 = 0;
            width1 = 0;

            foreach (string item in menuItems)
            {
                Vector2 size1 = spriteFont1.MeasureString(item);
                if (size1.X > width1)
                {
                    width1 = size1.X;
                }
                height1 += spriteFont1.LineSpacing + 5;
            }

            position1 = new Vector2((Game.Window.ClientBounds.Width - width1) / 2,
                (Game.Window.ClientBounds.Height - height1)/2 + 100);

            height2 = 0;
            width2 = 0;

            Vector2 size2 = spriteFont2.MeasureString(menuItems[0]);
            if (size2.X > width2)
                width2 = size2.X;
            height2 += spriteFont2.LineSpacing + 5;

            position2 = new Vector2((Game.Window.ClientBounds.Width - width2) / 2, 10);
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
            
            if (checkKey(Keys.Down))
            {
                selectedIndex++;
                if (selectedIndex == menuItems.Length)
                {
                    selectedIndex = 1;
                }
            }
            if (checkKey(Keys.Up))
            {
                selectedIndex--;
                if (selectedIndex < 1)
                {
                    selectedIndex = menuItems.Length - 1;
                }
            }

            base.Update(gameTime);

            oldKeyboardState = keyboardState;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Vector2 location = position2;
            Color tint;
            spriteBatch.DrawString(spriteFont2, menuItems[0], location, Color.Red);
            location = position1;
                for (int i = 1; i < menuItems.Length; i++)
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
                    spriteFont1, menuItems[i], location, tint);
                    location.Y += spriteFont1.LineSpacing + 5;
                }
        }

    }
}
