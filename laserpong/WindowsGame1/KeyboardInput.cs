using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame1
{
    class KeyboardInput
    {
        public enum InputMode {
            Player1,
            Player2
        };

        private InputMode m_Mode;
        private KeyboardState m_oldState;
        private KeyboardState m_newState;

        public KeyboardInput(InputMode mode)
        {
            m_Mode = mode;
            m_oldState = new KeyboardState();
            m_newState = new KeyboardState();
        }

        public string getMode()
        {   switch (m_Mode)
        {

            case InputMode.Player1:
            return "Player1";
            case InputMode.Player2:
            return "Player2";

            default:
            throw new NotImplementedException("Received unexpected input mode");
        }

        }

        public bool IsMoveUpPressed()
        {
            switch (m_Mode)
            {
                case InputMode.Player1:
                    return m_newState.IsKeyDown(Keys.W);
                case InputMode.Player2:
                    return m_newState.IsKeyDown(Keys.Up);
                default:
                    throw new NotImplementedException("Received unexpected input mode");
            }
        }
        public bool IsMoveDownPressed()
        {
            switch (m_Mode)
            {
                case InputMode.Player1:
                    return m_newState.IsKeyDown(Keys.S);
                case InputMode.Player2:
                    return m_newState.IsKeyDown(Keys.Down);
                default:
                    throw new NotImplementedException("Received unexpected input mode");
            }
        }

        public bool IsMoveLeftPressed()
        {
            switch (m_Mode)
            {
                case InputMode.Player1:
                    return m_newState.IsKeyDown(Keys.A);
                case InputMode.Player2:
                    return m_newState.IsKeyDown(Keys.Left);
                default:
                    throw new NotImplementedException("Received unexpected input mode");
            }
        }

        public bool IsMoveRightPressed()
        {
            switch (m_Mode)
            {
                case InputMode.Player1:
                    return m_newState.IsKeyDown(Keys.D);
                case InputMode.Player2:
                    return m_newState.IsKeyDown(Keys.Right);

                default:
                    throw new NotImplementedException("Received unexpected input mode");
            }

        }

        public bool IsCreateTowerNewlyPressed()
        {
            switch (m_Mode)
            {
                case InputMode.Player1:
                    return m_newState.IsKeyDown(Keys.R)&& m_oldState.IsKeyUp(Keys.R);
                case InputMode.Player2:
                    return m_newState.IsKeyDown(Keys.Add)&& m_oldState.IsKeyUp(Keys.Add);
                default:
                    throw new NotImplementedException("Receeved unexpected input");
            }
        }

        public bool IsCreateTowerNewlyReleased()
        {
            switch (m_Mode)
            {
                case InputMode.Player1:
                    return m_newState.IsKeyUp(Keys.R) && m_oldState.IsKeyDown(Keys.R);
                case InputMode.Player2:
                    return m_newState.IsKeyUp(Keys.Add) && m_oldState.IsKeyDown(Keys.Add);
                default:
                    throw new NotImplementedException("Receeved unexpected input");
            }
        }

        public bool IsSelectTowerNewlyPressed()
        {
            switch (m_Mode)
            {
                case InputMode.Player1:
                    return m_newState.IsKeyUp(Keys.D1) && m_oldState.IsKeyDown(Keys.D1);
                case InputMode.Player2:
                    return m_newState.IsKeyUp(Keys.NumPad7) && m_oldState.IsKeyDown(Keys.NumPad7);
                default:
                    throw new NotImplementedException("Receeved unexpected input");
            }

        }

        public bool IsIncrementTowerNewlyPressed()
        {
            switch (m_Mode)
            {
                case InputMode.Player1:
                    return m_newState.IsKeyUp(Keys.Tab) && m_oldState.IsKeyDown(Keys.Tab);
                case InputMode.Player2:
                    return m_newState.IsKeyUp(Keys.NumPad9) && m_oldState.IsKeyDown(Keys.NumPad9);
                default:
                    throw new NotImplementedException("Receeved unexpected input");
            }

        }


        public bool IsDecrementTowerNewlyPressed()
        {
            switch (m_Mode)
            {
                case InputMode.Player1:
                    return m_newState.IsKeyUp(Keys.LeftShift) && m_oldState.IsKeyDown(Keys.LeftShift);
                case InputMode.Player2:
                    return m_newState.IsKeyUp(Keys.NumPad3) && m_oldState.IsKeyDown(Keys.NumPad3);
                default:
                    throw new NotImplementedException("Receeved unexpected input");
            }

        }


        public bool IsToggleBuildNewlyPressed()
        {
            switch (m_Mode)
            {
                case InputMode.Player1:
                    return m_newState.IsKeyDown(Keys.D2) && m_oldState.IsKeyUp(Keys.D2);
                case InputMode.Player2:
                    return m_newState.IsKeyDown(Keys.Multiply) && m_oldState.IsKeyUp(Keys.Multiply);
                default:
                    throw new NotImplementedException("Receeved unexpected input");

            }
        }

        public bool IsCreateSurfaceNewlyPressed()
        {
            switch (m_Mode)
            {
                case InputMode.Player1:
                    return m_newState.IsKeyDown(Keys.F) && m_oldState.IsKeyUp(Keys.F);
                case InputMode.Player2:
                    return m_newState.IsKeyDown(Keys.Subtract) && m_oldState.IsKeyUp(Keys.Subtract);
                default:
                    throw new NotImplementedException("Received unexpected input mode");
            }
        }

        public bool IsCreateSurfaceNewlyReleased()
        {
            switch (m_Mode)
            {
                case InputMode.Player1:
                    return m_newState.IsKeyUp(Keys.F) && m_oldState.IsKeyDown(Keys.F);
                case InputMode.Player2:
                    return m_newState.IsKeyUp(Keys.Subtract) && m_oldState.IsKeyDown(Keys.Subtract);
                default:
                    throw new NotImplementedException("Received unexpected input mode");
            }
        }

        public bool IsAimUpPressed()
        {
            switch (m_Mode)
            {
                case InputMode.Player1:
                    return m_newState.IsKeyDown(Keys.R);
                case InputMode.Player2:
                    return m_newState.IsKeyDown(Keys.NumPad7);
                default:
                    throw new NotImplementedException("Received unexpected input mode");
            }
        }
        public bool IsAimDownPressed()
        {
            switch (m_Mode)
            {
                case InputMode.Player1:
                    return m_newState.IsKeyDown(Keys.F);
                case InputMode.Player2:
                    return m_newState.IsKeyDown(Keys.NumPad4);
                default:
                    throw new NotImplementedException("Received unexpected input mode");
            }
        }
        public bool IsFirePressed()
        {
            switch (m_Mode)
            {
                case InputMode.Player1:
                    return m_newState.IsKeyDown(Keys.Space);
                case InputMode.Player2:
                    return m_newState.IsKeyDown(Keys.NumPad0);
                default:
                    throw new NotImplementedException("Received unexpected input mode");
            }
        }

        public bool IsFireNewlyPressed()
        {
            switch (m_Mode)
            {
                case InputMode.Player1:
                    return m_newState.IsKeyDown(Keys.Space) && m_oldState.IsKeyUp(Keys.Space);
                case InputMode.Player2:
                    return m_newState.IsKeyDown(Keys.NumPad0) && m_oldState.IsKeyUp(Keys.NumPad0);
                default:
                    throw new NotImplementedException("Received unexpected input mode");
            }
        }
        public bool IsFireReleased()
        {
            switch (m_Mode)
            {
                case InputMode.Player1:
                    return m_newState.IsKeyUp(Keys.Space) && m_oldState.IsKeyDown(Keys.Space);
                case InputMode.Player2:
                    return m_newState.IsKeyUp(Keys.NumPad0) && m_oldState.IsKeyDown(Keys.NumPad0);
                default:
                    throw new NotImplementedException("Received unexpected input mode");
            }
        }

        public void Update()
        {
            m_oldState = m_newState;
            m_newState = Keyboard.GetState();
        }
    }
}
