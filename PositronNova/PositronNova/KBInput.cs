using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace PositronNova
{
    class KBInput
    {
        private Keys[] lastPressedKeys;
        public string current;
        private bool shift;
        public KBInput()
        {
            lastPressedKeys = new Keys[0];
            current = "";
            shift = false;
        }
        public string GetString(KeyboardState kbState)
        {
            current = "";
            Keys[] pressedKeys = kbState.GetPressedKeys();
            shift = kbState.IsKeyDown(Keys.RightShift) || kbState.IsKeyDown(Keys.LeftShift);
            //Si la touche vient d'être relevée
            foreach (Keys key in lastPressedKeys)
            {
                if (!pressedKeys.Contains(key))
                    OnKeyUp(key);
            }
            //Si la touche était deja pressée
            foreach (Keys key in pressedKeys)
            {
                if (!lastPressedKeys.Contains(key))
                    OnKeyDown(key);
            }
            lastPressedKeys = pressedKeys;
            return current;
        }
        private void OnKeyDown(Keys key)
        {
            current = "";
        }
        private void OnKeyUp(Keys key)
        {
            if (key != Keys.Tab && key != Keys.Enter)
            {
                if (key == Keys.Space)
                {
                    current += " ";
                }
                if (key.ToString().Length == 1 && key != Keys.Enter)
                {
                    char c = key.ToString()[0];
                    if (!shift)
                        c += (char)('a' - 'A');
                    current += "" + c;
                }
            }
        }
    }
}
