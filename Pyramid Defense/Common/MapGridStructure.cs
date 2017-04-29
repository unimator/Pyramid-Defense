using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

namespace Common
{
    internal class MapGridStructure
    {
        public static List<string> AvailableNodes = new List<string>()
        {
            "mapname",
            "data_file_name",
            "offset_x",
            "offset_z",
        };

        public int SizeX { get; private set; }
        public int SizeZ { get; private set; }
        public string MapName { get; private set; }
        public int OffsetX { get; private set; }
        public int OffsetZ { get; private set; }

        private readonly string _xmlFileName;

        internal MapGridStructure(string xmlFileName)
        {
            _xmlFileName = xmlFileName;
            ParseLevelInfoXml();
        }

        private void ParseLevelInfoXml()
        {
            var rawXmlData = File.ReadAllText(_xmlFileName);

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(rawXmlData);

            foreach (XmlNode node in xmlDocument.ChildNodes)
            {
                Debug.Log(node.Name + ", " + node.Value);
            }
        }
    }
}