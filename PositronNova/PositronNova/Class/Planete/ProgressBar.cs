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


namespace <insert your namespace here>
{
    public class ProgressBar : Microsoft.Xna.Framework.GameComponent
    {
        #region Variables
        Texture2D backgroundTexture;
        Texture2D barTexture;
        bool m_showText;
        bool m_isDecreasing = false;
        bool m_isIncreasing = false;
        float m_min = 0;
        float m_max = 100;
        float m_value = 0;
        float m_secondaryValue = 0;
        int m_height = 5;
        int m_width = 100;
        Color m_backgroundColor = Color.White;
        Color m_primaryColor = Color.LightBlue;
        Color m_secondaryColor = Color.Red;
        Vector2 m_position = Vector2.Zero;
        SpriteFont m_font = null;
        
        //reserved for future features
        Orientation m_orientation = Orientation.HorizontalLTR;
        Animation m_decreaseAnim = Animation.Fade;
        Animation m_increaseAnim = Animation.Bounce;


        //float pulseScale = 8f;
        //float pulseNormalize = 1f; 
        #endregion
        #region Properties
        public bool ShowText
        {
            get { return m_showText; }
            set { m_showText = value; }
        }
        public float Value
        {
            get { return m_value; }
            set
            {
                if (Minimum <= value && value <= Maximum)
                {
                    m_value = value;
                }
            }
        }
        public float Minimum
        {
            get { return m_min; }
            set { m_min = value; }
        }
        public float Maximum
        {
            get { return m_max; }
            set { m_max = value; }
        }
        public Color BackgroundColor
        {
            get { return m_backgroundColor; }
            set { m_backgroundColor = value; }
        }
        public Color PrimaryColor
        {
            get { return m_primaryColor; }
            set { m_primaryColor = value; }
        }
        public Color SecondaryColor
        {
            get { return m_secondaryColor; }
            set { m_secondaryColor = value; }
        }
        public Vector2 Position
        {
            get { return m_position; }
            set { m_position = value; }
        }
        public SpriteFont Font
        {
            get { return m_font; }
            set { m_font = value; }
        }
        public int Height
        {
            get { return m_height; }
            set { m_height = value; }
        }
        public int Width
        {
            get { return m_width; }
            set { m_width = value; }
        }
        #endregion
        #region Constructors
        public ProgressBar(Game game)
            : base(game)
        {
        }
        public ProgressBar(Game game, SpriteFont font)
            : base(game)
        {
            m_font = font;
        }
        #endregion
        #region Initialize & Update Methods
        public override void Initialize()
        {
            backgroundTexture = Game.Content.Load<Texture2D>("bg");
            barTexture = Game.Content.Load<Texture2D>("bg");

            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        #endregion
        #region Custom Methods
        public void Draw(SpriteBatch spriteBatch)
        {
            DrawBackground(spriteBatch);
            DrawSecondaryBar(spriteBatch);
            DrawPrimaryBar(spriteBatch);
            DrawNumber(spriteBatch);
        }
        private void DrawBackground(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTexture, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), BackgroundColor);
        }
        private void DrawPrimaryBar(SpriteBatch spriteBatch)
        {
            int BarWidth = (int)(Width / Maximum * Value);

            spriteBatch.Draw(barTexture, new Rectangle((int)Position.X, (int)Position.Y, BarWidth, 5), PrimaryColor);
        }
        private void DrawNumber(SpriteBatch spriteBatch)
        {
            if (Font != null)
            {
                Vector2 stringSize = Font.MeasureString((Value / Maximum * 100).ToString());
                spriteBatch.DrawString(Font, (Value / Maximum * 100).ToString(), new Vector2(Position.X + (Width / 2) - (stringSize.X / 2), Position.Y - 20f), Color.White);
            }
        }
        private void DrawSecondaryBar(SpriteBatch spriteBatch)
        {
            int BarWidth = (int)(Width / Maximum * Value);

            spriteBatch.Draw(barTexture, new Rectangle((int)Position.X, (int)Position.Y, BarWidth, 5), SecondaryColor);
        }
        public void Increase(float amount)
        {
            m_isIncreasing = true;

            if (amount <= (Maximum - Value))
            {
                Value += amount;
            }
            else
            {
                Value = Maximum;
            }

            m_isIncreasing = false;
        }
        public void Decrease(float amount)
        {
            m_isDecreasing = true;

            if (amount <= Value)
            {
                Value -= amount;
            }
            else
            {
                Value = Minimum;
            }

            m_isDecreasing = false;
        }
        #endregion
        #region Animation & Orientation
        public enum Animation
        {
            Bounce,
            Fade,
            None
        }
        public enum Orientation
        {
            HorizontalLTR,
            HorizontalRTL,
            VerticalLTR,
            VeritcalRTL
        }
        #endregion
    }
}
}