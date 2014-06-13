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

        int largeurTexture;

        public HealthBar(int vieMax, int largeurTexture)
        {
            this.vieMax = vieMax;
            vie = vieMax;

            this.largeurTexture = largeurTexture;
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
            if (vie > vieMax / 2)
                sb.Draw(texture, new Rectangle(x, y, largeurTexture, life.Height), life, Color.LightGreen);
            else if (vie > vieMax / 4)
                sb.Draw(texture, new Rectangle(x, y, largeurTexture, life.Height), life, Color.Orange);
            else
                sb.Draw(texture, new Rectangle(x, y, largeurTexture, life.Height), life, Color.Red);
        }
    }
}
