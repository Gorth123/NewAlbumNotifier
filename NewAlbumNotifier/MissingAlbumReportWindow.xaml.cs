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

namespace NewAlbumNotifier
{
    /// <summary>
    /// Interaction logic for MissingAlbumReportWindow.xaml
    /// </summary>
    public partial class MissingAlbumReportWindow : Window
    {
        internal MusicDatabase db { get; set; }

        public MissingAlbumReportWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (db == null)
                throw new Exception();

            foreach (Artist artist in db.Artists)
            {
                TreeViewItem artist_item = new TreeViewItem() { Header = artist.Name, Tag = artist };
                treeReport.Items.Add(artist_item);

                foreach (Album album in artist.MissingAlbums)
                {
                    artist_item.Items.Add(new TreeViewItem() { Header = album.Name, Tag = album });
                }
            }
        }
    }
}
