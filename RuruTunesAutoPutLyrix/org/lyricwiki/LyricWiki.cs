namespace RuruTunesAutoPutLyrix.org.lyricwiki
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web.Services;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using Properties;

    [GeneratedCode("System.Web.Services", "2.0.50727.3053"), WebServiceBinding(Name="LyricWikiBinding", Namespace="urn:LyricWiki"), DebuggerStepThrough, DesignerCategory("code")]
    public class LyricWiki : SoapHttpClientProtocol
    {
        private SendOrPostCallback checkSongExistsOperationCompleted;
        private SendOrPostCallback getAlbumOperationCompleted;
        private SendOrPostCallback getArtistOperationCompleted;
        private SendOrPostCallback getHometownOperationCompleted;
        private SendOrPostCallback getSongOperationCompleted;
        private SendOrPostCallback getSongResultOperationCompleted;
        private SendOrPostCallback getSOTDOperationCompleted;
        private SendOrPostCallback postAlbumOperationCompleted;
        private SendOrPostCallback postArtistOperationCompleted;
        private SendOrPostCallback postSong_flagsOperationCompleted;
        private SendOrPostCallback postSongOperationCompleted;
        private SendOrPostCallback searchAlbumsOperationCompleted;
        private SendOrPostCallback searchArtistsOperationCompleted;
        private SendOrPostCallback searchSongsOperationCompleted;
        private bool useDefaultCredentialsSetExplicitly;

        public event checkSongExistsCompletedEventHandler checkSongExistsCompleted;

        public event getAlbumCompletedEventHandler getAlbumCompleted;

        public event getArtistCompletedEventHandler getArtistCompleted;

        public event getHometownCompletedEventHandler getHometownCompleted;

        public event getSongCompletedEventHandler getSongCompleted;

        public event getSongResultCompletedEventHandler getSongResultCompleted;

        public event getSOTDCompletedEventHandler getSOTDCompleted;

        public event postAlbumCompletedEventHandler postAlbumCompleted;

        public event postArtistCompletedEventHandler postArtistCompleted;

        public event postSong_flagsCompletedEventHandler postSong_flagsCompleted;

        public event postSongCompletedEventHandler postSongCompleted;

        public event searchAlbumsCompletedEventHandler searchAlbumsCompleted;

        public event searchArtistsCompletedEventHandler searchArtistsCompleted;

        public event searchSongsCompletedEventHandler searchSongsCompleted;

        public LyricWiki()
        {
            this.Url = Settings.Default.RuruTunesAutoPutLyrix_org_lyricwiki_LyricWiki;
            if (this.IsLocalFileSystemWebService(this.Url))
            {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else
            {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }

        public new void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }

        [return: SoapElement("return")]
        [SoapRpcMethod("urn:LyricWiki#checkSongExists", RequestNamespace="urn:LyricWiki", ResponseNamespace="urn:LyricWiki")]
        public bool checkSongExists(string artist, string song)
        {
            return (bool) base.Invoke("checkSongExists", new object[] { artist, song })[0];
        }

        public void checkSongExistsAsync(string artist, string song)
        {
            this.checkSongExistsAsync(artist, song, null);
        }

        public void checkSongExistsAsync(string artist, string song, object userState)
        {
            if (this.checkSongExistsOperationCompleted == null)
            {
                this.checkSongExistsOperationCompleted = new SendOrPostCallback(this.OncheckSongExistsOperationCompleted);
            }
            base.InvokeAsync("checkSongExists", new object[] { artist, song }, this.checkSongExistsOperationCompleted, userState);
        }

        [return: SoapElement("amazonLink")]
        [SoapRpcMethod("urn:LyricWiki#getAlbum", RequestNamespace="urn:LyricWiki", ResponseNamespace="urn:LyricWiki")]
        public string getAlbum(ref string artist, ref string album, ref int year, out string[] songs)
        {
            object[] objArray = base.Invoke("getAlbum", new object[] { artist, album, (int) year });
            artist = (string) objArray[1];
            album = (string) objArray[2];
            year = (int) objArray[3];
            songs = (string[]) objArray[4];
            return (string) objArray[0];
        }

        public void getAlbumAsync(string artist, string album, int year)
        {
            this.getAlbumAsync(artist, album, year, null);
        }

        public void getAlbumAsync(string artist, string album, int year, object userState)
        {
            if (this.getAlbumOperationCompleted == null)
            {
                this.getAlbumOperationCompleted = new SendOrPostCallback(this.OngetAlbumOperationCompleted);
            }
            base.InvokeAsync("getAlbum", new object[] { artist, album, year }, this.getAlbumOperationCompleted, userState);
        }

        [return: SoapElement("albums")]
        [SoapRpcMethod("urn:LyricWiki#getArtist", RequestNamespace="urn:LyricWiki", ResponseNamespace="urn:LyricWiki")]
        public string[] getArtist(ref string artist)
        {
            object[] objArray = base.Invoke("getArtist", new object[] { artist });
            artist = (string) objArray[1];
            return (string[]) objArray[0];
        }

        public void getArtistAsync(string artist)
        {
            this.getArtistAsync(artist, null);
        }

        public void getArtistAsync(string artist, object userState)
        {
            if (this.getArtistOperationCompleted == null)
            {
                this.getArtistOperationCompleted = new SendOrPostCallback(this.OngetArtistOperationCompleted);
            }
            base.InvokeAsync("getArtist", new object[] { artist }, this.getArtistOperationCompleted, userState);
        }

        [return: SoapElement("country")]
        [SoapRpcMethod("urn:LyricWiki#getHometown", RequestNamespace="urn:LyricWiki", ResponseNamespace="urn:LyricWiki")]
        public string getHometown(string artist, out string state, out string hometown)
        {
            object[] objArray = base.Invoke("getHometown", new object[] { artist });
            state = (string) objArray[1];
            hometown = (string) objArray[2];
            return (string) objArray[0];
        }

        public void getHometownAsync(string artist)
        {
            this.getHometownAsync(artist, null);
        }

        public void getHometownAsync(string artist, object userState)
        {
            if (this.getHometownOperationCompleted == null)
            {
                this.getHometownOperationCompleted = new SendOrPostCallback(this.OngetHometownOperationCompleted);
            }
            base.InvokeAsync("getHometown", new object[] { artist }, this.getHometownOperationCompleted, userState);
        }

        [return: SoapElement("return")]
        [SoapRpcMethod("urn:LyricWiki#getSong", RequestNamespace="urn:LyricWiki", ResponseNamespace="urn:LyricWiki")]
        public LyricsResult getSong(string artist, string song)
        {
            return (LyricsResult) base.Invoke("getSong", new object[] { artist, song })[0];
        }

        public void getSongAsync(string artist, string song)
        {
            this.getSongAsync(artist, song, null);
        }

        public void getSongAsync(string artist, string song, object userState)
        {
            if (this.getSongOperationCompleted == null)
            {
                this.getSongOperationCompleted = new SendOrPostCallback(this.OngetSongOperationCompleted);
            }
            base.InvokeAsync("getSong", new object[] { artist, song }, this.getSongOperationCompleted, userState);
        }

        [return: SoapElement("songResult")]
        [SoapRpcMethod("urn:LyricWiki#getSongResult", RequestNamespace="urn:LyricWiki", ResponseNamespace="urn:LyricWiki")]
        public LyricsResult getSongResult(string artist, string song)
        {
            return (LyricsResult) base.Invoke("getSongResult", new object[] { artist, song })[0];
        }

        public void getSongResultAsync(string artist, string song)
        {
            this.getSongResultAsync(artist, song, null);
        }

        public void getSongResultAsync(string artist, string song, object userState)
        {
            if (this.getSongResultOperationCompleted == null)
            {
                this.getSongResultOperationCompleted = new SendOrPostCallback(this.OngetSongResultOperationCompleted);
            }
            base.InvokeAsync("getSongResult", new object[] { artist, song }, this.getSongResultOperationCompleted, userState);
        }

        [return: SoapElement("return")]
        [SoapRpcMethod("urn:LyricWiki#getSOTD", RequestNamespace="urn:LyricWiki", ResponseNamespace="urn:LyricWiki")]
        public SOTDResult getSOTD()
        {
            return (SOTDResult) base.Invoke("getSOTD", new object[0])[0];
        }

        public void getSOTDAsync()
        {
            this.getSOTDAsync(null);
        }

        public void getSOTDAsync(object userState)
        {
            if (this.getSOTDOperationCompleted == null)
            {
                this.getSOTDOperationCompleted = new SendOrPostCallback(this.OngetSOTDOperationCompleted);
            }
            base.InvokeAsync("getSOTD", new object[0], this.getSOTDOperationCompleted, userState);
        }

        private bool IsLocalFileSystemWebService(string url)
        {
            if ((url == null) || (url == string.Empty))
            {
                return false;
            }
            Uri uri = new Uri(url);
            return ((uri.Port >= 0x400) && (string.Compare(uri.Host, "localHost", StringComparison.OrdinalIgnoreCase) == 0));
        }

        private void OncheckSongExistsOperationCompleted(object arg)
        {
            if (this.checkSongExistsCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.checkSongExistsCompleted(this, new checkSongExistsCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void OngetAlbumOperationCompleted(object arg)
        {
            if (this.getAlbumCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.getAlbumCompleted(this, new getAlbumCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void OngetArtistOperationCompleted(object arg)
        {
            if (this.getArtistCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.getArtistCompleted(this, new getArtistCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void OngetHometownOperationCompleted(object arg)
        {
            if (this.getHometownCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.getHometownCompleted(this, new getHometownCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void OngetSongOperationCompleted(object arg)
        {
            if (this.getSongCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.getSongCompleted(this, new getSongCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void OngetSongResultOperationCompleted(object arg)
        {
            if (this.getSongResultCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.getSongResultCompleted(this, new getSongResultCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void OngetSOTDOperationCompleted(object arg)
        {
            if (this.getSOTDCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.getSOTDCompleted(this, new getSOTDCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void OnpostAlbumOperationCompleted(object arg)
        {
            if (this.postAlbumCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.postAlbumCompleted(this, new postAlbumCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void OnpostArtistOperationCompleted(object arg)
        {
            if (this.postArtistCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.postArtistCompleted(this, new postArtistCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void OnpostSong_flagsOperationCompleted(object arg)
        {
            if (this.postSong_flagsCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.postSong_flagsCompleted(this, new postSong_flagsCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void OnpostSongOperationCompleted(object arg)
        {
            if (this.postSongCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.postSongCompleted(this, new postSongCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void OnsearchAlbumsOperationCompleted(object arg)
        {
            if (this.searchAlbumsCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.searchAlbumsCompleted(this, new searchAlbumsCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void OnsearchArtistsOperationCompleted(object arg)
        {
            if (this.searchArtistsCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.searchArtistsCompleted(this, new searchArtistsCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void OnsearchSongsOperationCompleted(object arg)
        {
            if (this.searchSongsCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.searchSongsCompleted(this, new searchSongsCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        [return: SoapElement("dataUsed")]
        [SoapRpcMethod("urn:LyricWiki#postAlbum", RequestNamespace="urn:LyricWiki", ResponseNamespace="urn:LyricWiki")]
        public bool postAlbum(bool overwriteIfExists, ref string artist, ref string album, ref int year, string asin, string[] songs, out string message)
        {
            object[] objArray = base.Invoke("postAlbum", new object[] { overwriteIfExists, artist, album, (int) year, asin, songs });
            artist = (string) objArray[1];
            album = (string) objArray[2];
            year = (int) objArray[3];
            message = (string) objArray[4];
            return (bool) objArray[0];
        }

        public void postAlbumAsync(bool overwriteIfExists, string artist, string album, int year, string asin, string[] songs)
        {
            this.postAlbumAsync(overwriteIfExists, artist, album, year, asin, songs, null);
        }

        public void postAlbumAsync(bool overwriteIfExists, string artist, string album, int year, string asin, string[] songs, object userState)
        {
            if (this.postAlbumOperationCompleted == null)
            {
                this.postAlbumOperationCompleted = new SendOrPostCallback(this.OnpostAlbumOperationCompleted);
            }
            base.InvokeAsync("postAlbum", new object[] { overwriteIfExists, artist, album, year, asin, songs }, this.postAlbumOperationCompleted, userState);
        }

        [return: SoapElement("dataUsed")]
        [SoapRpcMethod("urn:LyricWiki#postArtist", RequestNamespace="urn:LyricWiki", ResponseNamespace="urn:LyricWiki")]
        public bool postArtist(bool overwriteIfExists, ref string artist, string[] albums, out string message)
        {
            object[] objArray = base.Invoke("postArtist", new object[] { overwriteIfExists, artist, albums });
            artist = (string) objArray[1];
            message = (string) objArray[2];
            return (bool) objArray[0];
        }

        public void postArtistAsync(bool overwriteIfExists, string artist, string[] albums)
        {
            this.postArtistAsync(overwriteIfExists, artist, albums, null);
        }

        public void postArtistAsync(bool overwriteIfExists, string artist, string[] albums, object userState)
        {
            if (this.postArtistOperationCompleted == null)
            {
                this.postArtistOperationCompleted = new SendOrPostCallback(this.OnpostArtistOperationCompleted);
            }
            base.InvokeAsync("postArtist", new object[] { overwriteIfExists, artist, albums }, this.postArtistOperationCompleted, userState);
        }

        [return: SoapElement("dataUsed")]
        [SoapRpcMethod("urn:LyricWiki#postSong", RequestNamespace="urn:LyricWiki", ResponseNamespace="urn:LyricWiki")]
        public bool postSong(bool overwriteIfExists, ref string artist, ref string song, string lyrics, string[] onAlbums, out string message)
        {
            object[] objArray = base.Invoke("postSong", new object[] { overwriteIfExists, artist, song, lyrics, onAlbums });
            artist = (string) objArray[1];
            song = (string) objArray[2];
            message = (string) objArray[3];
            return (bool) objArray[0];
        }

        [return: SoapElement("dataUsed")]
        [SoapRpcMethod("urn:LyricWiki#postSong_flags", RequestNamespace="urn:LyricWiki", ResponseNamespace="urn:LyricWiki")]
        public bool postSong_flags(bool overwriteIfExists, ref string artist, ref string song, string lyrics, string[] onAlbums, string flags, out string message)
        {
            object[] objArray = base.Invoke("postSong_flags", new object[] { overwriteIfExists, artist, song, lyrics, onAlbums, flags });
            artist = (string) objArray[1];
            song = (string) objArray[2];
            message = (string) objArray[3];
            return (bool) objArray[0];
        }

        public void postSong_flagsAsync(bool overwriteIfExists, string artist, string song, string lyrics, string[] onAlbums, string flags)
        {
            this.postSong_flagsAsync(overwriteIfExists, artist, song, lyrics, onAlbums, flags, null);
        }

        public void postSong_flagsAsync(bool overwriteIfExists, string artist, string song, string lyrics, string[] onAlbums, string flags, object userState)
        {
            if (this.postSong_flagsOperationCompleted == null)
            {
                this.postSong_flagsOperationCompleted = new SendOrPostCallback(this.OnpostSong_flagsOperationCompleted);
            }
            base.InvokeAsync("postSong_flags", new object[] { overwriteIfExists, artist, song, lyrics, onAlbums, flags }, this.postSong_flagsOperationCompleted, userState);
        }

        public void postSongAsync(bool overwriteIfExists, string artist, string song, string lyrics, string[] onAlbums)
        {
            this.postSongAsync(overwriteIfExists, artist, song, lyrics, onAlbums, null);
        }

        public void postSongAsync(bool overwriteIfExists, string artist, string song, string lyrics, string[] onAlbums, object userState)
        {
            if (this.postSongOperationCompleted == null)
            {
                this.postSongOperationCompleted = new SendOrPostCallback(this.OnpostSongOperationCompleted);
            }
            base.InvokeAsync("postSong", new object[] { overwriteIfExists, artist, song, lyrics, onAlbums }, this.postSongOperationCompleted, userState);
        }

        [return: SoapElement("return")]
        [SoapRpcMethod("urn:LyricWiki#searchAlbums", RequestNamespace="urn:LyricWiki", ResponseNamespace="urn:LyricWiki")]
        public string[] searchAlbums(string artist, string album, int year)
        {
            return (string[]) base.Invoke("searchAlbums", new object[] { artist, album, year })[0];
        }

        public void searchAlbumsAsync(string artist, string album, int year)
        {
            this.searchAlbumsAsync(artist, album, year, null);
        }

        public void searchAlbumsAsync(string artist, string album, int year, object userState)
        {
            if (this.searchAlbumsOperationCompleted == null)
            {
                this.searchAlbumsOperationCompleted = new SendOrPostCallback(this.OnsearchAlbumsOperationCompleted);
            }
            base.InvokeAsync("searchAlbums", new object[] { artist, album, year }, this.searchAlbumsOperationCompleted, userState);
        }

        [return: SoapElement("return")]
        [SoapRpcMethod("urn:LyricWiki#searchArtists", RequestNamespace="urn:LyricWiki", ResponseNamespace="urn:LyricWiki")]
        public string[] searchArtists(string searchString)
        {
            return (string[]) base.Invoke("searchArtists", new object[] { searchString })[0];
        }

        public void searchArtistsAsync(string searchString)
        {
            this.searchArtistsAsync(searchString, null);
        }

        public void searchArtistsAsync(string searchString, object userState)
        {
            if (this.searchArtistsOperationCompleted == null)
            {
                this.searchArtistsOperationCompleted = new SendOrPostCallback(this.OnsearchArtistsOperationCompleted);
            }
            base.InvokeAsync("searchArtists", new object[] { searchString }, this.searchArtistsOperationCompleted, userState);
        }

        [return: SoapElement("return")]
        [SoapRpcMethod("urn:LyricWiki#searchSongs", RequestNamespace="urn:LyricWiki", ResponseNamespace="urn:LyricWiki")]
        public SongResult searchSongs(string artist, string song)
        {
            return (SongResult) base.Invoke("searchSongs", new object[] { artist, song })[0];
        }

        public void searchSongsAsync(string artist, string song)
        {
            this.searchSongsAsync(artist, song, null);
        }

        public void searchSongsAsync(string artist, string song, object userState)
        {
            if (this.searchSongsOperationCompleted == null)
            {
                this.searchSongsOperationCompleted = new SendOrPostCallback(this.OnsearchSongsOperationCompleted);
            }
            base.InvokeAsync("searchSongs", new object[] { artist, song }, this.searchSongsOperationCompleted, userState);
        }

        public new string Url
        {
            get
            {
                return base.Url;
            }
            set
            {
                if ((this.IsLocalFileSystemWebService(base.Url) && !this.useDefaultCredentialsSetExplicitly) && !this.IsLocalFileSystemWebService(value))
                {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }

        public new bool UseDefaultCredentials
        {
            get
            {
                return base.UseDefaultCredentials;
            }
            set
            {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
    }
}

