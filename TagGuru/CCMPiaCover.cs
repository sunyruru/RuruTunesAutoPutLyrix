namespace TagGuru
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    internal class CCMPiaCover
    {
        private string m_album;
        private string m_artist;
        public MassUpdater m_form;
        private string m_song;
        private bool m_titleonly;
        public int mCoverSize = 500;

        private byte[] eucme(string src)
        {
            Encoding dstEncoding = Encoding.GetEncoding("EUC-KR");
            Encoding unicode = Encoding.Unicode;
            return Encoding.Convert(unicode, dstEncoding, unicode.GetBytes(src));
        }

        private string getCover(string query, bool bigsize)
        {
            string path = null;
            try
            {
                int mCoverSize = 0;
                if (bigsize)
                {
                    mCoverSize = this.mCoverSize;
                }
                string uri = "http://www.ccmpia.com/search/album.ajax.php";
                string text = this.HttpPost(uri + query, "");
                foreach (string str4 in this.returnSubstrings(text, "<li class=\"album_item clear_both\">", "</li>"))
                {
                    if ((str4.Contains(this.m_artist) || str4.Contains(this.m_artist.Replace(" ", ""))) || ((str4.Replace(" ", "").Contains(this.m_artist) || str4.Replace(" ", "").Contains(this.m_artist.Replace(" ", ""))) || this.m_form.CheckMultiArtist(this.m_artist, str4)))
                    {
                        string[] strArray2 = this.returnSubstrings(str4, "<a href=\"", "\"");
                        if (strArray2.Length > 0)
                        {
                            uri = "http://www.ccmpia.com" + strArray2[0];
                            text = this.HttpPost(uri, "");
                            string[] strArray3 = this.returnSubstrings(text, "<ul class=\"album_gallery clear_both\">", "</ul>");
                            string[] strArray4 = this.returnSubstrings(strArray3[0], "href=\"", "\"");
                            if (strArray4.Length > 0)
                            {
                                string fileName = Path.GetFileName(strArray4[0]);
                                strArray4[0] = "http://www.ccmpia.com" + strArray4[0];
                                Directory.CreateDirectory(Path.GetTempPath() + @"\lyricguru\");
                                string str6 = Path.GetTempPath() + @"lyricguru\" + fileName;
                                WebClient client = new WebClient();
                                if (!System.IO.File.Exists(str6))
                                {
                                    client.DownloadFile(strArray4[0], str6);
                                }
                                if (!System.IO.File.Exists(str6))
                                {
                                    client.DownloadFile(strArray4[0], str6);
                                }
                                Bitmap bitmap = new Bitmap(str6);
                                if (bitmap.Width >= mCoverSize)
                                {
                                    string destFileName = Regex.Replace(this.m_artist + "_" + this.m_album, "[?:\\/*\"<>|]", "");
                                    destFileName = Path.GetTempPath() + @"\lyricguru\dn_" + destFileName;
                                    try
                                    {
                                        System.IO.File.Copy(str6, destFileName, true);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    if (path != null)
                                    {
                                        System.IO.File.Delete(path);
                                    }
                                    mCoverSize = bitmap.Width;
                                    path = str6;
                                    bitmap.Dispose();
                                    return path;
                                }
                                bitmap.Dispose();
                                System.IO.File.Delete(str6);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return path;
        }

        public string getCover(string artist, string album, string song, bool bigsize, bool AlbumTitleOnly)
        {
            this.m_artist = artist;
            this.m_album = album;
            this.m_song = song;
            this.m_titleonly = AlbumTitleOnly;
            string str = null;
            string query = null;
            if ((song != "") && (song != null))
            {
                song = HttpUtility.UrlEncode(song);
            }
            if ((album != "") && (album != null))
            {
                album = HttpUtility.UrlEncode(album);
            }
            if ((artist != "") && (artist != null))
            {
                artist = HttpUtility.UrlEncode(artist);
            }
            if ((album != "") && (album != null))
            {
                this.m_form.Invoke(this.m_form.m_SetStatus, new object[] { "CCMPia 앨범커버 : '" + this.m_album + "' by '" + this.m_artist + "' 검색중" });
                query = "?m=search&q=" + album + "&limit=500";
                if (str == null)
                {
                    str = this.getCover(query, bigsize);
                }
            }
            if ((AlbumTitleOnly && (album != "")) && ((album != null) && (str == null)))
            {
                this.m_form.Invoke(this.m_form.m_SetStatus, new object[] { "CCMPia 앨범커버 : '" + this.m_album + "' 검색중 ( Experimental )" });
                str = this.getCover("?m=search_resul&section=album&album_title=" + album, bigsize);
            }
            return str;
        }

        private string HttpPost(string uri, string parameters)
        {
            string str2 = string.Empty;
            WebRequest request = WebRequest.Create(uri);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "GET";
            Encoding.UTF8.GetBytes(parameters);
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
                str2 = str;
            }
            catch (WebException)
            {
            }
            return str2;
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

        private static string StripTags(string HTML)
        {
            return Regex.Replace(HTML, "<[^>]*>", "").Replace("&lt;", "<");
        }
    }
}

