namespace TagGuru.org.lyricwiki
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;

    [DesignerCategory("code"), DebuggerStepThrough, GeneratedCode("System.Web.Services", "2.0.50727.3053")]
    public class postAlbumCompletedEventArgs : AsyncCompletedEventArgs
    {
        private object[] results;

        internal postAlbumCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
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

        public string message
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (string) this.results[4];
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

