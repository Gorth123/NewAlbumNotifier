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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using RestSharp;

namespace NewAlbumNotifer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MusicBrainzAPI mbapi = new MusicBrainzAPI();

        public MainWindow()
        {
            InitializeComponent();

        }

        private void Window_Initialized_1(object sender, EventArgs e)
        {
        }

        private void lbArtist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Artist artist = e.AddedItems.Count > 0 ? (Artist)e.AddedItems[0] : null;
            lbArtistDetails.ItemsSource = artist != null ? artist.Details() : null;
            lbAlbum.ItemsSource = artist != null ? mbapi.GetAlbumsForArtist(artist.Id) : null;
        }

        private void lbAlbum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbAlbumDetails.ItemsSource = e.AddedItems.Count > 0 ? ((Album)(e.AddedItems[0])).Details() : null;
        }

        private void txtArtist_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txtArtist.Text.Length > 0)
                {
                    List<Artist> artists = mbapi.SearchForArtist(txtArtist.Text);
                    lbArtist.ItemsSource = artists;
                    lbAlbum.ItemsSource = null;
                    lbArtistDetails.ItemsSource = null;
                    lbAlbumDetails.ItemsSource = null;
                }
            }
        }
    }
}
