using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayer.Model;
using MusicPlayer.Utils;
using MusicPlayer.View;

namespace MusicPlayer.ViewModel
{
    public class MusicViewModel : ViewModelBase
    {
        //Fields
        private Music _music;
        private string _name, _artist, _album;
        private bool _isPlaying;
        private RelayCommand _editCommand;

        //Properties
        public RelayCommand EditCommand 
        {
            get { return _editCommand ?? (_editCommand = new RelayCommand(Edit));}
        }

        public string Name 
        {
            get { return _name; }
            set { SetAndNotify(ref _name, value); }
        }

        public string FilePath 
        {
            get { return _music.FilePath; }
        }

        public string Artist 
        {
            get { return _artist; }
            set { SetAndNotify(ref _artist, value); }
        }

        public string Album 
        {
            get { return _album; }
            set { SetAndNotify(ref _album, value); }
        }

        public bool IsPlaying 
        {
            get { return _isPlaying;}
            set { SetAndNotify(ref _isPlaying, value); }
        }


        //Constructor
        public MusicViewModel(Music m) 
        {
            _music = m;
            _album = _music.Album;
            _artist = _music.Artist;
            _name = _music.Name;
        }

        //Methods
        private void Edit(object o) 
        {
            EditMusicDialog dlg = new EditMusicDialog(this);

            dlg.ShowDialog();
            if (dlg.DialogResult == true) 
            {
                
               Artist = _music.Artist = dlg.Artist;
               Album = _music.Album = dlg.Album;
               Name = _music.Name = dlg.SongName;
            }
        }

    }
}
