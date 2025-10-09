using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using SharpDX.XInput;
using static SNEStm.GamePadsManager;

namespace SNEStm
{
    public class GamePadInstance
    {
        public Controller m_Pad;

        // Юзаем c_SNESButtonsCount, вместо приобразования (int)SNESButtons.Count
        // Так и приобразовывать лишний раз не надо, и захардкожаную константу мы тоже в ручную не пишем,
        // и если чё призайдёт она всегда заскейлиться от enum'а в любом случаии.

        public GamepadButtonFlags[] m_MappedButtons = new GamepadButtonFlags[c_SNESButtonsCount]; // XInput -> SNES Button ID
        public bool[] m_SNESButtonsState = new bool[c_SNESButtonsCount]; // Зажата или отжата SNES кнопка

        public int m_InputsMask = 0xFFFF;
        public int m_LastInputs = 0xFFFF;
        public bool m_SentData = true;

        private State s_LastState = new State();

        private List<GamepadButtonFlags> s_BindableButtons = new List<GamepadButtonFlags>() 
        {
            GamepadButtonFlags.A,GamepadButtonFlags.B,GamepadButtonFlags.X,GamepadButtonFlags.Y,

            GamepadButtonFlags.DPadUp,GamepadButtonFlags.DPadDown,GamepadButtonFlags.DPadLeft,GamepadButtonFlags.DPadRight,

            GamepadButtonFlags.Start,GamepadButtonFlags.Back,

            GamepadButtonFlags.LeftShoulder,GamepadButtonFlags.RightShoulder,

            GamepadButtonFlags.LeftThumb,GamepadButtonFlags.RightThumb,

        };

        public class DpadOverAxis
        {
            public bool m_Up = false;
            public bool m_Down = false;
            public bool m_Left = false;
            public bool m_Right = false;

            public DpadOverAxis(short X, short Y)
            {
                if(X < 0)
                {
                    m_Left = true;
                }else if(X > 0)
                {
                    m_Right = true;
                }

                if(Y > 0)
                {
                    m_Up = true;
                }else if(Y < 0)
                {
                    m_Down = true;
                }
            }
        }


        public GamePadInstance(UserIndex Index)
        {
            m_Pad = new Controller(Index);
        }

        public int GetInputs()
        {
            return m_InputsMask;
        }

        public void Update(bool IsMappingMode = false)
        {
            if (!m_Pad.IsConnected)
            {
                return;
            }
            
            
            State CurrentState = m_Pad.GetState();

            if (IsMappingMode)
            {
                ProcessMapping(CurrentState);
            }

            m_InputsMask = UpdateButtons(CurrentState);

            if (m_InputsMask != m_LastInputs)
            {
                m_LastInputs = m_InputsMask;
                m_SentData = false;
            }

            s_LastState = CurrentState;
        }

        public void ProcessMapping(State CurrentState)
        {
            DpadOverAxis Dpad = new DpadOverAxis(CurrentState.Gamepad.LeftThumbX, CurrentState.Gamepad.LeftThumbY);
            DpadOverAxis LastDpad = new DpadOverAxis(s_LastState.Gamepad.LeftThumbX, s_LastState.Gamepad.LeftThumbY);
            for (int i = 0; i != s_BindableButtons.Count; i++)
            {
                GamepadButtonFlags ButtonToCheck = s_BindableButtons[i];

                bool NewState = ButtonToCheck == GamepadButtonFlags.None ? false : CurrentState.Gamepad.Buttons.HasFlag(ButtonToCheck);
                bool OldState = s_LastState.Gamepad.Buttons.HasFlag(ButtonToCheck);

                if (!NewState)
                {
                    if (ButtonToCheck == GamepadButtonFlags.DPadUp && Dpad.m_Up)
                    {
                        NewState = true;
                    }
                    else if (ButtonToCheck == GamepadButtonFlags.DPadDown && Dpad.m_Down)
                    {
                        NewState = true;
                    }
                    else if (ButtonToCheck == GamepadButtonFlags.DPadLeft && Dpad.m_Left)
                    {
                        NewState = true;
                    }
                    else if (ButtonToCheck == GamepadButtonFlags.DPadRight && Dpad.m_Right)
                    {
                        NewState = true;
                    }
                }

                if (!OldState)
                {
                    if (ButtonToCheck == GamepadButtonFlags.DPadUp && LastDpad.m_Up)
                    {
                        OldState = true;
                    }
                    else if (ButtonToCheck == GamepadButtonFlags.DPadDown && LastDpad.m_Down)
                    {
                        OldState = true;
                    }
                    else if (ButtonToCheck == GamepadButtonFlags.DPadLeft && LastDpad.m_Left)
                    {
                        OldState = true;
                    }
                    else if (ButtonToCheck == GamepadButtonFlags.DPadRight && LastDpad.m_Right)
                    {
                        OldState = true;
                    }
                }


                // Если до этого кнопка не была нажата, а теперь она нажата, тригерим бинд.
                if (!OldState && NewState)
                {
                    GamePadsManager.MapButton(this, ButtonToCheck);
                    break;
                }
            }
        }

        public int UpdateButtons(State CurrentState)
        {
            int input = 0xFFFF;

            for (int i = 0; i != c_SNESButtonsCount; i++)
            {

                GamepadButtonFlags ButtonToCheck = m_MappedButtons[i];

                DpadOverAxis Dpad = new DpadOverAxis(CurrentState.Gamepad.LeftThumbX, CurrentState.Gamepad.LeftThumbY);

                bool OldState = m_SNESButtonsState[i];
                bool NewState = ButtonToCheck == GamepadButtonFlags.None ? false : CurrentState.Gamepad.Buttons.HasFlag(ButtonToCheck);

                if (!NewState)
                {
                    if (ButtonToCheck == GamepadButtonFlags.DPadUp && Dpad.m_Up)
                    {
                        NewState = true;
                    }
                    else if (ButtonToCheck == GamepadButtonFlags.DPadDown && Dpad.m_Down)
                    {
                        NewState = true;
                    }
                    else if (ButtonToCheck == GamepadButtonFlags.DPadLeft && Dpad.m_Left)
                    {
                        NewState = true;
                    }
                    else if (ButtonToCheck == GamepadButtonFlags.DPadRight && Dpad.m_Right)
                    {
                        NewState = true;
                    }
                }

                m_SNESButtonsState[i] = NewState; // Для визуалайзера

                if (NewState)
                {
                    input &= ~(1 << i);
                }
            }
            return input;
        }

        public void MapButton(SNESButton Snes, GamepadButtonFlags FlagButton, int Port)
        {
            m_MappedButtons[(int)Snes] = FlagButton;
        }

        public string GetButtonPromt()
        {
            string Str = "";
            for (int i = 0; i != c_SNESButtonsCount; i++)
            {
                if(m_SNESButtonsState[i] == true)
                {
                    Str += "1";
                }
                else
                {
                    Str += "0";
                }
            }
            return Str;
        }

        public string GetDebugText()
        {
            return "↑↓←→ABXYLRSS\n" + GetButtonPromt();
        }
    }
}
