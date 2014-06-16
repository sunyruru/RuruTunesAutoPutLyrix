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
    using System.Xml;
    using System.Xml.XPath;

    internal class ManiaDB
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
                string uri = "http://www.maniadb.com/search.asp";
                string text = this.HttpPost(uri, query);
                string[] strArray = this.returnSubstrings(text, "<table width=\"100%\" style=\"table-layout: fixed\"><tr><td width=\"50\">", "</table>");
                if (strArray.Length == 0)
                {
                    strArray = this.returnSubstrings(text, "<table width=\"625\" border=\"0\">", "</table>");
                }
                foreach (string str4 in strArray)
                {
                    string[] strArray2 = this.returnSubstrings(str4, "<a class=\"highslide\" onclick=\"return hs.expand(this)\" href=\"", "\"");
                    if (strArray2.Length > 0)
                    {
                        string fileName = Path.GetFileName(strArray2[0]);
                        Directory.CreateDirectory(Path.GetTempPath() + @"\lyricguru\");
                        string str6 = Path.GetTempPath() + @"lyricguru\" + fileName;
                        WebClient client = new WebClient();
                        if (!System.IO.File.Exists(str6))
                        {
                            client.DownloadFile(strArray2[0], str6);
                        }
                        if (!System.IO.File.Exists(str6))
                        {
                            client.DownloadFile(strArray2[0], str6);
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
                this.m_form.Invoke(this.m_form.m_SetStatus, new object[] { "ManiaDB 앨범커버 : '" + this.m_album + "' by '" + this.m_artist + "' 검색중" });
                query = "sr=L&cof=FORID%3A1&q=" + album + "&src=S&qc2=&srp=P&ql2=" + artist;
                if (str == null)
                {
                    str = this.getCover(query, bigsize);
                }
            }
            query = "sr=S&cof=FORID%3A1&q=" + song + "&src=S&qc2=&srp=P&srs=A&qs2=" + artist;
            if (str == null)
            {
                this.m_form.Invoke(this.m_form.m_SetStatus, new object[] { "ManiaDB 앨범커버 : '" + this.m_song + "' by '" + this.m_artist + "' 검색중" });
                str = this.getCover(query, bigsize);
            }
            if ((AlbumTitleOnly && (album != "")) && ((album != null) && (str == null)))
            {
                this.m_form.Invoke(this.m_form.m_SetStatus, new object[] { "ManiaDB 앨범커버 : '" + this.m_album + "' 검색중 ( Experimental )" });
                str = this.getCover("sr=L&cof=FORID%3A1&q=" + album + "&src=S&qc2=&srp=P&srs=A&ql2=", bigsize);
            }
            return str;
        }

        private string getImage(string query, bool bigsize)
        {
            string path = null;
            try
            {
                int width = 0;
                if (bigsize)
                {
                    width = 300;
                }
                string uri = "http://www.maniadb.com/api/search.asp";
                string parameters = "key=dde36c701918e92290e6137dfd584b45&target=music&display=100" + query;
                string xml = this.HttpPost(uri, parameters);
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);
                XPathNodeIterator iterator = document.CreateNavigator().Select("//rss/channel/item/image");
                while (iterator.MoveNext())
                {
                    string str5 = iterator.Current.Value.Trim().Replace("\n", "").Replace("\r", "").Replace("www.", "image.");
                    string fileName = Path.GetFileName(str5);
                    Directory.CreateDirectory(Path.GetTempPath() + @"\lyricguru\");
                    string str7 = Path.GetTempPath() + @"\lyricguru\" + fileName;
                    new WebClient().DownloadFile(str5, str7);
                    Bitmap bitmap = new Bitmap(str7);
                    if (bitmap.Width > width)
                    {
                        if (path != null)
                        {
                            System.IO.File.Delete(path);
                        }
                        width = bitmap.Width;
                        path = str7;
                        bitmap.Dispose();
                    }
                    else
                    {
                        bitmap.Dispose();
                        System.IO.File.Delete(str7);
                    }
                }
            }
            catch (Exception)
            {
            }
            return path;
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

