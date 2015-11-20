using System.Collections.ObjectModel;
using System.Windows.Media;
using MusicPlayer.Model;

namespace MusicPlayer.ViewModel
{
    public class MusicPlayerViewModel : ViewModelBase
    {
        #region Fields
        private ObservableCollection<Music> _trackCollection;
        private MediaPlayer _currentPlayer;
        #endregion

        #region Properties
        public MediaPlayer CurrentPlayer
        {
            get { return _currentPlayer; }
        }
        #endregion

        #region Constructors
        public MusicPlayerViewModel()
        {
            _trackCollection = new ObservableCollection<Music>();

        }
        #endregion

        #region Methods

        #endregion

        #region Events

        #endregion 
    }
}