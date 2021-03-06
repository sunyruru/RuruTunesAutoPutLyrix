﻿namespace RuruTunesAutoPutLyrix
{
    using iTunesLib;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;

    public class RuruTunesAutoPutLyrix : Form
    {
        private Button btnDown;
        private Button btnDownC;
        private Button btnStart;
        private Button btnUp;
        private Button btnUpC;
        public CheckedListBox cbox;
        public CheckBox chkAlbumCover;
        public CheckBox chkAlbumTitleOnly;
        public CheckBox chkCoverBigSize;
        public CheckBox chkCoverItunes;
        public CheckBox chkCoverOverwrite;
        public CheckBox chkLyric;
        public CheckBox chkOverwrite;
        public CheckBox chkSongName;
        private IContainer components = null;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox grpCover;
        private GroupBox grpLyric;
        public IiTunes iTunesApp;
        private Label label1;
        private Label label2;
        public Label lblVersion;
        public CheckedListBox lbox;
        private LinkLabel linkLabel1;
        public Settings settings;
        public TextBox txtSize;

        public RuruTunesAutoPutLyrix()
        {
            this.InitializeComponent();
            this.iTunesApp = new iTunesAppClass();
            this.iTunesApp.BrowserWindow.Visible = true;
            this.iTunesApp.BrowserWindow.Minimized = false;
            this.settings = new Settings();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if ((this.lbox.SelectedIndex != -1) && (this.lbox.SelectedIndex != (this.lbox.Items.Count - 1)))
            {
                int selectedIndex = this.lbox.SelectedIndex;
                bool itemChecked = this.lbox.GetItemChecked(selectedIndex);
                this.lbox.Items.Insert(this.lbox.SelectedIndex + 2, this.lbox.SelectedItem);
                this.lbox.Items.RemoveAt(this.lbox.SelectedIndex);
                this.lbox.SelectedIndex = selectedIndex + 1;
                this.lbox.SetItemChecked(this.lbox.SelectedIndex, itemChecked);
                this.SaveSettings();
            }
        }

        private void btnDownC_Click(object sender, EventArgs e)
        {
            if ((this.cbox.SelectedIndex != -1) && (this.cbox.SelectedIndex != (this.cbox.Items.Count - 1)))
            {
                int selectedIndex = this.cbox.SelectedIndex;
                bool itemChecked = this.cbox.GetItemChecked(selectedIndex);
                this.cbox.Items.Insert(this.cbox.SelectedIndex + 2, this.cbox.SelectedItem);
                this.cbox.Items.RemoveAt(this.cbox.SelectedIndex);
                this.cbox.SelectedIndex = selectedIndex + 1;
                this.cbox.SetItemChecked(this.cbox.SelectedIndex, itemChecked);
                this.SaveSettings();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!this.chkLyric.Checked && !this.chkAlbumCover.Checked)
            {
                MessageBox.Show("동작을 선택하지 않았습니다. 가사,커버,Tag 중의 하나를 선택해주세요..", "RuruTunesAutoPutLyrix 오류");
            }
            else
            {
                IITTrackCollection selectedTracks = this.iTunesApp.BrowserWindow.SelectedTracks;
                if (selectedTracks == null)
                {
                    MessageBox.Show("iTunes에 선택된 곡이 없습니다. 곡을 선택후 실행하여 주세요.", "RuruTunesAutoPutLyrix 오류");
                }
                else if (this.iTunesApp.BrowserWindow.SelectedTracks.Count == 0)
                {
                    MessageBox.Show("iTunes에 선택된 곡이 없습니다. 곡을 선택후 실행하여 주세요", "RuruTunesAutoPutLyrix 오류");
                }
                else
                {
                    try
                    {
                        if (int.Parse(this.txtSize.Text) <= 0)
                        {
                            MessageBox.Show("앨범크기를 잘못 입력하셨습니다.", "RuruTunesAutoPutLyrix 오류");
                            return;
                        }
                    }
                    catch (FormatException exception)
                    {
                        MessageBox.Show("앨범크기를 잘못 입력하셨습니다. : " + exception.Message, "RuruTunesAutoPutLyrix 오류");
                        return;
                    }
                    new MassUpdater(selectedTracks) { iMain = this }.ShowDialog();
                }
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if ((this.lbox.SelectedIndex != -1) && (this.lbox.SelectedIndex != 0))
            {
                int selectedIndex = this.lbox.SelectedIndex;
                bool itemChecked = this.lbox.GetItemChecked(selectedIndex);
                this.lbox.Items.Insert(this.lbox.SelectedIndex - 1, this.lbox.SelectedItem);
                this.lbox.Items.RemoveAt(this.lbox.SelectedIndex);
                this.lbox.SelectedIndex = selectedIndex - 1;
                this.lbox.SetItemChecked(this.lbox.SelectedIndex, itemChecked);
                this.SaveSettings();
            }
        }

        private void btnUpC_Click(object sender, EventArgs e)
        {
            if ((this.cbox.SelectedIndex != -1) && (this.cbox.SelectedIndex != 0))
            {
                int selectedIndex = this.cbox.SelectedIndex;
                bool itemChecked = this.cbox.GetItemChecked(selectedIndex);
                this.cbox.Items.Insert(this.cbox.SelectedIndex - 1, this.cbox.SelectedItem);
                this.cbox.Items.RemoveAt(this.cbox.SelectedIndex);
                this.cbox.SelectedIndex = selectedIndex - 1;
                this.cbox.SetItemChecked(this.cbox.SelectedIndex, itemChecked);
                this.SaveSettings();
            }
        }

        private void chkAlbumCover_CheckedChanged(object sender, EventArgs e)
        {
            this.chkCoverOverwrite.Enabled = this.chkAlbumCover.Checked;
            this.chkCoverBigSize.Enabled = this.chkAlbumCover.Checked;
            this.chkCoverItunes.Enabled = this.chkAlbumCover.Checked;
            this.chkAlbumTitleOnly.Enabled = this.chkAlbumCover.Checked;
            this.grpCover.Enabled = this.chkAlbumCover.Checked;
            this.txtSize.Enabled = this.chkCoverBigSize.Checked && this.chkAlbumCover.Checked;
            this.SaveSettings();
        }

        private void chkCoverBigSize_CheckedChanged(object sender, EventArgs e)
        {
            this.txtSize.Enabled = this.chkCoverBigSize.Checked;
            this.SaveSettings();
        }

        private void chkLyric_CheckedChanged(object sender, EventArgs e)
        {
            this.chkOverwrite.Enabled = this.chkLyric.Checked;
            this.grpLyric.Enabled = this.chkLyric.Checked;
            this.chkSongName.Enabled = this.chkLyric.Checked;
            this.SaveSettings();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {
        }

        private void grpCover_Enter(object sender, EventArgs e)
        {
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkSongName = new System.Windows.Forms.CheckBox();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.chkAlbumTitleOnly = new System.Windows.Forms.CheckBox();
            this.chkLyric = new System.Windows.Forms.CheckBox();
            this.chkCoverItunes = new System.Windows.Forms.CheckBox();
            this.chkCoverOverwrite = new System.Windows.Forms.CheckBox();
            this.chkCoverBigSize = new System.Windows.Forms.CheckBox();
            this.chkAlbumCover = new System.Windows.Forms.CheckBox();
            this.chkOverwrite = new System.Windows.Forms.CheckBox();
            this.btnDown = new System.Windows.Forms.Button();
            this.grpLyric = new System.Windows.Forms.GroupBox();
            this.lbox = new System.Windows.Forms.CheckedListBox();
            this.btnUp = new System.Windows.Forms.Button();
            this.grpCover = new System.Windows.Forms.GroupBox();
            this.btnDownC = new System.Windows.Forms.Button();
            this.cbox = new System.Windows.Forms.CheckedListBox();
            this.btnUpC = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.btnStart = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.grpLyric.SuspendLayout();
            this.grpCover.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkSongName);
            this.groupBox1.Controls.Add(this.txtSize);
            this.groupBox1.Controls.Add(this.chkAlbumTitleOnly);
            this.groupBox1.Controls.Add(this.chkLyric);
            this.groupBox1.Controls.Add(this.chkCoverItunes);
            this.groupBox1.Controls.Add(this.chkCoverOverwrite);
            this.groupBox1.Controls.Add(this.chkCoverBigSize);
            this.groupBox1.Controls.Add(this.chkAlbumCover);
            this.groupBox1.Controls.Add(this.chkOverwrite);
            this.groupBox1.Location = new System.Drawing.Point(12, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(325, 206);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " 실행 옵션 ";
            // 
            // chkSongName
            // 
            this.chkSongName.AutoSize = true;
            this.chkSongName.Checked = true;
            this.chkSongName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSongName.Location = new System.Drawing.Point(29, 64);
            this.chkSongName.Name = "chkSongName";
            this.chkSongName.Size = new System.Drawing.Size(168, 16);
            this.chkSongName.TabIndex = 8;
            this.chkSongName.Text = "노래제목 가사 맨앞에 추가";
            this.chkSongName.UseVisualStyleBackColor = true;
            // 
            // txtSize
            // 
            this.txtSize.Enabled = false;
            this.txtSize.Location = new System.Drawing.Point(137, 150);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(33, 21);
            this.txtSize.TabIndex = 7;
            this.txtSize.Text = "500";
            this.txtSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // chkAlbumTitleOnly
            // 
            this.chkAlbumTitleOnly.AutoSize = true;
            this.chkAlbumTitleOnly.Enabled = false;
            this.chkAlbumTitleOnly.Location = new System.Drawing.Point(29, 174);
            this.chkAlbumTitleOnly.Name = "chkAlbumTitleOnly";
            this.chkAlbumTitleOnly.Size = new System.Drawing.Size(246, 16);
            this.chkAlbumTitleOnly.TabIndex = 6;
            this.chkAlbumTitleOnly.Text = "앨범 타이틀 만으로 검색 ( 사용에 주의! )";
            this.chkAlbumTitleOnly.UseVisualStyleBackColor = true;
            // 
            // chkLyric
            // 
            this.chkLyric.AutoSize = true;
            this.chkLyric.Checked = true;
            this.chkLyric.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLyric.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkLyric.Location = new System.Drawing.Point(6, 20);
            this.chkLyric.Name = "chkLyric";
            this.chkLyric.Size = new System.Drawing.Size(227, 16);
            this.chkLyric.TabIndex = 5;
            this.chkLyric.Text = "Lyrics - 노래 가사를 가져옵니다.";
            this.chkLyric.UseVisualStyleBackColor = true;
            this.chkLyric.CheckedChanged += new System.EventHandler(this.chkLyric_CheckedChanged);
            // 
            // chkCoverItunes
            // 
            this.chkCoverItunes.AutoSize = true;
            this.chkCoverItunes.Checked = true;
            this.chkCoverItunes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCoverItunes.Enabled = false;
            this.chkCoverItunes.Location = new System.Drawing.Point(53, 130);
            this.chkCoverItunes.Name = "chkCoverItunes";
            this.chkCoverItunes.Size = new System.Drawing.Size(227, 16);
            this.chkCoverItunes.TabIndex = 4;
            this.chkCoverItunes.Text = "iTunes 에서 받은 커버는 제외합니다.";
            this.chkCoverItunes.UseVisualStyleBackColor = true;
            // 
            // chkCoverOverwrite
            // 
            this.chkCoverOverwrite.AutoSize = true;
            this.chkCoverOverwrite.Checked = true;
            this.chkCoverOverwrite.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCoverOverwrite.Enabled = false;
            this.chkCoverOverwrite.Location = new System.Drawing.Point(29, 108);
            this.chkCoverOverwrite.Name = "chkCoverOverwrite";
            this.chkCoverOverwrite.Size = new System.Drawing.Size(259, 16);
            this.chkCoverOverwrite.TabIndex = 3;
            this.chkCoverOverwrite.Text = "Overwrite - 앨범커버가 있어도 덮어씁니다.";
            this.chkCoverOverwrite.UseVisualStyleBackColor = true;
            // 
            // chkCoverBigSize
            // 
            this.chkCoverBigSize.AutoSize = true;
            this.chkCoverBigSize.Checked = true;
            this.chkCoverBigSize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCoverBigSize.Enabled = false;
            this.chkCoverBigSize.Location = new System.Drawing.Point(29, 152);
            this.chkCoverBigSize.Name = "chkCoverBigSize";
            this.chkCoverBigSize.Size = new System.Drawing.Size(287, 16);
            this.chkCoverBigSize.TabIndex = 2;
            this.chkCoverBigSize.Text = "Size Big Only -           이상 크기만 가져옵니다.";
            this.chkCoverBigSize.UseVisualStyleBackColor = true;
            this.chkCoverBigSize.CheckedChanged += new System.EventHandler(this.chkCoverBigSize_CheckedChanged);
            // 
            // chkAlbumCover
            // 
            this.chkAlbumCover.AutoSize = true;
            this.chkAlbumCover.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkAlbumCover.Location = new System.Drawing.Point(6, 86);
            this.chkAlbumCover.Name = "chkAlbumCover";
            this.chkAlbumCover.Size = new System.Drawing.Size(270, 16);
            this.chkAlbumCover.TabIndex = 1;
            this.chkAlbumCover.Text = "Album Cover - 앨범 커버를 가져옵니다.";
            this.chkAlbumCover.UseVisualStyleBackColor = true;
            this.chkAlbumCover.CheckedChanged += new System.EventHandler(this.chkAlbumCover_CheckedChanged);
            // 
            // chkOverwrite
            // 
            this.chkOverwrite.AutoSize = true;
            this.chkOverwrite.Checked = true;
            this.chkOverwrite.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOverwrite.Location = new System.Drawing.Point(29, 42);
            this.chkOverwrite.Name = "chkOverwrite";
            this.chkOverwrite.Size = new System.Drawing.Size(235, 16);
            this.chkOverwrite.TabIndex = 0;
            this.chkOverwrite.Text = "Overwrite - 가사가 있어도 덮어씁니다.";
            this.chkOverwrite.UseVisualStyleBackColor = true;
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(286, 68);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(30, 39);
            this.btnDown.TabIndex = 1;
            this.btnDown.Text = "▼";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // grpLyric
            // 
            this.grpLyric.Controls.Add(this.btnDown);
            this.grpLyric.Controls.Add(this.lbox);
            this.grpLyric.Controls.Add(this.btnUp);
            this.grpLyric.Location = new System.Drawing.Point(12, 220);
            this.grpLyric.Name = "grpLyric";
            this.grpLyric.Size = new System.Drawing.Size(325, 178);
            this.grpLyric.TabIndex = 8;
            this.grpLyric.TabStop = false;
            this.grpLyric.Text = " 가사 검색엔진 사용 ";
            // 
            // lbox
            // 
            this.lbox.AllowDrop = true;
            this.lbox.CheckOnClick = true;
            this.lbox.FormattingEnabled = true;
            this.lbox.Items.AddRange(new object[] {
            "가요 검색 - http://gasazip.com",
            "가요 검색 - http://im.new21.org",
            "가요 검색 - http://inmuz.com",
            "Jpop 검색 - http://utamap.com",
            "CPop 검색 - http://sing8.com",
            "CCM 검색 - http://ccmpia.com",
            "Jpop 검색 - http://jieumai.com(지음아이)"});
            this.lbox.Location = new System.Drawing.Point(6, 21);
            this.lbox.Name = "lbox";
            this.lbox.Size = new System.Drawing.Size(274, 148);
            this.lbox.TabIndex = 0;
            this.lbox.ThreeDCheckBoxes = true;
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(286, 21);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(30, 41);
            this.btnUp.TabIndex = 1;
            this.btnUp.Text = "▲";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // grpCover
            // 
            this.grpCover.Controls.Add(this.btnDownC);
            this.grpCover.Controls.Add(this.cbox);
            this.grpCover.Controls.Add(this.btnUpC);
            this.grpCover.Enabled = false;
            this.grpCover.Location = new System.Drawing.Point(12, 414);
            this.grpCover.Name = "grpCover";
            this.grpCover.Size = new System.Drawing.Size(325, 111);
            this.grpCover.TabIndex = 9;
            this.grpCover.TabStop = false;
            this.grpCover.Text = " 앨범커버 검색엔진 사용 ";
            this.grpCover.Enter += new System.EventHandler(this.grpCover_Enter);
            // 
            // btnDownC
            // 
            this.btnDownC.Location = new System.Drawing.Point(286, 68);
            this.btnDownC.Name = "btnDownC";
            this.btnDownC.Size = new System.Drawing.Size(30, 39);
            this.btnDownC.TabIndex = 1;
            this.btnDownC.Text = "▼";
            this.btnDownC.UseVisualStyleBackColor = true;
            this.btnDownC.Click += new System.EventHandler(this.btnDownC_Click);
            // 
            // cbox
            // 
            this.cbox.AllowDrop = true;
            this.cbox.CheckOnClick = true;
            this.cbox.FormattingEnabled = true;
            this.cbox.Items.AddRange(new object[] {
            "매니아DB - http://maniadb.com",
            "All CD Covers - http://allcdcovers.com",
            "Coverholic - http://coverholic.com",
            "즐즐넷 - http://cover.zzlzzl.net",
            "CCMPia - http://ccmpia.com"});
            this.cbox.Location = new System.Drawing.Point(6, 21);
            this.cbox.Name = "cbox";
            this.cbox.Size = new System.Drawing.Size(274, 84);
            this.cbox.TabIndex = 0;
            this.cbox.ThreeDCheckBoxes = true;
            // 
            // btnUpC
            // 
            this.btnUpC.Location = new System.Drawing.Point(286, 21);
            this.btnUpC.Name = "btnUpC";
            this.btnUpC.Size = new System.Drawing.Size(30, 41);
            this.btnUpC.TabIndex = 1;
            this.btnUpC.Text = "▲";
            this.btnUpC.UseVisualStyleBackColor = true;
            this.btnUpC.Click += new System.EventHandler(this.btnUpC_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.Location = new System.Drawing.Point(345, 509);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(122, 13);
            this.linkLabel1.TabIndex = 10;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "http://i-ruru.com";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(348, 18);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(138, 70);
            this.btnStart.TabIndex = 11;
            this.btnStart.Text = "작업 시작";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(421, 495);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "sunyruru";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(347, 495);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "Programmed by";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.Location = new System.Drawing.Point(345, 469);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(141, 11);
            this.lblVersion.TabIndex = 14;
            this.lblVersion.Text = "RuruTunesLyrix 1.2.8 - 20090910";
            this.lblVersion.Click += new System.EventHandler(this.lblVersion_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(348, 482);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(137, 10);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // RuruTunesAutoPutLyrix
            // 
            this.ClientSize = new System.Drawing.Size(496, 537);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.grpCover);
            this.Controls.Add(this.grpLyric);
            this.Controls.Add(this.groupBox1);
            this.Name = "RuruTunesAutoPutLyrix";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RuruTunesLyrix 1.2 - iTunes Lyrics / Album Cover Importer ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RuruTunesAutoPutLyrix_FormClosing);
            this.Load += new System.EventHandler(this.LyricGuru_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpLyric.ResumeLayout(false);
            this.grpCover.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void lblVersion_Click(object sender, EventArgs e)
        {
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://i-ruru.com");
        }

        private void LoadSettings()
        {
            Savior.Read(this.settings, @"Software\RuruTunesAutoPutLyrix\Settings");
            this.lbox.Items.Clear();
            foreach (string str in this.settings.lstLyrics)
            {
                int index = this.lbox.Items.Add(str);
                this.lbox.SetItemChecked(index, this.settings.chkLyrics[index]);
            }
            this.cbox.Items.Clear();
            foreach (string str2 in this.settings.lstCovers)
            {
                int num2 = this.cbox.Items.Add(str2);
                this.cbox.SetItemChecked(num2, this.settings.chkCovers[num2]);
            }
            this.chkLyric.Checked = this.settings.chkLyric;
            this.chkOverwrite.Checked = this.settings.chkLyricOverwrite;
            this.chkSongName.Checked = this.settings.chkSongName;
            this.chkAlbumCover.Checked = this.settings.chkCover;
            this.chkCoverOverwrite.Checked = this.settings.chkCoverOverwrite;
            this.chkCoverItunes.Checked = this.settings.chkCoveriTunes;
            this.chkCoverBigSize.Checked = this.settings.chkCoverBigSize;
            this.chkAlbumTitleOnly.Checked = this.settings.chkAlbumTitleOnly;
            this.txtSize.Text = this.settings.txtCoverSize;
        }

        private void LyricGuru_Load(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(Application.StartupPath + @"\RuruTunesAutoPutLyrix_log.txt"))
                {
                    File.Delete(Application.StartupPath + @"\RuruTunesAutoPutLyrix_log.txt");
                }
            }
            catch (Exception)
            {
            }
            try
            {
                AppDomain.CurrentDomain.UnhandledException += delegate (object sender2, UnhandledExceptionEventArgs e2) {
                    if (e2.IsTerminating)
                    {
                        object exceptionObject = e2.ExceptionObject;
                        StreamWriter writer = new StreamWriter(Application.StartupPath + @"\RuruTunesAutoPutLyrix_log.txt", true);
                        writer.Write(exceptionObject.ToString());
                        writer.Close();
                    }
                };
            }
            catch (Exception)
            {
            }
            try
            {
                this.LoadSettings();
            }
            catch (Exception exception)
            {
                StreamWriter writer = new StreamWriter(Application.StartupPath + @"\RuruTunesAutoPutLyrix_log.txt", true);
                writer.Write(exception.ToString());
                writer.Close();
            }
            try
            {
                AutoUpdate update = new AutoUpdate(this);
                ThreadStart start = new ThreadStart(update.UpdateMe);
                new Thread(start).Start();
            }
            catch (Exception exception2)
            {
                StreamWriter writer2 = new StreamWriter(Application.StartupPath + @"\RuruTunesAutoPutLyrix_log.txt", true);
                writer2.Write(exception2.ToString());
                writer2.Close();
            }
            try
            {
                if (Directory.Exists(Path.GetTempPath() + @"\lyricguru"))
                {
                    Directory.Delete(Path.GetTempPath() + @"\lyricguru", true);
                }
            }
            catch (Exception)
            {
            }
        }

        private void SaveSettings()
        {
            this.settings.chkLyric = this.chkLyric.Checked;
            this.settings.chkLyricOverwrite = this.chkOverwrite.Checked;
            this.settings.chkSongName = this.chkSongName.Checked;
            this.settings.chkCover = this.chkAlbumCover.Checked;
            this.settings.chkCoverOverwrite = this.chkCoverOverwrite.Checked;
            this.settings.chkCoveriTunes = this.chkCoverItunes.Checked;
            this.settings.chkCoverBigSize = this.chkCoverBigSize.Checked;
            this.settings.chkAlbumTitleOnly = this.chkAlbumTitleOnly.Checked;
            this.settings.txtCoverSize = this.txtSize.Text;
            for (int i = 0; i < this.lbox.Items.Count; i++)
            {
                this.settings.lstLyrics.SetValue(this.lbox.Items[i].ToString(), i);
                this.settings.chkLyrics.SetValue(this.lbox.GetItemChecked(i), i);
            }
            for (int j = 0; j < this.cbox.Items.Count; j++)
            {
                this.settings.lstCovers.SetValue(this.cbox.Items[j].ToString(), j);
                this.settings.chkCovers.SetValue(this.cbox.GetItemChecked(j), j);
            }
            Savior.Save(this.settings, @"Software\RuruTunesAutoPutLyrix\Settings");
        }

        private void RuruTunesAutoPutLyrix_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SaveSettings();
        }
    }
}

