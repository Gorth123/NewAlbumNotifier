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
        

        public MainWindow()
        {
            InitializeComponent();

            db = MusicDatabase.Load();

            foreach (string s in System.IO.Directory.EnumerateDirectories("C:\\Music"))//\\\\readyshare\\USB_Storage\\theshed\\music\\mp3"))
            {
                System.Console.WriteLine(s);
                if (db.Artists.Find(a => string.Compare(a.FolderPath, s) == 0) == null)
                {
                    // new artist detected
                    string name = System.IO.Path.GetFileNameWithoutExtension(s);
                    if (db.AmbiguousArtists.Find(a => string.Compare(a.Name, name) == 0) == null)
                    {
                        // not already ambiguated, so try to find online
                        List<Artist> artists = mbapi.SearchForArtist(name);
                        if (artists.Count > 0)
                        {
                            if (artists.Count == 1)
                            {
                                artists[0].FolderPath = s;

                                db.Artists.Add(artists[0]);
                            }
                            else
                            {
                                db.AmbiguousArtists.AddRange(artists);
                            }
                        }
                    }
                }
            }

            foreach (Artist artist in db.Artists)
            {
                // first look online for any new albums
                List<Album> albums = mbapi.GetAlbumsForArtist(artist.Id);
                foreach (Album album in albums)
                {
                    List<string> album_folders = System.IO.Directory.EnumerateDirectories(artist.FolderPath).ToList<string>();

                    if (artist.HasAlbum(album.Name) == false)
                    {
                        string album_folder = album_folders.Find(s => {
                            string[] tokens = System.IO.Path.GetFileNameWithoutExtension(s).Split('-');
                            if (tokens.Length == 2)
                                return string.Compare(tokens[1].Trim(), album.Name, true) == 0;
                            return false;
                        });
                        if ( album_folder != null)
                        {
                            album.FolderPath = album_folder;
                            artist.AddAlbum(album);
                        }
                        else
                        {
                            if (artist.HasMissingAlbum(album.Name) == false)
                            {
                                artist.AddMissingAlbum(album);
                            }
                        }
                    }
                }

                // then look in the folder for any albums that don't exist online
                foreach (string s in System.IO.Directory.EnumerateDirectories(artist.FolderPath))
                {
                    string name = System.IO.Path.GetFileNameWithoutExtension(s);

                    if (artist.HasAlbum(name) == false)
                    {
                        if (artist.HasExtraAlbum(name) == false)
                        {
                            artist.AddExtraAlbum(new Album() {Name = name});
                        }
                    }
                }
            }

            db.Save();
        }

        private void cmbArtistListFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cmbArtistListFilter.SelectedIndex)
            {
                case 0:
                    lbArtistList.ItemsSource = db.Artists;
                    break;
                case 1:
                    lbArtistList.ItemsSource = db.AmbiguousArtists;
                    break;
                case 2:
                    lbArtistList.ItemsSource = db.MissingArtists;
                    break;
            }
            
        }

        private void lbArtistList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                Artist artist = (Artist)e.AddedItems[0];
                lbAlbums.ItemsSource = artist.Albums;
                lbMissingAlbums.ItemsSource = artist.MissingAlbums;
                lbExtraAlbums.ItemsSource = artist.ExtraAlbums;
            }
        }


    }
}
