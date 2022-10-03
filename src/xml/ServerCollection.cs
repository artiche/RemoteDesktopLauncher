using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace RemoteDesktopLauncher.xml
{
    [XmlRoot("Servers")]
    public class ServerCollection : IEnumerable<Server>
    {

        public ServerCollection()
        {
            Servers = new List<Server>();
        }

        [XmlElement("Server")]
        public List<Server> Servers { get; set; }


        public void Serialize(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ServerCollection));
            using (XmlWriter writer = XmlWriter.Create(path, new XmlWriterSettings() { Indent = true }))
            {
                serializer.Serialize(writer, this);
            }
        }

        public static ServerCollection Deserialize(string path)
        {
            ServerCollection bc = null;
            XmlSerializer serializer = new XmlSerializer(typeof(ServerCollection));
            using (XmlReader reader = XmlReader.Create(path))
            {
                bc = (ServerCollection)serializer.Deserialize(reader);
            }

            return bc;
        }

        public void Add(object o)
        {
            var server = o as Server;
            if (o != null) Servers.Add(server);
        }

        public IEnumerator<Server> GetEnumerator()
        {
            return Servers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Servers.GetEnumerator();
        }
    }
}
