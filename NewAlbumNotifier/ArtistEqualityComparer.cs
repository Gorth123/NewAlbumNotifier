using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewAlbumNotifier
{
    class ArtistEqualityComparer : IEqualityComparer<Artist>
    {
        public enum ArtistCompareOptions
        {
            ByName,
            ByPath,
        }

        public ArtistCompareOptions CompareOptions { get; set; }

        ArtistEqualityComparer(ArtistCompareOptions options)
        {
            CompareOptions = options;
        }

        #region IEqualityComparer<Artist> Members

        bool IEqualityComparer<Artist>.Equals(Artist x, Artist y)
        {
            switch (CompareOptions)
            {
                case ArtistCompareOptions.ByName:
                    return string.Compare(x.Name, y.Name, true) == 0;
                case ArtistCompareOptions.ByPath:
                    return string.Compare(x.FolderPath, y.FolderPath, true) == 0;
            }

            throw new ArgumentException("Invalid CompreOptions Value", "CompareOptions");
        }

        int IEqualityComparer<Artist>.GetHashCode(Artist obj)
        {
            switch (CompareOptions)
            {
                case ArtistCompareOptions.ByName:
                    return obj.Name.GetHashCode();
                case ArtistCompareOptions.ByPath:
                    return obj.FolderPath.GetHashCode();
            }

            throw new ArgumentException("Invalid CompreOptions Value", "CompareOptions");
        }

        #endregion
    }
}
