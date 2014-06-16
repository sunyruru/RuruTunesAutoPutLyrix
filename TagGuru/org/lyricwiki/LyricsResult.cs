namespace TagGuru.org.lyricwiki
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, GeneratedCode("System.Xml", "2.0.50727.3053"), SoapType(Namespace="urn:LyricWiki"), DebuggerStepThrough, DesignerCategory("code")]
    public class LyricsResult
    {
        private string artistField;
        private string lyricsField;
        private string songField;
        private string urlField;

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

        public string url
        {
            get
            {
                return this.urlField;
            }
            set
            {
                this.urlField = value;
            }
        }
    }
}

