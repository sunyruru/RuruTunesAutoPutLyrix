namespace RuruTunesAutoPutLyrix
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;

    internal class UtaMap
    {
        public MassUpdater m_form;

        public bool checkSongExists(string artist, string song)
        {
            string uri = "http://www.utamap.com/searchkasi.php";
            string text = this.HttpPost(uri, "searchname=title&act=search&sortname=1&pattern=3&word=" + song, false);
            if (text != null)
            {
                foreach (string str3 in this.returnSubstrings(text, "./showkasi.php?surl=", "</tr>"))
                {
                    if (str3.ToUpper().Contains(artist.ToUpper()) || str3.ToUpper().Contains(artist.ToUpper().Replace(" ", "")))
                    {
                        return true;
                    }
                }
                if (song.Contains(" "))
                {
                    text = this.HttpPost(uri, "searchname=title&act=search&sortname=1&pattern=3&word=" + song.Replace(" ", ""), false);
                    foreach (string str4 in this.returnSubstrings(text, "./showkasi.php?surl=", "</tr>"))
                    {
                        if (str4.ToUpper().Contains(artist.ToUpper()) || str4.ToUpper().Contains(artist.ToUpper().Replace(" ", "")))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private byte[] eucme(string src)
        {
            Encoding dstEncoding = Encoding.GetEncoding("Shift_JIS");
            Encoding unicode = Encoding.Unicode;
            return Encoding.Convert(unicode, dstEncoding, unicode.GetBytes(src));
        }

        public string getSong(string artist, string song)
        {
            string uri = "http://www.utamap.com/searchkasi.php";
            string text = this.HttpPost(uri, "searchname=title&act=search&sortname=1&pattern=3&word=" + song, false);
            foreach (string str3 in this.returnSubstrings(text, "./showkasi.php?surl=", "</tr>"))
            {
                if (str3.ToUpper().Contains(artist.ToUpper()) || str3.ToUpper().Contains(artist.ToUpper().Replace(" ", "")))
                {
                    uri = "http://www.utamap.com/phpflash/flashfalsephp.php";
                    string parameters = "unum=" + str3.Substring(0, str3.IndexOf('"'));
                    text = this.HttpPost(uri, parameters, true);
                    return StripTags(text.Substring(text.IndexOf("test2=") + 6));
                }
            }
            if (song.Contains(" "))
            {
                text = this.HttpPost(uri, "searchname=title&act=search&sortname=1&pattern=3&word=" + song.Replace(" ", ""), false);
                foreach (string str6 in this.returnSubstrings(text, "./showkasi.php?surl=", "</tr>"))
                {
                    if (str6.ToUpper().Contains(artist.ToUpper()) || str6.ToUpper().Contains(artist.ToUpper().Replace(" ", "")))
                    {
                        uri = "http://www.utamap.com/phpflash/flashfalsephp.php";
                        string str7 = "unum=" + str6.Substring(0, str6.IndexOf('"'));
                        text = this.HttpPost(uri, str7, true);
                        return StripTags(text.Substring(text.IndexOf("test2=") + 6));
                    }
                }
            }
            return "";
        }

        private string HttpPost(string uri, string parameters, bool utf)
        {
            WebRequest request = WebRequest.Create(uri);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            byte[] buffer = this.eucme(parameters);
            Stream requestStream = null;
            try
            {
                request.ContentLength = buffer.Length;
                requestStream = request.GetRequestStream();
                requestStream.Write(buffer, 0, buffer.Length);
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
                StreamReader reader;
                WebResponse response = request.GetResponse();
                if (response == null)
                {
                    return null;
                }
                if (utf)
                {
                    reader = new StreamReader(response.GetResponseStream());
                }
                else
                {
                    reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("Shift_JIS"));
                }
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
            return Regex.Replace(HTML, "<[^>]*>", "");
        }
    }
}

