using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using XmlRenderTestProject.Models;
using XmlRenderTestProject.Models.Enums;

namespace XmlRenderTestProject.Logic
{
    public class XmlParserManager
    {
        public XDocument TryOpenXmlFile(string path)
        {
            try
            {
                XDocument  doc = XDocument.Load(path);
                return doc;
            }
            catch
            {
                return null;
            }
        }

        public RootElement ParseXaml(XDocument doc)
        {
            var tree = new RootElement();
            FillRootInfo(tree, doc);

            var attachedElements = doc.Root.Element("panels").Element("item").Element("attachedPanels");
            FillAttachedElementsInfo(attachedElements, tree,null);



            return tree;
        }

        private void FillRootInfo(RootElement tree, XDocument doc)
        {
            tree.X=Single.Parse( doc.Root.Attribute("rootX").Value, CultureInfo.InvariantCulture);
            tree.Y = Single.Parse(doc.Root.Attribute("rootY").Value, CultureInfo.InvariantCulture);
            tree.Height= Single.Parse(doc.Root.Attribute("originalDocumentHeight").Value, CultureInfo.InvariantCulture);
            tree.Width = Single.Parse(doc.Root.Attribute("originalDocumentWidth").Value, CultureInfo.InvariantCulture);
        }
        private void FillChildInfo(ChildElement child, XElement doc)
        {
            child.HingeOffset = Single.Parse(doc.Attribute("hingeOffset").Value, CultureInfo.InvariantCulture);
            child.AttachedSide = GetSide(doc.Attribute("attachedToSide").Value);
            child.Height = Single.Parse(doc.Attribute("panelHeight").Value, CultureInfo.InvariantCulture);
            child.Width = Single.Parse(doc.Attribute("panelWidth").Value, CultureInfo.InvariantCulture);
        }
        private void FillAttachedElementsInfo(XElement attachedElements, RootElement rootTree, ChildElement childTree)
        {
            if (attachedElements.HasElements)
            {
                if (rootTree != null)
                {
                    rootTree.Childrens = new List<ChildElement>();
                }
                else
                {
                    childTree.Childrens= new List<ChildElement>();
                }
            }
            else
            {
                return;
            }

            foreach (XElement childElementXml in attachedElements.Elements())
            {
                var newChild = new ChildElement();
                if (rootTree != null)
                {
                    rootTree.Childrens.Add(newChild);
                }
                else
                {
                    childTree.Childrens.Add(newChild);
                }
                FillChildInfo(newChild, childElementXml);

                FillAttachedElementsInfo(childElementXml.Element("attachedPanels"), null, newChild);
            }
        }

        private AttachedSides GetSide(string side)
        {
            switch (side)
            {
                case "1":
                    return AttachedSides.rigth;
                case "0":
                    return AttachedSides.bottom;
                case "2":
                    return AttachedSides.top;
                case "3":
                    return AttachedSides.left;
                default:
                    return  AttachedSides.bottom;
            }
        }
    }
}
