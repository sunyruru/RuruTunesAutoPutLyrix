namespace RuruTunesAutoPutLyrix.org.lyricwiki
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;

    [GeneratedCode("System.Web.Services", "2.0.50727.3053"), DesignerCategory("code"), DebuggerStepThrough]
    public class checkSongExistsCompletedEventArgs : AsyncCompletedEventArgs
    {
        private object[] results;

        internal checkSongExistsCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
        {
            this.results = results;
        }

        public bool Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (bool) this.results[0];
            }
        }
    }
}

