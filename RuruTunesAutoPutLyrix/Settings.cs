namespace RuruTunesAutoPutLyrix
{
    using System;

    [Serializable]
    public class Settings
    {
        public bool chkAlbumTitleOnly;
        public bool chkCover;
        public bool chkCoverBigSize = true;
        public bool chkCoveriTunes = true;
        public bool chkCoverOverwrite = true;
        public bool[] chkCovers;
        public bool chkLyric = true;
        public bool chkLyricOverwrite = true;
        public bool[] chkLyrics;
        public bool chkSongName = true;
        public string[] lstCovers = new string[] { "매니아DB - http://maniadb.com", "Coverholic - http://coverholic.com", "All CD Covers - http://allcdcovers.com", "즐즐넷 - http://cover.zzlzzl.net", "CCMPia - http://ccmpia.com" };
        public string[] lstLyrics = new string[] { "가요/팝/일음 검색 - http://gasazip.com", "가요/팝/일음 검색 - http://mnet.com", "가요 검색 - http://im.new21.org", "가요 검색 - http://inmuz.com", "Jpop 검색 - http://utamap.com", "Jpop 검색 - http://jieumai.com(지음아이)", "CPop 검색 - http://sing8.com", "CCM 검색 - http://ccmpia.com" };
        public string txtCoverSize = "500";

        public Settings()
        {
            bool[] flagArray = new bool[8];
            flagArray[0] = true;
            this.chkLyrics = flagArray;
            bool[] flagArray2 = new bool[6];
            flagArray2[0] = true;
            this.chkCovers = flagArray2;
        }
    }
}

