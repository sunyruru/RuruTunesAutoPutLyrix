namespace RuruTunesAutoPutLyrix
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;

    internal class inMuz
    {
        public MassUpdater m_form;

        public bool checkSongExists(string artist, string song)
        {
            string uri = "http://www.inmuz.com/";
            string text = this.HttpPost(uri, "act=IS&mid=main&x=0&y=0&search_target=title_extra_vars1&is_keyword=" + song);
            if (text != null)
            {
                foreach (string str3 in this.returnSubstrings(text, "<a href=\"http://www.inmuz.com/", "</a>"))
                {
                    if (str3.Contains(artist))
                    {
                        return true;
                    }
                    if (this.m_form.CheckMultiArtist(artist, str3))
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

        public string getSong(string artist, string song)
        {
            string uri = "http://www.inmuz.com/";
            string text = this.HttpPost(uri, "act=IS&mid=main&x=0&y=0&search_target=title_extra_vars1&is_keyword=" + song);
            foreach (string str3 in this.returnSubstrings(text, "<a href=\"http://www.inmuz.com/", "</a>"))
            {
                if ((str3.Contains(artist) || str3.Contains(artist.Replace(" ", ""))) || ((str3.Replace(" ", "").Contains(artist) || str3.Replace(" ", "").Contains(artist.Replace(" ", ""))) || this.m_form.CheckMultiArtist(artist, str3)))
                {
                    uri = "http://inmuz.com/" + str3.Substring(0, str3.IndexOf('"'));
                    text = this.HttpPost(uri, "");
                    string[] strArray2 = this.returnSubstrings(text, "<div class=\"readHeader\">", "<div class=\"document_popup_menu\">");
                    if (strArray2.Length > 0)
                    {
                        return StripTags(strArray2[0]);
                    }
                    return null;
                }
            }
            return null;
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
            return Regex.Replace(HTML, "<[^>]*>", "");
        }
    }
}

