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

namespace MusicPlayer.View
{
    /// <summary>
    /// Interaction logic for EditMusicDialog.xaml
    /// </summary>
    public partial class EditMusicDialog : Window
    {
        private MusicViewModel _viewModel;
        public EditMusicDialog(MusicViewModel vM)
        {
            _viewModel = vM;
            DataContext = _viewModel;
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
    }
}
