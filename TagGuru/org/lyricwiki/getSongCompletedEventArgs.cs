namespace TagGuru.org.lyricwiki
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;

    [DesignerCategory("code"), GeneratedCode("System.Web.Services", "2.0.50727.3053"), DebuggerStepThrough]
    public class getSongCompletedEventArgs : AsyncCompletedEventArgs
    {
        private object[] results;

        internal getSongCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
        {
            this.results = results;
        }

        public LyricsResult Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (LyricsResult) this.results[0];
            }
        }
    }
}

