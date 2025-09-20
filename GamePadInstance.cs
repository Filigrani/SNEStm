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

        private byte[] s_InputsData = new byte[c_SNESButtonsCount]; // Готовый массив байт для отправки по USB. По сути тот же 
                                                                    // m_SNESButtonsState но в байтовом эквиваленте что бы лушний раз потом
                                                                    // не конвертировать булы в байты.

        private State s_LastState = new State();

        private List<GamepadButtonFlags> s_BindableButtons = new List<GamepadButtonFlags>() 
        {
            GamepadButtonFlags.A,GamepadButtonFlags.B,GamepadButtonFlags.X,GamepadButtonFlags.Y,

            GamepadButtonFlags.DPadUp,GamepadButtonFlags.DPadDown,GamepadButtonFlags.DPadLeft,GamepadButtonFlags.DPadRight,

            GamepadButtonFlags.Start,GamepadButtonFlags.Back,

            GamepadButtonFlags.LeftShoulder,GamepadButtonFlags.RightShoulder,

            GamepadButtonFlags.LeftThumb,GamepadButtonFlags.RightThumb,

        };


        public GamePadInstance(UserIndex Index)
        {
            m_Pad = new Controller(Index);
        }

        public byte[] GetInputs()
        {
            return s_InputsData;
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

            bool Changed = UpdateButtons(CurrentState);

            s_LastState = CurrentState;
        }

        public void ProcessMapping(State CurrentState)
        {
            for (int i = 0; i != s_BindableButtons.Count; i++)
            {
                GamepadButtonFlags ButtonToCheck = s_BindableButtons[i];
                // Если до этого кнопка не была нажата, а теперь она нажата, тригерим бинд.
                if(!s_LastState.Gamepad.Buttons.HasFlag(ButtonToCheck) && CurrentState.Gamepad.Buttons.HasFlag(ButtonToCheck))
                {
                    GamePadsManager.MapButton(this, ButtonToCheck);
                    break;
                }
            }
        }

        public bool UpdateButtons(State CurrentState)
        {
            bool Changed = false;
            for (int i = 0; i != c_SNESButtonsCount; i++)
            {

                GamepadButtonFlags ButtonToCheck = m_MappedButtons[i];


                bool OldState = m_SNESButtonsState[i];
                bool NewState = ButtonToCheck == GamepadButtonFlags.None ? false : CurrentState.Gamepad.Buttons.HasFlag(ButtonToCheck);

                if(NewState != OldState)
                {
                    m_SNESButtonsState[i] = NewState;
                    s_InputsData[i] = NewState ? (byte) 1 : (byte) 0; // сразу задаём в байтовом массиве.
                    Changed = true;
                }
            }

            return Changed;
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
