﻿namespace TagGuru.org.lyricwiki
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;

    [DesignerCategory("code"), GeneratedCode("System.Web.Services", "2.0.50727.3053"), DebuggerStepThrough]
    public class getSOTDCompletedEventArgs : AsyncCompletedEventArgs
    {
        private object[] results;

        internal getSOTDCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
        {
            this.results = results;
        }

        public SOTDResult Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (SOTDResult) this.results[0];
            }
        }
    }
}

