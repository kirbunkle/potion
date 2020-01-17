using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotionMaster
{
    public enum GameButtons
    {
        Down,
        Up,
        Right,
        Left,
        A,
        B,
        Esc,
        Tab
    }

    public class GameInput
    {
        private KeyboardState keyboardInput;
        private KeyboardState oldKeyboardInput;
        // TODO: options for multiple input modes
        
        private Keys GetKeyFromGameButton(GameButtons b)
        {
            switch (b)
            {
                case GameButtons.Down:    return Keys.Down;
                case GameButtons.Up:      return Keys.Up;
                case GameButtons.Left:    return Keys.Left;
                case GameButtons.Right:   return Keys.Right;
                case GameButtons.A:       return Keys.Enter;
                case GameButtons.Esc:     return Keys.Escape;
                case GameButtons.Tab:     return Keys.Tab;
                case GameButtons.B:       return Keys.Space;
            }
            return Keys.None;
        }

        public GameInput()
        {

        }

        public void SetKeyboardState(KeyboardState k)
        {
            oldKeyboardInput = keyboardInput;
            keyboardInput = k;
        }

        public bool ButtonDown(GameButtons b)
        {
            return keyboardInput.IsKeyDown(GetKeyFromGameButton(b));
        }

        public bool ButtonPressed(GameButtons b)
        {
            Keys k = GetKeyFromGameButton(b);
            if (keyboardInput.IsKeyDown(k) && (!oldKeyboardInput.IsKeyDown(k)))
            {
                oldKeyboardInput = keyboardInput;
                return true;
            }
            return false;
        }
    }
}
