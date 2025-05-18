namespace FocusStudyReminder
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.nudStudyMinutes = new System.Windows.Forms.NumericUpDown();
            this.nudRestMinutes = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nudMinRandomMinutes = new System.Windows.Forms.NumericUpDown();
            this.nudMaxRandomMinutes = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.nudMeditationSeconds = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSoundFile = new System.Windows.Forms.TextBox();
            this.btnBrowseSoundFile = new System.Windows.Forms.Button();
            this.btnTestSound = new System.Windows.Forms.Button();
            this.btnDefaultSound = new System.Windows.Forms.Button();
            this.chkShowPopup = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblTimingHeader = new System.Windows.Forms.Label();
            this.lblRandomHeader = new System.Windows.Forms.Label();
            this.lblNotificationHeader = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblWindowBehaviorHeader = new System.Windows.Forms.Label();
            this.groupCloseAction = new System.Windows.Forms.GroupBox();
            this.radExit = new System.Windows.Forms.RadioButton();
            this.radMinimize = new System.Windows.Forms.RadioButton();
            this.radMinimizeToTray = new System.Windows.Forms.RadioButton();
            this.radAskEveryTime = new System.Windows.Forms.RadioButton();
            this.chkSilentMinimize = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudStudyMinutes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRestMinutes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinRandomMinutes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxRandomMinutes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeditationSeconds)).BeginInit();
            this.groupCloseAction.SuspendLayout();
            this.SuspendLayout();
            // 
            // nudStudyMinutes
            // 
            this.nudStudyMinutes.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudStudyMinutes.Location = new System.Drawing.Point(120, 48);
            this.nudStudyMinutes.Maximum = new decimal(new int[] {
            240,
            0,
            0,
            0});
            this.nudStudyMinutes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudStudyMinutes.Name = "nudStudyMinutes";
            this.nudStudyMinutes.Size = new System.Drawing.Size(70, 33);
            this.nudStudyMinutes.TabIndex = 6;
            this.nudStudyMinutes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudStudyMinutes.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            // 
            // nudRestMinutes
            // 
            this.nudRestMinutes.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudRestMinutes.Location = new System.Drawing.Point(120, 78);
            this.nudRestMinutes.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nudRestMinutes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRestMinutes.Name = "nudRestMinutes";
            this.nudRestMinutes.Size = new System.Drawing.Size(70, 33);
            this.nudRestMinutes.TabIndex = 8;
            this.nudRestMinutes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudRestMinutes.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(20, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 28);
            this.label1.TabIndex = 5;
            this.label1.Text = "学习时间(分)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.Location = new System.Drawing.Point(20, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 28);
            this.label2.TabIndex = 7;
            this.label2.Text = "休息时间(分)";
            // 
            // nudMinRandomMinutes
            // 
            this.nudMinRandomMinutes.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudMinRandomMinutes.Location = new System.Drawing.Point(120, 148);
            this.nudMinRandomMinutes.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nudMinRandomMinutes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMinRandomMinutes.Name = "nudMinRandomMinutes";
            this.nudMinRandomMinutes.Size = new System.Drawing.Size(70, 33);
            this.nudMinRandomMinutes.TabIndex = 10;
            this.nudMinRandomMinutes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudMinRandomMinutes.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nudMaxRandomMinutes
            // 
            this.nudMaxRandomMinutes.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudMaxRandomMinutes.Location = new System.Drawing.Point(270, 148);
            this.nudMaxRandomMinutes.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nudMaxRandomMinutes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMaxRandomMinutes.Name = "nudMaxRandomMinutes";
            this.nudMaxRandomMinutes.Size = new System.Drawing.Size(70, 33);
            this.nudMaxRandomMinutes.TabIndex = 12;
            this.nudMaxRandomMinutes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudMaxRandomMinutes.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label3.Location = new System.Drawing.Point(20, 150);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 28);
            this.label3.TabIndex = 9;
            this.label3.Text = "最小值(分)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label4.Location = new System.Drawing.Point(200, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 28);
            this.label4.TabIndex = 11;
            this.label4.Text = "最大值(分)";
            // 
            // nudMeditationSeconds
            // 
            this.nudMeditationSeconds.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudMeditationSeconds.Location = new System.Drawing.Point(120, 178);
            this.nudMeditationSeconds.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.nudMeditationSeconds.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudMeditationSeconds.Name = "nudMeditationSeconds";
            this.nudMeditationSeconds.Size = new System.Drawing.Size(70, 33);
            this.nudMeditationSeconds.TabIndex = 14;
            this.nudMeditationSeconds.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudMeditationSeconds.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label5.Location = new System.Drawing.Point(20, 180);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(124, 28);
            this.label5.TabIndex = 13;
            this.label5.Text = "冥想时间(秒)";
            // 
            // txtSoundFile
            // 
            this.txtSoundFile.BackColor = System.Drawing.Color.White;
            this.txtSoundFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSoundFile.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSoundFile.Location = new System.Drawing.Point(20, 290);
            this.txtSoundFile.Name = "txtSoundFile";
            this.txtSoundFile.Size = new System.Drawing.Size(222, 33);
            this.txtSoundFile.TabIndex = 16;
            this.txtSoundFile.Text = "default.wav";
            // 
            // btnBrowseSoundFile
            // 
            this.btnBrowseSoundFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnBrowseSoundFile.FlatAppearance.BorderSize = 0;
            this.btnBrowseSoundFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseSoundFile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseSoundFile.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnBrowseSoundFile.Location = new System.Drawing.Point(250, 290);
            this.btnBrowseSoundFile.Name = "btnBrowseSoundFile";
            this.btnBrowseSoundFile.Size = new System.Drawing.Size(90, 25);
            this.btnBrowseSoundFile.TabIndex = 17;
            this.btnBrowseSoundFile.Text = "浏览...";
            this.btnBrowseSoundFile.UseVisualStyleBackColor = false;
            this.btnBrowseSoundFile.Click += new System.EventHandler(this.btnBrowseSoundFile_Click);
            // 
            // btnTestSound
            // 
            this.btnTestSound.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnTestSound.FlatAppearance.BorderSize = 0;
            this.btnTestSound.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestSound.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTestSound.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnTestSound.Location = new System.Drawing.Point(20, 325);
            this.btnTestSound.Name = "btnTestSound";
            this.btnTestSound.Size = new System.Drawing.Size(110, 30);
            this.btnTestSound.TabIndex = 18;
            this.btnTestSound.Text = "测试音效";
            this.btnTestSound.UseVisualStyleBackColor = false;
            this.btnTestSound.Click += new System.EventHandler(this.btnTestSound_Click);
            // 
            // btnDefaultSound
            // 
            this.btnDefaultSound.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnDefaultSound.FlatAppearance.BorderSize = 0;
            this.btnDefaultSound.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDefaultSound.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDefaultSound.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnDefaultSound.Location = new System.Drawing.Point(140, 325);
            this.btnDefaultSound.Name = "btnDefaultSound";
            this.btnDefaultSound.Size = new System.Drawing.Size(110, 30);
            this.btnDefaultSound.TabIndex = 19;
            this.btnDefaultSound.Text = "默认音效";
            this.btnDefaultSound.UseVisualStyleBackColor = false;
            this.btnDefaultSound.Click += new System.EventHandler(this.btnDefaultSound_Click);
            // 
            // chkShowPopup
            // 
            this.chkShowPopup.AutoSize = true;
            this.chkShowPopup.Checked = true;
            this.chkShowPopup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowPopup.FlatAppearance.BorderSize = 0;
            this.chkShowPopup.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkShowPopup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.chkShowPopup.Location = new System.Drawing.Point(20, 260);
            this.chkShowPopup.Name = "chkShowPopup";
            this.chkShowPopup.Size = new System.Drawing.Size(158, 32);
            this.chkShowPopup.TabIndex = 15;
            this.chkShowPopup.Text = "显示提醒弹窗";
            this.chkShowPopup.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(102, 550);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 30);
            this.btnSave.TabIndex = 24;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(192, 550);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 25;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblTimingHeader
            // 
            this.lblTimingHeader.AutoSize = true;
            this.lblTimingHeader.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimingHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblTimingHeader.Location = new System.Drawing.Point(20, 20);
            this.lblTimingHeader.Name = "lblTimingHeader";
            this.lblTimingHeader.Size = new System.Drawing.Size(118, 32);
            this.lblTimingHeader.TabIndex = 0;
            this.lblTimingHeader.Text = "时间设置";
            // 
            // lblRandomHeader
            // 
            this.lblRandomHeader.AutoSize = true;
            this.lblRandomHeader.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRandomHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblRandomHeader.Location = new System.Drawing.Point(20, 120);
            this.lblRandomHeader.Name = "lblRandomHeader";
            this.lblRandomHeader.Size = new System.Drawing.Size(170, 32);
            this.lblRandomHeader.TabIndex = 1;
            this.lblRandomHeader.Text = "随机提醒设置";
            // 
            // lblNotificationHeader
            // 
            this.lblNotificationHeader.AutoSize = true;
            this.lblNotificationHeader.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNotificationHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblNotificationHeader.Location = new System.Drawing.Point(20, 230);
            this.lblNotificationHeader.Name = "lblNotificationHeader";
            this.lblNotificationHeader.Size = new System.Drawing.Size(118, 32);
            this.lblNotificationHeader.TabIndex = 2;
            this.lblNotificationHeader.Text = "通知设置";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.panel1.Location = new System.Drawing.Point(20, 110);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(320, 1);
            this.panel1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.panel2.Location = new System.Drawing.Point(20, 220);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(320, 1);
            this.panel2.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.panel3.Location = new System.Drawing.Point(20, 355);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(320, 1);
            this.panel3.TabIndex = 21;
            // 
            // lblWindowBehaviorHeader
            // 
            this.lblWindowBehaviorHeader.AutoSize = true;
            this.lblWindowBehaviorHeader.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWindowBehaviorHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblWindowBehaviorHeader.Location = new System.Drawing.Point(20, 365);
            this.lblWindowBehaviorHeader.Name = "lblWindowBehaviorHeader";
            this.lblWindowBehaviorHeader.Size = new System.Drawing.Size(170, 32);
            this.lblWindowBehaviorHeader.TabIndex = 20;
            this.lblWindowBehaviorHeader.Text = "窗口行为设置";
            // 
            // groupCloseAction
            // 
            this.groupCloseAction.Controls.Add(this.radExit);
            this.groupCloseAction.Controls.Add(this.radMinimize);
            this.groupCloseAction.Controls.Add(this.radMinimizeToTray);
            this.groupCloseAction.Controls.Add(this.radAskEveryTime);
            this.groupCloseAction.Location = new System.Drawing.Point(20, 395);
            this.groupCloseAction.Name = "groupCloseAction";
            this.groupCloseAction.Size = new System.Drawing.Size(320, 110);
            this.groupCloseAction.TabIndex = 22;
            this.groupCloseAction.TabStop = false;
            this.groupCloseAction.Text = "点击关闭按钮时";
            // 
            // radExit
            // 
            this.radExit.AutoSize = true;
            this.radExit.Location = new System.Drawing.Point(15, 25);
            this.radExit.Name = "radExit";
            this.radExit.Size = new System.Drawing.Size(117, 32);
            this.radExit.TabIndex = 0;
            this.radExit.Text = "退出应用";
            this.radExit.UseVisualStyleBackColor = true;
            // 
            // radMinimize
            // 
            this.radMinimize.AutoSize = true;
            this.radMinimize.Location = new System.Drawing.Point(15, 50);
            this.radMinimize.Name = "radMinimize";
            this.radMinimize.Size = new System.Drawing.Size(177, 32);
            this.radMinimize.TabIndex = 1;
            this.radMinimize.Text = "最小化到任务栏";
            this.radMinimize.UseVisualStyleBackColor = true;
            // 
            // radMinimizeToTray
            // 
            this.radMinimizeToTray.AutoSize = true;
            this.radMinimizeToTray.Checked = true;
            this.radMinimizeToTray.Location = new System.Drawing.Point(15, 75);
            this.radMinimizeToTray.Name = "radMinimizeToTray";
            this.radMinimizeToTray.Size = new System.Drawing.Size(197, 32);
            this.radMinimizeToTray.TabIndex = 2;
            this.radMinimizeToTray.TabStop = true;
            this.radMinimizeToTray.Text = "最小化到系统托盘";
            this.radMinimizeToTray.UseVisualStyleBackColor = true;
            // 
            // radAskEveryTime
            // 
            this.radAskEveryTime.AutoSize = true;
            this.radAskEveryTime.Location = new System.Drawing.Point(170, 25);
            this.radAskEveryTime.Name = "radAskEveryTime";
            this.radAskEveryTime.Size = new System.Drawing.Size(117, 32);
            this.radAskEveryTime.TabIndex = 3;
            this.radAskEveryTime.Text = "每次询问";
            this.radAskEveryTime.UseVisualStyleBackColor = true;
            // 
            // chkSilentMinimize
            // 
            this.chkSilentMinimize.AutoSize = true;
            this.chkSilentMinimize.Checked = true;
            this.chkSilentMinimize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSilentMinimize.Location = new System.Drawing.Point(20, 515);
            this.chkSilentMinimize.Name = "chkSilentMinimize";
            this.chkSilentMinimize.Size = new System.Drawing.Size(258, 32);
            this.chkSilentMinimize.TabIndex = 23;
            this.chkSilentMinimize.Text = "最小化时不显示系统通知";
            this.chkSilentMinimize.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(360, 611);
            this.Controls.Add(this.chkSilentMinimize);
            this.Controls.Add(this.groupCloseAction);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.lblWindowBehaviorHeader);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnDefaultSound);
            this.Controls.Add(this.btnTestSound);
            this.Controls.Add(this.btnBrowseSoundFile);
            this.Controls.Add(this.txtSoundFile);
            this.Controls.Add(this.chkShowPopup);
            this.Controls.Add(this.nudMeditationSeconds);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.nudMaxRandomMinutes);
            this.Controls.Add(this.nudMinRandomMinutes);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nudRestMinutes);
            this.Controls.Add(this.nudStudyMinutes);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblNotificationHeader);
            this.Controls.Add(this.lblRandomHeader);
            this.Controls.Add(this.lblTimingHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "设置";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudStudyMinutes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRestMinutes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinRandomMinutes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxRandomMinutes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeditationSeconds)).EndInit();
            this.groupCloseAction.ResumeLayout(false);
            this.groupCloseAction.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudStudyMinutes;
        private System.Windows.Forms.NumericUpDown nudRestMinutes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudMinRandomMinutes;
        private System.Windows.Forms.NumericUpDown nudMaxRandomMinutes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudMeditationSeconds;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSoundFile;
        private System.Windows.Forms.Button btnBrowseSoundFile;
        private System.Windows.Forms.Button btnTestSound;
        private System.Windows.Forms.Button btnDefaultSound;
        private System.Windows.Forms.CheckBox chkShowPopup;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblTimingHeader;
        private System.Windows.Forms.Label lblRandomHeader;
        private System.Windows.Forms.Label lblNotificationHeader;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblWindowBehaviorHeader;
        private System.Windows.Forms.GroupBox groupCloseAction;
        private System.Windows.Forms.RadioButton radAskEveryTime;
        private System.Windows.Forms.RadioButton radMinimizeToTray;
        private System.Windows.Forms.RadioButton radMinimize;
        private System.Windows.Forms.RadioButton radExit;
        private System.Windows.Forms.CheckBox chkSilentMinimize;
    }
} 