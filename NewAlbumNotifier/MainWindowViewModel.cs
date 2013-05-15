using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NewAlbumNotifier
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            PropertyChanged = delegate { };
        }

        private Artist _SelectedArtist;
        private AmbiguousArtistGroup _SelectedGroup;
        private ObservableCollection<Artist> _SelectedArtistArray = new ObservableCollection<Artist>();

        public MusicDatabase Database { get; set; }
        public Artist SelectedArtist {
            get { return _SelectedArtist; }
            set
            {
                _SelectedArtist = value;
                if (_SelectedArtistArray.Any())
                    _SelectedArtistArray[0] = value;
                else
                    _SelectedArtistArray.Add(value);

                OnPropertyChanged();
            } 
        }
        public IList<Artist> SelectedArtistArray { get { return _SelectedArtistArray; } }

        private Album _SelectedAlbum;
        private ObservableCollection<Album> _SelectedAlbumArray = new ObservableCollection<Album>();

        public Album SelectedAlbum
        {
            get { return _SelectedAlbum; }
            set 
            {
                _SelectedAlbum = value;
                _SelectedAlbumArray[0] = value;
                OnPropertyChanged();
                OnPropertyChanged("SelectedAlbumArray");
            }
        }

        public AmbiguousArtistGroup SelectedGroup
        {
            get { return _SelectedGroup; }
            set
            {
                _SelectedGroup = value;
                OnPropertyChanged();
            }
        }
        public bool ShowAlbums { get; set; }
        public bool ShowMisssingAlbums { get; set; }
        public bool ShowExtraAlbums { get; set; }

        protected virtual void OnPropertyChanged(
            [CallerMemberName]string propertyName = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
