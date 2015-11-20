namespace MusicPlayer.Model
{
    public class Music
    {
        #region Fields
        private string _filePath, _name, _artist, _album;
        private bool _isPlaying;
        #endregion

        #region Properties
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Artist
        {
            get { return _artist; }
            set { _artist = value; }
        }

        public string Album
        {
            get { return _album; }
            set { _album = value; }
        }

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set { _isPlaying = value; }
        }
        #endregion

        #region Constructors

        #endregion

        #region Methods

        #endregion

        #region Events

        #endregion 
    }
}