using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PositronNova
{
    class Chat
    {
        //File du texte à afficher
        private string[] texts = new string[] {"hello", "world", "!", "zboub"};
        //Position de l'affichage
        private Vector2 position;
        public Vector2 GetPosition()
        {
            position = new Vector2(0, GraphicsDeviceManager.DefaultBackBufferHeight - 12*texts.Length);
            return position;
        }
        public string ReturnString()
        {
            string ans = "";
            foreach (string text in texts)
            {
                ans = ans + text + "\n";
            }
            return ans;
        }
    }
}
