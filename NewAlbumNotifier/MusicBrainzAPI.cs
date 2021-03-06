﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Xml;
using System.Xml.Linq;
using System.Collections.ObjectModel;

namespace NewAlbumNotifier
{
    class MusicBrainzAPI
    {
        const string BaseURL = "http://musicbrainz.org/ws/2";
        DateTime LastCallTime = DateTime.Now;
        XNamespace xns = "http://musicbrainz.org/ns/mmd-2.0#";
        XNamespace extxns = "http://musicbrainz.org/ns/ext#-2.0";

        private XDocument DoServiceCall(string request_string, params KeyValuePair<string,string>[] segments)//List<KeyValuePair<string,string>> segments);//string param_name, string param_value)
        {
            DateTime entered = DateTime.Now;
            System.Console.WriteLine("Do Service Call Entered: {0}", entered);

            if ((DateTime.Now - LastCallTime).Seconds < 1)
                System.Threading.Thread.Sleep(1000);
            RestClient client = new RestClient();
            client.BaseUrl = BaseURL;

            RestRequest request = new RestRequest(request_string);
            foreach (var segment in segments)
            {
                request.AddUrlSegment(segment.Key, segment.Value);
            }

            IRestResponse response = client.Execute(request);

            if (response != null)
            {
                if (response.ErrorException != null)
                    throw response.ErrorException;

                DateTime finished = DateTime.Now;
                System.Console.WriteLine("Do Service Call Fiished: {0}\nTotal Elapsed Time (s): {1}", finished, finished - entered);

                return XDocument.Parse(response.Content);
            }

            return null;
        }

        public List<Artist> SearchForArtist(string artist_name)
        {
            XDocument xml_doc = DoServiceCall("artist/?query=artist:{ArtistName}", new KeyValuePair<string, string>("ArtistName", artist_name));
            if (xml_doc != null)
            {
                List<Artist> artists = (from artist in xml_doc.Descendants(xns + "artist")
                                        select new Artist
                                        {
                                            Name = artist.Element(xns + "name").Value,
                                            Id = artist.Attribute("id").Value,
                                            Type = artist.Attribute("type") == null?"":artist.Attribute("type").Value,
                                            Score = Convert.ToInt32(artist.Attribute(XName.Get("score", "http://musicbrainz.org/ns/ext#-2.0")).Value),
                                        }).ToList<Artist>();

                return (from a in artists
                        where a.Score == artists.Max(p => p.Score)
                        & a.Type == "Group"
                        select a).ToList<Artist>();
            }

            return null;
        }

        public List<Album> GetAlbumsForArtist(string arid)
        {
            string album_request = "release-group?artist={ArtistMBID}&limit=100&type=album|ep&offset={Offset}";
            int offset = 0;
            Album comparer = new Album();

            XDocument xml_doc = DoServiceCall(album_request, 
                                               new KeyValuePair<string,string>("ArtistMBID", arid),
                                               new KeyValuePair<string,string>("Offset","0"));
            if (xml_doc != null)
            {
                List<Album> all_albums = new List<Album>();
                int num_albums = Convert.ToInt32(xml_doc.Descendants(xns + "release-group-list").First().Attribute("count").Value);
                while (offset < num_albums)
                {
                    List<Album> albums = (from album in xml_doc.Descendants(xns + "release-group")
                                select new Album
                                {
                                    Name = album.Element(xns + "title").Value,
                                    Id = album.Attribute("id").Value,
                                    Date = album.Element(xns + "first-release-date") == null ? "" : album.Element(xns + "first-release-date").Value,
                                    Type = album.Attribute("type").Value,
                                }).ToList<Album>();
                    all_albums.AddRange(albums);

                    offset += 100;
                    xml_doc = DoServiceCall(album_request, 
                                            new KeyValuePair<string,string>("ArtistMBID", arid),
                                            new KeyValuePair<string,string>("Offset",all_albums.Count.ToString()));
                }
                all_albums = (from a in all_albums
                          where (a.Type == "Album" || a.Type == "Ep")
                          orderby a.Name ascending, a.Date ascending
                          select a).Distinct(comparer).ToList<Album>();
                return all_albums;
            }

            return null;
        }

        public void Scan(string path, MusicDatabase db)
        {
            foreach (Artist a in db.Artists)
            {
                if (a.ExtraAlbums != null)
                    a.ExtraAlbums.Clear();
            }

            foreach (string s in System.IO.Directory.EnumerateDirectories(path))
            {
                System.Console.WriteLine(s);
                if (db.Artists.FirstOrDefault(a => string.Compare(a.FolderPath, s) == 0) == null)
                {
                    // new artist detected
                    string name = System.IO.Path.GetFileNameWithoutExtension(s);
                    if (db.AmbiguousArtists.FirstOrDefault(a => string.Compare(a.Name, name) == 0) == null)
                    {
                        // not already ambiguated, so try to find online
                        List<Artist> artists = SearchForArtist(name);
                        if (artists.Count > 0)
                        {
                            if (artists.Count == 1)
                            {
                                artists[0].FolderPath = s;

                                db.Artists.Add(artists[0]);
                            }
                            else
                            {
                                AmbiguousArtistGroup group = new AmbiguousArtistGroup { Name = name, FolderPath = s, Artists = new ObservableCollection<Artist>() };
                                foreach (Artist a in artists)
                                    group.Artists.Add(a);
                                db.AmbiguousArtists.Add(group);
                            }
                        }
                    }
                }
            }

            foreach (Artist artist in db.Artists)
            {
                var query = (from f in System.IO.Directory.EnumerateDirectories(artist.FolderPath)
                            let tokens = System.IO.Path.GetFileNameWithoutExtension(f).Split('-')
                            where tokens.Length == 2
                            let result = tokens[1].Trim()
                            select new { folder = f, name = result }).ToList();

                // first look online for any new albums
                List<Album> albums = GetAlbumsForArtist(artist.Id);
                foreach (Album album in albums)
                {
                    if (artist.HasAlbum(album.Name) == false)
                    {

                        var album_name = query.Find(s =>
                        {
                            return string.Compare(s.name, album.Name, true) == 0;
                        });

                        if (album_name != null)
                        {
                            album.FolderPath = album_name.folder;
                            album.Name = album_name.name;
                            artist.AddAlbum(album);
                        }
                        else
                        {
                            if (artist.HasMissingAlbum(album.Name) == false)
                            {
                                artist.AddMissingAlbum(album);
                            }
                        }
                    }
                }

                // then look in the folder for any albums that don't exist online
                foreach (var v in query)
                {
                    if (artist.HasAlbum(v.name) == false)
                    {
                        if (artist.HasExtraAlbum(v.name) == false)
                        {
                            artist.AddExtraAlbum(new Album() { Name = v.name, FolderPath = v.folder });
                        }
                    }
                }
            }

            db.Save();
        }
    }
}
