using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NewAlbumNotifer
{
    [JsonObjectAttribute("artist-list")]
    class ArtistList
    {
        [JsonProperty(PropertyName = "@count")]
        public string Count { get; set; }
        [JsonProperty(PropertyName = "@offset")]
        public string Offset { get; set; }

        [JsonProperty(PropertyName="artist")]
        public List<Artist> Artists { get; set; }
    }
}
