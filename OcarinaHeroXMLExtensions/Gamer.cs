using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace OcarinaHeroLibrary
{
    public class Gamer
    {
        public int CumulativeScore { get; set; }
        public string Name { get; set; }
        public int TotalGamesPlayed { get; set; }
        public int GamesWon { get; set; }

        public static Gamer Load(string filepath)
        {
            Gamer gamer = new Gamer();
            Stream stream = new FileStream(filepath, FileMode.Open);
            XElement xmlGamer = XDocument.Load(XmlReader.Create(stream), LoadOptions.None).Element("Gamer");
            stream.Close();
            gamer.Name = xmlGamer.Element("Name").Value;
            return gamer;
        }
    }
}
