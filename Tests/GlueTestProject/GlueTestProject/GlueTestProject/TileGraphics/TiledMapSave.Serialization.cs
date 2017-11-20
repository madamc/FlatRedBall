﻿using System.Xml.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;
using System.Xml.Schema;

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 
namespace TMXGlueLib
{
    #region TiledMapSave Class
    /// <remarks/>
    [XmlType(AnonymousType = true)]
    [XmlRoot(ElementName = "map", Namespace = "", IsNullable = false)]
    public partial class TiledMapSave
    {

        public enum LayerVisibleBehavior { Ignore, Match, Skip };

        #region Fields

        private IDictionary<string, string> propertyDictionaryField = null;

        List<property> mProperties = new List<property>();
        #endregion

        [XmlIgnore]
        public string FileName
        {
            get;
            set;
        }

        [XmlIgnore]
        public IDictionary<string, string> PropertyDictionary
        {
            get
            {
                lock (this)
                {
                    if (propertyDictionaryField == null)
                    {
                        propertyDictionaryField = TiledMapSave.BuildPropertyDictionaryConcurrently(properties);
                    }
                    if (!propertyDictionaryField.Any(p => p.Key.Equals("name", StringComparison.OrdinalIgnoreCase)))
                    {
                        propertyDictionaryField.Add("name", "map");
                    }
                    return propertyDictionaryField;
                }
            }
        }

        public static IDictionary<string, string> BuildPropertyDictionaryConcurrently(IEnumerable<property> properties)
        {
            ConcurrentDictionary<string, string> propertyDictionary = new ConcurrentDictionary<string, string>();
            Parallel.ForEach(properties, (p) =>
            {
                if (p != null && !propertyDictionary.ContainsKey(p.name))
                {
                    // Don't ToLower it - it causes problems when we try to get the column name out again.
                    //propertyDictionaryField.Add(p.name.ToLower(), p.value);

                    propertyDictionary[p.name] = p.value;
                }
            });
            return propertyDictionary;
        }
        
        public List<property> properties
        {
            get { return mProperties; }
            set
            {
                mProperties = value;
            }
        }

        public bool ShouldSerializeproperties()
        {
            return mProperties != null && mProperties.Count != 0;
        }


        /// <remarks/>
        [XmlElement("tileset", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public List<Tileset> Tilesets
        {
            get;
            set;
        }

        
        [XmlElement("layer", typeof(MapLayer))]
        [XmlElement("imagelayer", typeof(MapImageLayer))]
        [XmlElement("objectgroup", typeof(mapObjectgroup))]
        public List<AbstractMapLayer> MapLayers { get; set; }

        /// <remarks/>


        [XmlIgnore]
        public List<MapLayer> Layers => MapLayers.OfType<MapLayer>().ToList();

        /// <remarks/>
        [XmlIgnore]
        public List<mapObjectgroup> objectgroup => MapLayers.OfType<mapObjectgroup>().ToList();

        [XmlIgnore]
        public List<MapImageLayer> ImageLayers => MapLayers.OfType<MapImageLayer>().ToList();

        /// <remarks/>
        [XmlAttribute()]
        public string version
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttribute()]
        public string orientation
        {
            get;
            set;
        }

        /// <summary>
        /// The number of cells this map has on the X axis.
        /// </summary>
        [XmlAttribute("width")]
        public int Width
        {
            get;
            set;
        }

        /// <summary>
        /// The number of cells this map has on the Y axis.
        /// </summary>
        [XmlAttribute("height")]
        public int Height
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttribute()]
        public int tilewidth
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttribute()]
        public int tileheight
        {
            get;
            set;
        }

        public TiledMapSave()
        {
            MapLayers = new List<AbstractMapLayer>();
            Tilesets = new List<Tileset>();
        }

        public List<string> GetReferencedFiles()
        {
            List<string> referencedFiles = new List<string>();

            if(this.Tilesets != null)
            {
                foreach (var tileset in this.Tilesets)
                {
                    if(!string.IsNullOrEmpty(tileset.Source ))
                    {
                        referencedFiles.Add(tileset.Source);
                    }
                    else if (tileset?.Images != null && tileset.Images.Length != 0)
                    {
                        var image = tileset.Images[0];

                        string fileName = image.Source;
                    
                        // keep it relative
                        referencedFiles.Add(fileName);

                    }

                }
            }


            return referencedFiles;
        }
    }

    #endregion

    #region property Class
    public partial class property
    {
        [XmlAttribute()]
        public string name
        {
            get;
            set;
        }

        [XmlAttribute()]
        public string value
        {
            get;
            set;
        }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlIgnore]
        public string StrippedName
        {
            get
            {
                return GetStrippedName(this.name);
            }
        }

        public string StrippedNameLower
        {
            get
            {
                return GetStrippedName(this.name).ToLowerInvariant();
            }
        }


        public static string GetStrippedName(string name)
        {
            string nameWithoutType;
            if (name.Contains('(') && name.Contains(')'))
            {
                int open = name.IndexOf('(');
                int close = name.IndexOf(')');

                nameWithoutType = name.Substring(0, open).Trim();
            }
            else
            {
                nameWithoutType = name;
            }

            return nameWithoutType;
        }

        public static string GetPropertyName(string name)
        {
            if (name.Contains('(') && name.Contains(')'))
            {
                int open = name.IndexOf('(');
                int close = name.IndexOf(')');

                int afterOpen = open+1;

                return name.Substring(afterOpen, close - afterOpen);

            }
            else
            {
                return null;
            }

        }



        public override string ToString()
        {
            return name + " = " + value;
        }
    }
    #endregion

#if !UWP
    [Serializable]
#endif
    public partial class MapImageLayer : AbstractMapLayer
    {
        private MapImageLayerImage imageField;

        private float _offsetX;
        private float _offsetY;
        private float _opacity;

        List<property> mProperties = new List<property>();

        public List<property> properties
        {
            get { return mProperties; }
            set
            {
                mProperties = value;
            }
        }

        private IDictionary<string, string> propertyDictionaryField = null;
        private int _visibleAsInt = 1;

        [XmlIgnore]
        public IDictionary<string, string> PropertyDictionary
        {
            get
            {
                lock (this)
                {
                    if (propertyDictionaryField == null)
                    {
                        propertyDictionaryField = TiledMapSave.BuildPropertyDictionaryConcurrently(properties);
                    }
                    return propertyDictionaryField;
                }
            }
        }

        /// <remarks/>
        [XmlElement("image", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public MapImageLayerImage ImageObject
        {
            get
            {
                return this.imageField;
            }
            set
            {
                this.imageField = value;
            }
        }


        [XmlAttribute("visible")]
        public int VisibleAsInt
        {
            get { return _visibleAsInt; }
            set { _visibleAsInt = value; }
        }


        [XmlAttribute("opacity")]
        public float Opacity
        {
            get { return _opacity; }
            set { _opacity = value; }
        }

        [XmlAttribute("offsetx")]
        public float OffsetX
        {
            get { return _offsetX; }
            set { _offsetX = value; }
        }

        [XmlAttribute("offsety")]
        public float OffsetY
        {
            get { return _offsetY; }
            set { _offsetY = value; }
        }

        [XmlIgnore]
        public bool Visible
        {
            get
            {
                return VisibleAsInt != 0;
            }
        }
    }

    public partial class MapImageLayerImage
    {
        private string _source;

        [XmlAttributeAttribute(AttributeName = "source")]
        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }

        /// <remarks/>
        [XmlAttributeAttribute(AttributeName = "width")]
        public float Width { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute(AttributeName = "height")]
        public float Height { get; set; }
    }


    /// <remarks/>
    [XmlType(AnonymousType = true)]
    [XmlRoot(ElementName = "mapTilesetImage", Namespace = "", IsNullable = false)]
    public partial class TilesetImage
    {

        private string sourceField;

        /// <remarks/>
        [XmlAttribute("source")]
        public string Source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
                if (this.sourceField != null)
                {
                    this.sourceField = this.sourceField.Replace("/", "\\");
                }
            }
        }

        [XmlIgnore]
        public string sourceFileName
        {
            get
            {
                if (!string.IsNullOrEmpty(Source) && Source.Contains("\\"))
                {
                    return Source.Substring(Source.LastIndexOf('\\') + 1);
                }
                else
                {
                    return Source;
                }
            }
        }

        [XmlIgnore]
        public string sourceDirectory
        {
            get
            {
                if (!string.IsNullOrEmpty(Source) && Source.Contains("\\"))
                {
                    return Source.Substring(0, Source.LastIndexOf('\\'));
                }
                else
                {
                    return Source;
                }
            }
        }


        /// <remarks/>
        [XmlAttribute()]
        public int width
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttribute()]
        public int height
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public partial class mapTilesetTileOffset
    {

        private int xField;

        private int yField;

        /// <remarks/>
        [XmlAttribute()]
        public int x
        {
            get
            {
                return xField;
            }
            set
            {
                xField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public int y
        {
            get
            {
                return yField;
            }
            set
            {
                yField = value;
            }
        }
    }



    public partial class mapLayerDataTile
    {
        [XmlAttribute()]
        public string gid { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public partial class mapLayerData
    {

        private string encodingField;

        private string compressionField;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public string encoding
        {
            get
            {
                return this.encodingField;
            }
            set
            {
                this.encodingField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string compression
        {
            get
            {
                return this.compressionField;
            }
            set
            {
                this.compressionField = value;
            }
        }

        [XmlElement("tile", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public mapLayerDataTile[] dataTiles { get; set; }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }


        /// <summary>
        /// Represents the index that this tile is displaying from the source tile map.  This is 1-based.  0 means no tile.  
        /// This can span multiple tilesets.
        /// </summary>
        private List<uint> _ids = null;
        
        
        /// <summary>
        /// Represents all of the tiles in this layer.  A tile with index 0 means there is no tile there.  Non-zero values
        /// mean that the value is painted.  Painted values are global IDs of tiles. Index 0 is the top-left tile. Increasing
        /// the index moves towards the right. When reading the end of the row, the next index represents the first tile in the 
        /// next row.
        /// </summary>
        [XmlIgnore]
        public List<uint> tiles
        {
            get
            {
                if (encodingField != "base64" && encodingField != null && encodingField != "csv")
                {
                    throw new NotImplementedException("Unknown encoding: " + encodingField);
                }

                if (_ids == null)
                {
                    if (encodingField != null && encodingField != "csv")
                    {
                        _ids = new List<uint>(length);
                        // get a stream to the decoded Base64 text

                        var trimmedValue = Value.Trim();

                        Stream data = new MemoryStream(Convert.FromBase64String(trimmedValue), false);
                        switch (compression)
                        {
                            case "gzip":
                                data = new GZipStream(data, CompressionMode.Decompress, false);
                                break;
                            case "zlib":
#if SUPPORTS_ZLIB
                                data = new Ionic.Zlib.ZlibStream(data, Ionic.Zlib.CompressionMode.Decompress, false);
#else
                                throw new NotImplementedException("Does not support zlib");
#endif
                                break;
                            case null:
                                // Not compressed. Data is already decoded.
                                break;
                            default:
                                throw new InvalidOperationException("Unknown compression: " + compression);
                        }

                        // simply read in all the integers
                        using (data)
                        {
                            using (BinaryReader reader = new BinaryReader(data))
                            {
                                _ids = new List<uint>();
                                for (int i = 0; i < length; i++)
                                {
                                    _ids.Add(reader.ReadUInt32());
                                }
                            }
                        }
                    }
                    else if (encodingField == "csv")
                    {
                        string[] idStrs = Value.Split(",\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        _ids = idStrs.AsParallel().Select(id => 
                        {
                            uint gid;
                            if (!uint.TryParse(id, out gid))
                            {
                                gid = 0;
                            }
                            return gid;
                        }).ToList();
                    }
                    else if (encodingField == null)
                    {
                        _ids = dataTiles.AsParallel().Select(dt =>
                        {
                            uint gid;
                            if (!uint.TryParse(dt.gid, out gid))
                            {
                                gid = 0;
                            }
                            return gid;
                        }).ToList();
                    }
                }

                return _ids;
            }

        }

        public int length { get; set; }
    }

#if !UWP
    [Serializable]
#endif
    public partial class mapObjectgroup : AbstractMapLayer
    {
        private mapObjectgroupObject[] objectField;


        List<property> mProperties = new List<property>();

        public List<property> properties
        {
            get { return mProperties; }
            set
            {
                mProperties = value;
            }
        }

        private IDictionary<string, string> propertyDictionaryField = null;
        private int _visibleAsInt = 1;

        [XmlIgnore]
        public IDictionary<string, string> PropertyDictionary
        {
            get
            {
                lock (this)
                {
                    if (propertyDictionaryField == null)
                    {
                        propertyDictionaryField = TiledMapSave.BuildPropertyDictionaryConcurrently(properties);
                    }
                    return propertyDictionaryField;
                }
            }
        }

        /// <remarks/>
        [XmlElement("object", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public mapObjectgroupObject[] @object
        {
            get
            {
                return this.objectField;
            }
            set
            {
                this.objectField = value;
            }
        }


        [XmlAttribute("visible")]
        public int VisibleAsInt
        {
            get { return _visibleAsInt; }
            set { _visibleAsInt = value; }
        }

        [XmlIgnore]
        public bool Visible
        {
            get
            {
                return VisibleAsInt != 0;
            }
        }
    }

    /// <remarks/>
    public partial class mapObjectgroupObject
    {
        private mapObjectgroupObjectEllipse ellipseField = null;

        private mapObjectgroupObjectPolygon[] polygonField;

        private mapObjectgroupObjectPolyline[] polylineField;

        private double xField;

        private double yField;

        private string _name;


        [XmlAttributeAttribute(AttributeName = "name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [XmlAttribute(AttributeName = "gid")]
        public string __proxygid
        {
            get { return gid?.ToString(); }
            set { gid = uint.Parse(value); }
        }

        [XmlIgnore]
        public uint? gid { get; set; }

        private IDictionary<string, string> propertyDictionaryField = null;

        [XmlIgnore]
        public IDictionary<string, string> PropertyDictionary
        {
            get
            {
                lock (this)
                {
                    if (propertyDictionaryField == null)
                    {
                        propertyDictionaryField = TiledMapSave.BuildPropertyDictionaryConcurrently(properties);
                    }
                    if (!string.IsNullOrEmpty(this.Name) && !propertyDictionaryField.Any(p => p.Key.Equals("name", StringComparison.OrdinalIgnoreCase)))
                    {
                        propertyDictionaryField.Add("name", this.Name);
                    }
                    return propertyDictionaryField;
                }
            }
        }

        List<property> mProperties = new List<property>();

        public List<property> properties
        {
            get { return mProperties; }
            set
            {
                mProperties = value;
            }
        }

        [XmlElement("ellipse", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public mapObjectgroupObjectEllipse ellipse
        {
            get
            {
                return ellipseField;
            }
            set
            {
                ellipseField = value; 
            }
        }

        /// <remarks/>
        [XmlElement("polygon", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public mapObjectgroupObjectPolygon[] polygon
        {
            get
            {
                return this.polygonField;
            }
            set
            {
                this.polygonField = value;
            }
        }

        /// <remarks/>
        [XmlElement("polyline", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public mapObjectgroupObjectPolyline[] polyline
        {
            get
            {
                return this.polylineField;
            }
            set
            {
                this.polylineField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute("x")]
        public double x
        {
            get
            {
                return this.xField;
            }
            set
            {
                this.xField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute("y")]
        public double y
        {
            get
            {
                return this.yField;
            }
            set
            {
                this.yField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public float width { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public float height { get; set; }

        [XmlAttribute("rotation")]
        public double Rotation
        {
            get;
            set;
        }
    }

    [XmlType(AnonymousType = true)]
    public partial class mapObjectgroupObjectEllipse
    {
        
    }

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public partial class mapObjectgroupObjectPolygon
    {

        private string pointsField;

        /// <remarks/>
        [XmlAttribute()]
        public string points
        {
            get
            {
                return this.pointsField;
            }
            set
            {
                this.pointsField = value;
            }
        }
    }

#region mapObjectgroupObjectPolyline

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public partial class mapObjectgroupObjectPolyline
    {

        private string pointsField;

        /// <remarks/>
        [XmlAttribute()]
        public string points
        {
            get
            {
                return this.pointsField;
            }
            set
            {
                this.pointsField = value;
            }
        }
    }

#endregion

#region NewDataSet Class
    /// <remarks/>
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public partial class NewDataSet
    {

        private TiledMapSave[] itemsField;

        /// <remarks/>
        [XmlElement("map")]
        public TiledMapSave[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
    }

#endregion

}