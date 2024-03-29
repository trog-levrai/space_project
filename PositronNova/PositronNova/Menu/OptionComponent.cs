﻿using System;
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
    public class OptionComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        string[] optionItems;
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
                if (selectedIndex > optionItems.Length)
                {
                    selectedIndex = optionItems.Length - 1;
                }
            }
        }


        public OptionComponent(Game game, SpriteBatch spriteBatch, SpriteFont spriteFont, string[] optionItems)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;
            this.optionItems = optionItems;
            MeasureMenu();
            // TODO: Construct any child components here
        }

        private void MeasureMenu()
        {
            height = 0;
            width = 0;

            foreach (string item in optionItems)
            {
                Vector2 size = spriteFont.MeasureString(item);
                if (size.X > width)
                {
                    width = size.X;
                }
                height += spriteFont.LineSpacing + 5;
            }

            position = new Vector2((Game.Window.ClientBounds.Width/2 - width) / 2,
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

            if (checkKey(Keys.Down))
            {
                selectedIndex++;
                if (selectedIndex == optionItems.Length)
                {
                    selectedIndex = 0;
                }
            }
            if (checkKey(Keys.Up))
            {
                selectedIndex--;
                if (selectedIndex < 0)
                {
                    selectedIndex = optionItems.Length - 1;
                }
            }

            base.Update(gameTime);

            oldKeyboardState = keyboardState;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Vector2 location = position;
            Color tint;

            for (int i = 0; i < optionItems.Length; i++)
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
                spriteFont, optionItems[i], location, tint);
                location.Y += spriteFont.LineSpacing + 5;
            }
        }

    }
}