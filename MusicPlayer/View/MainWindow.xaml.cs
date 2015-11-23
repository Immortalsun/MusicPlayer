using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
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
    }
}
