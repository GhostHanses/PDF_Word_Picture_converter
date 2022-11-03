using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace Controller
{
    public class DrawPic : TextBlock
    {
        public bool Choosed;
        public string path;
        public static bool fullpath = false;
        public DrawPic(string text)
        {
            FontSize = 14;
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            Margin = new Thickness(0);
            Choosed = false;
            path = text;
            if (!fullpath)
            {
                Text = text.Substring(text.LastIndexOf("\\") + 1,text.Length-1-text.LastIndexOf("\\"));
            }
            else
            {
                Text = text;
            }
            
            Background = Brushes.White;
            ClickEventAction.AddClickEventAction(this,Click);
        }
        

        private void Click(object obj)
        {
            if (((DrawPic)obj).Choosed == false)
            {
                ((DrawPic)obj).Choosed = true;
                if (Picture.ChoosedDrawPic != null)
                {
                    Picture.ChoosedDrawPic.Background = Brushes.White;
                    Picture.ChoosedDrawPic.Choosed = false;
                }
                Picture.ChoosedDrawPic = (DrawPic)obj;
                ((DrawPic)obj).Background = Brushes.LightBlue;
                Main.Picture.up.IsEnabled = true;
                Main.Picture.down.IsEnabled = true;
                Main.Picture.delete.IsEnabled = true;
            }
            else
            {
                Picture.ChoosedDrawPic = null;
                ((DrawPic)obj).Choosed = false;
                ((DrawPic)obj).Background = Brushes.White;
                Main.Picture.up.IsEnabled = false;
                Main.Picture.down.IsEnabled = false;
                Main.Picture.delete.IsEnabled = false;
            }
        }
    }
}