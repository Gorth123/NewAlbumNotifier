using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewAlbumNotifier
{
    class Artist
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public int Score { get; set; }
        public string FolderPath { get; set; }

        public List<Album> Albums { get; set; }
        public List<Album> MissingAlbums { get; set; }
        public List<Album> ExtraAlbums { get; set; }

        public bool HasAlbum(string name)
        {
            return HasAlbumEntry(name, Albums);
        }

        public bool HasMissingAlbum(string name)
        {
            return HasAlbumEntry(name, MissingAlbums);
        }

        public bool HasExtraAlbum(string name)
        {
            return HasAlbumEntry(name, ExtraAlbums);
        }

        private bool HasAlbumEntry(string name, List<Album> list)
        {
            if (list != null)
            {
                return list.Find(a=> string.Compare(a.Name, name, true) == 0) != null;
            }

            return false;
        }

        public void AddAlbum(Album album)
        {
            if (Albums == null)
                Albums = new List<Album>();
            Albums.Add(album);
        }

        public void AddMissingAlbum(Album album)
        {
            if (MissingAlbums == null)
                MissingAlbums = new List<Album>();
            MissingAlbums.Add(album);
        }

        public void AddExtraAlbum(Album album)
        {
            if (ExtraAlbums == null)
                ExtraAlbums = new List<Album>();
            ExtraAlbums.Add(album);
        }

        public override string ToString()
        {
            return Name;
        }

        public Dictionary<string, string> Details()
        {
            Dictionary<string, string> details = new Dictionary<string, string>();
            details.Add("Name", Name);
            details.Add("Id", Id);
            details.Add("Type", Type);
            details.Add("Score", Score.ToString());

            return details;
        }
    }
}
