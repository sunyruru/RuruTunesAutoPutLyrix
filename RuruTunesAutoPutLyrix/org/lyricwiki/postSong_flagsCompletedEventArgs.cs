namespace RuruTunesAutoPutLyrix.org.lyricwiki
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;

    [DebuggerStepThrough, DesignerCategory("code"), GeneratedCode("System.Web.Services", "2.0.50727.3053")]
    public class postSong_flagsCompletedEventArgs : AsyncCompletedEventArgs
    {
        private object[] results;

        internal postSong_flagsCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
        {
            this.results = results;
        }

        public string artist
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (string) this.results[1];
            }
        }

        public string message
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (string) this.results[3];
            }
        }

        public bool Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (bool) this.results[0];
            }
        }

        public string song
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (string) this.results[2];
            }
        }
    }
}

