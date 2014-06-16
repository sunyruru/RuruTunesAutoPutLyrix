namespace RuruTunesAutoPutLyrix
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;

    internal class AllCD
    {
        private string m_album;
        private string m_artist;
        public MassUpdater m_form;
        private string m_song;
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
                string uri = "http://www.allcdcovers.com/search/music/all/" + query;
                string text = this.HttpPost(uri, "");
                foreach (string str4 in this.returnSubstrings(text, "<td><a href=\"", "\">Front"))
                {
                    string str5 = "http://www.allcdcovers.com" + str4;
                    string[] strArray2 = null;
                    string[] strArray3 = null;
                    string str6 = this.HttpPost(str5, "");
                    if (str6 != null)
                    {
                        strArray2 = this.returnSubstrings(str6, "<div class=\"selectedCoverThumb\">", "<img alt=\"Download cover\"");
                    }
                    if (strArray2.Length > 0)
                    {
                        strArray3 = this.returnSubstrings(strArray2[0], "<a href=\"", "\">");
                    }
                    str5 = "http://www.allcdcovers.com" + strArray3[0];
                    string str7 = strArray3[0].Substring(strArray3[0].LastIndexOf('/') + 1);
                    Directory.CreateDirectory(Path.GetTempPath() + @"\lyricguru\");
                    string str8 = Path.GetTempPath() + @"\lyricguru\acc" + str7;
                    WebClient client = new WebClient();
                    if (!System.IO.File.Exists(str8))
                    {
                        client.DownloadFile(str5, str8);
                    }
                    if (!System.IO.File.Exists(str8))
                    {
                        client.DownloadFile(str5, str8);
                    }
                    Bitmap bitmap = new Bitmap(str8);
                    if (bitmap.Width > mCoverSize)
                    {
                        string destFileName = Regex.Replace(this.m_artist + "_" + this.m_album, "[?:\\/*\"<>|]", "");
                        destFileName = Path.GetTempPath() + @"\lyricguru\dn_" + destFileName;
                        try
                        {
                            System.IO.File.Copy(str8, destFileName, true);
                        }
                        catch (Exception)
                        {
                        }
                        if (path != null)
                        {
                            System.IO.File.Delete(path);
                        }
                        mCoverSize = bitmap.Width;
                        path = str8;
                        bitmap.Dispose();
                        return path;
                    }
                    bitmap.Dispose();
                    System.IO.File.Delete(str8);
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
            string str = null;
            string query = null;
            if ((album != "") && (album != null))
            {
                this.m_form.Invoke(this.m_form.m_SetStatus, new object[] { "AllCDCovers 앨범커버 : '" + this.m_album + "' by '" + this.m_artist + "' 검색중" });
                query = album + " " + artist;
                if (str == null)
                {
                    str = this.getCover(query, bigsize);
                }
            }
            query = song + " " + artist;
            if (str == null)
            {
                this.m_form.Invoke(this.m_form.m_SetStatus, new object[] { "AllCDCovers 앨범커버 : '" + this.m_song + "' by '" + this.m_artist + "' 검색중" });
                str = this.getCover(query, bigsize);
            }
            if ((AlbumTitleOnly && (album != "")) && ((album != null) && (str == null)))
            {
                this.m_form.Invoke(this.m_form.m_SetStatus, new object[] { "AllCDCovers 앨범커버 : '" + this.m_album + "' 검색중 ( Experimental )" });
                query = album;
                str = this.getCover(query, bigsize);
            }
            return str;
        }

        private string HttpPost(string uri, string parameters)
        {
            string str2 = string.Empty;
            WebRequest request = WebRequest.Create(uri);
            request.ContentType = "text/html; charset=ks_c_5601-1987";
            request.Method = "GET";
            request.Headers.Add("Cookie: ASP.NET_SessionId=yjlscsyquuqcvf3ptovsxzzo; " + parameters);
            try
            {
                WebResponse response = request.GetResponse();
                if (response == null)
                {
                    return null;
                }
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("EUC-KR"));
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

