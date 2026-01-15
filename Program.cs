using HidApi;
using CSInputs.Enums;
using CSInputs.ReadInput;
using CSInputs.Structs;

namespace SNEStm
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        /// 

        public static bool s_LMB = false;
        public static bool s_RMB = false;
        public static int s_LastMouseX = 0;
        public static int s_LastMouseY = 0;
        public static int s_AccumulatedDeltaX = 0;
        public static int s_AccumulatedDeltaY = 0;

        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            InputListener inputListener = new InputListener();

            // Subscribe to MouseInputs event handler to listen mouse inputs.
            inputListener.MouseInputs += InputListener_MouseInputs;

            Application.Run(new Form1());

            Hid.Exit();
        }

        public static void InputListener_MouseInputs(MouseData data, ref ModifierKey modifierKey)
        {
            if (data.Key == MouseKeys.MouseLeft)
            {
                s_LMB = (data.Flags == KeyFlags.KeyDown);
            }
            if (data.Key == MouseKeys.MouseRight)
            {
                s_RMB = (data.Flags == KeyFlags.KeyDown);
            }

            // Накопление дельты движения
            int deltaX = data.PositionRelative.X - s_LastMouseX;
            int deltaY = data.PositionRelative.Y - s_LastMouseY;

            s_LastMouseX = data.PositionRelative.X;
            s_LastMouseY = data.PositionRelative.Y;

            s_AccumulatedDeltaX += deltaX;
            s_AccumulatedDeltaY += deltaY;

            // Ограничим накопленную дельту (дополнительная защита от переполнения)
            if (s_AccumulatedDeltaX > 127) s_AccumulatedDeltaX = 127;
            if (s_AccumulatedDeltaX < -128) s_AccumulatedDeltaX = -128;
            if (s_AccumulatedDeltaY > 127) s_AccumulatedDeltaY = 127;
            if (s_AccumulatedDeltaY < -128) s_AccumulatedDeltaY = -128;
        }
    }
}