using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using XmlRenderTestProject.Models;
using XmlRenderTestProject.Models.Enums;
using XmlRenderTestProject.Helpers;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace XmlRenderTestProject.ViewModels
{
    public class MainWindowViewModel
    {
        public ICommand ParseXamlCommand { get; set; }
        public ICommand CleanDrawFieldCommand { get; set; }
        public ICommand SaveToFileCommand { get; set; }
        public MainWindowViewModel( Canvas drawParent)
        {
            _drawParent = drawParent;

            ParseXamlCommand = new DelegateCommand(OnParseXamlClicked);
            CleanDrawFieldCommand = new DelegateCommand(OnCleanDrawFieldClicked);
            SaveToFileCommand = new DelegateCommand(OnSaveClicked);
        }


        private Canvas _drawParent;


        private void OnSaveClicked(object obj)
        {
            RenderTargetBitmap bitmap = new RenderTargetBitmap(1000, 1000, 96, 96,
                                               PixelFormats.Pbgra32);
            bitmap.Render(_drawParent);

            PngBitmapEncoder image = new PngBitmapEncoder();
            image.Frames.Add(BitmapFrame.Create(bitmap));
            using (Stream fs = File.Create("Result.jpg"))
            {
                image.Save(fs);
            }
        }

        private void OnCleanDrawFieldClicked(object obj)
        {
            _drawParent.Children.Clear();
        }

        private void OnParseXamlClicked(object obj)
        {
            var xmlParseManager = new Logic.XmlParserManager();
            var xmlFile = xmlParseManager.TryOpenXmlFile("BeerPack.xml");
            if (xmlFile == null)
            {
                return;
            }

            var parsedTree = xmlParseManager.ParseXaml(xmlFile);

            var drawManager = new Logic.DrawManager();

            drawManager.DrawRectangle(parsedTree.Height, parsedTree.Width, parsedTree.X, parsedTree.Y, _drawParent);

            DrawTree(parsedTree.Childrens, new DrawInfo() { Heigth = parsedTree.Height, Width = parsedTree.Width, X = parsedTree.X, Y = parsedTree.Y }, drawManager);
        }


        private void DrawTree(List<ChildElement> childElements, DrawInfo parentDrawInfo, Logic.DrawManager drawManager)
        {
            DrawInfo childDrawInfo = new DrawInfo();
            foreach (var child in childElements)
            {
                if (child.AttachedSide == AttachedSides.left || child.AttachedSide == AttachedSides.rigth)
                {
                    childDrawInfo.Heigth = parentDrawInfo.Heigth;
                    childDrawInfo.Width = child.Width;
                }
                else
                {
                    childDrawInfo.Heigth = child.Height;
                    childDrawInfo.Width = child.Width;
                }
                switch (child.AttachedSide)
                {
                    case AttachedSides.rigth:
                        {
                            childDrawInfo.X = parentDrawInfo.X + parentDrawInfo.Width;
                            childDrawInfo.Y = parentDrawInfo.Y + (parentDrawInfo.Heigth - childDrawInfo.Heigth) / 2;
                            break;
                        }
                    case AttachedSides.left:
                        {
                            childDrawInfo.X = parentDrawInfo.X - childDrawInfo.Width;
                            childDrawInfo.Y = parentDrawInfo.Y + (parentDrawInfo.Heigth - childDrawInfo.Heigth) / 2;
                            break;
                        }
                    case AttachedSides.top:
                        {
                            childDrawInfo.X = parentDrawInfo.X + (parentDrawInfo.Width - childDrawInfo.Width) / 2;
                            childDrawInfo.Y = parentDrawInfo.Y - childDrawInfo.Heigth;
                            break;
                        }
                    case AttachedSides.bottom:
                        {
                            childDrawInfo.X = parentDrawInfo.X + (parentDrawInfo.Width - childDrawInfo.Width) / 2;
                            childDrawInfo.Y = parentDrawInfo.Y + parentDrawInfo.Heigth;
                            break;
                        }
                }

                //MessageBox.Show("wef");
                drawManager.DrawRectangle(childDrawInfo.Heigth, childDrawInfo.Width, childDrawInfo.X, childDrawInfo.Y, _drawParent);


                if (child.Childrens != null)
                {
                    DrawTree(child.Childrens, new DrawInfo()
                    {
                        Width = childDrawInfo.Width,
                        Heigth = childDrawInfo.Heigth,
                        X = childDrawInfo.X,
                        Y = childDrawInfo.Y
                    }
                    , drawManager);
                }
            }
        }
        }
}
