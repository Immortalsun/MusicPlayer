using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using MusicPlayer.ViewModel;

namespace MusicPlayer.Model
{
    public class Music : ViewModelBase
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

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set { _isPlaying = value; OnPropertyChanged(); }
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
        #endregion

        #region Constructors
        public Music(string filePath)
        {
            FilePath = filePath;
            SetupSongMetadata();
        }
        #endregion

        #region Methods
        

        public void SetupSongMetadata()
        {
            TagLib.File musicFile = TagLib.File.Create(FilePath);
            Name = musicFile.Tag.Title;
            GenerateArtistList(musicFile.Tag.Performers);
            Album = musicFile.Tag.Album;
        }

        public void GenerateArtistList(string[] performers)
        {
            if(!performers.Any())
                return;

            if (performers.Count() == 1)
            {
               Artist = performers[0];
            }
            else 
            {
                for (int i = 0; i < performers.Length; i++)
                {
                    Artist += performers[i];

                    if (i < performers.Length - 1)
                    {
                        Artist += ", ";
                    }
                }
            }
        }
        #endregion

        #region Events

        #endregion
    }
}