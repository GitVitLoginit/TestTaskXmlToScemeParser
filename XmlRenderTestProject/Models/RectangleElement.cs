using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlRenderTestProject.Models
{
    public  class RectangleElement
    {
        public   float Width { get; set; }
        public float Height { get; set; }
        public List<ChildElement> Childrens { get; set; }
    }
}
