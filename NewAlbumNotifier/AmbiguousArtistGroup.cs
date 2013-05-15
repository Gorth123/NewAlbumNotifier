using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewAlbumNotifier
{
    class AmbiguousArtistGroup
    {
        public ObservableCollection<Artist> Artists { get; set; }
        public string Name { get; set; }
        public string FolderPath { get; set; }
    }
}
