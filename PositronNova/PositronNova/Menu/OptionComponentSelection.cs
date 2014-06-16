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
    public class OptionComponentSelection : Microsoft.Xna.Framework.DrawableGameComponent
    {
        string[] optionItems;
        int sselectedIndex;
        int locationY;

        OptionComponent optionComponent;

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

        public int SSelectedIndex
        {
            get { return sselectedIndex; }
            set
            {
                sselectedIndex = value;

                if (sselectedIndex < 0)
                {
                    sselectedIndex = 0;
                }
                if (sselectedIndex > optionItems.Length)
                {
                    sselectedIndex = optionItems.Length - 1;
                }
            }
        }

        public int LocationY
        {
            get { return locationY; }
            set { locationY = value; }
        }


        public OptionComponentSelection(Game game, SpriteBatch spriteBatch, SpriteFont spriteFont, string[] optionItems, OptionComponent optionComponent)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;
            this.optionItems = optionItems;
            this.optionComponent = optionComponent;
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

            position = new Vector2((Game.Window.ClientBounds.Width - width) / 2,
                (Game.Window.ClientBounds.Height - height) / 2 + 54);
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
                if (optionComponent.SelectedIndex == 0)
                {
                    if (sselectedIndex > 1)
                        sselectedIndex = 0;
                    if (checkKey(Keys.Right))
                    {

                        sselectedIndex++;
                        if (sselectedIndex == 2)
                        {
                            sselectedIndex = 1;
                        }
                    }
                    if (checkKey(Keys.Left))
                    {                       
                        sselectedIndex--;
                        if (sselectedIndex < 0)
                        {
                            sselectedIndex = 0;
                        }
                    }
                }
                if (optionComponent.SelectedIndex == 1)
                {
                    if (sselectedIndex < 2)
                        sselectedIndex = 2;
                    if (sselectedIndex > 3)
                        sselectedIndex = 3;
                    if (checkKey(Keys.Right))
                    {
                        sselectedIndex++;
                        if (sselectedIndex == 4)
                        {
                            sselectedIndex = 3;
                        }
                    }
                    if (checkKey(Keys.Left))
                    {
                        sselectedIndex--;
                        if (sselectedIndex < 2)
                        {
                            sselectedIndex = 2;
                        }
                    }
                }
                if (optionComponent.SelectedIndex == 2)
                {
                    if (sselectedIndex < 4)
                        sselectedIndex = 4;
                    if (sselectedIndex > 6)
                        sselectedIndex = 6;
                    if (checkKey(Keys.Right))
                    {
                        sselectedIndex++;
                        if (sselectedIndex == 7)
                            sselectedIndex = 6;
                    }
                    if (checkKey(Keys.Left))
                    {
                        sselectedIndex--;
                        if (sselectedIndex < 4)
                            sselectedIndex = 4;
                    }
                }
                if (optionComponent.SelectedIndex == 3)
                {
                    if (sselectedIndex < 7)
                        sselectedIndex = 7;
                    if (checkKey(Keys.Right))
                    {
                        sselectedIndex++;
                        if (sselectedIndex == 9)
                            sselectedIndex = 8;
                    }
                    if (checkKey(Keys.Left))
                    {
                        sselectedIndex--;
                        if (sselectedIndex < 7)
                            sselectedIndex = 7;
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

                if (optionComponent.SelectedIndex == 0)
                {

                    for (int i = 0; i < 2; i++)
                    {
                        if (i == sselectedIndex)
                        {
                            tint = hilite;
                        }
                        else
                        {
                            tint = normal;
                        }
                        spriteBatch.DrawString(
                        spriteFont, optionItems[i], location, tint);
                        location.X += 100;

                    }
                    location.Y += spriteFont.LineSpacing + 5;
                }
                else
                {
                    
                    for (int i = 0; i < 2; i++)
                    {
                        spriteBatch.DrawString(
                        spriteFont, optionItems[i], location, normal);
                        location.X += 100;
                    }
                    location.Y += spriteFont.LineSpacing + 5;
                }
                location.X -= 200;
                if (optionComponent.SelectedIndex == 1)
                {
                    
                    for (int i = 2; i < 4; i++)
                    {
                        if (i == sselectedIndex)
                        {
                            tint = hilite;
                        }
                        else
                        {
                            tint = normal;
                        }
                        spriteBatch.DrawString(
                        spriteFont, optionItems[i], location, tint);
                        location.X += 200;
                    }
                    location.Y += spriteFont.LineSpacing + 5;
                }
                else
                {
                    
                    for (int i = 2; i < 4; i++)
                    {
                        spriteBatch.DrawString(
                        spriteFont, optionItems[i], location, normal);
                        location.X += 200;
                    }
                    location.Y += spriteFont.LineSpacing + 5;
                }
                if (optionComponent.SelectedIndex == 2)
                {
                    location = new Vector2((Game.Window.ClientBounds.Width - width) / 2,
                (Game.Window.ClientBounds.Height - height) / 2 +112);
                    for (int i = 4; i < 7; i++)
                    {
                        if (i == sselectedIndex)
                        {
                            tint = hilite;
                        }
                        else
                        {
                            tint = normal;
                        }
                        spriteBatch.DrawString(
                        spriteFont, optionItems[i], location, tint);
                        location.X += 100;
                    }
                    location.Y += spriteFont.LineSpacing + 5;
                }
                else
                {
                    location = new Vector2((Game.Window.ClientBounds.Width - width) / 2,
                    (Game.Window.ClientBounds.Height - height) / 2 + 112);
                    for (int i = 4; i < 7; i++)
                    {
                        spriteBatch.DrawString(
                        spriteFont, optionItems[i], location, normal);
                        location.X += 100;
                    }
                    location.Y += spriteFont.LineSpacing + 5;
                }
                if (optionComponent.SelectedIndex == 3)
                {
                    location = new Vector2((Game.Window.ClientBounds.Width - width) / 2,
                    (Game.Window.ClientBounds.Height - height) / 2 + 143);
                    for (int i = 7; i < 9; i++)
                    {
                        if (i == sselectedIndex)
                        {
                            tint = hilite;
                        }
                        else
                        {
                            tint = normal;
                        }
                        spriteBatch.DrawString(
                        spriteFont, optionItems[i], location, tint);
                        location.X += 100;
                    }
                    location.Y += spriteFont.LineSpacing + 5;
                }
                else
                {
                    location = new Vector2((Game.Window.ClientBounds.Width - width) / 2,
                    (Game.Window.ClientBounds.Height - height) / 2 + 143);
                    for (int i = 7; i < 9; i++)
                    {
                        spriteBatch.DrawString(
                        spriteFont, optionItems[i], location, normal);
                        location.X += 100;
                    }
                    location.Y += spriteFont.LineSpacing + 5;
                }
            
        }

    }
}