namespace TagGuru.org.lyricwiki
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, DesignerCategory("code"), SoapType(Namespace="urn:LyricWiki"), DebuggerStepThrough, GeneratedCode("System.Xml", "2.0.50727.3053")]
    public class SongResult
    {
        private string artistField;
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

