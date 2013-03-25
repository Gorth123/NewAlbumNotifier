using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NewAlbumNotifier
{
    class MusicDatabase
    {
        public List<Artist> Artists { get; set; }
        public List<Artist> AmbiguousArtists { get; set; }
        public List<string> MissingArtists { get; set; }

        public MusicDatabase()
        {
            Artists = new List<Artist>();
            AmbiguousArtists = new List<Artist>();
            MissingArtists = new List<string>();
        }

        public static MusicDatabase Load()
        {
            if (System.IO.File.Exists("C:\\temp\\temp.txt"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Include;
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
            serializer.NullValueHandling = NullValueHandling.Include;

            using (StreamWriter sw = new StreamWriter("C:\\temp\\temp.txt"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, this);
            }

            return true;
        }
    }
}
