namespace RuruTunesAutoPutLyrix
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;

    internal class GuljaBada
    {
        public MassUpdater m_form;

        public bool checkSongExists(string artist, string song)
        {
            string uri = "http://im.new21.org/alpha/guljabada25/gulja_list_2500.php";
            string text = this.HttpPost(uri, "?mode=1&type=1&search[]=" + song);
            if (text == null)
            {
                return false;
            }
            foreach (string str3 in this.returnSongs(text))
            {
                if (str3.ToUpper().Contains(artist.ToUpper()) || str3.ToUpper().Contains(artist.ToUpper().Replace(" ", "")))
                {
                    return true;
                }
                if (this.m_form.CheckMultiArtist(artist, str3))
                {
                    return true;
                }
            }
            if (song.Contains(" "))
            {
                text = this.HttpPost(uri, "?mode=1&type=1&search[]=" + song.Replace(" ", ""));
                foreach (string str4 in this.returnSongs(text))
                {
                    if (str4.ToUpper().Contains(artist.ToUpper()) || str4.ToUpper().Contains(artist.ToUpper().Replace(" ", "")))
                    {
                        return true;
                    }
                    if (this.m_form.CheckMultiArtist(artist, str4))
                    {
                        return true;
                    }
                }
            }
            return this.checkSongExists2(artist, song);
        }

        public bool checkSongExists2(string artist, string song)
        {
            string uri = "http://im.new21.org/alpha/guljabada25/gulja_list_2500.php";
            string text = this.HttpPost(uri, "?mode=1&type=1&search[]=" + artist);
            if (text != null)
            {
                foreach (string str3 in this.returnSongs(text))
                {
                    if (str3.ToUpper().Contains(song.ToUpper()) || str3.ToUpper().Contains(song.ToUpper().Replace(" ", "")))
                    {
                        return true;
                    }
                }
                if (song.Contains(" "))
                {
                    text = this.HttpPost(uri, "?mode=1&type=1&search[]=" + artist);
                    foreach (string str4 in this.returnSongs(text))
                    {
                        if (str4.ToUpper().Contains(song.ToUpper()) || str4.ToUpper().Contains(song.ToUpper().Replace(" ", "")))
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
            Encoding dstEncoding = Encoding.GetEncoding("EUC-KR");
            Encoding unicode = Encoding.Unicode;
            return Encoding.Convert(unicode, dstEncoding, unicode.GetBytes(src));
        }

        public string getSong(string artist, string song)
        {
            string uri = "http://im.new21.org/alpha/guljabada25/gulja_list_2500.php";
            string text = this.HttpPost(uri, "?mode=1&type=1&search[]=" + song);
            string[] strArray = this.returnSongs(text);
            foreach (string str3 in strArray)
            {
                if ((str3.Contains(artist) || str3.Contains(artist.Replace(" ", ""))) || ((str3.Replace(" ", "").Contains(artist) || str3.Replace(" ", "").Contains(artist.Replace(" ", ""))) || this.m_form.CheckMultiArtist(artist, str3)))
                {
                    uri = "http://im.new21.org/alpha/guljabada25/gulja_view_2500.php";
                    string parameters = "index=" + str3.Substring(0, str3.IndexOf('|'));
                    text = this.HttpPost(uri, parameters);
                    string[] strArray2 = this.returnSubstrings(text, "<tr valign=\"top\">", "</tr>");
                    if (strArray2.Length > 0)
                    {
                        strArray2[0] = strArray2[0].Substring(0, strArray2[0].LastIndexOf("<br />"));
                        strArray2[0] = strArray2[0].Substring(0, strArray2[0].LastIndexOf("<br />"));
                        return StripTags(strArray2[0]);
                    }
                    return null;
                }
            }
            if (song.Contains(" "))
            {
                text = this.HttpPost(uri, "?mode=1&type=1&search[]=" + song.Replace(" ", ""));
                strArray = this.returnSongs(text);
                song = song.Replace(" ", "");
            }
            foreach (string str5 in strArray)
            {
                if (str5.ToUpper().Contains(artist.ToUpper()) || str5.ToUpper().Contains(artist.ToUpper().Replace(" ", "")))
                {
                    uri = "http://im.new21.org/alpha/guljabada25/gulja_view_2500.php";
                    string str6 = "index=" + str5.Substring(0, str5.IndexOf('|'));
                    text = this.HttpPost(uri, str6);
                    string[] strArray3 = this.returnSubstrings(text, "<tr valign=\"top\">", "</tr>");
                    if (strArray3.Length > 0)
                    {
                        strArray3[0] = strArray3[0].Substring(0, strArray3[0].LastIndexOf("<br />"));
                        strArray3[0] = strArray3[0].Substring(0, strArray3[0].LastIndexOf("<br />"));
                        return StripTags(strArray3[0]);
                    }
                    return null;
                }
            }
            return this.getSong2(artist, song);
        }

        public string getSong2(string artist, string song)
        {
            string uri = "http://im.new21.org/alpha/guljabada25/gulja_list_2500.php";
            string text = this.HttpPost(uri, "?mode=1&type=1&search[]=" + artist);
            foreach (string str3 in this.returnSongs(text))
            {
                if ((str3.Contains(song) || str3.Contains(song.Replace(" ", ""))) || (str3.Replace(" ", "").Contains(song) || str3.Replace(" ", "").Contains(song.Replace(" ", ""))))
                {
                    uri = "http://im.new21.org/alpha/guljabada25/gulja_view_2500.php";
                    string parameters = "index=" + str3.Substring(0, str3.IndexOf('|'));
                    text = this.HttpPost(uri, parameters);
                    string[] strArray2 = this.returnSubstrings(text, "<tr valign=\"top\">", "</tr>");
                    if (strArray2.Length > 0)
                    {
                        strArray2[0] = strArray2[0].Substring(0, strArray2[0].LastIndexOf("<br />"));
                        strArray2[0] = strArray2[0].Substring(0, strArray2[0].LastIndexOf("<br />"));
                        return StripTags(strArray2[0]);
                    }
                    return null;
                }
            }
            text = this.HttpPost(uri, "?mode=1&type=1&search[]=" + artist);
            foreach (string str5 in this.returnSongs(text))
            {
                if (str5.ToUpper().Contains(song.ToUpper()) || str5.ToUpper().Contains(song.ToUpper().Replace(" ", "")))
                {
                    uri = "http://im.new21.org/alpha/guljabada25/gulja_view_2500.php";
                    string str6 = "index=" + str5.Substring(0, str5.IndexOf('|'));
                    text = this.HttpPost(uri, str6);
                    string[] strArray3 = this.returnSubstrings(text, "<tr valign=\"top\">", "</tr>");
                    if (strArray3.Length > 0)
                    {
                        strArray3[0] = strArray3[0].Substring(0, strArray3[0].LastIndexOf("<br />"));
                        strArray3[0] = strArray3[0].Substring(0, strArray3[0].LastIndexOf("<br />"));
                        return StripTags(strArray3[0]);
                    }
                    return null;
                }
            }
            return "";
        }

        private string HttpPost(string uri, string parameters)
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
                WebResponse response = request.GetResponse();
                if (response == null)
                {
                    return null;
                }
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("EUC-KR"));
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

        private string[] returnSongs(string text)
        {
            string[] strArray = text.Replace("Guljabada2|\n", "").Split(new char[] { '|' });
            ArrayList list = new ArrayList();
            int num = 1;
            int num2 = 0;
            string str = "";
            foreach (string str2 in strArray)
            {
                str = str + str2 + "|";
                if ((num++ % 5) == 0)
                {
                    num2++;
                    list.Add(str);
                    str = "";
                }
            }
            return (string[]) list.ToArray(typeof(string));
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

