namespace TagGuru
{
    using iTunesLib;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows.Forms;

    public class MassUpdater : Form
    {
        private DataGridViewTextBoxColumn Artist;
        private IContainer components = null;
        private DataGridViewImageColumn Cover;
        private DataGridViewTextBoxColumn coversize;
        private DataGridView grid;
        private GroupBox groupBox1;
        public TagGuru iMain;
        private PictureBox img;
        private PictureBox imgFloat;
        private Label label1;
        private Label label2;
        private Label label4;
        private Label lblAlbum;
        private Label lblArtist;
        private Label lblSize;
        private Label lblSong;
        private Label lblStatus;
        private LyricsUpdater lu;
        public DelegateAddRow m_DelegateAddRow;
        public DelegateUpdateRow m_DelegateUpdateRow;
        private IITTrackCollection m_selectedTracks;
        public SetCoverColumn m_SetCoverColumn;
        public SetProgress m_SetProgress;
        public SetSplitSize m_SetSplitSize;
        public SetStatus m_SetStatus;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private Panel panel5;
        private ProgressBar pgb;
        private DataGridViewTextBoxColumn Result;
        public SplitContainer splitContainer1;
        private DataGridViewTextBoxColumn Track;
        private TextBox txtLyric;

        public MassUpdater(IITTrackCollection selectedTracks)
        {
            this.m_selectedTracks = selectedTracks;
            this.InitializeComponent();
            this.m_DelegateAddRow = new DelegateAddRow(this.addRow);
            this.m_DelegateUpdateRow = new DelegateUpdateRow(this.updateRow);
            this.m_SetSplitSize = new SetSplitSize(this.SetSplitSize);
            this.m_SetStatus = new SetStatus(this.SetStatus);
            this.m_SetProgress = new SetProgress(this.SetProgress);
            this.m_SetCoverColumn = new SetCoverColumn(this.SetCoverColumn);
        }

        private int addRow(object[] row)
        {
            int num = this.grid.Rows.Add(row);
            this.grid.Rows[num].Height = 20;
            return num;
        }

        public bool CheckMultiArtist(string artist, string info)
        {
            string[] separator = new string[] { ",", "&", "와 ", "과 ", " and ", "/" };
            foreach (string str in artist.Split(separator, StringSplitOptions.RemoveEmptyEntries))
            {
                string str2 = str.Trim().ToUpper();
                string str3 = info.ToUpper();
                if (((str3.Contains(str2) || str3.Contains(str2.Replace(" ", ""))) || (str3.ToUpper().Contains(str2.ToUpper()) || str3.ToUpper().Contains(str2.ToUpper().Replace(" ", "")))) || (str3.Replace(" ", "").Contains(str2) || str3.Replace(" ", "").Contains(str2.Replace(" ", ""))))
                {
                    return true;
                }
            }
            string str4 = "";
            if (artist.ToUpper() == "BEYONCE")
            {
                str4 = "BEYONC\x00e9";
                if ((info.Contains(str4) || info.Contains(str4.Replace(" ", ""))) || (info.ToUpper().Contains(str4.ToUpper()) || info.ToUpper().Contains(str4.ToUpper().Replace(" ", ""))))
                {
                    return true;
                }
            }
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                IITFileOrCDTrack track = (IITFileOrCDTrack) this.m_selectedTracks[e.RowIndex + 1];
                this.lblArtist.Text = track.Artist;
                this.lblAlbum.Text = track.Album;
                this.lblSong.Text = track.Name;
                if (track.Artwork[1] != null)
                {
                    Directory.CreateDirectory(Path.GetTempPath() + @"\lyricguru\");
                    string path = Regex.Replace(track.Artist + "_" + track.Name, "[?:\\/*\"<>|]", "");
                    path = Path.GetTempPath() + @"\lyricguru\" + path;
                    if (File.Exists(path))
                    {
                        try
                        {
                            File.Delete(path);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    if (!File.Exists(path))
                    {
                        track.Artwork[1].SaveArtworkToFile(path);
                    }
                    if (this.img.Image != null)
                    {
                        this.img.Image.Dispose();
                    }
                    Bitmap bitmap = new Bitmap(path);
                    this.img.Image = bitmap;
                    this.lblSize.Text = bitmap.Width + " X " + bitmap.Height;
                }
                else
                {
                    if (this.img.Image != null)
                    {
                        this.img.Image.Dispose();
                    }
                    this.img.Image = null;
                    this.lblSize.Text = "커버 이미지 없음";
                }
                this.txtLyric.Text = track.Lyrics;
            }
            catch (Exception)
            {
            }
        }

        private void img_MouseEnter(object sender, EventArgs e)
        {
            if (this.img.Image != null)
            {
                if (this.img.Image.Width > 600)
                {
                    this.imgFloat.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    this.imgFloat.SizeMode = PictureBoxSizeMode.CenterImage;
                }
                this.imgFloat.Image = this.img.Image;
                this.imgFloat.Visible = true;
            }
        }

        private void img_MouseLeave(object sender, EventArgs e)
        {
            this.imgFloat.Visible = false;
            this.imgFloat.Image = null;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(MassUpdater));
            this.splitContainer1 = new SplitContainer();
            this.grid = new DataGridView();
            this.panel3 = new Panel();
            this.img = new PictureBox();
            this.lblSize = new Label();
            this.panel1 = new Panel();
            this.panel2 = new Panel();
            this.txtLyric = new TextBox();
            this.panel4 = new Panel();
            this.lblSong = new Label();
            this.label4 = new Label();
            this.lblAlbum = new Label();
            this.lblArtist = new Label();
            this.label2 = new Label();
            this.label1 = new Label();
            this.imgFloat = new PictureBox();
            this.panel5 = new Panel();
            this.pgb = new ProgressBar();
            this.groupBox1 = new GroupBox();
            this.lblStatus = new Label();
            this.Track = new DataGridViewTextBoxColumn();
            this.Artist = new DataGridViewTextBoxColumn();
            this.Cover = new DataGridViewImageColumn();
            this.coversize = new DataGridViewTextBoxColumn();
            this.Result = new DataGridViewTextBoxColumn();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((ISupportInitialize) this.grid).BeginInit();
            this.panel3.SuspendLayout();
            ((ISupportInitialize) this.img).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            ((ISupportInitialize) this.imgFloat).BeginInit();
            this.panel5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.splitContainer1.Dock = DockStyle.Fill;
            this.splitContainer1.Location = new Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = Orientation.Horizontal;
            this.splitContainer1.Panel1.Controls.Add(this.grid);
            this.splitContainer1.Panel2.Controls.Add(this.panel3);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new Size(0x30d, 0x2b8);
            this.splitContainer1.SplitterDistance = 0x20e;
            this.splitContainer1.TabIndex = 2;
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Columns.AddRange(new DataGridViewColumn[] { this.Track, this.Artist, this.Cover, this.coversize, this.Result });
            this.grid.Dock = DockStyle.Fill;
            this.grid.Location = new Point(0, 0);
            this.grid.Name = "grid";
            this.grid.RowTemplate.Height = 0x17;
            this.grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.grid.Size = new Size(0x30d, 0x20e);
            this.grid.TabIndex = 1;
            this.grid.CellClick += new DataGridViewCellEventHandler(this.grid_CellClick);
            this.panel3.Controls.Add(this.img);
            this.panel3.Controls.Add(this.lblSize);
            this.panel3.Dock = DockStyle.Left;
            this.panel3.Location = new Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(0x98, 0xa6);
            this.panel3.TabIndex = 0x10;
            this.img.Dock = DockStyle.Fill;
            this.img.Location = new Point(0, 20);
            this.img.Name = "img";
            this.img.Size = new Size(0x98, 0x92);
            this.img.SizeMode = PictureBoxSizeMode.Zoom;
            this.img.TabIndex = 0x10;
            this.img.TabStop = false;
            this.img.MouseLeave += new EventHandler(this.img_MouseLeave);
            this.img.MouseEnter += new EventHandler(this.img_MouseEnter);
            this.lblSize.BorderStyle = BorderStyle.Fixed3D;
            this.lblSize.Dock = DockStyle.Top;
            this.lblSize.Font = new Font("Verdana", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblSize.ForeColor = Color.Blue;
            this.lblSize.Location = new Point(0, 0);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new Size(0x98, 20);
            this.lblSize.TabIndex = 15;
            this.lblSize.TextAlign = ContentAlignment.MiddleCenter;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x30d, 0xa6);
            this.panel1.TabIndex = 2;
            this.panel2.Controls.Add(this.txtLyric);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = DockStyle.Right;
            this.panel2.Location = new Point(0x98, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0x275, 0xa6);
            this.panel2.TabIndex = 0;
            this.txtLyric.BackColor = SystemColors.Control;
            this.txtLyric.Dock = DockStyle.Fill;
            this.txtLyric.Location = new Point(0, 0x25);
            this.txtLyric.Multiline = true;
            this.txtLyric.Name = "txtLyric";
            this.txtLyric.ScrollBars = ScrollBars.Vertical;
            this.txtLyric.Size = new Size(0x275, 0x81);
            this.txtLyric.TabIndex = 13;
            this.panel4.Controls.Add(this.lblSong);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.lblAlbum);
            this.panel4.Controls.Add(this.lblArtist);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = DockStyle.Top;
            this.panel4.Location = new Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new Size(0x275, 0x25);
            this.panel4.TabIndex = 12;
            this.lblSong.AutoEllipsis = true;
            this.lblSong.BorderStyle = BorderStyle.FixedSingle;
            this.lblSong.Font = new Font("Gulim", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x81);
            this.lblSong.Location = new Point(420, 8);
            this.lblSong.Name = "lblSong";
            this.lblSong.Size = new Size(0xc0, 20);
            this.lblSong.TabIndex = 13;
            this.lblSong.TextAlign = ContentAlignment.MiddleCenter;
            this.label4.BorderStyle = BorderStyle.Fixed3D;
            this.label4.Font = new Font("Gulim", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x81);
            this.label4.Location = new Point(0x16d, 8);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x2f, 20);
            this.label4.TabIndex = 12;
            this.label4.Text = "곡목";
            this.label4.TextAlign = ContentAlignment.MiddleCenter;
            this.lblAlbum.AutoEllipsis = true;
            this.lblAlbum.BorderStyle = BorderStyle.FixedSingle;
            this.lblAlbum.Font = new Font("Gulim", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x81);
            this.lblAlbum.Location = new Point(230, 8);
            this.lblAlbum.Name = "lblAlbum";
            this.lblAlbum.Size = new Size(0x81, 20);
            this.lblAlbum.TabIndex = 11;
            this.lblAlbum.TextAlign = ContentAlignment.MiddleCenter;
            this.lblArtist.AutoEllipsis = true;
            this.lblArtist.BorderStyle = BorderStyle.FixedSingle;
            this.lblArtist.Font = new Font("Gulim", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x81);
            this.lblArtist.Location = new Point(0x3d, 8);
            this.lblArtist.Name = "lblArtist";
            this.lblArtist.Size = new Size(0x6a, 20);
            this.lblArtist.TabIndex = 10;
            this.lblArtist.TextAlign = ContentAlignment.MiddleCenter;
            this.label2.BorderStyle = BorderStyle.Fixed3D;
            this.label2.Font = new Font("Gulim", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x81);
            this.label2.Location = new Point(0xaf, 8);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x2f, 20);
            this.label2.TabIndex = 9;
            this.label2.Text = "앨범";
            this.label2.TextAlign = ContentAlignment.MiddleCenter;
            this.label1.BorderStyle = BorderStyle.Fixed3D;
            this.label1.Font = new Font("Gulim", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x81);
            this.label1.Location = new Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x2f, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "가수";
            this.label1.TextAlign = ContentAlignment.MiddleCenter;
            this.imgFloat.BackColor = Color.White;
            this.imgFloat.Location = new Point(0xa3, 0x20);
            this.imgFloat.Name = "imgFloat";
            this.imgFloat.Size = new Size(600, 600);
            this.imgFloat.SizeMode = PictureBoxSizeMode.CenterImage;
            this.imgFloat.TabIndex = 6;
            this.imgFloat.TabStop = false;
            this.imgFloat.Visible = false;
            this.panel5.BorderStyle = BorderStyle.Fixed3D;
            this.panel5.Controls.Add(this.pgb);
            this.panel5.Controls.Add(this.groupBox1);
            this.panel5.Location = new Point(0, 0x273);
            this.panel5.Name = "panel5";
            this.panel5.Size = new Size(0x30d, 0x45);
            this.panel5.TabIndex = 0x11;
            this.pgb.Location = new Point(5, 0x25);
            this.pgb.Name = "pgb";
            this.pgb.Size = new Size(0x303, 0x17);
            this.pgb.TabIndex = 1;
            this.groupBox1.Controls.Add(this.lblStatus);
            this.groupBox1.Location = new Point(5, -3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x303, 0x25);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.lblStatus.AutoEllipsis = true;
            this.lblStatus.Dock = DockStyle.Fill;
            this.lblStatus.Location = new Point(3, 0x11);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(0x2fd, 0x11);
            this.lblStatus.TabIndex = 0;
            this.Track.HeaderText = "곡목";
            this.Track.Name = "Track";
            this.Track.ReadOnly = true;
            this.Track.Width = 250;
            this.Artist.HeaderText = "가수";
            this.Artist.Name = "Artist";
            this.Artist.ReadOnly = true;
            this.Artist.Width = 120;
            this.Cover.HeaderText = "CV";
            this.Cover.ImageLayout = DataGridViewImageCellLayout.Stretch;
            this.Cover.Name = "Cover";
            this.Cover.ReadOnly = true;
            this.Cover.Width = 20;
            this.coversize.HeaderText = "커버 검색 결과";
            this.coversize.Name = "coversize";
            this.coversize.ReadOnly = true;
            this.coversize.Width = 150;
            this.Result.HeaderText = "가사 검색 결과";
            this.Result.Name = "Result";
            this.Result.ReadOnly = true;
            this.Result.Width = 150;
            base.AutoScaleDimensions = new SizeF(7f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x30d, 0x2b8);
            base.Controls.Add(this.panel5);
            base.Controls.Add(this.imgFloat);
            base.Controls.Add(this.splitContainer1);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "MassUpdater";
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "TagGuru MassUpdater";
            base.Load += new EventHandler(this.MassUpdater_Load);
            base.FormClosed += new FormClosedEventHandler(this.MassUpdater_FormClosed);
            base.FormClosing += new FormClosingEventHandler(this.MassUpdater_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((ISupportInitialize) this.grid).EndInit();
            this.panel3.ResumeLayout(false);
            ((ISupportInitialize) this.img).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((ISupportInitialize) this.imgFloat).EndInit();
            this.panel5.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void MassUpdater_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in (IEnumerable) this.grid.Rows)
                {
                    ((Bitmap) row.Cells[2].Value).Dispose();
                }
                if (this.img.Image != null)
                {
                    this.img.Image.Dispose();
                }
                Directory.Delete(Path.GetTempPath() + @"\lyricguru\", true);
            }
            catch (Exception)
            {
            }
        }

        private void MassUpdater_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.lu.onwork)
            {
                if (MessageBox.Show("작업을 중단하시겠습니까 ?", "TagGuru", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.lu.onwork = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void MassUpdater_Load(object sender, EventArgs e)
        {
            this.lu = new LyricsUpdater(this.m_selectedTracks, this);
            ThreadStart start = new ThreadStart(this.lu.UpdateLyrics);
            new Thread(start).Start();
        }

        private void SetCoverColumn(bool show)
        {
            this.grid.Columns[2].Visible = show;
        }

        private void SetProgress(int cur, int max)
        {
            this.pgb.Maximum = max;
            this.pgb.Value = cur;
        }

        private void SetSplitSize(int size)
        {
            this.splitContainer1.SplitterDistance = size;
            if (size == 0x20e)
            {
                this.panel5.Visible = false;
            }
            else
            {
                this.panel5.Visible = true;
            }
        }

        private void SetStatus(string status)
        {
            double num = this.pgb.Value;
            double maximum = this.pgb.Maximum;
            double num3 = Math.Round((double) ((num / maximum) * 100.0));
            this.lblStatus.Text = string.Concat(new object[] { status, " - 총 ", this.pgb.Maximum, " 곡중 ", this.pgb.Value, " 곡 완료 (", num3.ToString(), "%)" });
        }

        private void updateRow(int index, string result, Bitmap coverimg, string loc)
        {
            if (result == "album")
            {
                if (coverimg != null)
                {
                    this.grid.Rows[index].Cells[2].Value = coverimg;
                }
                if (loc.Contains("실패"))
                {
                    this.grid.Rows[index].Cells[3].Style.BackColor = Color.Red;
                }
                this.grid.Rows[index].Cells[3].Value = loc;
                if (this.iMain.chkLyric.Checked)
                {
                    this.grid.Rows[index].Cells[4].Value = "가사 검색중...";
                }
            }
            if (result == "true")
            {
                this.grid.Rows[index].Cells[4].Value = "가사 입력 (" + loc + ")";
                this.grid.Rows[index].DefaultCellStyle.BackColor = Color.Yellow;
            }
            else if (result == "false")
            {
                this.grid.Rows[index].Cells[4].Value = "가사 없음";
                this.grid.Rows[index].ErrorText = "가사를 찾지 못했습니다.";
                this.grid.Rows[index].Cells[4].Style.BackColor = Color.Red;
                if (this.grid.Rows[index].Cells[3].Style.BackColor == Color.Red)
                {
                    this.grid.Rows[index].DefaultCellStyle.BackColor = Color.Red;
                }
            }
            else if (result == "skip")
            {
                this.grid.Rows[index].Cells[4].Value = "가사 있음";
                this.grid.Rows[index].DefaultCellStyle.BackColor = Color.SkyBlue;
            }
        }
    }
}

