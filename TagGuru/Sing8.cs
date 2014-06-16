namespace TagGuru
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;

    internal class Sing8
    {
        public MassUpdater m_form;

        public bool checkSongExists(string artist, string song)
        {
            string uri = "http://www.sing8.com/sch.php";
            string text = this.HttpPost(uri, "kw=" + song + "&clz=1");
            if (text != null)
            {
                foreach (string str3 in this.returnSubstrings(text, "<li class=\"gm\"><a href=\"", "<li class=\"ls\">"))
                {
                    string[] strArray2 = this.returnSubstrings(str3, "<li class=\"gs\">", "</li>");
                    if ((strArray2[0].Contains(artist) || strArray2[0].Contains(artist.Replace(" ", ""))) || (strArray2[0].Replace(" ", "").Contains(artist) || strArray2[0].Replace(" ", "").Contains(artist.Replace(" ", ""))))
                    {
                        return true;
                    }
                    if (this.m_form.CheckMultiArtist(artist, strArray2[0]))
                    {
                        return true;
                    }
                }
                string[] strArray3 = this.returnSubstrings(text, "javascript:nextpg(", ");");
                if (strArray3.Length > 0)
                {
                    int num = 0;
                    foreach (string str4 in strArray3)
                    {
                        text = this.HttpPost(uri + "?pg=" + str4, "kw=" + song + "&clz=4");
                        foreach (string str5 in this.returnSubstrings(text, "<li class=\"gm\"><a href=\"", "<li class=\"ls\">"))
                        {
                            string[] strArray4 = this.returnSubstrings(str5, "<li class=\"gs\">", "</li>");
                            if ((strArray4[0].Contains(artist) || strArray4[0].Contains(artist.Replace(" ", ""))) || (strArray4[0].Replace(" ", "").Contains(artist) || strArray4[0].Replace(" ", "").Contains(artist.Replace(" ", ""))))
                            {
                                return true;
                            }
                            if (this.m_form.CheckMultiArtist(artist, strArray4[0]))
                            {
                                return true;
                            }
                        }
                        if (num++ > 5)
                        {
                            break;
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

        private byte[] gbme(string src)
        {
            Encoding dstEncoding = Encoding.GetEncoding("GB2312");
            Encoding unicode = Encoding.Unicode;
            return Encoding.Convert(unicode, dstEncoding, unicode.GetBytes(src));
        }

        public string getSong(string artist, string song)
        {
            string uri = "http://www.sing8.com/sch.php";
            string text = this.HttpPost(uri, "kw=" + song + "&clz=1");
            string[] strArray = this.returnSubstrings(text, "<li class=\"gm\"><a href=\"", "<li class=\"ls\">");
            if (strArray.Length == 1)
            {
                uri = "http://sing8.com";
                text = this.HttpPost(uri + strArray[0].Substring(0, strArray[0].IndexOf('"')), "");
                string[] strArray2 = this.returnSubstrings(text, "<div id=\"lrc\" >", "</div>");
                if (strArray2.Length > 0)
                {
                    return StripTags(strArray2[0]);
                }
                return null;
            }
            foreach (string str4 in strArray)
            {
                string[] strArray3 = this.returnSubstrings(str4, "<li class=\"gs\">", "</li>");
                if ((strArray3[0].Contains(artist) || strArray3[0].Contains(artist.Replace(" ", ""))) || ((strArray3[0].Replace(" ", "").Contains(artist) || strArray3[0].Replace(" ", "").Contains(artist.Replace(" ", ""))) || this.m_form.CheckMultiArtist(artist, strArray3[0])))
                {
                    uri = "http://sing8.com";
                    text = this.HttpPost(uri + str4.Substring(0, strArray[0].IndexOf('"')), "");
                    string[] strArray4 = this.returnSubstrings(text, "<div id=\"lrc\" >", "</div>");
                    if (strArray4.Length > 0)
                    {
                        return StripTags(strArray4[0]);
                    }
                    return null;
                }
            }
            string[] strArray5 = this.returnSubstrings(text, "javascript:nextpg(", ");");
            if (strArray5.Length > 0)
            {
                foreach (string str6 in strArray5)
                {
                    text = this.HttpPost(uri + "?pg=" + str6, "kw=" + song + "&clz=4");
                    strArray = this.returnSubstrings(text, "<li class=\"gm\"><a href=\"", "<li class=\"ls\">");
                    foreach (string str7 in strArray)
                    {
                        string[] strArray6 = this.returnSubstrings(str7, "<li class=\"gs\">", "</li>");
                        if ((strArray6[0].Contains(artist) || strArray6[0].Contains(artist.Replace(" ", ""))) || ((strArray6[0].Replace(" ", "").Contains(artist) || strArray6[0].Replace(" ", "").Contains(artist.Replace(" ", ""))) || this.m_form.CheckMultiArtist(artist, strArray6[0])))
                        {
                            text = this.HttpPost("http://sing8.com" + str7.Substring(0, strArray[0].IndexOf('"')), "");
                            string[] strArray7 = this.returnSubstrings(text, "<div id=\"lrc\" >", "</div>");
                            if (strArray7.Length > 0)
                            {
                                return StripTags(strArray7[0]);
                            }
                            return null;
                        }
                    }
                }
            }
            return null;
        }

        private string HttpPost(string uri, string parameters)
        {
            WebRequest request = WebRequest.Create(uri);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            byte[] buffer = this.gbme(parameters);
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
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("GB2312"));
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

        private string HttpPost2(string uri, string parameters)
        {
            string str2 = string.Empty;
            WebRequest request = WebRequest.Create(uri);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "GET";
            request.Headers.Add("Referer: http://www.jpwy.net/gc/search.php");
            request.Headers.Add("Cookie: cck_count=0; cck_lasttime=0;is_use_cookie=yes; is_get_1013917=yes ");
            try
            {
                WebResponse response = request.GetResponse();
                if (response == null)
                {
                    return null;
                }
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("GB2312"));
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
            return Regex.Replace(Regex.Replace(HTML.Replace("<br />", "\r\n").Replace("<br>", "\r\n"), "<[^>]*>", ""), @"(\(|\{|\[|<).*(\)|\}|\]|>)", "").Replace("&lt;", "<").Replace("www.Sing8.com", "").Replace("Sing8.com", "").Trim();
        }
    }
}

