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

        private static SNESButton s_ButtonToMap = SNESButton.Up;
        private static int s_PortMapTo = 1;
        private static bool s_MappingActive = false;
        private static bool s_EndMapingAfterOneButton = true;

        // Моргание кнопки во время биндинга
        private static int s_MapingBlinkFrame = 0; 
        private const int c_BlinkFrameTime = 200; // Сколько занимает цикл моргания
        private const int c_BlinkFrameWindow = 120; // Сколько кнопка видема во время моргания


        public static string s_DebugText = "test";
        private static int s_DebugIncrement = 0;

        public static void SetButtonToMap(SNESButton Button, int Port, bool EndMapingAfterOneButton = true)
        {
            // Если мы уже настраиваем кнопку, не пытаемся настроить другую.
            if(EndMapingAfterOneButton && s_MappingActive)
            {
                return;
            }

            // Если контролера для этого порта не было выбрано, ни чё не делаем.
            if(s_PlayerPads[Port] == null)
            {
                return;
            }
            
            
            s_ButtonToMap = Button;
            s_PortMapTo = Port;
            s_MappingActive = true;
            s_EndMapingAfterOneButton = EndMapingAfterOneButton;
        }

        public static void NextButtonToMap()
        {
            s_ButtonToMap++;
            if (s_ButtonToMap == SNESButton.Count)
            {
                InteruptMaping();
            }
        }

        public static void InteruptMaping()
        {
            s_MappingActive = false;
            s_EndMapingAfterOneButton = true;
        }


        public enum SNESButton
        {
            Up,
            Down,
            Left,
            Right,
            A,
            B,
            X,
            Y,
            L,
            R,
            Start,
            Select,

            Count,
        }

        public const int c_SNESButtonsCount = (int)SNESButton.Count;

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
                        UpdateVisual(Pad.m_SNESButtonsState, 0);
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
            }
        }

        public static void UpdateVisual(bool[] ButtonStates, int PlayerPort)
        {
            UnlitButtonsByForce(PlayerPort);
            if (PlayerPort == 0)
            {
                for (int i = 0; i != ButtonStates.Length; i++)
                {
                    PictureBox img;
                    if (s_Player1ButtonImages.TryGetValue((SNESButton)i, out img))
                    {
                        if (ButtonStates[i] || (s_MappingActive && s_ButtonToMap == (SNESButton)i && s_MapingBlinkFrame > c_BlinkFrameWindow))
                        {
                            img.Image = img.InitialImage;
                        }
                    }
                }
            }
        }

        public static void MapButton(GamePadInstance Pad, GamepadButtonFlags Button)
        {
            Pad.MapButton(s_ButtonToMap, Button, s_PortMapTo);

            if (s_EndMapingAfterOneButton)
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
    }
}
