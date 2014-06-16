namespace RuruTunesAutoPutLyrix
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    internal class AutoUpdate
    {
        private string curBuild;
        private string curVersion;
        private RuruTunesAutoPutLyrix RuruTunesAutoPutLyrix;

        public AutoUpdate(RuruTunesAutoPutLyrix tg)
        {
            this.RuruTunesAutoPutLyrix = tg;
            string text = this.RuruTunesAutoPutLyrix.lblVersion.Text;
            this.curVersion = this.returnSubstrings(text, "RuruTunesAutoPutLyrix ", " -")[0];
            this.curBuild = this.returnSubstrings(text + "aa", "- ", "aa")[0];
        }

        private string HttpPost(string uri, string parameters)
        {
            string str = string.Empty;
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

        public void UpdateMe()
        {
            try
            {
                string text = "";
                try
                {
                    string uri = "http://xguru.egloos.com/4037376";
                    text = this.HttpPost(uri, "");
                }
                catch
                {
                }
                string str3 = this.returnSubstrings(text, "<version>", "</version>")[0];
                string s = this.returnSubstrings(text, "<build>", "</build>")[0];
                string address = this.returnSubstrings(text, "<url>", "</url>")[0];
                string fileName = this.returnSubstrings(text, "<gourl>", "</gourl>")[0];
                if (((int.Parse(str3.Replace(".", "")) > int.Parse(this.curVersion.Replace(".", ""))) || (int.Parse(s) > int.Parse(this.curBuild))) && (MessageBox.Show("RuruTunesAutoPutLyrix 가 [" + str3 + " - " + s + "] 버전으로 업데이트 되었습니다." + Environment.NewLine + "새로운 버전으로 업데이트 하시겠습니까 ?" + Environment.NewLine + Environment.NewLine + "[확인]을 누르면 새 버전을 다운로드하고 설치를 시작합니다.", "RuruTunesAutoPutLyrix AutoUpdate", MessageBoxButtons.OKCancel) == DialogResult.OK))
                {
                    Directory.CreateDirectory(Path.GetTempPath() + @"\lyricguru\");
                    string path = Path.GetTempPath() + @"lyricguru\RuruTunesAutoPutLyrix_" + str3 + s + ".exe";
                    WebClient client = new WebClient();
                    try
                    {
                        System.IO.File.Delete(path);
                    }
                    catch
                    {
                    }
                    if (!System.IO.File.Exists(path))
                    {
                        client.DownloadFile(address, path);
                    }
                    Process.Start(fileName);
                    Thread.Sleep(0x7d0);
                    Process.Start(path);
                    Application.Exit();
                }
            }
            catch (Exception)
            {
            }
        }
    }
}

