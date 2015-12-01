using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media;
using MusicPlayer.Model;
using MusicPlayer.Utils;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace MusicPlayer.ViewModel
{
    public class MusicPlayerViewModel : ViewModelBase
    {
        #region Fields
        private ObservableCollection<Music> _trackCollection;
        private MediaPlayer _currentPlayer;
        private int _currentSongIdx;
        private bool _isPlaying;
        private RelayCommand _playCommand, _pauseCommand, _skipForwardCommand, _skipBackwardCommand,
            _openFileCommand, _openDirectoryCommand;
        #endregion

        #region Properties
        public MediaPlayer CurrentPlayer
        {
            get { return _currentPlayer; }
        }

        public ObservableCollection<Music> TrackCollection
        {
            get { return _trackCollection; }
        }

        public RelayCommand PlayCommand
        {
            get { return _playCommand ?? (_playCommand = new RelayCommand(PlaySong)); }
        }

        public RelayCommand PauseCommand
        {
            get { return _pauseCommand ?? (_pauseCommand = new RelayCommand(PauseSong)); }
        }

        public RelayCommand SkipForwardCommand
        {
            get { return _skipForwardCommand ?? (_skipForwardCommand = new RelayCommand(NextSong)); }
        }

        public RelayCommand SkipBackwardCommand
        {
            get { return _skipBackwardCommand ?? (_skipBackwardCommand = new RelayCommand(PrevSong)); }
        }

        public RelayCommand OpenFileCommand
        {
            get { return _openFileCommand ?? (_openFileCommand = new RelayCommand(OpenFile)); }
        }

        public RelayCommand OpenDirectoryCommand
        {
            get { return _openDirectoryCommand ?? (_openDirectoryCommand = new RelayCommand(OpenDirectory)); }
        }

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set { SetAndNotify(ref _isPlaying, value);}
        }

        #endregion

        #region Constructors
        public MusicPlayerViewModel()
        {
            _trackCollection = new ObservableCollection<Music>();

        }
        #endregion

        #region Methods
        private void PlaySong(object o)
        {
            if(_currentPlayer == null)
            {
                BuildPlayer();
            }
           
            _currentPlayer.Play();
            IsPlaying = true;

        }

        private void PauseSong(object o)
        {
            if(!IsPlaying || _currentPlayer == null)
                return;

            _currentPlayer.Pause();
            IsPlaying = false;
        }

        private void NextSong(object o)
        {
            if (IsPlaying)
            {
                _currentPlayer.Stop();
            }

            if (_currentSongIdx < _trackCollection.Count - 1)
            {
                _currentSongIdx++;
            }
            else
            {
                _currentSongIdx = 0;
            }

            BuildPlayer();
            PlaySong(o);
        }

        private void PrevSong(object o)
        {
            if (IsPlaying)
            {
                _currentPlayer.Stop();
            }

            if (_currentSongIdx > 0)
            {
                _currentSongIdx--;
            }
            else
            {
                _currentSongIdx = _trackCollection.Count-1;
            }

            BuildPlayer();
            PlaySong(o);
        }

        private void OpenFile(object o)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "MP3 Files (*.mp3;)|*.mp3;";
            if (openFileDialog.ShowDialog() == true)
            {
                var files = openFileDialog.FileNames;
                AddToMusicCollection(files);
            }
        }

        private void OpenDirectory(object o)
        {
            var dialog = new FolderBrowserDialog();
            dialog.ShowDialog();

            if (!String.IsNullOrEmpty(dialog.SelectedPath))
            {
                var files = Directory.GetFiles(dialog.SelectedPath);
                if (files.Any())
                {
                    AddToMusicCollection(files);
                }
            }
        }

        private void AddToMusicCollection(string[] files)
        {
            foreach (var file in files)
            {
                if (ValidateFile(file))
                {
                    TrackCollection.Add(new Music(file));
                }
            }
        }

        private bool ValidateFile(string filePath)
        {
            if (File.Exists(filePath) && Path.GetExtension(filePath).ToLower().Equals(".mp3"))
            {
                return true;
            }

            return false;
        }

        private void BuildPlayer()
        {
            if (_currentPlayer != null)
            {
                _currentPlayer.Stop();
                _currentPlayer = null;
            }

            if (_trackCollection.Any())
            {
                var currentTrack = _trackCollection[_currentSongIdx];
                _currentPlayer = new MediaPlayer();
                _currentPlayer.Open(new Uri(currentTrack.FilePath));
            }
        }
        #endregion

        #region Events

        #endregion 
    }
}