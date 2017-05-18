using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;

namespace Pyramid_Defense.Common
{
    public class MapGridStructure
    {
        private Dictionary<string, string> xmlAttributes = new Dictionary<string, string>();
        private readonly string _xmlFileName;

        public int SizeX { get; private set; }

        public int SizeZ { get; private set; }

        public string MapName { get; private set; }

        public string DataFileName { get; private set; }

        public float OffsetX { get; private set; }

        public float OffsetZ { get; private set; }

        public MapElement[,] MapStructure { get; private set; }

        public MapGridStructure(string xmlFileName)
        {
            this._xmlFileName = xmlFileName;
            this.ParseLevelInfoXml();
            this.LoadMapStructure();
        }

        private void LoadMapStructure()
        {
            Bitmap bitmap = new Bitmap(this.DataFileName);
            this.SizeX = bitmap.Width;
            this.SizeZ = bitmap.Height;
            this.MapStructure = new MapElement[this.SizeX, this.SizeZ];
            for (int x = 0; x < this.SizeX; ++x)
            {
                for (int y = 0; y < this.SizeZ; ++y)
                {
                    Color pixel = bitmap.GetPixel(x, y);
                    int argb = pixel.ToArgb();
                    if (argb.Equals(Color.Black.ToArgb()))
                    {
                        this.MapStructure[x, y] = MapElement.StandardHex;
                    }
                    else
                    {
                        argb = pixel.ToArgb();
                        if (argb.Equals(Color.White.ToArgb()))
                        {
                            this.MapStructure[x, y] = MapElement.None;
                        }
                        else
                        {
                            argb = pixel.ToArgb();
                            if (argb.Equals(Color.Blue.ToArgb()))
                            {
                                this.MapStructure[x, y] = MapElement.Finish;
                            }
                            else
                            {
                                argb = pixel.ToArgb();
                                if (argb.Equals(Color.Red.ToArgb()))
                                    this.MapStructure[x, y] = MapElement.Start;
                            }
                        }
                    }
                }
            }
        }

        private void ParseLevelInfoXml()
        {
            string xml = File.ReadAllText(this._xmlFileName);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            foreach (XmlNode childNode in xmlDocument.GetElementsByTagName("root").Item(0).ChildNodes)
                this.xmlAttributes.Add(childNode.Name, childNode.InnerText.Trim());
            if (this.xmlAttributes.ContainsKey("mapname"))
                this.MapName = this.xmlAttributes["mapname"];
            if (this.xmlAttributes.ContainsKey("data_file_name"))
                this.DataFileName = this.xmlAttributes["data_file_name"];
            if (this.xmlAttributes.ContainsKey("offset_x"))
                this.OffsetX = float.Parse(this.xmlAttributes["offset_x"]);
            if (!this.xmlAttributes.ContainsKey("offset_z"))
                return;
            this.OffsetZ = float.Parse(this.xmlAttributes["offset_z"]);
        }
    }
}
