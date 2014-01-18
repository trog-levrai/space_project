using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PositronNova
{
    class Chat
    {
        //Capte le texte
        //Le code sera à compléter pour l'implémentation de l'autocomplete pour shell
        private bool tab = false;
        private string input;
        public void KBInput(KeyboardState keyboardState)
        {
            if (tab)
            {
                if (keyboardState.IsKeyDown(Keys.Enter))
                {
                    texto.Enqueue(input);
                    input = "";
                }
                else
                {
                    input += keyboardState.ToString();
                }
            }
            else
            {
                tab = keyboardState.IsKeyDown(Keys.Tab);
            }
        }
        //File du texte à afficher
        private Queue<string> texto = new Queue<string>(10);
        private string[] texts = new string[10];
        //Position de l'affichage
        private Vector2 position;
        public Vector2 GetPosition()
        {
            position = new Vector2(0, GraphicsDeviceManager.DefaultBackBufferHeight - 12*texts.Length);
            return position;
        }
        public string ReturnString()
        {
            texto.CopyTo(texts, 0);
            string ans = "";
            foreach (string text in texts)
            {
                ans = ans + text + "\n";
            }
            return ans;
        }

    }
}
