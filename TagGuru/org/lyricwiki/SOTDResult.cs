namespace TagGuru.org.lyricwiki
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, DesignerCategory("code"), DebuggerStepThrough, GeneratedCode("System.Xml", "2.0.50727.3053"), SoapType(Namespace="urn:LyricWiki")]
    public class SOTDResult
    {
        private string artistField;
        private string lyricsField;
        private string nominatedByField;
        private string reasonField;
        private string songField;

        public string artist
        {
            get
            {
                return this.artistField;
            }
            set
            {
                this.artistField = value;
            }
        }

        public string lyrics
        {
            get
            {
                return this.lyricsField;
            }
            set
            {
                this.lyricsField = value;
            }
        }

        public string nominatedBy
        {
            get
            {
                return this.nominatedByField;
            }
            set
            {
                this.nominatedByField = value;
            }
        }

        public string reason
        {
            get
            {
                return this.reasonField;
            }
            set
            {
                this.reasonField = value;
            }
        }

        public string song
        {
            get
            {
                return this.songField;
            }
            set
            {
                this.songField = value;
            }
        }
    }
}

