using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace NewAlbumNotifer
{
    class ArtistAmbiguation
    {
        public string Name { get; set; }
        [PrimaryKey]
        public string Id { get; set; }
        public string FolderPath { get; set; }

    }
}
