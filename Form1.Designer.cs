namespace SNEStm
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            XButtonP1 = new PictureBox();
            BButtonP1 = new PictureBox();
            AButtonP1 = new PictureBox();
            YButtonP1 = new PictureBox();
            StartButtonP1 = new PictureBox();
            SelectButtonP1 = new PictureBox();
            DebugText = new Label();
            GamePadSelect1 = new ComboBox();
            AutoAssign = new Button();
            UpButtonP1 = new PictureBox();
            DownButtonP1 = new PictureBox();
            RightButtonP1 = new PictureBox();
            LeftButtonP1 = new PictureBox();
            LButtonP1 = new PictureBox();
            RButtonP1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)XButtonP1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)BButtonP1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AButtonP1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)YButtonP1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)StartButtonP1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)SelectButtonP1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)UpButtonP1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)DownButtonP1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RightButtonP1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)LeftButtonP1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)LButtonP1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RButtonP1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.pad_inactive;
            pictureBox1.Location = new Point(12, 32);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(303, 229);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // XButtonP1
            // 
            XButtonP1.BackColor = Color.FromArgb(255, 192, 192);
            XButtonP1.Cursor = Cursors.Hand;
            XButtonP1.ErrorImage = Properties.Resources.Xbutton_inactive;
            XButtonP1.Image = Properties.Resources.Xbutton;
            XButtonP1.InitialImage = Properties.Resources.Xbutton;
            XButtonP1.Location = new Point(260, 130);
            XButtonP1.Name = "XButtonP1";
            XButtonP1.Size = new Size(18, 24);
            XButtonP1.TabIndex = 10;
            XButtonP1.TabStop = false;
            XButtonP1.Click += XButtonP1_Click;
            // 
            // BButtonP1
            // 
            BButtonP1.BackColor = Color.RosyBrown;
            BButtonP1.Cursor = Cursors.Hand;
            BButtonP1.ErrorImage = Properties.Resources.Bbutton_inactive;
            BButtonP1.Image = Properties.Resources.Bbutton;
            BButtonP1.InitialImage = Properties.Resources.Bbutton;
            BButtonP1.Location = new Point(222, 162);
            BButtonP1.Name = "BButtonP1";
            BButtonP1.Size = new Size(36, 38);
            BButtonP1.TabIndex = 11;
            BButtonP1.TabStop = false;
            BButtonP1.Click += BButtonP1_Click;
            // 
            // AButtonP1
            // 
            AButtonP1.BackColor = Color.FromArgb(255, 192, 192);
            AButtonP1.Cursor = Cursors.Hand;
            AButtonP1.ErrorImage = Properties.Resources.Abutton_inactive;
            AButtonP1.Image = Properties.Resources.Abutton;
            AButtonP1.InitialImage = Properties.Resources.Abutton;
            AButtonP1.Location = new Point(264, 160);
            AButtonP1.Name = "AButtonP1";
            AButtonP1.Size = new Size(16, 26);
            AButtonP1.TabIndex = 12;
            AButtonP1.TabStop = false;
            AButtonP1.Click += AButtonP1_Click;
            // 
            // YButtonP1
            // 
            YButtonP1.BackColor = Color.Transparent;
            YButtonP1.Cursor = Cursors.Hand;
            YButtonP1.ErrorImage = Properties.Resources.Ybutton_inactive;
            YButtonP1.Image = Properties.Resources.Ybutton;
            YButtonP1.InitialImage = Properties.Resources.Ybutton;
            YButtonP1.Location = new Point(221, 124);
            YButtonP1.Name = "YButtonP1";
            YButtonP1.Size = new Size(31, 32);
            YButtonP1.TabIndex = 13;
            YButtonP1.TabStop = false;
            YButtonP1.Click += YButtonP1_Click;
            // 
            // StartButtonP1
            // 
            StartButtonP1.Cursor = Cursors.Hand;
            StartButtonP1.ErrorImage = Properties.Resources.StartSelectButton_inactive;
            StartButtonP1.Image = Properties.Resources.StartSelectButton;
            StartButtonP1.InitialImage = Properties.Resources.StartSelectButton;
            StartButtonP1.Location = new Point(163, 124);
            StartButtonP1.Name = "StartButtonP1";
            StartButtonP1.Size = new Size(24, 26);
            StartButtonP1.TabIndex = 14;
            StartButtonP1.TabStop = false;
            StartButtonP1.Click += StartButtonP1_Click;
            // 
            // SelectButtonP1
            // 
            SelectButtonP1.Cursor = Cursors.Hand;
            SelectButtonP1.ErrorImage = Properties.Resources.StartSelectButton_inactive;
            SelectButtonP1.Image = Properties.Resources.StartSelectButton;
            SelectButtonP1.InitialImage = Properties.Resources.StartSelectButton;
            SelectButtonP1.Location = new Point(133, 124);
            SelectButtonP1.Name = "SelectButtonP1";
            SelectButtonP1.Size = new Size(25, 26);
            SelectButtonP1.TabIndex = 15;
            SelectButtonP1.TabStop = false;
            SelectButtonP1.Click += SelectButtonP1_Click;
            // 
            // DebugText
            // 
            DebugText.AutoSize = true;
            DebugText.Location = new Point(12, 275);
            DebugText.Name = "DebugText";
            DebugText.Size = new Size(63, 15);
            DebugText.TabIndex = 16;
            DebugText.Text = "DebugText";
            // 
            // GamePadSelect1
            // 
            GamePadSelect1.FormattingEnabled = true;
            GamePadSelect1.Location = new Point(12, 3);
            GamePadSelect1.Name = "GamePadSelect1";
            GamePadSelect1.Size = new Size(175, 23);
            GamePadSelect1.TabIndex = 17;
            GamePadSelect1.SelectedIndexChanged += GamePadSelect1_SelectedIndexChanged;
            // 
            // AutoAssign
            // 
            AutoAssign.Location = new Point(193, 3);
            AutoAssign.Name = "AutoAssign";
            AutoAssign.Size = new Size(122, 23);
            AutoAssign.TabIndex = 18;
            AutoAssign.Text = "Start Assign";
            AutoAssign.UseVisualStyleBackColor = true;
            AutoAssign.Click += AutoAssign_Click;
            // 
            // UpButtonP1
            // 
            UpButtonP1.ErrorImage = Properties.Resources.Up_inactive;
            UpButtonP1.Image = Properties.Resources.Up;
            UpButtonP1.InitialImage = Properties.Resources.Up;
            UpButtonP1.Location = new Point(47, 127);
            UpButtonP1.Name = "UpButtonP1";
            UpButtonP1.Size = new Size(23, 29);
            UpButtonP1.TabIndex = 19;
            UpButtonP1.TabStop = false;
            UpButtonP1.Click += UpButtonP1_Click;
            // 
            // DownButtonP1
            // 
            DownButtonP1.ErrorImage = Properties.Resources.Down_inactive;
            DownButtonP1.Image = Properties.Resources.Down;
            DownButtonP1.InitialImage = Properties.Resources.Down;
            DownButtonP1.Location = new Point(71, 165);
            DownButtonP1.Name = "DownButtonP1";
            DownButtonP1.Size = new Size(33, 37);
            DownButtonP1.TabIndex = 20;
            DownButtonP1.TabStop = false;
            DownButtonP1.Click += DownButtonP1_Click;
            // 
            // RightButtonP1
            // 
            RightButtonP1.ErrorImage = Properties.Resources.Right_inactive;
            RightButtonP1.Image = Properties.Resources.Right;
            RightButtonP1.InitialImage = Properties.Resources.Right;
            RightButtonP1.Location = new Point(76, 125);
            RightButtonP1.Name = "RightButtonP1";
            RightButtonP1.Size = new Size(32, 34);
            RightButtonP1.TabIndex = 21;
            RightButtonP1.TabStop = false;
            RightButtonP1.Click += RightButtonP1_Click;
            // 
            // LeftButtonP1
            // 
            LeftButtonP1.BackColor = Color.FromArgb(255, 128, 128);
            LeftButtonP1.ErrorImage = Properties.Resources.Left_inactive;
            LeftButtonP1.Image = Properties.Resources.Left;
            LeftButtonP1.InitialImage = Properties.Resources.Left;
            LeftButtonP1.Location = new Point(47, 170);
            LeftButtonP1.Name = "LeftButtonP1";
            LeftButtonP1.Size = new Size(21, 33);
            LeftButtonP1.TabIndex = 22;
            LeftButtonP1.TabStop = false;
            LeftButtonP1.Click += LeftButtonP1_Click;
            // 
            // LButtonP1
            // 
            LButtonP1.BackColor = Color.Red;
            LButtonP1.ErrorImage = Properties.Resources.L;
            LButtonP1.Image = Properties.Resources.L_active;
            LButtonP1.InitialImage = Properties.Resources.L_active;
            LButtonP1.Location = new Point(45, 85);
            LButtonP1.Name = "LButtonP1";
            LButtonP1.Size = new Size(51, 39);
            LButtonP1.TabIndex = 23;
            LButtonP1.TabStop = false;
            LButtonP1.Click += LButtonP1_Click;
            // 
            // RButtonP1
            // 
            RButtonP1.BackColor = Color.FromArgb(255, 192, 192);
            RButtonP1.ErrorImage = Properties.Resources.R;
            RButtonP1.Image = Properties.Resources.R_active;
            RButtonP1.InitialImage = Properties.Resources.R_active;
            RButtonP1.Location = new Point(214, 85);
            RButtonP1.Name = "RButtonP1";
            RButtonP1.Size = new Size(53, 38);
            RButtonP1.TabIndex = 24;
            RButtonP1.TabStop = false;
            RButtonP1.Click += RButtonP1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(107, 107, 107);
            ClientSize = new Size(800, 450);
            Controls.Add(RButtonP1);
            Controls.Add(LButtonP1);
            Controls.Add(LeftButtonP1);
            Controls.Add(RightButtonP1);
            Controls.Add(DownButtonP1);
            Controls.Add(UpButtonP1);
            Controls.Add(AutoAssign);
            Controls.Add(GamePadSelect1);
            Controls.Add(SelectButtonP1);
            Controls.Add(StartButtonP1);
            Controls.Add(YButtonP1);
            Controls.Add(BButtonP1);
            Controls.Add(AButtonP1);
            Controls.Add(XButtonP1);
            Controls.Add(DebugText);
            Controls.Add(pictureBox1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)XButtonP1).EndInit();
            ((System.ComponentModel.ISupportInitialize)BButtonP1).EndInit();
            ((System.ComponentModel.ISupportInitialize)AButtonP1).EndInit();
            ((System.ComponentModel.ISupportInitialize)YButtonP1).EndInit();
            ((System.ComponentModel.ISupportInitialize)StartButtonP1).EndInit();
            ((System.ComponentModel.ISupportInitialize)SelectButtonP1).EndInit();
            ((System.ComponentModel.ISupportInitialize)UpButtonP1).EndInit();
            ((System.ComponentModel.ISupportInitialize)DownButtonP1).EndInit();
            ((System.ComponentModel.ISupportInitialize)RightButtonP1).EndInit();
            ((System.ComponentModel.ISupportInitialize)LeftButtonP1).EndInit();
            ((System.ComponentModel.ISupportInitialize)LButtonP1).EndInit();
            ((System.ComponentModel.ISupportInitialize)RButtonP1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        public PictureBox pictureBox1;
        public PictureBox XButtonP1;
        public PictureBox BButtonP1;
        public PictureBox AButtonP1;
        public PictureBox YButtonP1;
        public PictureBox StartButtonP1;
        public PictureBox SelectButtonP1;
        public Label DebugText;
        public ComboBox GamePadSelect1;
        private Button AutoAssign;
        public PictureBox UpButtonP1;
        public PictureBox DownButtonP1;
        public PictureBox RightButtonP1;
        public PictureBox LeftButtonP1;
        private PictureBox LButtonP1;
        private PictureBox RButtonP1;
    }
}
