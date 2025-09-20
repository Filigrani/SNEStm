using static SNEStm.GamePadsManager;

namespace SNEStm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Task.Run(Update);
        }

        public void Update()
        {
            GamePadsManager.Update();
            UsbWorker.Update();
            DebugText.Text = s_DebugText;
            DebugText2.Text = s_DebugText2;

            if (UsbWorker.HIDDevice != null)
            {
                HIDDeviceName.Text = UsbWorker.HIDDevice.GetManufacturer() + "\n" + UsbWorker.s_DebugText;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            GamePadsManager.InteruptMaping();
        }

        private void UpButtonP1_Click(object sender, EventArgs e)
        {
            SetButtonToMap(SNESButton.Up, 0);
        }

        private void RightButtonP1_Click(object sender, EventArgs e)
        {
            SetButtonToMap(SNESButton.Right, 0);
        }

        private void DownButtonP1_Click(object sender, EventArgs e)
        {
            SetButtonToMap(SNESButton.Down, 0);
        }

        private void LeftButtonP1_Click(object sender, EventArgs e)
        {
            SetButtonToMap(SNESButton.Left, 0);
        }

        private void AButtonP1_Click(object sender, EventArgs e)
        {
            SetButtonToMap(SNESButton.A, 0);
        }

        private void BButtonP1_Click(object sender, EventArgs e)
        {
            SetButtonToMap(SNESButton.B, 0);
        }

        private void XButtonP1_Click(object sender, EventArgs e)
        {
            SetButtonToMap(SNESButton.X, 0);
        }

        private void YButtonP1_Click(object sender, EventArgs e)
        {
            SetButtonToMap(SNESButton.Y, 0);
        }

        private void LButtonP1_Click(object sender, EventArgs e)
        {
            SetButtonToMap(SNESButton.L, 0);
        }

        private void RButtonP1_Click(object sender, EventArgs e)
        {
            SetButtonToMap(SNESButton.R, 0);
        }

        private void StartButtonP1_Click(object sender, EventArgs e)
        {
            SetButtonToMap(SNESButton.Start, 0);
        }

        private void SelectButtonP1_Click(object sender, EventArgs e)
        {
            SetButtonToMap(SNESButton.Select, 0);
        }

        public void RefreshGamepads()
        {
            GamePadSelect1.Items.Clear();
            GamePadSelect1.Items.Add("None");
            foreach (string padName in GetAllPads())
            {
                GamePadSelect1.Items.Add(padName);
            }
            GamePadSelect1.SelectedItem = GamePadSelect1.Items[0];
            GamePadSelect1.SelectedIndex = 0;

            GamePadSelect2.Items.Clear();
            GamePadSelect2.Items.Add("None");
            foreach (string padName in GetAllPads())
            {
                GamePadSelect2.Items.Add(padName);
            }
            GamePadSelect2.SelectedItem = GamePadSelect2.Items[0];
            GamePadSelect2.SelectedIndex = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshGamepads();

            s_Player1ButtonImages.Add(SNESButton.A, AButtonP1);
            s_Player1ButtonImages.Add(SNESButton.B, BButtonP1);
            s_Player1ButtonImages.Add(SNESButton.X, XButtonP1);
            s_Player1ButtonImages.Add(SNESButton.Y, YButtonP1);

            s_Player1ButtonImages.Add(SNESButton.Up, UpButtonP1);
            s_Player1ButtonImages.Add(SNESButton.Left, LeftButtonP1);
            s_Player1ButtonImages.Add(SNESButton.Right, RightButtonP1);
            s_Player1ButtonImages.Add(SNESButton.Down, DownButtonP1);

            s_Player1ButtonImages.Add(SNESButton.L, LButtonP1);
            s_Player1ButtonImages.Add(SNESButton.R, RButtonP1);

            s_Player1ButtonImages.Add(SNESButton.Start, StartButtonP1);
            s_Player1ButtonImages.Add(SNESButton.Select, SelectButtonP1);
            UnlitButtonsByForce(0);

            s_Player2ButtonImages.Add(SNESButton.A, AButtonP2);
            s_Player2ButtonImages.Add(SNESButton.B, BButtonP2);
            s_Player2ButtonImages.Add(SNESButton.X, XButtonP2);
            s_Player2ButtonImages.Add(SNESButton.Y, YButtonP2);

            s_Player2ButtonImages.Add(SNESButton.Up, UpButtonP2);
            s_Player2ButtonImages.Add(SNESButton.Left, LeftButtonP2);
            s_Player2ButtonImages.Add(SNESButton.Right, RightButtonP2);
            s_Player2ButtonImages.Add(SNESButton.Down, DownButtonP2);

            s_Player2ButtonImages.Add(SNESButton.L, LButtonP2);
            s_Player2ButtonImages.Add(SNESButton.R, RButtonP2);

            s_Player2ButtonImages.Add(SNESButton.Start, StartButtonP2);
            s_Player2ButtonImages.Add(SNESButton.Select, SelectButtonP2);
            UnlitButtonsByForce(1);
        }

        private void GamePadSelect1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetPadForPort(GamePadSelect1.SelectedIndex, 0);
        }


        private void GamePadSelect2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetPadForPort(GamePadSelect2.SelectedIndex, 1);
        }

        private void AutoAssign_Click(object sender, EventArgs e)
        {
            SetButtonToMap(SNESButton.Up, 0, false);
        }

        private void AutoAssign2_Click(object sender, EventArgs e)
        {
            SetButtonToMap(SNESButton.Up, 1, false);
        }

        private void UpButtonP2_Click(object sender, EventArgs e)
        {
            SetButtonToMap(SNESButton.Up, 1);
        }

        private void RightButtonP2_Click(object sender, EventArgs e)
        {
            SetButtonToMap(SNESButton.Right, 1);
        }

        private void DownButtonP2_Click(object sender, EventArgs e)
        {
            SetButtonToMap(SNESButton.Down, 1);
        }

        private void LeftButtonP2_Click(object sender, EventArgs e)
        {
            SetButtonToMap(SNESButton.Left, 1);
        }

        private void AButtonP2_Click(object sender, EventArgs e)
        {
            SetButtonToMap(SNESButton.A, 1);
        }

        private void BButtonP2_Click(object sender, EventArgs e)
        {
            SetButtonToMap(SNESButton.B, 1);
        }

        private void XButtonP2_Click(object sender, EventArgs e)
        {
            SetButtonToMap(SNESButton.X, 1);
        }

        private void YButtonP2_Click(object sender, EventArgs e)
        {
            SetButtonToMap(SNESButton.Y, 1);
        }

        private void LButtonP2_Click(object sender, EventArgs e)
        {
            SetButtonToMap(SNESButton.L, 1);
        }

        private void RButtonP2_Click(object sender, EventArgs e)
        {
            SetButtonToMap(SNESButton.R, 1);
        }

        private void StartButtonP2_Click(object sender, EventArgs e)
        {
            SetButtonToMap(SNESButton.Start, 1);
        }

        private void SelectButtonP2_Click(object sender, EventArgs e)
        {
            SetButtonToMap(SNESButton.Select, 1);
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            GamePadsManager.InteruptMaping();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           
        }
    }
}
