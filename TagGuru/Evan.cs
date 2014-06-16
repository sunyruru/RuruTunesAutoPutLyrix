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

    internal class Evan
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
                string uri = "http://www.evan.co.kr/evan/Search/search_result.aspx";
                string text = this.HttpPost(uri, query);
                foreach (string str4 in this.returnSubstrings(text, "<A href=javascript:RecordDetailView('", "'"))
                {
                    string str5 = str4;
                    string address = "http://www.evan.co.kr/recorddata/images/Album/" + str5 + "_f.jpg";
                    Directory.CreateDirectory(Path.GetTempPath() + @"\lyricguru\");
                    string str7 = Path.GetTempPath() + @"\lyricguru\evan" + str5;
                    WebClient client = new WebClient();
                    if (!System.IO.File.Exists(str7))
                    {
                        client.DownloadFile(address, str7);
                    }
                    if (!System.IO.File.Exists(str7))
                    {
                        client.DownloadFile(address, str7);
                    }
                    Bitmap bitmap = new Bitmap(str7);
                    if (bitmap.Width >= mCoverSize)
                    {
                        string destFileName = Regex.Replace(this.m_artist + "_" + this.m_album, "[?:\\/*\"<>|]", "");
                        destFileName = Path.GetTempPath() + @"\lyricguru\dn_" + destFileName;
                        try
                        {
                            System.IO.File.Copy(str7, destFileName, true);
                        }
                        catch (Exception)
                        {
                        }
                        if (path != null)
                        {
                            System.IO.File.Delete(path);
                        }
                        mCoverSize = bitmap.Width;
                        path = str7;
                        bitmap.Dispose();
                        return path;
                    }
                    bitmap.Dispose();
                    System.IO.File.Delete(str7);
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
                song = HttpUtility.UrlEncode(this.eucme(song));
            }
            if ((album != "") && (album != null))
            {
                album = HttpUtility.UrlEncode(this.eucme(album));
            }
            if ((artist != "") && (artist != null))
            {
                artist = HttpUtility.UrlEncode(this.eucme(artist));
            }
            if ((album != "") && (album != null))
            {
                this.m_form.Invoke(this.m_form.m_SetStatus, new object[] { "Evan 앨범커버 : '" + this.m_album + "' by '" + this.m_artist + "' 검색중" });
                query = "EvanSearchKeyword=" + album + "; EvanSearchQ=(++recordname+like+'%25" + album + "%25'+AND++artistname+like+'%25" + artist + "%25'+)+; EvanSearchArtist=yes";
                if (str == null)
                {
                    str = this.getCover(query, bigsize);
                }
            }
            query = "EvanSearchKeyword=" + song + "; EvanSearchQ=(++artistname+like+'%25" + artist + "%25'+AND++recordid+in+(select+top+100+percent+recordid+from+view_track_search+where+songtitle+like+'" + song + "%25'++group+by+recordid+)+)+; EvanSearchArtist=yes";
            if (str == null)
            {
                this.m_form.Invoke(this.m_form.m_SetStatus, new object[] { "Evan 앨범커버 : '" + this.m_song + "' by '" + this.m_artist + "' 검색중" });
                str = this.getCover(query, bigsize);
            }
            if ((AlbumTitleOnly && (album != "")) && ((album != null) && (str == null)))
            {
                this.m_form.Invoke(this.m_form.m_SetStatus, new object[] { "Evan 앨범커버 : '" + this.m_album + "' 검색중 ( Experimental )" });
                query = "EvanSearchKeyword=" + album + "; EvanSearchQ=(++recordname+like+'%25" + album + "%25'+)+; EvanSearchArtist=no";
                str = this.getCover(query, bigsize);
            }
            return str;
        }

        private string HttpPost(string uri, string parameters)
        {
            string str = string.Empty;
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
                str = reader.ReadToEnd().Trim();
            }
            catch (WebException)
            {
            }
            return str;
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

