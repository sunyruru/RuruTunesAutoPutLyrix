namespace TagGuru.org.lyricwiki
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;

    [GeneratedCode("System.Web.Services", "2.0.50727.3053"), DebuggerStepThrough, DesignerCategory("code")]
    public class searchArtistsCompletedEventArgs : AsyncCompletedEventArgs
    {
        private object[] results;

        internal searchArtistsCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
        {
            this.results = results;
        }

        public string[] Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (string[]) this.results[0];
            }
        }
    }
}

