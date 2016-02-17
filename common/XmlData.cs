using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;

namespace common
{
    public class XmlData : IDisposable
    {
        private static string AssemblyDirectory => Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        private static ILog log = LogManager.GetLogger(nameof(XmlData));

        public XmlData(string path = "resources/xml")
        {
            ObjectTypeToElement = new ReadOnlyDictionary<ushort, XElement>
                (typeToElem = new Dictionary<ushort, XElement>());
            ObjectTypeToId = new ReadOnlyDictionary<ushort, string>
                (typeToId = new Dictionary<ushort, string>());
            IdToObjectType = new ReadOnlyDictionary<string, ushort>
                (idToType = new Dictionary<string, ushort>(StringComparer.InvariantCultureIgnoreCase));

            TileTypeToElement = new ReadOnlyDictionary<ushort, XElement>
                (typeToTileElem = new Dictionary<ushort, XElement>());
            TileTypeToId = new ReadOnlyDictionary<ushort, string>
                (typeToTileId = new Dictionary<ushort, string>());
            IdToTileType = new ReadOnlyDictionary<string, ushort>
                (idToTileType = new Dictionary<string, ushort>(StringComparer.InvariantCultureIgnoreCase));

            Tiles = new ReadOnlyDictionary<ushort, TileDesc>
                (tiles = new Dictionary<ushort, TileDesc>());
            Items = new ReadOnlyDictionary<ushort, Item>
                (items = new Dictionary<ushort, Item>());
            ObjectDescs = new ReadOnlyDictionary<ushort, ObjectDesc>
                (objDescs = new Dictionary<ushort, ObjectDesc>());
            Portals = new ReadOnlyDictionary<ushort, PortalDesc>
                (portals = new Dictionary<ushort, PortalDesc>());

            addition = new XElement("ExtData");

            string basePath = Path.Combine(AssemblyDirectory, path);
            log.Info($"Loading game data from \"{basePath}\"");
            var xmls = Directory.EnumerateFiles(basePath, "*.xml", SearchOption.AllDirectories).ToArray();
            for (int i = 0; i < xmls.Length; i++)
            {
                log.Info($"Loading \"{path + xmls[i].Replace(path, "")}\" ({i + 1}/{xmls.Length})");
                using (var stream = File.OpenRead(xmls[i]))
                    ProcessXml(XElement.Load(stream));
            }
            log.Info("Finish loading game data.");
            log.Info($"{items.Count} Items");
            log.Info($"{tiles.Count} Tiles");
            log.Info($"{objDescs.Count} Objects");
            log.Info($"{addition.Elements().Count()} Additions");
        }

        #region Local

        #region Object

        private Dictionary<ushort, XElement> typeToElem { get; set; }
        private Dictionary<ushort, string> typeToId { get; set; }
        private Dictionary<string, ushort> idToType { get; set; }

        #endregion

        #region Tile

        private Dictionary<ushort, XElement> typeToTileElem { get; set; }
        private Dictionary<ushort, string> typeToTileId { get; set; }
        private Dictionary<string, ushort> idToTileType { get; set; }

        #endregion

        private Dictionary<ushort, TileDesc> tiles;
        private Dictionary<ushort, Item> items;
        private Dictionary<ushort, ObjectDesc> objDescs;
        private Dictionary<ushort, PortalDesc> portals;

        #endregion

        #region Public

        #region Object

        public IDictionary<ushort, XElement> ObjectTypeToElement { get; private set; }
        public IDictionary<ushort, string> ObjectTypeToId { get; private set; }
        public IDictionary<string, ushort> IdToObjectType { get; private set; }

        #endregion

        #region Tile

        public IDictionary<ushort, XElement> TileTypeToElement { get; private set; }
        public IDictionary<ushort, string> TileTypeToId { get; private set; }
        public IDictionary<string, ushort> IdToTileType { get; private set; }

        #endregion

        public IDictionary<ushort, TileDesc> Tiles { get; private set; }
        public IDictionary<ushort, Item> Items { get; private set; }
        public IDictionary<ushort, ObjectDesc> ObjectDescs { get; private set; }
        public IDictionary<ushort, PortalDesc> Portals { get; private set; }

        #endregion

        private int updateCount = 0;
        private int prevUpdateCount = -1;
        private XElement addition { get; set; }
        private string[] addXml { get; set; }
        private AutoAssign assign { get; set; }

        private void ProcessXml(XElement root)
        {
            AddObjects(root);
            AddGrounds(root);
        }

        public void AddObjects(XElement root)
        {
            foreach (var elem in root.XPathSelectElements("//Object"))
            {
                if (elem.Element("Class") == null) continue;
                string cls = elem.Element("Class").Value;
                string id = elem.Attribute("id").Value;

                ushort type;
                var typeAttr = elem.Attribute("type");
                if (typeAttr == null)
                    type = assign.Assign(id, elem);
                else
                    type = (ushort)Utils.FromString(typeAttr.Value);

                if (cls == "PetBehavior" || cls == "PetAbility") continue;

                if (typeToId.ContainsKey(type))
                    log.Warn($"\"{id}\" and \"{typeToId[type]}\" has the same ID of 0x{type:x4}!");
                if (idToType.ContainsKey(id))
                    log.Warn($"0x{type:x4} and 0x{idToType[id]:x4} has the same name of {id}!");

                typeToId[type] = id;
                idToType[id] = type;
                typeToElem[type] = elem;

                switch (cls)
                {
                    case "Equipment":
                    case "Dye":
                        items[type] = new Item(type, elem);
                        break;
                    case "Portal":
                    case "GuildHallPortal":
                        try
                        {
                            portals[type] = new PortalDesc(type, elem);
                        }
                        catch
                        {
                            log.Error($"Error for portal \"{type}\":\"{id}\"");
                            /*3392,1792,1795,1796,1805,1806,1810,1825 -- no location, assume nexus?*
        *  Tomb Portal of Cowardice,  Dungeon Portal,  Portal of Cowardice,  Realm Portal,  Glowing Portal of Cowardice,  Glowing Realm Portal,  Nexus Portal,  Locked Wine Cellar Portal*/
                        }
                        break;
                    case "Pet":
                    case "PetSkin":
                    case "PetBehavior":
                    case "PetAbility":
                        break;
                    default:
                        objDescs[type] = new ObjectDesc(type, elem);
                        break;
                }

                var extAttr = elem.Attribute("ext");
                bool ext;
                if (extAttr != null && bool.TryParse(extAttr.Value, out ext) && ext)
                {
                    if (elem.Attribute("type") == null)
                        elem.Add(new XAttribute("type", type));
                    addition.Add(elem);
                    updateCount++;
                }
            }
        }

        public void AddGrounds(XElement root)
        {
            foreach (var elem in root.XPathSelectElements("//Ground"))
            {
                string id = elem.Attribute("id").Value;

                ushort type;
                var typeAttr = elem.Attribute("type");
                type = (ushort)Utils.FromString(typeAttr.Value);

                if (typeToTileId.ContainsKey(type))
                    log.Warn($"\"{id}\" and \"{typeToTileId[type]}\" has the same ID of 0x{type:x4}!");
                if (idToTileType.ContainsKey(id))
                    log.Warn($"0x{type:x4} and 0x{idToTileType[id]:x4} has the same name of {id}!");

                typeToTileId[type] = id;
                idToTileType[id] = type;
                typeToTileElem[type] = elem;

                tiles[type] = new TileDesc(type, elem);

                var extAttr = elem.Attribute("ext");
                bool ext;
                if (extAttr != null && bool.TryParse(extAttr.Value, out ext) && ext)
                {
                    addition.Add(elem);
                    updateCount++;
                }
            }
        }

        private void UpdateXml()
        {
            if (prevUpdateCount != updateCount)
            {
                addXml = new string[] { addition.ToString() };
                prevUpdateCount = updateCount;
            }
        }

        public string[] AdditionXml
        {
            get
            {
                UpdateXml();
                return addXml;
            }
        }

        public void Dispose()
        {
            assign.Dispose();
        }

        private class AutoAssign : Settings
        {
            private XmlData dat { get; set; }
            private ushort nextSignedId { get; set; }
            private ushort nextFullId { get; set; }

            internal AutoAssign(XmlData dat) : base("autoId")
            {
                this.dat = dat;
                nextSignedId = GetValue<ushort>("nextSigned", "24576"); //0x6000
                nextFullId = GetValue<ushort>("nextFull", "32768");     //0x8000
            }

            public ushort Assign(string id, XElement elem)
            {
                ushort type = GetValue<ushort>(id, "0");
                if (type == 0)
                {
                    var cls = elem.Element("Class");
                    bool isFull = cls.Value == "Dye" ||
                        (cls.Value == "Equipment" && !elem.Elements("Projectile").Any());
                    if (isFull)
                    {
                        type = nextFullId++;
                        SetValue("nextFull", nextFullId.ToString());
                    }
                    else
                    {
                        type = nextSignedId++;
                        SetValue("nextSigned", nextSignedId.ToString());
                    }
                    SetValue(id, type.ToString());
                    log.Info($"Auto assigned \"{id}\" to 0x{type:x4}");
                }
                return type;
            }
        }
    }
}
