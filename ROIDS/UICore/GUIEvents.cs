using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace UICore
{
    public delegate void MouseEventHandler(GUIElement sender, MouseEventArgs e);
    public delegate void KeyEventHandler(GUIElement sender, KeyEventArgs e);

    public enum MouseButtons
    {
        Left,
        Right,
        Middle
    }

    public class MouseEventArgs : EventArgs
    {
        public MouseState PreviousMouseState { get; private set; }
        public MouseState CurrentMouseState { get; private set; }

        public Vector2 CurrentPosition
        {
            get { return new Vector2(CurrentMouseState.X, CurrentMouseState.Y); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="button">the mouse button (left,right,center)</param>
        /// <returns></returns>
        public bool isClicked(MouseButtons b)
        {
            switch (b)
            {
                case MouseButtons.Left: return PreviousMouseState.LeftButton == ButtonState.Pressed &&
                    CurrentMouseState.LeftButton == ButtonState.Released;
                case MouseButtons.Right: return PreviousMouseState.RightButton == ButtonState.Pressed &&
                    CurrentMouseState.RightButton == ButtonState.Released;
                case MouseButtons.Middle: return PreviousMouseState.MiddleButton == ButtonState.Pressed &&
                    CurrentMouseState.MiddleButton == ButtonState.Released;
                default:
                    throw new Exception("Unrecognized Button");
            }
        }

        public bool isPressed(MouseButtons b)
        {
            switch (b)
            {
                case MouseButtons.Left: return PreviousMouseState.LeftButton == ButtonState.Released &&
                    CurrentMouseState.LeftButton == ButtonState.Pressed;
                case MouseButtons.Right: return PreviousMouseState.RightButton == ButtonState.Released &&
                    CurrentMouseState.RightButton == ButtonState.Pressed;
                case MouseButtons.Middle: return PreviousMouseState.MiddleButton == ButtonState.Released &&
                    CurrentMouseState.MiddleButton == ButtonState.Pressed;
                default:
                    throw new Exception("Unrecognized Button");
            }
        }

        public bool isDown(MouseButtons b)
        {
            switch (b)
            {
                case MouseButtons.Left: return CurrentMouseState.LeftButton == ButtonState.Pressed;
                case MouseButtons.Right: return CurrentMouseState.RightButton == ButtonState.Pressed;
                case MouseButtons.Middle: return CurrentMouseState.MiddleButton == ButtonState.Pressed;
                default:
                    throw new Exception("Unrecognized Button");
            }
        }

        public MouseEventArgs(MouseState previousMouseState, MouseState currentMouseState)
            : base()
        {
            CurrentMouseState = currentMouseState;
            PreviousMouseState = previousMouseState;
        }

    }

    public class KeyEventArgs : EventArgs
    {
        /// <summary>
        /// The keys that are applicable to the event
        /// </summary>
        public Keys[] InterestingKeys { get; private set; }
        public KeyEventArgs(Keys[] interestinKeys)
            : base()
        {
            InterestingKeys = interestinKeys;
        }
    }
}
