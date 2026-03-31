using CSInputs;
using CSInputs.Enums;
using CSInputs.ReadInput;
using CSInputs.Structs;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using static NESEps.GamePadsManager;

namespace NESEps
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
        public bool m_MouseMode = false;
        public int m_MouseSensitivity = 0;

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
        public GamePadInstance(bool Mouse)
        {
            m_Pad = null;
            m_MouseMode = true;
        }

        public GamePadInstance(UserIndex Index)
        {
            m_Pad = new Controller(Index);

            MapButton(SNESButton.Up, GamepadButtonFlags.DPadUp, 0);
            MapButton(SNESButton.Down, GamepadButtonFlags.DPadDown, 0);
            MapButton(SNESButton.Left, GamepadButtonFlags.DPadLeft, 0);
            MapButton(SNESButton.Right, GamepadButtonFlags.DPadRight, 0);

            MapButton(SNESButton.A, GamepadButtonFlags.B, 0);
            MapButton(SNESButton.B, GamepadButtonFlags.A, 0);
            MapButton(SNESButton.X, GamepadButtonFlags.Y, 0);
            MapButton(SNESButton.Y, GamepadButtonFlags.X, 0);

            MapButton(SNESButton.L, GamepadButtonFlags.LeftShoulder, 0);
            MapButton(SNESButton.R, GamepadButtonFlags.RightShoulder, 0);

            MapButton(SNESButton.Start, GamepadButtonFlags.Start, 0);
            MapButton(SNESButton.Select, GamepadButtonFlags.Back, 0);
        }

        public int GetInputs()
        {
            return m_InputsMask;
        }

        public void Update(bool IsMappingMode = false)
        {
            if(!m_MouseMode && !m_Pad.IsConnected)
            {
                return; 
            }

            State CurrentState = new State();

            if (!m_MouseMode)
            {
                CurrentState = m_Pad.GetState();
            }

            if (IsMappingMode)
            {
                ProcessMapping(CurrentState);
                if (m_MouseMode)
                {
                    GamePadsManager.InteruptMaping();
                }
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
                if (!m_MouseMode)
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
                else
                {
                    switch ((SNESButton)i)
                    {
                        case SNESButton.MouseAlwaysZero:
                            break;
                        case SNESButton.MouseSignatureAndButtons:
                            // Второй байт: 76543210
                            // ||||++++- Signature: 0001
                            // ||++----- Current sensitivity (0: low; 1: medium; 2: high)
                            // |+------- Left button (1: pressed)
                            // +-------- Right button (1: pressed)
                            byte mouseByte2 = 0x01; // Сигнатура 0001

                            // Чувствительность (биты 4-5)
                            mouseByte2 |= (byte)((m_MouseSensitivity & 0x03) << 4);

                            // Левая кнопка (бит 6)
                            if (Program.s_LMB) mouseByte2 |= 0x40;

                            // Правая кнопка (бит 7)
                            if (Program.s_RMB) mouseByte2 |= 0x80;

                            // Устанавливаем младшие 8 бит
                            input &= ~(mouseByte2 << i);
                            break;
                        case SNESButton.MouseY:
                            // Третий байт: вертикальное смещение
                            // |+++++++- Vertical displacement since last read
                            // +-------- Direction (1: up; 0: down)
                            byte mouseByte3 = 0;
                            int deltaY = Program.s_AccumulatedDeltaY;

                            if (deltaY != 0)
                            {
                                // Берем абсолютное значение (7 бит)
                                int absDeltaY = Math.Abs(deltaY);
                                if (absDeltaY > 127) absDeltaY = 127;

                                mouseByte3 = (byte)absDeltaY;

                                // Устанавливаем бит направления (бит 7)
                                // 1 = up (отрицательное delta), 0 = down (положительное delta)
                                if (deltaY < 0)
                                {
                                    mouseByte3 |= 0x80; // Движение вверх
                                }

                                // Сбрасываем накопленную дельту
                                Program.s_AccumulatedDeltaY = 0;
                            }

                            // Устанавливаем младшие 8 бит
                            input &= ~(mouseByte3 << i);
                            break;

                        case SNESButton.MouseX:
                            // Четвертый байт: горизонтальное смещение
                            // |+++++++- Horizontal displacement since last read
                            // +-------- Direction (1: left; 0: right)
                            byte mouseByte4 = 0;
                            int deltaX = Program.s_AccumulatedDeltaX;

                            if (deltaX != 0)
                            {
                                // Берем абсолютное значение (7 бит)
                                int absDeltaX = Math.Abs(deltaX);
                                if (absDeltaX > 127) absDeltaX = 127;

                                mouseByte4 = (byte)absDeltaX;

                                // Устанавливаем бит направления (бит 7)
                                // 1 = left (отрицательное delta), 0 = right (положительное delta)
                                if (deltaX < 0)
                                {
                                    mouseByte4 |= 0x80; // Движение влево
                                }

                                // Сбрасываем накопленную дельту
                                Program.s_AccumulatedDeltaX = 0;
                            }

                            // Устанавливаем младшие 8 бит
                            input &= ~(mouseByte4 << i);
                            break;

                        default:
                            input &= ~(1 << i);
                            break;
                    }
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
