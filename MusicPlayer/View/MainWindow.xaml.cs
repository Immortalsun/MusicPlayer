using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using MusicPlayer.Model;
using MusicPlayer.ViewModel;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MusicPlayerViewModel _viewModel;

        public MainWindow()
        {
            _viewModel = new MusicPlayerViewModel();
            InitializeComponent();
            DataContext = _viewModel;
        }


        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listView = sender as ListView;

            if (listView != null)
            {
                var music = listView.SelectedItem as MusicViewModel;

                _viewModel.GetSelectedMusic(music);
            }
        }

        private void ProgressSlider_OnDragStarted(object sender, DragStartedEventArgs e)
        {
            _viewModel.IsSeeking = true;
        }

        private void ProgressSlider_OnDragCompleted(object sender, DragCompletedEventArgs e)
        {
            _viewModel.IsSeeking = false;
            _viewModel.ProgressValue = TimeSpan.FromSeconds(ProgressSlider.Value).TotalSeconds;
        }

        private void ProgressSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void ListView_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var listView = sender as ListView;

            if (listView != null)
            {
                var selectedItem = listView.SelectedItem;
            }
        }
    }
}
