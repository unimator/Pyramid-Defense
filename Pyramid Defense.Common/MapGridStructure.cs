using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;
using Color = System.Drawing.Color;

namespace Pyramid_Defense.Common
{
    public enum MapElement
    {
        None,
        Start,
        Finish,
        StandardHex
    }

    public class MapGridStructure
    {
        public int SizeX { get; private set; }
        public int SizeZ { get; private set; }
        public string MapName { get; private set; }
        public string DataFileName { get; private set; }
        public float OffsetX { get; private set; }
        public float OffsetZ { get; private set; }
        public MapElement[,] MapStructure { get; private set; }

        private readonly string _xmlFileName;

        private Dictionary<string, string> xmlAttributes = new Dictionary<string, string>();

        public MapGridStructure(string xmlFileName)
        {
            _xmlFileName = xmlFileName;
            ParseLevelInfoXml();
            LoadMapStructure();
        }

        private void LoadMapStructure()
        {
            Bitmap bitmap = new Bitmap(DataFileName);
            SizeX = bitmap.Width;
            SizeZ = bitmap.Height;
            MapStructure = new MapElement[SizeX, SizeZ];
            for (int x = 0; x < SizeX; ++x)
            {
                for (int z = 0; z < SizeZ; ++z)
                {
                    Color pixel = bitmap.GetPixel(x, z);

                    if (pixel.ToArgb().Equals(Color.Black.ToArgb()))
                    {
                        MapStructure[x, z] = MapElement.StandardHex;
                    } else if (pixel.ToArgb().Equals(Color.White.ToArgb()))
                    {
                        MapStructure[x, z] = MapElement.None;
                    } else if (pixel.ToArgb().Equals(Color.Blue.ToArgb()))
                    {
                        MapStructure[x, z] = MapElement.Finish;
                    } else if (pixel.ToArgb().Equals(Color.Red.ToArgb()))
                    {
                        MapStructure[x, z] = MapElement.Start;
                    }
                }
            }
        }

        private void ParseLevelInfoXml()
        {
            var rawXmlData = File.ReadAllText(_xmlFileName);

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(rawXmlData);

            foreach (XmlNode node in xmlDocument.GetElementsByTagName("root").Item(0).ChildNodes)
            {
                xmlAttributes.Add(node.Name, node.InnerText.Trim());
            }

            if (xmlAttributes.ContainsKey("mapname"))
            {
                MapName = xmlAttributes["mapname"];
            }

            if (xmlAttributes.ContainsKey("data_file_name"))
            {
                DataFileName = xmlAttributes["data_file_name"];
            }

            if (xmlAttributes.ContainsKey("offset_x"))
            {
                OffsetX = float.Parse(xmlAttributes["offset_x"]);
            }

            if (xmlAttributes.ContainsKey("offset_z"))
            {
                OffsetZ = float.Parse(xmlAttributes["offset_z"]);
            }
        }
    }
}