using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media;
using MusicPlayer.Model;
using MusicPlayer.Utils;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using System.Collections.Generic;

namespace MusicPlayer.ViewModel
{
    public class MusicPlayerViewModel : ViewModelBase
    {
        #region Fields
        private ObservableCollection<MusicViewModel> _trackCollection;
        private MediaPlayer _currentPlayer;
        private int _currentSongIdx;
        private MusicViewModel _currentSong;
        private Timer _songTimer;
        private bool _isPlaying,_shuffleMusic,_continuousPlay;
        private double _volume;
        public bool IsSeeking;
        private List<int> indexList;
        private RelayCommand _playCommand, _pauseCommand, _skipForwardCommand, _skipBackwardCommand,
            _openFileCommand, _openDirectoryCommand;
        #endregion

        #region Properties
        public MediaPlayer CurrentPlayer
        {
            get { return _currentPlayer; }
        }

        public MusicViewModel CurrentSong
        {
            get { return _currentSong; }
            set { SetAndNotify(ref _currentSong, value);}

        }

        public bool AnyMusic
        {
            get { return _trackCollection.Any(); }
        }

        public bool ShuffleMusic
        {
            get { return _shuffleMusic; }
            set 
            { 
                SetAndNotify(ref _shuffleMusic, value);
                if (_shuffleMusic)
                {
                    ShuffleSongs();
                }
                else 
                {
                    indexList = null;
                }
            }
        }

        public bool ContinuousPlay
        {
            get { return _continuousPlay; }
            set { SetAndNotify(ref _continuousPlay, value);}
        }

        public double ProgressValue
        {
            get
            {
                if (_currentPlayer != null)
                {
                    return _currentPlayer.Position.TotalSeconds;
                }
                return 0;

            }
            set
            {
                if (_currentPlayer != null)
                {
                    _currentPlayer.Position = TimeSpan.FromSeconds(value);
                    OnPropertyChanged();
                    OnPropertyChanged("ProgressTextValue");
                }
            }
        }

        public double Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                if (_currentPlayer != null)
                {
                    _currentPlayer.Volume = value;
                    SetAndNotify(ref _volume, value);
                    OnPropertyChanged("VolumeTextValue");
                }
            }
        }

        public string VolumeTextValue
        {
            get { return Volume.ToString("P0"); }
        }

        public double TotalDuration
        {
            get
            {
                if (_currentPlayer != null && _currentPlayer.NaturalDuration.HasTimeSpan)
                {
                    return _currentPlayer.NaturalDuration.TimeSpan.TotalSeconds;                    
                }
                return .1;
            }
        }

        public string ProgressTextValue
        {
            get
            {
                return TimeSpan.FromSeconds(ProgressValue).ToString(@"hh\:mm\:ss");
            }
        }

        public string TotalDurationTextValue
        {
            get
            {
                return TimeSpan.FromSeconds(TotalDuration).ToString(@"hh\:mm\:ss");
            }
        }

        public ObservableCollection<MusicViewModel> TrackCollection
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
            _trackCollection = new ObservableCollection<MusicViewModel>();

        }
        #endregion

        #region Methods
        private void PlaySong(object o)
        {
            if(!AnyMusic)
                return;

            if(_currentPlayer == null)
            {
                BuildPlayer();
            }
            _currentPlayer.Play();
            CurrentSong.IsPlaying = true;
            IsPlaying = true;

        }

        private void PauseSong(object o)
        {
            if (!AnyMusic)
                return;

            if(!IsPlaying || _currentPlayer == null)
                return;

            _currentPlayer.Pause();
            IsPlaying = false;
        }

        private void NextSong(object o)
        {
            if (!AnyMusic)
                return;

            if (IsPlaying)
            {
                CurrentSong.IsPlaying = false;
                _currentPlayer.Stop();
            }

            if (indexList == null)
            {
                if (_currentSongIdx < _trackCollection.Count - 1)
                {
                    _currentSongIdx++;
                }
                else
                {
                    if (_continuousPlay)
                    {
                        _currentSongIdx = 0;
                    }
                }
            }
            else 
            {
                _currentSongIdx = indexList.IndexOf(_currentSongIdx++);
            }
            BuildPlayer();
            PlaySong(o);
        }

        private void PrevSong(object o)
        {
            if (!AnyMusic)
                return;

            if (IsPlaying)
            {
                CurrentSong.IsPlaying = false;
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

        private void ShuffleSongs() 
        {
            indexList = new List<int>(Enumerable.Range(0,_trackCollection.Count));

            int count = indexList.Count;

            while(count > 1)
            {
                count--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(count+1);
                int value = indexList[k];
                indexList[k] = indexList[count];
                indexList[count] = value;
            }

        }

        public void GetSelectedMusic(MusicViewModel m)
        {
            if (_currentPlayer != null)
            {
                if (IsPlaying)
                {
                    CurrentSong.IsPlaying = false;
                    _currentPlayer.Stop();
                }
            }

            _currentPlayer = null;
            var trackIdx = _trackCollection.IndexOf(m);
            _currentSongIdx = trackIdx;
            PlaySong(null);
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
                    TrackCollection.Add(new MusicViewModel(new Music(file)));
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
                CurrentSong = currentTrack;
                _currentPlayer = new MediaPlayer();
                _currentPlayer.Open(new Uri(currentTrack.FilePath));
                if (_songTimer != null)
                {
                    _songTimer.Stop();
                    _songTimer.Tick -= TimerTick;
                }
                _songTimer = new Timer();
                _songTimer.Tick += TimerTick;
                _songTimer.Start();

                if (Volume.Equals(0.0))
                {
                    Volume = _currentPlayer.Volume;
                }
                _currentPlayer.Volume = Volume;
                OnPropertyChanged("VolumeTextValue");
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (_currentPlayer != null && IsPlaying && _currentPlayer.NaturalDuration.HasTimeSpan && !IsSeeking)
            {
                OnPropertyChanged("ProgressValue");
                OnPropertyChanged("ProgressTextValue");
                OnPropertyChanged("TotalDuration");
                OnPropertyChanged("TotalDurationTextValue");
            }

            if (ProgressValue.Equals(TotalDuration)) 
            {
                NextSong(null);
            }
        }

        #endregion

        #region Events

        #endregion 
    }
}