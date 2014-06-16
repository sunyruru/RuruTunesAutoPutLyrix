namespace RuruTunesAutoPutLyrix
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(RuruTunesAutoPutLyrix));
            this.groupBox1 = new GroupBox();
            this.chkSongName = new CheckBox();
            this.txtSize = new TextBox();
            this.chkAlbumTitleOnly = new CheckBox();
            this.chkLyric = new CheckBox();
            this.chkCoverItunes = new CheckBox();
            this.chkCoverOverwrite = new CheckBox();
            this.chkCoverBigSize = new CheckBox();
            this.chkAlbumCover = new CheckBox();
            this.chkOverwrite = new CheckBox();
            this.btnDown = new Button();
            this.grpLyric = new GroupBox();
            this.lbox = new CheckedListBox();
            this.btnUp = new Button();
            this.grpCover = new GroupBox();
            this.btnDownC = new Button();
            this.cbox = new CheckedListBox();
            this.btnUpC = new Button();
            this.linkLabel1 = new LinkLabel();
            this.btnStart = new Button();
            this.label2 = new Label();
            this.label1 = new Label();
            this.lblVersion = new Label();
            this.groupBox2 = new GroupBox();
            this.groupBox1.SuspendLayout();
            this.grpLyric.SuspendLayout();
            this.grpCover.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.chkSongName);
            this.groupBox1.Controls.Add(this.txtSize);
            this.groupBox1.Controls.Add(this.chkAlbumTitleOnly);
            this.groupBox1.Controls.Add(this.chkLyric);
            this.groupBox1.Controls.Add(this.chkCoverItunes);
            this.groupBox1.Controls.Add(this.chkCoverOverwrite);
            this.groupBox1.Controls.Add(this.chkCoverBigSize);
            this.groupBox1.Controls.Add(this.chkAlbumCover);
            this.groupBox1.Controls.Add(this.chkOverwrite);
            this.groupBox1.Location = new Point(12, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x145, 0xce);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " 실행 옵션 ";
            this.chkSongName.AutoSize = true;
            this.chkSongName.Checked = true;
            this.chkSongName.CheckState = CheckState.Checked;
            this.chkSongName.Location = new Point(0x1d, 0x40);
            this.chkSongName.Name = "chkSongName";
            this.chkSongName.Size = new Size(0xa8, 0x10);
            this.chkSongName.TabIndex = 8;
            this.chkSongName.Text = "노래제목 가사 맨앞에 추가";
            this.chkSongName.UseVisualStyleBackColor = true;
            this.txtSize.Enabled = false;
            this.txtSize.Location = new Point(0x89, 150);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new Size(0x21, 0x15);
            this.txtSize.TabIndex = 7;
            this.txtSize.Text = "500";
            this.txtSize.TextAlign = HorizontalAlignment.Center;
            this.chkAlbumTitleOnly.AutoSize = true;
            this.chkAlbumTitleOnly.Enabled = false;
            this.chkAlbumTitleOnly.Location = new Point(0x1d, 0xae);
            this.chkAlbumTitleOnly.Name = "chkAlbumTitleOnly";
            this.chkAlbumTitleOnly.Size = new Size(0xf6, 0x10);
            this.chkAlbumTitleOnly.TabIndex = 6;
            this.chkAlbumTitleOnly.Text = "앨범 타이틀 만으로 검색 ( 사용에 주의! )";
            this.chkAlbumTitleOnly.UseVisualStyleBackColor = true;
            this.chkLyric.AutoSize = true;
            this.chkLyric.Checked = true;
            this.chkLyric.CheckState = CheckState.Checked;
            this.chkLyric.Font = new Font("Gulim", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x81);
            this.chkLyric.Location = new Point(6, 20);
            this.chkLyric.Name = "chkLyric";
            this.chkLyric.Size = new Size(0xe3, 0x10);
            this.chkLyric.TabIndex = 5;
            this.chkLyric.Text = "Lyrics - 노래 가사를 가져옵니다.";
            this.chkLyric.UseVisualStyleBackColor = true;
            this.chkLyric.CheckedChanged += new EventHandler(this.chkLyric_CheckedChanged);
            this.chkCoverItunes.AutoSize = true;
            this.chkCoverItunes.Checked = true;
            this.chkCoverItunes.CheckState = CheckState.Checked;
            this.chkCoverItunes.Enabled = false;
            this.chkCoverItunes.Location = new Point(0x35, 130);
            this.chkCoverItunes.Name = "chkCoverItunes";
            this.chkCoverItunes.Size = new Size(0xe3, 0x10);
            this.chkCoverItunes.TabIndex = 4;
            this.chkCoverItunes.Text = "iTunes 에서 받은 커버는 제외합니다.";
            this.chkCoverItunes.UseVisualStyleBackColor = true;
            this.chkCoverOverwrite.AutoSize = true;
            this.chkCoverOverwrite.Checked = true;
            this.chkCoverOverwrite.CheckState = CheckState.Checked;
            this.chkCoverOverwrite.Enabled = false;
            this.chkCoverOverwrite.Location = new Point(0x1d, 0x6c);
            this.chkCoverOverwrite.Name = "chkCoverOverwrite";
            this.chkCoverOverwrite.Size = new Size(0x103, 0x10);
            this.chkCoverOverwrite.TabIndex = 3;
            this.chkCoverOverwrite.Text = "Overwrite - 앨범커버가 있어도 덮어씁니다.";
            this.chkCoverOverwrite.UseVisualStyleBackColor = true;
            this.chkCoverBigSize.AutoSize = true;
            this.chkCoverBigSize.Checked = true;
            this.chkCoverBigSize.CheckState = CheckState.Checked;
            this.chkCoverBigSize.Enabled = false;
            this.chkCoverBigSize.Location = new Point(0x1d, 0x98);
            this.chkCoverBigSize.Name = "chkCoverBigSize";
            this.chkCoverBigSize.Size = new Size(0x11f, 0x10);
            this.chkCoverBigSize.TabIndex = 2;
            this.chkCoverBigSize.Text = "Size Big Only -           이상 크기만 가져옵니다.";
            this.chkCoverBigSize.UseVisualStyleBackColor = true;
            this.chkCoverBigSize.CheckedChanged += new EventHandler(this.chkCoverBigSize_CheckedChanged);
            this.chkAlbumCover.AutoSize = true;
            this.chkAlbumCover.Font = new Font("Gulim", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x81);
            this.chkAlbumCover.Location = new Point(6, 0x56);
            this.chkAlbumCover.Name = "chkAlbumCover";
            this.chkAlbumCover.Size = new Size(270, 0x10);
            this.chkAlbumCover.TabIndex = 1;
            this.chkAlbumCover.Text = "Album Cover - 앨범 커버를 가져옵니다.";
            this.chkAlbumCover.UseVisualStyleBackColor = true;
            this.chkAlbumCover.CheckedChanged += new EventHandler(this.chkAlbumCover_CheckedChanged);
            this.chkOverwrite.AutoSize = true;
            this.chkOverwrite.Checked = true;
            this.chkOverwrite.CheckState = CheckState.Checked;
            this.chkOverwrite.Location = new Point(0x1d, 0x2a);
            this.chkOverwrite.Name = "chkOverwrite";
            this.chkOverwrite.Size = new Size(0xeb, 0x10);
            this.chkOverwrite.TabIndex = 0;
            this.chkOverwrite.Text = "Overwrite - 가사가 있어도 덮어씁니다.";
            this.chkOverwrite.UseVisualStyleBackColor = true;
            this.btnDown.Location = new Point(0x11e, 0x44);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new Size(30, 0x27);
            this.btnDown.TabIndex = 1;
            this.btnDown.Text = "▼";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new EventHandler(this.btnDown_Click);
            this.grpLyric.Controls.Add(this.btnDown);
            this.grpLyric.Controls.Add(this.lbox);
            this.grpLyric.Controls.Add(this.btnUp);
            this.grpLyric.Location = new Point(12, 220);
            this.grpLyric.Name = "grpLyric";
            this.grpLyric.Size = new Size(0x145, 0xb2);
            this.grpLyric.TabIndex = 8;
            this.grpLyric.TabStop = false;
            this.grpLyric.Text = " 가사 검색엔진 사용 ";
            this.lbox.AllowDrop = true;
            this.lbox.CheckOnClick = true;
            this.lbox.FormattingEnabled = true;
            this.lbox.Items.AddRange(new object[] { "가요 검색 - http://gasazip.com", "가요 검색 - http://im.new21.org", "가요 검색 - http://inmuz.com", "Jpop 검색 - http://utamap.com", "CPop 검색 - http://sing8.com", "CCM 검색 - http://ccmpia.com", "Jpop 검색 - http://jieumai.com(지음아이)" });
            this.lbox.Location = new Point(6, 0x15);
            this.lbox.Name = "lbox";
            this.lbox.Size = new Size(0x112, 0x94);
            this.lbox.TabIndex = 0;
            this.lbox.ThreeDCheckBoxes = true;
            this.btnUp.Location = new Point(0x11e, 0x15);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new Size(30, 0x29);
            this.btnUp.TabIndex = 1;
            this.btnUp.Text = "▲";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new EventHandler(this.btnUp_Click);
            this.grpCover.Controls.Add(this.btnDownC);
            this.grpCover.Controls.Add(this.cbox);
            this.grpCover.Controls.Add(this.btnUpC);
            this.grpCover.Enabled = false;
            this.grpCover.Location = new Point(12, 0x19e);
            this.grpCover.Name = "grpCover";
            this.grpCover.Size = new Size(0x145, 0x6f);
            this.grpCover.TabIndex = 9;
            this.grpCover.TabStop = false;
            this.grpCover.Text = " 앨범커버 검색엔진 사용 ";
            this.grpCover.Enter += new EventHandler(this.grpCover_Enter);
            this.btnDownC.Location = new Point(0x11e, 0x44);
            this.btnDownC.Name = "btnDownC";
            this.btnDownC.Size = new Size(30, 0x27);
            this.btnDownC.TabIndex = 1;
            this.btnDownC.Text = "▼";
            this.btnDownC.UseVisualStyleBackColor = true;
            this.btnDownC.Click += new EventHandler(this.btnDownC_Click);
            this.cbox.AllowDrop = true;
            this.cbox.CheckOnClick = true;
            this.cbox.FormattingEnabled = true;
            this.cbox.Items.AddRange(new object[] { "매니아DB - http://maniadb.com", "All CD Covers - http://allcdcovers.com", "Coverholic - http://coverholic.com", "즐즐넷 - http://cover.zzlzzl.net", "CCMPia - http://ccmpia.com" });
            this.cbox.Location = new Point(6, 0x15);
            this.cbox.Name = "cbox";
            this.cbox.Size = new Size(0x112, 0x54);
            this.cbox.TabIndex = 0;
            this.cbox.ThreeDCheckBoxes = true;
            this.btnUpC.Location = new Point(0x11e, 0x15);
            this.btnUpC.Name = "btnUpC";
            this.btnUpC.Size = new Size(30, 0x29);
            this.btnUpC.TabIndex = 1;
            this.btnUpC.Text = "▲";
            this.btnUpC.UseVisualStyleBackColor = true;
            this.btnUpC.Click += new EventHandler(this.btnUpC_Click);
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.linkLabel1.Location = new Point(0x159, 0x1fd);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new Size(0x74, 13);
            this.linkLabel1.TabIndex = 10;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "http://xguru.net";
            this.linkLabel1.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            this.btnStart.Location = new Point(0x15c, 0x12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new Size(0x6c, 0x30);
            this.btnStart.TabIndex = 11;
            this.btnStart.Text = "작업 시작";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new EventHandler(this.btnStart_Click);
            this.label2.AutoSize = true;
            this.label2.Font = new Font("Gulim", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x81);
            this.label2.Location = new Point(0x1a5, 0x1ef);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x1f, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "구루";
            this.label2.Click += new EventHandler(this.label2_Click);
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Arial", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(0x15b, 0x1ef);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x48, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "Programmed by";
            this.label1.Click += new EventHandler(this.label1_Click);
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new Font("Tahoma", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblVersion.Location = new Point(0x159, 0x1d5);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new Size(0x72, 11);
            this.lblVersion.TabIndex = 14;
            this.lblVersion.Text = "RuruTunesAutoPutLyrix 1.2.8 - 20090910";
            this.lblVersion.Click += new EventHandler(this.lblVersion_Click);
            this.groupBox2.Location = new Point(0x15c, 0x1e2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(120, 10);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Enter += new EventHandler(this.groupBox2_Enter);
            base.AutoScaleDimensions = new SizeF(7f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x1d2, 0x219);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.lblVersion);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.btnStart);
            base.Controls.Add(this.linkLabel1);
            base.Controls.Add(this.grpCover);
            base.Controls.Add(this.grpLyric);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "RuruTunesAutoPutLyrix";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "RuruTunesAutoPutLyrix 1.2 - iTunes Lyrics / Album Cover Importer ";
            base.Load += new EventHandler(this.LyricGuru_Load);
            base.FormClosing += new FormClosingEventHandler(this.RuruTunesAutoPutLyrix_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpLyric.ResumeLayout(false);
            this.grpCover.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
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
            Process.Start("http://xguru.net");
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

