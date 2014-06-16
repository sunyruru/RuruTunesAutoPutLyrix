namespace RuruTunesAutoPutLyrix.org.lyricwiki
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;

    [GeneratedCode("System.Web.Services", "2.0.50727.3053"), DesignerCategory("code"), DebuggerStepThrough]
    public class getHometownCompletedEventArgs : AsyncCompletedEventArgs
    {
        private object[] results;

        internal getHometownCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
        {
            this.results = results;
        }

        public string hometown
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (string) this.results[2];
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

        public string state
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (string) this.results[1];
            }
        }
    }
}

