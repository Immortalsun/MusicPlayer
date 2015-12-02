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
                var music = listView.SelectedItem as Music;

                _viewModel.GetSelectedMusic(music);
            }
        }
    }
}
