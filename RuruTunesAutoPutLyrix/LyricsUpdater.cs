namespace RuruTunesAutoPutLyrix
{
    using iTunesLib;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using org.lyricwiki;

    internal class LyricsUpdater
    {
        private AllCD ac = new AllCD();
        private CCMPia cc = new CCMPia();
        private CCMPiaCover ccc = new CCMPiaCover();
        private Coverholic ch = new Coverholic();
        private bool dontShowCover;
        private Evan ev = new Evan();
        private GuljaBada gb = new GuljaBada();
        private Gasazip gz = new Gasazip();
        private inMuz im = new inMuz();
        private RuruTunesAutoPutLyrix iMain;
        private Jieumai ji = new Jieumai();
        private Sing8 jp = new Sing8();
        private bool m_Cover;
        private bool m_CoverBigSize;
        private bool m_CoveriTunes;
        private bool m_CoverOverwrite;
        private int m_CoverSize;
        private MassUpdater m_form;
        private bool m_LyricSongName;
        private LyricWiki m_lyricsWiki;
        private bool m_overwrite;
        private IITTrackCollection m_selectedTracks;
        private ManiaDB mdb = new ManiaDB();
        private MNet mn = new MNet();
        private string oartist = "";
        public bool onwork;
        private string osong = "";
        private UtaMap um = new UtaMap();
        private ZzlZzl zz = new ZzlZzl();

        public LyricsUpdater(IITTrackCollection selectedTracks, MassUpdater form)
        {
            this.iMain = form.iMain;
            this.im.m_form = form;
            this.um.m_form = form;
            this.gb.m_form = form;
            this.gz.m_form = form;
            this.mdb.m_form = form;
            this.mn.m_form = form;
            this.ev.m_form = form;
            this.ac.m_form = form;
            this.jp.m_form = form;
            this.cc.m_form = form;
            this.ji.m_form = form;
            this.ccc.m_form = form;
            this.ch.m_form = form;
            this.zz.m_form = form;
            this.m_selectedTracks = selectedTracks;
            this.m_lyricsWiki = new LyricWiki();
            this.m_form = form;
            this.m_overwrite = this.iMain.chkOverwrite.Checked;
            this.m_Cover = this.iMain.chkAlbumCover.Checked;
            this.m_CoverOverwrite = this.iMain.chkCoverOverwrite.Checked;
            this.m_CoveriTunes = this.iMain.chkCoverItunes.Checked;
            this.m_CoverBigSize = this.iMain.chkCoverBigSize.Checked;
            this.m_CoverSize = int.Parse(this.iMain.txtSize.Text);
            this.m_LyricSongName = this.iMain.chkSongName.Checked;
            this.ac.mCoverSize = this.m_CoverSize;
            this.ccc.mCoverSize = this.m_CoverSize;
            this.mdb.mCoverSize = this.m_CoverSize;
            this.ev.mCoverSize = this.m_CoverSize;
            this.ch.mCoverSize = this.m_CoverSize;
            this.zz.mCoverSize = this.m_CoverSize;
        }

        private string HttpPost(string uri, string parameters)
        {
            WebRequest request = WebRequest.Create(uri);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            byte[] bytes = Encoding.UTF8.GetBytes(parameters);
            Stream requestStream = null;
            try
            {
                request.ContentLength = bytes.Length;
                requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
            }
            catch (WebException)
            {
            }
            finally
            {
                if (requestStream != null)
                {
                    requestStream.Close();
                }
            }
            try
            {
                WebResponse response = request.GetResponse();
                if (response == null)
                {
                    return null;
                }
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                string str = reader.ReadToEnd().Trim();
                reader.Close();
                response.Close();
                return str;
            }
            catch (WebException)
            {
            }
            return null;
        }

        private string ReformatLyrics(string p)
        {
            string str = "";
            if (this.m_LyricSongName)
            {
                str = this.osong + "\r\n by [" + this.oartist + "]\r\n\r\n";
            }
            p = p.Replace("\r\n\r\n\r\n", "\r\n");
            p = p.Replace("\r\n\r\n", "\r\n");
            return (str + p);
        }

        private string[] returnSubstrings(string text, string openingMarker, string closingMarker)
        {
            int length = openingMarker.Length;
            int num2 = closingMarker.Length;
            int index = 0;
            ArrayList list = new ArrayList();
            int startIndex = 0;
            while ((startIndex = text.IndexOf(openingMarker, startIndex)) != -1)
            {
                startIndex += length;
                index = text.IndexOf(closingMarker, startIndex);
                if (index != -1)
                {
                    list.Add(text.Substring(startIndex, index - startIndex));
                    startIndex = index + num2;
                }
            }
            return (string[]) list.ToArray(typeof(string));
        }

        public string stripme(string str)
        {
            string input = str;
            input = Regex.Replace(Regex.Replace(input, @"^\p{Nd}*(\.)", ""), @"(\(|\{|\[|<).*(\)|\}|\]|>)", "");
            if ((input == "") && ((str.Contains("[") || str.Contains("{")) || (str.Contains("(") || str.Contains("<"))))
            {
                str = str.Replace("]", "");
                str = str.Replace("[", "");
                str = str.Replace("(", "");
                str = str.Replace(")", "");
                str = str.Replace("{", "");
                str = str.Replace("}", "");
                str = str.Replace("<", "");
                str = str.Replace(">", "");
                input = Regex.Replace(str, @"^\p{Nd}*(\.)", "");
            }
            return Regex.Replace(Regex.Replace(Regex.Replace(input, @"feat\..*$", "", RegexOptions.IgnoreCase), "featuring.*$", "", RegexOptions.IgnoreCase), @"ft\..*$", "", RegexOptions.IgnoreCase).Trim();
        }

        public string stripmore(string song, string artist)
        {
            int num3;
            song = song.Replace(" - ", " ");
            song = song.Replace(" _ ", " ");
            song = song.Replace("- ", " ");
            song = song.Replace("_ ", " ");
            song = song.Replace(" -", " ");
            song = song.Replace(" _", " ");
            song = song.Replace("-", " ");
            song = song.Replace("_", " ");
            if (song.IndexOf(artist) >= 0)
            {
                song = song.Replace(artist, "");
            }
            else
            {
                string[] separator = new string[] { ",", "&", "와 ", "과 ", " and ", "/" };
                foreach (string str in artist.Split(separator, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (song.IndexOf(str) >= 0)
                    {
                        song = song.Replace(str, "");
                    }
                }
            }
            song = song.Trim();
            if (song.IndexOf("집") >= 0)
            {
                double num2;
                song = " " + song;
                int startIndex = song.LastIndexOf(" ", song.IndexOf("집"));
                if (startIndex < 0)
                {
                    startIndex = 0;
                }
                if (double.TryParse(song.Substring(startIndex + 1, (song.IndexOf("집") - startIndex) - 1), out num2))
                {
                    song = song.Replace(song.Substring(startIndex, (song.IndexOf("집") - startIndex) + 1), "");
                }
            }
            song = song.Trim();
            string[] strArray3 = song.Split(new char[] { ' ', '-', '.', ':' });
            if (((strArray3.Length > 0) && int.TryParse(strArray3[0], out num3)) && (num3 < 20))
            {
                song = song.Replace(strArray3[0], "");
            }
            return song.Trim();
        }

        private static string StripTags(string HTML)
        {
            return Regex.Replace(HTML, "<[^>]*>", "");
        }

        private void UpdateCover(int index, IITFileOrCDTrack currentTrack, string martist, string msong, string malbum, Bitmap coverimg, string cover)
        {
            string path = "";
            try
            {
                string artist = this.stripme(martist);
                string song = this.stripme(msong);
                song = this.stripmore(song, artist);
                string str4 = this.stripme(malbum);
                str4 = this.stripmore(str4, artist);
                bool flag = false;
                string str5 = null;
                for (int i = 0; i < this.iMain.cbox.Items.Count; i++)
                {
                    string str6 = Regex.Replace(martist + "_" + str4, "[?:\\/*\"<>|]", "");
                    str6 = Path.GetTempPath() + @"lyricguru\dn_" + str6;
                    try
                    {
                        if (System.IO.File.Exists(str6))
                        {
                            path = str6;
                            str5 = "SameAlbum";
                            flag = true;
                            break;
                        }
                    }
                    catch (Exception)
                    {
                    }
                    if (this.iMain.cbox.GetItemChecked(i) && this.iMain.cbox.Items[i].ToString().Contains("maniadb"))
                    {
                        path = this.mdb.getCover(artist, str4, song, this.m_CoverBigSize, this.iMain.chkAlbumTitleOnly.Checked);
                        if (path != null)
                        {
                            str5 = "ManiaDB";
                            flag = true;
                            break;
                        }
                    }
                    if (this.iMain.cbox.GetItemChecked(i) && this.iMain.cbox.Items[i].ToString().Contains("evan"))
                    {
                        path = this.ev.getCover(artist, str4, song, this.m_CoverBigSize, this.iMain.chkAlbumTitleOnly.Checked);
                        if (path != null)
                        {
                            str5 = "Evan";
                            flag = true;
                            break;
                        }
                    }
                    if (this.iMain.cbox.GetItemChecked(i) && this.iMain.cbox.Items[i].ToString().Contains("allcd"))
                    {
                        path = this.ac.getCover(artist, str4, song, this.m_CoverBigSize, this.iMain.chkAlbumTitleOnly.Checked);
                        if (path != null)
                        {
                            str5 = "AllCD";
                            flag = true;
                            break;
                        }
                    }
                    if (this.iMain.cbox.GetItemChecked(i) && this.iMain.cbox.Items[i].ToString().Contains("coverholic"))
                    {
                        path = this.ch.getCover(artist, str4, song, this.m_CoverBigSize, this.iMain.chkAlbumTitleOnly.Checked);
                        if (path != null)
                        {
                            str5 = "Coverholic";
                            flag = true;
                            break;
                        }
                    }
                    if (this.iMain.cbox.GetItemChecked(i) && this.iMain.cbox.Items[i].ToString().Contains("zzlzzl"))
                    {
                        path = this.zz.getCover(artist, str4, song, this.m_CoverBigSize, this.iMain.chkAlbumTitleOnly.Checked);
                        if (path != null)
                        {
                            str5 = "즐즐넷";
                            flag = true;
                            break;
                        }
                    }
                    if (this.iMain.cbox.GetItemChecked(i) && this.iMain.cbox.Items[i].ToString().Contains("ccmpia"))
                    {
                        path = this.ccc.getCover(artist, str4, song, this.m_CoverBigSize, this.iMain.chkAlbumTitleOnly.Checked);
                        if (path != null)
                        {
                            str5 = "CCMPia";
                            flag = true;
                            break;
                        }
                    }
                }
                if (flag)
                {
                    if (coverimg != null)
                    {
                        coverimg.Dispose();
                    }
                    if (System.IO.File.Exists(path))
                    {
                        if (!this.dontShowCover)
                        {
                            coverimg = new Bitmap(path);
                            this.m_form.Invoke(this.m_form.m_DelegateUpdateRow, new object[] { index, "album", coverimg, string.Concat(new object[] { str5, "-", coverimg.Width, "x", coverimg.Height }) });
                            if (currentTrack.Artwork[1] != null)
                            {
                                currentTrack.Artwork[1].Delete();
                            }
                            currentTrack.AddArtworkFromFile(path);
                        }
                        else
                        {
                            if (currentTrack.Artwork[1] != null)
                            {
                                currentTrack.Artwork[1].Delete();
                            }
                            currentTrack.AddArtworkFromFile(path);
                            object[] args = new object[4];
                            args[0] = index;
                            args[1] = "album";
                            args[3] = str5;
                            this.m_form.Invoke(this.m_form.m_DelegateUpdateRow, args);
                        }
                    }
                    else
                    {
                        this.m_form.Invoke(this.m_form.m_DelegateUpdateRow, new object[] { index, "album", coverimg, "커버 다운 실패" });
                    }
                }
                else
                {
                    this.m_form.Invoke(this.m_form.m_DelegateUpdateRow, new object[] { index, "album", coverimg, "커버 검색 실패" });
                }
            }
            catch (Exception)
            {
                try
                {
                    this.m_form.Invoke(this.m_form.m_DelegateUpdateRow, new object[] { index, "album", coverimg, "커버 검색 실패" });
                }
                catch (Exception)
                {
                }
            }
        }

        public void UpdateLyrics()
        {
            this.onwork = true;
            int count = this.m_selectedTracks.Count;
            if ((count >= 500) && (MessageBox.Show("500곡 이상을 선택하셨습니다. 리스트에서 앨범커버 보기 기능이 메모리 오류를 낼수 있으므로 미리보기를 꺼두시겠습니까 ?", "RuruTunesAutoPutLyrix 오류", MessageBoxButtons.YesNo) == DialogResult.Yes))
            {
                this.dontShowCover = true;
            }
            this.m_form.Invoke(this.m_form.m_SetSplitSize, new object[] { 0x2c8 });
            this.m_form.Invoke(this.m_form.m_SetProgress, new object[] { 0, count });
            try
            {
                for (int i = 1; i <= this.m_selectedTracks.Count; i++)
                {
                    if (!this.onwork)
                    {
                        return;
                    }
                    IITFileOrCDTrack currentTrack = (IITFileOrCDTrack) this.m_selectedTracks[i];
                    string artist = currentTrack.Artist;
                    string name = currentTrack.Name;
                    string album = currentTrack.Album;
                    if (artist == null)
                    {
                        object[] objArray5 = new object[4];
                        objArray5[0] = name;
                        objArray5[1] = artist;
                        objArray5[3] = "가수이름 없음";
                        object[] objArray = objArray5;
                        int num1 = (int) this.m_form.Invoke(this.m_form.m_DelegateAddRow, new object[] { objArray });
                    }
                    else
                    {
                        if (artist.ToUpper() == "BEYONCE")
                        {
                            artist = "Beyonc\x00e9";
                        }
                        this.oartist = artist;
                        this.osong = name;
                        string input = "";
                        string str5 = "";
                        Bitmap coverimg = null;
                        bool flag = false;
                        if (!string.IsNullOrEmpty(artist) && !string.IsNullOrEmpty(name))
                        {
                            if (currentTrack.Artwork[1] != null)
                            {
                                try
                                {
                                    Directory.CreateDirectory(Path.GetTempPath() + @"\lyricguru\");
                                    input = currentTrack.Artist + "_" + currentTrack.Name;
                                    input = Regex.Replace(input, "[?:\\/*\"<>|]", "");
                                    input = Path.GetTempPath() + @"\lyricguru\" + input;
                                    if (!System.IO.File.Exists(input))
                                    {
                                        currentTrack.Artwork[1].SaveArtworkToFile(input);
                                    }
                                    coverimg = new Bitmap(input);
                                    if (!this.m_CoverOverwrite)
                                    {
                                        flag = true;
                                    }
                                    else
                                    {
                                        string path = Regex.Replace(currentTrack.Artist + "_" + currentTrack.Name, "[?:\\/*\"<>|]", "");
                                        path = Path.GetTempPath() + @"\lyricguru\dn_" + path;
                                        try
                                        {
                                            System.IO.File.Delete(path);
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                    if (currentTrack.Artwork[1].IsDownloadedArtwork && this.m_CoveriTunes)
                                    {
                                        flag = true;
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                            str5 = "검색중...";
                            if (this.m_Cover)
                            {
                                str5 = "앨범 커버 검색중...";
                            }
                            if ((currentTrack.Artwork[1] != null) && currentTrack.Artwork[1].IsDownloadedArtwork)
                            {
                                str5 = "iTunes커버";
                            }
                            if (this.dontShowCover)
                            {
                                if (coverimg != null)
                                {
                                    coverimg.Dispose();
                                }
                                coverimg = null;
                            }
                            object[] objArray2 = new object[] { name, artist, coverimg, str5 };
                            int index = (int) this.m_form.Invoke(this.m_form.m_DelegateAddRow, new object[] { objArray2 });
                            if (this.m_Cover && !flag)
                            {
                                this.UpdateCover(index, currentTrack, artist, name, album, coverimg, input);
                            }
                            if (this.iMain.chkLyric.Checked)
                            {
                                if ((currentTrack.Lyrics != null) && !this.m_overwrite)
                                {
                                    this.m_form.Invoke(this.m_form.m_DelegateUpdateRow, new object[] { index, "skip", coverimg, "" });
                                    goto Label_115F;
                                }
                                artist = this.stripme(artist);
                                name = this.stripme(name);
                                name = this.stripmore(name, artist);
                                try
                                {
                                    bool flag2 = false;
                                    for (int j = 0; j < this.iMain.lbox.Items.Count; j++)
                                    {
                                        if (this.iMain.lbox.GetItemChecked(j) && this.iMain.lbox.Items[j].ToString().Contains("lyricwiki"))
                                        {
                                            this.m_form.Invoke(this.m_form.m_SetStatus, new object[] { "LyricWiki 가사 : '" + this.osong + "' by '" + this.oartist + "' 검색중 ('" + name + "','" + artist + "')" });
                                            if (this.m_lyricsWiki.checkSongExists(artist, name))
                                            {
                                                LyricsResult result = this.m_lyricsWiki.getSong(artist, name);
                                                if ((result.lyrics != "Not found") && (this.m_overwrite || (currentTrack.Lyrics == null)))
                                                {
                                                    Encoding encoding = Encoding.GetEncoding("ISO-8859-1");
                                                    string text = this.ReformatLyrics(Encoding.UTF8.GetString(encoding.GetBytes(result.lyrics)));
                                                    if (text.IndexOf("The lyrics for this song can be found") > 0)
                                                    {
                                                        string[] strArray = this.returnSubstrings(text, "<a href='", "'");
                                                        if (strArray.Length > 0)
                                                        {
                                                            string uri = strArray[0];
                                                            string str9 = this.HttpPost(uri, "");
                                                            string[] strArray2 = this.returnSubstrings(str9, "<div class='lyricbox' >", "</div>");
                                                            if (strArray2.Length > 0)
                                                            {
                                                                text = strArray2[0];
                                                                text = StripTags(text.Replace("<br />", "\r\n").Replace("<br>", "\r\n"));
                                                            }
                                                        }
                                                    }
                                                    currentTrack.Lyrics = text;
                                                    this.m_form.Invoke(this.m_form.m_DelegateUpdateRow, new object[] { index, "true", coverimg, "LyricWiki" });
                                                    flag2 = true;
                                                    break;
                                                }
                                            }
                                        }
                                        if (this.iMain.lbox.GetItemChecked(j) && this.iMain.lbox.Items[j].ToString().Contains("gasazip"))
                                        {
                                            this.m_form.Invoke(this.m_form.m_SetStatus, new object[] { "Gasazip 가사 : '" + this.osong + "' by '" + this.oartist + "' 검색중 ('" + name + "','" + artist + "')" });
                                            if (this.gz.checkSongExists(artist, name))
                                            {
                                                string p = this.gz.getSong(artist, name);
                                                if (p != null)
                                                {
                                                    if (this.m_overwrite || (currentTrack.Lyrics == null))
                                                    {
                                                        currentTrack.Lyrics = this.ReformatLyrics(p);
                                                    }
                                                    this.m_form.Invoke(this.m_form.m_DelegateUpdateRow, new object[] { index, "true", coverimg, "Gasazip" });
                                                    flag2 = true;
                                                    break;
                                                }
                                            }
                                        }
                                        if (this.iMain.lbox.GetItemChecked(j) && this.iMain.lbox.Items[j].ToString().Contains("mnet"))
                                        {
                                            this.m_form.Invoke(this.m_form.m_SetStatus, new object[] { "MNet 가사 : '" + this.osong + "' by '" + this.oartist + "' 검색중 ('" + name + "','" + artist + "')" });
                                            if (this.mn.checkSongExists(artist, name))
                                            {
                                                string str11 = this.mn.getSong(artist, name);
                                                if (str11 != null)
                                                {
                                                    if (this.m_overwrite || (currentTrack.Lyrics == null))
                                                    {
                                                        currentTrack.Lyrics = this.ReformatLyrics(str11);
                                                    }
                                                    this.m_form.Invoke(this.m_form.m_DelegateUpdateRow, new object[] { index, "true", coverimg, "MNet" });
                                                    flag2 = true;
                                                    break;
                                                }
                                            }
                                        }
                                        if (this.iMain.lbox.GetItemChecked(j) && this.iMain.lbox.Items[j].ToString().Contains("inmuz"))
                                        {
                                            this.m_form.Invoke(this.m_form.m_SetStatus, new object[] { "InMuz 가사 : '" + this.osong + "' by '" + this.oartist + "' 검색중 ('" + name + "','" + artist + "')" });
                                            if (this.im.checkSongExists(artist, name))
                                            {
                                                string str12 = this.im.getSong(artist, name);
                                                if (str12 != null)
                                                {
                                                    if (this.m_overwrite || (currentTrack.Lyrics == null))
                                                    {
                                                        currentTrack.Lyrics = this.ReformatLyrics(str12);
                                                    }
                                                    this.m_form.Invoke(this.m_form.m_DelegateUpdateRow, new object[] { index, "true", coverimg, "InMuz" });
                                                    flag2 = true;
                                                    break;
                                                }
                                            }
                                        }
                                        if (this.iMain.lbox.GetItemChecked(j) && this.iMain.lbox.Items[j].ToString().Contains("new21"))
                                        {
                                            this.m_form.Invoke(this.m_form.m_SetStatus, new object[] { "글자바다 가사 : '" + this.osong + "' by '" + this.oartist + "' 검색중 ('" + name + "','" + artist + "')" });
                                            if (this.gb.checkSongExists(artist, name))
                                            {
                                                string str13 = this.gb.getSong(artist, name);
                                                if (str13 != null)
                                                {
                                                    if (this.m_overwrite || (currentTrack.Lyrics == null))
                                                    {
                                                        currentTrack.Lyrics = this.ReformatLyrics(str13);
                                                    }
                                                    this.m_form.Invoke(this.m_form.m_DelegateUpdateRow, new object[] { index, "true", coverimg, "글자바다" });
                                                    flag2 = true;
                                                    break;
                                                }
                                            }
                                        }
                                        if (this.iMain.lbox.GetItemChecked(j) && this.iMain.lbox.Items[j].ToString().Contains("utamap"))
                                        {
                                            this.m_form.Invoke(this.m_form.m_SetStatus, new object[] { "Utamap 가사 : '" + this.osong + "' by '" + this.oartist + "' 검색중 ('" + name + "','" + artist + "')" });
                                            if (this.um.checkSongExists(artist, name))
                                            {
                                                string str14 = this.um.getSong(artist, name);
                                                if (this.m_overwrite || (currentTrack.Lyrics == null))
                                                {
                                                    currentTrack.Lyrics = this.ReformatLyrics(str14);
                                                }
                                                this.m_form.Invoke(this.m_form.m_DelegateUpdateRow, new object[] { index, "true", coverimg, "UtaMap" });
                                                flag2 = true;
                                                break;
                                            }
                                        }
                                        if (this.iMain.lbox.GetItemChecked(j) && this.iMain.lbox.Items[j].ToString().Contains("sing8"))
                                        {
                                            this.m_form.Invoke(this.m_form.m_SetStatus, new object[] { "Sing8 가사 : '" + this.osong + "' by '" + this.oartist + "' 검색중 ('" + name + "','" + artist + "')" });
                                            if (this.jp.checkSongExists(artist, name))
                                            {
                                                string str15 = this.jp.getSong(artist, name);
                                                if (this.m_overwrite || (currentTrack.Lyrics == null))
                                                {
                                                    currentTrack.Lyrics = this.ReformatLyrics(str15);
                                                }
                                                this.m_form.Invoke(this.m_form.m_DelegateUpdateRow, new object[] { index, "true", coverimg, "Sing8" });
                                                flag2 = true;
                                                break;
                                            }
                                        }
                                        if (this.iMain.lbox.GetItemChecked(j) && this.iMain.lbox.Items[j].ToString().Contains("ccmpia"))
                                        {
                                            this.m_form.Invoke(this.m_form.m_SetStatus, new object[] { "CCMPia 가사 : '" + this.osong + "' by '" + this.oartist + "' 검색중 ('" + name + "','" + artist + "')" });
                                            if (this.cc.checkSongExists(artist, name))
                                            {
                                                string str16 = this.cc.getSong(artist, name);
                                                if (this.m_overwrite || (currentTrack.Lyrics == null))
                                                {
                                                    currentTrack.Lyrics = this.ReformatLyrics(str16);
                                                }
                                                this.m_form.Invoke(this.m_form.m_DelegateUpdateRow, new object[] { index, "true", coverimg, "CCMPia" });
                                                flag2 = true;
                                                break;
                                            }
                                        }
                                        if (this.iMain.lbox.GetItemChecked(j) && this.iMain.lbox.Items[j].ToString().Contains("jieumai"))
                                        {
                                            this.m_form.Invoke(this.m_form.m_SetStatus, new object[] { "지음아이 가사 : '" + this.osong + "' by '" + this.oartist + "' 검색중 ('" + name + "','" + artist + "')" });
                                            if (this.ji.checkSongExists(artist, name))
                                            {
                                                string str17 = this.ji.getSong(artist, name);
                                                if (this.m_overwrite || (currentTrack.Lyrics == null))
                                                {
                                                    currentTrack.Lyrics = this.ReformatLyrics(str17);
                                                }
                                                this.m_form.Invoke(this.m_form.m_DelegateUpdateRow, new object[] { index, "true", coverimg, "지음아이" });
                                                flag2 = true;
                                                break;
                                            }
                                        }
                                    }
                                    if (!flag2)
                                    {
                                        this.m_form.Invoke(this.m_form.m_DelegateUpdateRow, new object[] { index, "false", coverimg, "" });
                                    }
                                }
                                catch (Exception)
                                {
                                    try
                                    {
                                        this.m_form.Invoke(this.m_form.m_DelegateUpdateRow, new object[] { index, "false", coverimg, "" });
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }
                        }
                        try
                        {
                            this.m_form.Invoke(this.m_form.m_SetProgress, new object[] { i, count });
                        }
                        catch (Exception)
                        {
                        }
                    Label_115F:;
                    }
                }
                try
                {
                    this.m_form.Invoke(this.m_form.m_SetSplitSize, new object[] { 0x20e });
                    MessageBox.Show("가사 및 앨범표지 찾기 완료!", "LyricGuru");
                }
                catch (Exception)
                {
                }
            }
            catch (Exception)
            {
                return;
            }
            this.onwork = false;
        }
    }
}

