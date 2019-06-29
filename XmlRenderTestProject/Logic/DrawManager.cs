using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace XmlRenderTestProject.Logic
{
    public class DrawManager
    {
        public void DrawRectangle(float heigth, float width, float x, float y, Canvas layoutRoot)
        {
            const int scale = 20;

            var rectangle = new Rectangle();
            rectangle.Height = heigth/ scale;
            rectangle.Width = width/ scale;


            SolidColorBrush blackBrush = new SolidColorBrush();
            blackBrush.Color = Colors.Black;
      
            rectangle.StrokeThickness = 2;
            rectangle.Stroke = blackBrush;
            //rectangle.Margin = new System.Windows.Thickness((x-width/2)/ scale, (y-heigth)/ scale, 0,0);
            rectangle.Margin = new System.Windows.Thickness((x ) / scale, (y ) / scale, 0, 0);
            // Add Rectangle to the Grid.  
            layoutRoot.Children.Add(rectangle);
        }
    }
}
