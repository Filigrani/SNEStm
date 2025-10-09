using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.XInput;

namespace SNEStm
{
    public static class GamePadsManager
    {
        public static GamePadInstance[] s_Pads =
        {
            new GamePadInstance(UserIndex.One),
            new GamePadInstance(UserIndex.Two),
            new GamePadInstance(UserIndex.Three),
            new GamePadInstance(UserIndex.Four),
        };

        public static GamePadInstance[] s_PlayerPads =
        {
            null,
            null,
        };

        public static Dictionary<SNESButton, PictureBox> s_Player1ButtonImages = new Dictionary<SNESButton, PictureBox>();
        public static Dictionary<SNESButton, PictureBox> s_Player2ButtonImages = new Dictionary<SNESButton, PictureBox>();

        private static SNESButton s_ButtonToMap = SNESButton.Up;
        private static int s_AutoMapIndex = 0;
        private static int s_PortMapTo = 1;
        private static bool s_MappingActive = false;

        // Моргание кнопки во время биндинга
        private static int s_MapingBlinkFrame = 0; 
        private const int c_BlinkFrameTime = 5; // Сколько занимает цикл моргания
        private const int c_BlinkFrameWindow = 3; // Сколько кнопка видема во время моргания


        public static string s_DebugText = "test";
        public static string s_DebugText2 = "test";

        private static int s_DebugIncrement = 0;


        public static void AutoMap(int Port)
        {
            s_AutoMapIndex = 0;
            s_ButtonToMap = s_BindingOrder[s_AutoMapIndex];
            s_PortMapTo = Port;
            s_MappingActive = true;
        }

        public static void SetButtonToMap(SNESButton Button, int Port)
        {
            // Если мы уже настраиваем кнопку, не пытаемся настроить другую.
            if(s_MappingActive)
            {
                return;
            }

            // Если контролера для этого порта не было выбрано, ни чё не делаем.
            if(s_PlayerPads[Port] == null)
            {
                return;
            }

            s_AutoMapIndex = -1;
            s_ButtonToMap = Button;
            s_PortMapTo = Port;
            s_MappingActive = true;
        }

        public static void NextButtonToMap()
        {
            s_AutoMapIndex++;
            if (s_AutoMapIndex == s_BindingOrder.Count)
            {
                InteruptMaping();
            }
            else
            {
                s_ButtonToMap = s_BindingOrder[s_AutoMapIndex];
            }
        }

        public static void InteruptMaping()
        {
            s_MappingActive = false;
            s_AutoMapIndex = -1;
        }

        public enum SNESButton
        {
            Unused_1,
            Unused_2,
            Unused_3,
            Unused_4,


            R,
            L,
            X,
            A,
            Right,
            Left,
            Down,
            Up,
            Start,
            Select,
            Y,
            B,

            Count,
        }

        public const int c_SNESButtonsCount = (int)SNESButton.Count;

        public static List<SNESButton> s_BindingOrder = new List<SNESButton>() 
        { 
            SNESButton.A, SNESButton.B, 
            SNESButton.Y, SNESButton.X, 
            SNESButton.Up, SNESButton.Down, 
            SNESButton.Left, SNESButton.Right, 
            SNESButton.L, SNESButton.R,
            SNESButton.Start, SNESButton.Select 
        };

        public static void Update()
        {
            for (int i = 0; i != 2; i++) // != быстрее чем <=, а юзать s_Pads.Length, без толку, ибо мы знаем что число всегда 2.
            {
                GamePadInstance Pad = s_PlayerPads[i];

                if(Pad != null)
                {
                    Pad.Update(s_MappingActive);

                    if(i == 0)
                    {
                        s_DebugText = Pad.GetDebugText()+ " tick " + s_DebugIncrement;
                    }else if(i == 1)
                    {
                        s_DebugText2 = Pad.GetDebugText() + " tick " + s_DebugIncrement;
                    }
                }
            }
            s_DebugIncrement++;
            if (s_MappingActive)
            {
                s_MapingBlinkFrame++;
                if (s_MapingBlinkFrame == c_BlinkFrameTime)
                {
                    s_MapingBlinkFrame = 0;
                }
            }
        }

        public static void UnlitButtonsByForce(int PlayerPort)
        {
            if (PlayerPort == 0)
            {
                foreach (var item in s_Player1ButtonImages)
                {
                    item.Value.Image = item.Value.ErrorImage;
                }
            }else if(PlayerPort == 1)
            {
                foreach (var item in s_Player2ButtonImages)
                {
                    item.Value.Image = item.Value.ErrorImage;
                }
            }
        }

        public static void UpdateVisual(bool[] ButtonStates, int PlayerPort)
        {
            UnlitButtonsByForce(PlayerPort);
            for (int i = 0; i != ButtonStates.Length; i++)
            {
                PictureBox img = null;

                if (PlayerPort == 0)
                {
                    s_Player1ButtonImages.TryGetValue((SNESButton)i, out img);
                }else if(PlayerPort == 1)
                {
                    s_Player2ButtonImages.TryGetValue((SNESButton)i, out img);
                }
                if (img != null)
                {
                    if (ButtonStates[i] || (s_MappingActive && s_ButtonToMap == (SNESButton)i && s_PortMapTo == PlayerPort && s_MapingBlinkFrame > c_BlinkFrameWindow))
                    {
                        img.Image = img.InitialImage;
                    }
                }
            }
        }

        public static void MapButton(GamePadInstance Pad, GamepadButtonFlags Button)
        {
            Pad.MapButton(s_ButtonToMap, Button, s_PortMapTo);

            if (s_AutoMapIndex == -1)
            {
                InteruptMaping();
            }
            else
            {
                NextButtonToMap();
            }
        }

        public static List<string> GetAllPads()
        {
            List<string> result = new List<string>();

            for (int i = 0; i != 4; i++)
            {
                result.Add("XInput device "+(i+1));
            }

            return result;
        }

        public static void SetPadForPort(int PadIndex, int Port)
        {
            UnlitButtonsByForce(0);
            UnlitButtonsByForce(1);
            InteruptMaping();
            if (PadIndex == 0)
            {
                s_PlayerPads[Port] = null;
            }
            else
            {
                s_PlayerPads[Port] = s_Pads[PadIndex-1];
            }
        }

        public static bool HasChange()
        {
            for (int i = 0; i != 2; i++)
            {
                GamePadInstance Pad = s_PlayerPads[i];
                if(Pad != null && !Pad.m_SentData)
                {
                    return true;
                }
            }
            return false;
        }

        public static ReadOnlySpan<byte> GetInputs()
        {
            List<byte> Buffer = new List<byte>();
            int Pos = 0;
            
            for (int i = 0; i != 2;i++)
            {
                GamePadInstance Pad = s_PlayerPads[i];

                int PadInput = 0xFFFF;

                if (Pad != null)
                {
                    PadInput = Pad.GetInputs();
                    Pad.m_SentData = true;
                }
                Buffer.InsertRange(Pos, BitConverter.GetBytes(PadInput));
                Pos += 2;
            }
            Buffer.Insert(0, 0x0);

            return (ReadOnlySpan<byte>)Buffer.ToArray();
        }
    }
}
