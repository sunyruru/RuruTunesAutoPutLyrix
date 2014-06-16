namespace TagGuru.org.lyricwiki
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;

    [GeneratedCode("System.Web.Services", "2.0.50727.3053"), DebuggerStepThrough, DesignerCategory("code")]
    public class getAlbumCompletedEventArgs : AsyncCompletedEventArgs
    {
        private object[] results;

        internal getAlbumCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
        {
            this.results = results;
        }

        public string album
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (string) this.results[2];
            }
        }

        public string artist
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (string) this.results[1];
            }
        }

        public string Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (string) this.results[0];
            }
        }

        public string[] songs
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (string[]) this.results[4];
            }
        }

        public int year
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (int) this.results[3];
            }
        }
    }
}

