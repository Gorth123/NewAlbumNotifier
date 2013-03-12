using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewAlbumNotifer
{
    class ArtistList
    {
        public string Count { get; set; }
        public string Offset { get; set; }

        public List<Artist> Artists { get; set; }
    }
}
