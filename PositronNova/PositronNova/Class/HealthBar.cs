using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PositronNova.Class
{
    public class HealthBar
    {
        Texture2D texture;
        Rectangle life;

        int vieMax;
        int vie;

        int largeurTexture, largeurTextureFixe;

        public HealthBar(int vieMax, int largeurTexture)
        {
            this.vieMax = vieMax;
            vie = vieMax;

            this.largeurTexture = largeurTexture;
            largeurTextureFixe = largeurTexture;
            texture = Manager.lifeBrick_t;
            life = new Rectangle(0, 0, 3, 5);
        }

        public void Update(int vie)
        {
            if (this.vie != vie)
            {
                largeurTexture -= Math.Abs(this.vie - vie) * largeurTexture / (vie + 1);
                this.vie = vie;
            }
        }

        public void Draw(SpriteBatch sb, int x, int y)
        {
            sb.Draw(texture, new Rectangle(x - 2, y - 2, largeurTextureFixe + 4, 1), Color.LightGreen); // Up
            sb.Draw(texture, new Rectangle(x - 2, y + life.Height + 1, largeurTextureFixe + 4, 1), Color.LightGreen);// Down
            sb.Draw(texture, new Rectangle(x - 2, y - 2, 1, life.Height + 4), Color.LightGreen); // Left
            sb.Draw(texture, new Rectangle(x + largeurTextureFixe + 1, y - 2, 1, life.Height + 4), Color.LightGreen); // Right

            if (vie > vieMax / 2)
                sb.Draw(texture, new Rectangle(x, y, largeurTexture, life.Height), life, Color.LightGreen);
            else if (vie > vieMax / 4)
                sb.Draw(texture, new Rectangle(x, y, largeurTexture, life.Height), life, Color.Orange);
            else
                sb.Draw(texture, new Rectangle(x, y, largeurTexture, life.Height), life, Color.Red);
        }
    }
}
