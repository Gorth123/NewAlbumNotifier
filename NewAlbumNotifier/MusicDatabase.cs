using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace NewAlbumNotifier
{
    class MusicDatabase : INotifyPropertyChanged
    {
        public ObservableCollection<Artist> Artists { get; set; }
        public ObservableCollection<AmbiguousArtistGroup> AmbiguousArtists { get; set; }

        public MusicDatabase()
        {
            PropertyChanged = delegate { };

            Artists = new ObservableCollection<Artist>();
            AmbiguousArtists = new ObservableCollection<AmbiguousArtistGroup>();
        }

        public static MusicDatabase Load()
        {
            if (System.IO.File.Exists("C:\\temp\\temp.txt"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.MissingMemberHandling = MissingMemberHandling.Ignore;

                using (StreamReader sr = new StreamReader("C:\\temp\\temp.txt"))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    try
                    {
                        return serializer.Deserialize<MusicDatabase>(reader);
                    }
                    catch (JsonSerializationException)
                    {
                    }
                }
            }
            return new MusicDatabase();
        }

        public bool Save()
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = new StreamWriter("C:\\temp\\temp.txt"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, this);
            }

            return true;
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
