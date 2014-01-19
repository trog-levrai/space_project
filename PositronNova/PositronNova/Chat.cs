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
        private string input = "";
        private KBInput kb = new KBInput();
        public void KBInput(KeyboardState keyboardState)
        {
            if (tab)
            {
                if (keyboardState.IsKeyDown(Keys.Enter))
                {
                    texto.Enqueue(input);
                    input = "";
                    kb.current = "";
                    tab = false;
                }
                else
                {
                    input += kb.GetString(keyboardState);
                }
            }
            else
            {
                tab = keyboardState.IsKeyDown(Keys.Tab);
            }
        }
        //File du texte à afficher
        private Queue<string> texto = new Queue<string>(10);
        //Position de l'affichage
        private Vector2 position;
        public Vector2 GetPosition()
        {
            position = new Vector2(0, GraphicsDeviceManager.DefaultBackBufferHeight - 12*texto.LongCount());
            return position;
        }
        public string ReturnString(KeyboardState kb)
        {
            string[] texts = new string[texto.Count];
            texts = texto.ToArray();
            string ans = "";
            for (int i = 0; i < texts.Length; i++)
            {
                if (texts[i] != "")
                { ans = ans + texts[i] + "\n"; }
            }
            return ans;
        }

    }
}
