using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MusicPlayer.ViewModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MusicPlayer.View
{
    /// <summary>
    /// Interaction logic for EditMusicDialog.xaml
    /// </summary>
    public partial class EditMusicDialog : Window, INotifyPropertyChanged
    {
        private string _name, _artist, _album;

        public string SongName 
        {
            get { return _name; }
            set { SetAndNotify(ref _name, value); }
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

        public EditMusicDialog(MusicViewModel vM)
        {
            SongName = vM.Name;
            Artist = vM.Artist;
            Album = vM.Album;
            DataContext = this;
            InitializeComponent();

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        #region INotifyPropertyChanged Members
        /// <summary>
        /// This handler is raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// This method raised the change event
        /// </summary>
        /// <param name="propertyName">The name of the property with a new value.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Signals the on property changed event.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="oldValue">The old value</param>
        /// <param name="newValue">The new value</param>
        /// <param name="broadcast">Whether to broadcast the notification or not.</param>
        protected virtual void OnPropertyChanged<T>(
            T oldValue = default(T),
            T newValue = default(T),
            bool broadcast = false,
            [CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("This method cannot be called with an empty string", "propertyName");
            }

            OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// This method allows for passing the ref to a field and a value, which will allow for one line setting and notifying.
        /// <para>This method relies on the CallerMemberName attribute to get the property name,
        ///  so if it used outside of a member, the name must be passed to nofify a change.</para>
        /// </summary>
        /// <typeparam name="T">Type of the Property/Field</typeparam>
        /// <param name="field">Field to set.</param>
        /// <param name="newValue">Value to apply to field.</param>
        /// <param name="broadcast">Whether to broadcast the property change with the MessangerInstance.</param>
        /// <param name="propertyName">Name of property to nofify.</param>
        protected virtual void SetAndNotify<T>(
            ref T field,
            T newValue,
            bool broadcast = false,
            [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
                return;

            var oldValue = field;
            field = newValue;
            OnPropertyChanged(oldValue, newValue, broadcast, propertyName);
        }
        #endregion INotifyPropertyChanged Members
    }
}
