namespace RuruTunesAutoPutLyrix
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    internal class CCMPia
    {
        public MassUpdater m_form;

        public bool checkSongExists(string artist, string msong)
        {
            string str = null;
            if ((msong != "") && (msong != null))
            {
                str = HttpUtility.UrlEncode(msong);
            }
            string text = this.HttpPost("http://www.ccmpia.com/search/song.ajax.php?" + "m=search&q=" + str, "&limit=500");
            if (text != null)
            {
                foreach (string str4 in this.returnSubstrings(text, "class=\"artist_name\">", "</a>"))
                {
                    if ((str4.Contains(artist) || str4.Contains(artist.Replace(" ", ""))) || (str4.Replace(" ", "").Contains(artist) || str4.Replace(" ", "").Contains(artist.Replace(" ", ""))))
                    {
                        return true;
                    }
                    if (this.m_form.CheckMultiArtist(artist, str4))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private byte[] eucme(string src)
        {
            Encoding dstEncoding = Encoding.GetEncoding("EUC-KR");
            Encoding unicode = Encoding.Unicode;
            return Encoding.Convert(unicode, dstEncoding, unicode.GetBytes(src));
        }

        public string getSong(string artist, string msong)
        {
            string str = null;
            if ((msong != "") && (msong != null))
            {
                str = HttpUtility.UrlEncode(msong);
            }
            string uri = "http://www.ccmpia.com/search/song.ajax.php?";
            string text = this.HttpPost(uri + "m=search&q=" + str, "&limit=500");
            foreach (string str4 in this.returnSubstrings(text, "<li class=\"clear_both \">", "</li>"))
            {
                if ((str4.Contains(artist) || str4.Contains(artist.Replace(" ", ""))) || ((str4.Replace(" ", "").Contains(artist) || str4.Replace(" ", "").Contains(artist.Replace(" ", ""))) || this.m_form.CheckMultiArtist(artist, str4)))
                {
                    string[] strArray2 = this.returnSubstrings(str4, "javascript:openLyricsLayer(", ");");
                    if (strArray2.Length > 0)
                    {
                        uri = "http://www.ccmpia.com/music/lyrics.ajax.php?songId=" + strArray2[0];
                        text = this.HttpPost(uri, null);
                        string[] strArray3 = this.returnSubstrings(text, "class='lyric'>", "</div>");
                        if (strArray3.Length > 0)
                        {
                            return StripTags(strArray3[0]).Trim();
                        }
                        return null;
                    }
                }
            }
            return null;
        }

        private string HttpPost(string uri, string parameters)
        {
            string str2 = string.Empty;
            WebRequest request = WebRequest.Create(uri);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "GET";
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

