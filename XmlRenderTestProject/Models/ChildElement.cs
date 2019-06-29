using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlRenderTestProject.Models.Enums;

namespace XmlRenderTestProject.Models
{
    public class ChildElement: RectangleElement
    {
        public AttachedSides AttachedSide { get; set; }

        public float HingeOffset { get; set; }

    }
}
