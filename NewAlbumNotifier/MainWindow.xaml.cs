using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;

namespace NewAlbumNotifier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MusicBrainzAPI mbapi = new MusicBrainzAPI();
        MusicDatabase db = new MusicDatabase();

        MainWindowViewModel ViewModel = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();

            //BindingOperations.SetBinding(lblAmbiguousArtistCountValue, Label.ContentProperty, new Binding { Source = db.AmbiguousArtists, Path = new PropertyPath("Count") });
            //BindingOperations.SetBinding(lblArtistCountValue, Label.ContentProperty, new Binding { Source = db.Artists, Path = new PropertyPath("Count") });
            //lblAmbiguousArtistCountValue.DataContext = new Binding { Source = db.Artists, Path = new PropertyPath("Count") };
            ViewModel.Database = MusicDatabase.Load();
            DataContext = ViewModel;
        }

        private void btnScan_Click(object sender, RoutedEventArgs e)
        {
            mbapi.Scan("C:\\Music", ViewModel.Database);
            //UpdateLabels();
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            MissingAlbumReportWindow win = new MissingAlbumReportWindow();
            win.db = ViewModel.Database;
            win.ShowDialog();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
        }

        private void OnAmbiguousArtistButtonClick(object sender, RoutedEventArgs e)
        {
            lbArtist.Visibility = Visibility.Collapsed;
            lbAmbiguousArtist.Visibility = Visibility.Visible;
        }

        private void OnArtistButtonClick(object sender, RoutedEventArgs e)
        {
            lbAmbiguousArtist.Visibility = Visibility.Collapsed;
            lbArtist.Visibility = Visibility.Visible;
        }

        private void lbArtistSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbArtistDetails.Visibility = Visibility.Visible;
            lbAmiguousArtistSelection.Visibility = Visibility.Collapsed;
        }

        private void lbAmbiguousArtistSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbArtistDetails.Visibility = Visibility.Collapsed;
            lbAmiguousArtistSelection.Visibility = Visibility.Visible;
        }
    }
}
