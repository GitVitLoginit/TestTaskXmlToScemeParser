using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlRenderTestProject.Models
{
    public class RootElement: RectangleElement
    {
        public float X { get; set; }
        public float Y { get; set; }
        public List<ChildElement> Childrens { get; set; }
    }
}
