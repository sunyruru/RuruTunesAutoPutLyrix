namespace RuruTunesAutoPutLyrix
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;

    internal class MNet
    {
        public MassUpdater m_form;

        public bool checkSongExists(string artist, string song)
        {
            string uri = "http://search.mnet.com/search_SongName.asp";
            string text = this.HttpPost(uri, "searchArea=SONG&searchWord=" + song + "&findWord=" + song);
            if (text != null)
            {
                foreach (string str3 in this.returnSubstrings(text, "javascript:fnArtistInfo2", "</a>"))
                {
                    if ((str3.Contains(artist) || str3.Contains(artist.Replace(" ", ""))) || (str3.Replace(" ", "").Contains(artist) || str3.Replace(" ", "").Contains(artist.Replace(" ", ""))))
                    {
                        return true;
                    }
                    if (this.m_form.CheckMultiArtist(artist, str3))
                    {
                        return true;
                    }
                }
                string[] strArray2 = this.returnSubstrings(text, "PageNum=", " ");
                if (strArray2.Length > 0)
                {
                    int num = 0;
                    foreach (string str4 in strArray2)
                    {
                        text = this.HttpPost(uri, "searchArea=SONG&searchWord=" + song + "&findWord=" + song + "&pageNum=" + str4);
                        foreach (string str5 in this.returnSubstrings(text, "javascript:fnArtistInfo2", "</a>"))
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
                        if (num++ > 5)
                        {
                            break;
                        }
                    }
                }
                uri = "http://search.mnet.com/search_SongSinger.asp";
                text = this.HttpPost(uri, "searchArea=SONG&searchWord=" + artist + "&findWord=" + artist);
                if (text == null)
                {
                    return false;
                }
                foreach (string str6 in this.returnSubstrings(text, "<td class=\"Lpd_3006\">", "</td>"))
                {
                    if ((str6.Contains(song) || str6.Contains(song.Replace(" ", ""))) || (str6.Replace(" ", "").Contains(song) || str6.Replace(" ", "").Contains(song.Replace(" ", ""))))
                    {
                        return true;
                    }
                }
                strArray2 = this.returnSubstrings(text, "PageNum=", " ");
                if (strArray2.Length > 0)
                {
                    int num2 = 0;
                    foreach (string str7 in strArray2)
                    {
                        text = this.HttpPost(uri, "searchArea=SONG&searchWord=" + artist + "&findWord=" + artist + "&pageNum=" + str7);
                        foreach (string str8 in this.returnSubstrings(text, "javascript:fnArtistInfo2", "</a>"))
                        {
                            if ((str8.Contains(song) || str8.Contains(song.Replace(" ", ""))) || (str8.Replace(" ", "").Contains(song) || str8.Replace(" ", "").Contains(song.Replace(" ", ""))))
                            {
                                return true;
                            }
                        }
                        if (num2++ > 5)
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

        public string getSong(string artist, string song)
        {
            string uri = "http://search.mnet.com/search_SongName.asp";
            string text = this.HttpPost(uri, "searchArea=SONG&searchWord=" + song + "&findWord=" + song);
            string[] strArray = this.returnSubstrings(text, "name=\"CheckSongID\"", "title=\"가사\"");
            if (strArray.Length == 1)
            {
                uri = "http://music.mnet.com/Lyrics/LyricsView.asp";
                string parameters = "SongID=" + this.returnSubstrings(strArray[0], "javascript:fnLyricsView2('", "'")[0];
                text = this.HttpPost(uri, parameters);
                string[] strArray2 = this.returnSubstrings(text, "<div  id=\"divLyrics\"", "</div>");
                if (strArray2.Length > 0)
                {
                    return StripTags("<div " + strArray2[0]);
                }
                return null;
            }
            foreach (string str4 in strArray)
            {
                if ((str4.Contains(artist) || str4.Contains(artist.Replace(" ", ""))) || ((str4.Replace(" ", "").Contains(artist) || str4.Replace(" ", "").Contains(artist.Replace(" ", ""))) || this.m_form.CheckMultiArtist(artist, str4)))
                {
                    uri = "http://music.mnet.com/Lyrics/LyricsView.asp";
                    string str5 = "SongID=" + this.returnSubstrings(str4, "javascript:fnLyricsView2('", "'")[0];
                    text = this.HttpPost(uri, str5);
                    string[] strArray3 = this.returnSubstrings(text, "<div  id=\"divLyrics\"", "</div>");
                    if (strArray3.Length > 0)
                    {
                        return StripTags("<div " + strArray3[0]);
                    }
                    return null;
                }
            }
            string[] strArray4 = this.returnSubstrings(text, "PageNum=", " ");
            if (strArray4.Length > 0)
            {
                int num = 0;
                foreach (string str6 in strArray4)
                {
                    text = this.HttpPost(uri, "searchArea=SONG&searchWord=" + song + "&findWord=" + song + "&pageNum=" + str6);
                    foreach (string str7 in this.returnSubstrings(text, "MLi_F8_01 algnc", "title=\"가사\""))
                    {
                        if ((str7.Contains(artist) || str7.Contains(artist.Replace(" ", ""))) || ((str7.Replace(" ", "").Contains(artist) || str7.Replace(" ", "").Contains(artist.Replace(" ", ""))) || this.m_form.CheckMultiArtist(artist, str7)))
                        {
                            uri = "http://music.mnet.com/Lyrics/LyricsView.asp";
                            string str8 = "SongID=" + this.returnSubstrings(str7, "javascript:fnLyricsView2('", "'")[0];
                            text = this.HttpPost(uri, str8);
                            string[] strArray5 = this.returnSubstrings(text, "<div  id=\"divLyrics\"", "</div>");
                            if (strArray5.Length > 0)
                            {
                                return StripTags("<div " + strArray5[0]);
                            }
                            return null;
                        }
                    }
                    if (num++ > 5)
                    {
                        break;
                    }
                }
            }
            return this.getSong2(artist, song);
        }

        public string getSong2(string artist, string song)
        {
            string uri = "http://search.mnet.com/search_SongSinger.asp";
            string text = this.HttpPost(uri, "searchArea=SONG&searchWord=" + artist + "&findWord=" + artist);
            foreach (string str3 in this.returnSubstrings(text, "MLi_F8_01 algnc", "title=\"가사\""))
            {
                if ((str3.Contains(song) || str3.Contains(song.Replace(" ", ""))) || (str3.Replace(" ", "").Contains(song) || str3.Replace(" ", "").Contains(song.Replace(" ", ""))))
                {
                    uri = "http://music.mnet.com/Lyrics/LyricsView.asp";
                    string parameters = "SongID=" + this.returnSubstrings(str3, "javascript:fnLyricsView2('", "'")[0];
                    text = this.HttpPost(uri, parameters);
                    string[] strArray2 = this.returnSubstrings(text, "<div  id=\"divLyrics\"", "</div>");
                    if (strArray2.Length > 0)
                    {
                        return StripTags("<div " + strArray2[0]);
                    }
                    return null;
                }
            }
            string[] strArray3 = this.returnSubstrings(text, "PageNum=", " ");
            if (strArray3.Length > 0)
            {
                int num = 0;
                foreach (string str5 in strArray3)
                {
                    text = this.HttpPost(uri, "searchArea=SONG&searchWord=" + artist + "&findWord=" + artist + "&pageNum=" + str5);
                    foreach (string str6 in this.returnSubstrings(text, "MLi_F8_01 algnc", "title=\"가사\""))
                    {
                        if ((str6.Contains(artist) || str6.Contains(artist.Replace(" ", ""))) || ((str6.Replace(" ", "").Contains(artist) || str6.Replace(" ", "").Contains(artist.Replace(" ", ""))) || this.m_form.CheckMultiArtist(artist, str6)))
                        {
                            uri = "http://music.mnet.com/Lyrics/LyricsView.asp";
                            string str7 = "SongID=" + this.returnSubstrings(str6, "javascript:fnLyricsView2('", "'")[0];
                            text = this.HttpPost(uri, str7);
                            string[] strArray4 = this.returnSubstrings(text, "<div  id=\"divLyrics\"", "</div>");
                            if (strArray4.Length > 0)
                            {
                                return StripTags("<div " + strArray4[0]);
                            }
                            return null;
                        }
                    }
                    if (num++ > 5)
                    {
                        break;
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
            return Regex.Replace(HTML, "<[^>]*>", "").Replace("&lt;", "<").Replace("\t", "");
        }
    }
}

