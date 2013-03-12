using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewAlbumNotifer
{
    class Album : IEqualityComparer<Album>
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public Dictionary<string, string> Details()
        {
            Dictionary<string, string> details = new Dictionary<string,string>();
            details.Add("Name", Name);
            details.Add("Type", Type);
            details.Add("Id", Id);
            details.Add("Date", Date);

            return details;
        }

        #region IEqualityComparer<Album> Members

        public bool Equals(Album x, Album y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(Album obj)
        {
            return obj.Name.GetHashCode();
        }

        #endregion
    }
}
