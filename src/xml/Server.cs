using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RemoteDesktopLauncher.xml
{
    public class Server
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Host { get; set; }

        [XmlAttribute]
        public string IP { get; set; }

        [XmlAttribute]
        public string Description { get; set; }
    }
}
