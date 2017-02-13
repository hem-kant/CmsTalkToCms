using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CmsTalkToCms.Common.Model
{
     
        [XmlRoot(ElementName = "c2c", Namespace = "http://www.sdl.com/web/schemas/core")]
        public class News
        {
            [XmlElement(ElementName = "subheader", Namespace = "http://www.sdl.com/web/schemas/core")]
            public string Subheader { get; set; }
            [XmlElement(ElementName = "bodyText", Namespace = "http://www.sdl.com/web/schemas/core")]
            public string BodyText { get; set; }
            [XmlElement(ElementName = "image", Namespace = "http://www.sdl.com/web/schemas/core")]
            public string Image { get; set; }
            [XmlAttribute(AttributeName = "xmlns")]
            public string Xmlns { get; set; }        
    }
}
