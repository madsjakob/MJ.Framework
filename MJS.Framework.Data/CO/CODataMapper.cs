using System.IO;
using System.Xml;
using System.Xml.XPath;
using MJS.Framework.Base.Utils;
using MJS.Framework.Data.Extensions;
using MJS.Framework.Data.Interfaces;

namespace MJS.Framework.Data.CO
{
    public class CODataMapper
    {
        public static void DataClassToXmlFile(IDataClass dataClass, string filename)
        {
            XmlDocument xmlDoc = new XmlDocument();
            DataClassToXml(dataClass, xmlDoc);
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                XmlUtils.XmlToStream(xmlDoc, fs);
            }
        }

        public static void XmlFileToDataClass(IDataClass dataClass, string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                XPathDocument xpathDoc = XmlUtils.StreamToXPathDocument(fs);
                XmlToDataClass(dataClass, xpathDoc);
            }
        }

        public static void DataClassToZLibFile(IDataClass dataClass, string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                DataClassToBlob(dataClass, fs);
            }
        }

        public static void ZLibFileToDataClass(IDataClass dataClass, string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                BlobToDataClass(dataClass, fs);
            }
        }

        public static void DataClassToXml(IDataClass dataClass, XmlDocument xmlDoc)
        {
            XmlElement root = xmlDoc.CreateElement("root");
            root.AddDataClass(dataClass);
            xmlDoc.AppendChild(root);
        }

        public static void XmlToDataClass(IDataClass dataClass, IXPathNavigable xml)
        {
            XPathNavigator nav = xml.CreateNavigator();
            XPathNavigator root = nav.SelectSingleNode("root");
            root.ReadDataClass(dataClass);
        }

        public static void DataClassToBlob(IDataClass dataClass, Stream blobStream)
        {
            XmlDocument xmlDoc = new XmlDocument();
            DataClassToXml(dataClass, xmlDoc);
            using (Stream compressedStream = ZLibUtils.CompressStream(blobStream))
            {
                XmlUtils.XmlToStream(xmlDoc, compressedStream);
            }
        }

        public static void BlobToDataClass(IDataClass dataClass, Stream blobStream)
        {
            using (Stream stream = ZLibUtils.DeCompressStream(blobStream))
            {
                XPathDocument temp = XmlUtils.StreamToXPathDocument(stream);
                XmlToDataClass(dataClass, temp);
            }
        }
    }
}
