using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewAlbumNotifer
{
    class Artist : IEqualityComparer<Artist>
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public int Score { get; set; }

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

        #region IEqualityComparer<Artist> Members

        public bool Equals(Artist x, Artist y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(Artist obj)
        {
            return obj.Name.GetHashCode();
        }

        #endregion
    }
}
