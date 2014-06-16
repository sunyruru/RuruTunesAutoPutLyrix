namespace RuruTunesAutoPutLyrix.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.Configuration;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [CompilerGenerated, GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0")]
    internal sealed class Settings : ApplicationSettingsBase
    {
        private static Settings defaultInstance = ((Settings) SettingsBase.Synchronized(new Settings()));

        public static Settings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [SpecialSetting(SpecialSetting.WebServiceUrl), DefaultSettingValue("http://lyricwiki.org/server.php"), ApplicationScopedSetting, DebuggerNonUserCode]
        public string RuruTunesAutoPutLyrix_org_lyricwiki_LyricWiki
        {
            get
            {
                return (string) this["RuruTunesAutoPutLyrix_org_lyricwiki_LyricWiki"];
            }
        }
    }
}

