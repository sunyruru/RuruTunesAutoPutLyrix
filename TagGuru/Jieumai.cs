namespace TagGuru
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    internal class Jieumai
    {
        public MassUpdater m_form;

        public bool checkSongExists(string artist, string msong)
        {
            string str = null;
            if ((msong != "") && (msong != null))
            {
                str = HttpUtility.UrlEncode(this.eucme(msong));
            }
            string str2 = "http://www.jieumai.com/zboard/zboard.php?id=lyrics&";
            string text = this.HttpPost(str2 + "keyword=" + str, "");
            if (text != null)
            {
                foreach (string str4 in this.returnSubstrings(text, "<input type=\"checkbox\" name=\"cart\"", "</tr>"))
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
                string[] strArray2 = this.returnSubstrings(text, "<a class=\"link_move\"", "[계속 검색]");
                if (strArray2.Length > 0)
                {
                    string[] strArray3 = this.returnSubstrings(strArray2[0], "divpage=", "'");
                    for (int i = int.Parse(strArray3[0]); i > 0; i--)
                    {
                        str2 = "http://www.jieumai.com/zboard/zboard.php?id=lyrics&&select_arrange=headnum&desc=asc&category=&sn=off&ss=on&sc=on&";
                        text = this.HttpPost(str2 + "keyword=" + str + "&page=" + i.ToString() + "&divpage=" + strArray3[0], "");
                        foreach (string str5 in this.returnSubstrings(text, "<input type=\"checkbox\" name=\"cart\"", "</tr>"))
                        {
                            if ((str5.Contains(artist) || str5.Contains(artist.Replace(" ", ""))) || (str5.Replace(" ", "").Contains(artist) || str5.Replace(" ", "").Contains(artist.Replace(" ", ""))))
                            {
                                return true;
                            }
                            if (this.m_form.CheckMultiArtist(artist, str5))
                            {
                                return true;
                            }
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

        public string getSong(string artist, string msong)
        {
            string str = null;
            if ((msong != "") && (msong != null))
            {
                str = HttpUtility.UrlEncode(this.eucme(msong));
            }
            string uri = "http://www.jieumai.com/zboard/zboard.php?id=lyrics&";
            string text = this.HttpPost(uri + "keyword=" + str, "");
            foreach (string str4 in this.returnSubstrings(text, "<input type=\"checkbox\" name=\"cart\"", "</tr>"))
            {
                if ((str4.Contains(artist) || str4.Contains(artist.Replace(" ", ""))) || ((str4.Replace(" ", "").Contains(artist) || str4.Replace(" ", "").Contains(artist.Replace(" ", ""))) || this.m_form.CheckMultiArtist(artist, str4)))
                {
                    string[] strArray2 = this.returnSubstrings(str4, "&no=", "\"");
                    if (strArray2.Length > 0)
                    {
                        uri = "http://www.jieumai.com/zboard/zboard.php?id=lyrics&no=" + strArray2[0];
                        text = this.HttpPost(uri, null);
                        string[] strArray3 = this.returnSubstrings(text, "<div id=\"viewMemo\">", "</div>");
                        if (strArray3.Length > 0)
                        {
                            return StripTags(strArray3[0]);
                        }
                        return null;
                    }
                }
            }
            string[] strArray4 = this.returnSubstrings(text, "<a class=\"link_move\"", "[계속 검색]");
            if (strArray4.Length > 0)
            {
                string[] strArray5 = this.returnSubstrings(strArray4[0], "divpage=", "'");
                for (int i = int.Parse(strArray5[0]); i > 0; i--)
                {
                    uri = "http://www.jieumai.com/zboard/zboard.php?id=lyrics&&select_arrange=headnum&desc=asc&category=&sn=off&ss=on&sc=on&";
                    text = this.HttpPost(uri + "keyword=" + str + "&page=" + i.ToString() + "&divpage=" + strArray5[0], "");
                    foreach (string str5 in this.returnSubstrings(text, "<input type=\"checkbox\" name=\"cart\"", "</tr>"))
                    {
                        if ((str5.Contains(artist) || str5.Contains(artist.Replace(" ", ""))) || ((str5.Replace(" ", "").Contains(artist) || str5.Replace(" ", "").Contains(artist.Replace(" ", ""))) || this.m_form.CheckMultiArtist(artist, str5)))
                        {
                            string[] strArray6 = this.returnSubstrings(str5, "&no=", "\"");
                            if (strArray6.Length > 0)
                            {
                                uri = "http://www.jieumai.com/zboard/zboard.php?id=lyrics&no=" + strArray6[0];
                                text = this.HttpPost(uri, null);
                                string[] strArray7 = this.returnSubstrings(text, "<div id=\"viewMemo\">", "</div>");
                                if (strArray7.Length > 0)
                                {
                                    return StripTags(strArray7[0]);
                                }
                                return null;
                            }
                        }
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

