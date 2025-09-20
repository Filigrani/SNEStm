using static SNEStm.GamePadsManager;

namespace SNEStm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Application.Idle += HandleApplicationIdle;
        }

        void HandleApplicationIdle(object sender, EventArgs e)
        {
            GamePadsManager.Update();
            DebugText.Text = s_DebugText;
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
        }

        private void GamePadSelect1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetPadForPort(GamePadSelect1.SelectedIndex, 0);
        }

        private void AutoAssign_Click(object sender, EventArgs e)
        {
            SetButtonToMap(SNESButton.Up, 0, false);
        }
    }
}
