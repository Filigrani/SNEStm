using static NESEps.GamePadsManager;

namespace NESEps
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
            while (true)
            {
                GamePadsManager.Update();
                TCPWorker.Update();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            GamePadsManager.InteruptMaping();
        }

        private void UpButtonP1_Click(object sender, EventArgs e)
        {
            SetButtonToMap(NESButton.Up, 0);
        }

        private void RightButtonP1_Click(object sender, EventArgs e)
        {
            SetButtonToMap(NESButton.Right, 0);
        }

        private void DownButtonP1_Click(object sender, EventArgs e)
        {
            SetButtonToMap(NESButton.Down, 0);
        }

        private void LeftButtonP1_Click(object sender, EventArgs e)
        {
            SetButtonToMap(NESButton.Left, 0);
        }

        private void AButtonP1_Click(object sender, EventArgs e)
        {
            SetButtonToMap(NESButton.A, 0);
        }

        private void BButtonP1_Click(object sender, EventArgs e)
        {
            SetButtonToMap(NESButton.B, 0);
        }

        private void XButtonP1_Click(object sender, EventArgs e)
        {

        }

        private void YButtonP1_Click(object sender, EventArgs e)
        {

        }

        private void LButtonP1_Click(object sender, EventArgs e)
        {

        }

        private void RButtonP1_Click(object sender, EventArgs e)
        {

        }

        private void StartButtonP1_Click(object sender, EventArgs e)
        {
            SetButtonToMap(NESButton.Start, 0);
        }

        private void SelectButtonP1_Click(object sender, EventArgs e)
        {
            SetButtonToMap(NESButton.Select, 0);
        }

        public void RefreshGamepads()
        {
            GamePadSelect1.Items.Clear();
            GamePadSelect1.Items.Add("None");
            foreach (string padName in GetAllPads())
            {
                GamePadSelect1.Items.Add(padName);
            }
            GamePadSelect1.Items.Add("Mouse");
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

            s_Player1ButtonImages.Add(NESButton.A, AButtonP1);
            s_Player1ButtonImages.Add(NESButton.B, BButtonP1);

            s_Player1ButtonImages.Add(NESButton.Up, UpButtonP1);
            s_Player1ButtonImages.Add(NESButton.Left, LeftButtonP1);
            s_Player1ButtonImages.Add(NESButton.Right, RightButtonP1);
            s_Player1ButtonImages.Add(NESButton.Down, DownButtonP1);


            s_Player1ButtonImages.Add(NESButton.Start, StartButtonP1);
            s_Player1ButtonImages.Add(NESButton.Select, SelectButtonP1);
            UnlitButtonsByForce(0);

            s_Player2ButtonImages.Add(NESButton.A, AButtonP2);
            s_Player2ButtonImages.Add(NESButton.B, BButtonP2);

            s_Player2ButtonImages.Add(NESButton.Up, UpButtonP2);
            s_Player2ButtonImages.Add(NESButton.Left, LeftButtonP2);
            s_Player2ButtonImages.Add(NESButton.Right, RightButtonP2);
            s_Player2ButtonImages.Add(NESButton.Down, DownButtonP2);

            s_Player2ButtonImages.Add(NESButton.Start, StartButtonP2);
            s_Player2ButtonImages.Add(NESButton.Select, SelectButtonP2);
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
            AutoMap(0);
        }

        private void AutoAssign2_Click(object sender, EventArgs e)
        {
            AutoMap(1);
        }

        private void UpButtonP2_Click(object sender, EventArgs e)
        {
            SetButtonToMap(NESButton.Up, 1);
        }

        private void RightButtonP2_Click(object sender, EventArgs e)
        {
            SetButtonToMap(NESButton.Right, 1);
        }

        private void DownButtonP2_Click(object sender, EventArgs e)
        {
            SetButtonToMap(NESButton.Down, 1);
        }

        private void LeftButtonP2_Click(object sender, EventArgs e)
        {
            SetButtonToMap(NESButton.Left, 1);
        }

        private void AButtonP2_Click(object sender, EventArgs e)
        {
            SetButtonToMap(NESButton.A, 1);
        }

        private void BButtonP2_Click(object sender, EventArgs e)
        {
            SetButtonToMap(NESButton.B, 1);
        }

        private void XButtonP2_Click(object sender, EventArgs e)
        {

        }

        private void YButtonP2_Click(object sender, EventArgs e)
        {

        }

        private void LButtonP2_Click(object sender, EventArgs e)
        {

        }

        private void RButtonP2_Click(object sender, EventArgs e)
        {

        }

        private void StartButtonP2_Click(object sender, EventArgs e)
        {
            SetButtonToMap(NESButton.Start, 1);
        }

        private void SelectButtonP2_Click(object sender, EventArgs e)
        {
            SetButtonToMap(NESButton.Select, 1);
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            GamePadsManager.InteruptMaping();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DebugText.Text = s_DebugText;
            DebugText2.Text = s_DebugText2;
            for (int i = 0; i != 2; i++) // != быстрее чем <=, а юзать s_Pads.Length, без толку, ибо мы знаем что число всегда 2.
            {
                GamePadInstance Pad = s_PlayerPads[i];

                if (Pad != null)
                {
                    UpdateVisual(Pad.m_SNESButtonsState, i);
                }
            }
        }

        private void ManualInput_CheckedChanged(object sender, EventArgs e)
        {
            GamePadsManager.s_ManualInput = ManualInput.Checked;
        }
    }
}
