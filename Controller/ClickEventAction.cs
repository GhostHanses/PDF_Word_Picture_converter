using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Controller
{
    class ClickEventAction
    {
        static List<ClickEventAction> clickEventActions = new List<ClickEventAction>();

        public static void AddClickEventAction(FrameworkElement frameworkElement, Action<object> action = null)
        {
            if (frameworkElement == null) return;
            clickEventActions.Add(new ClickEventAction(frameworkElement, action));
        }

        public static void RemoveClickEventAction(FrameworkElement frameworkElement)
        {
            foreach (ClickEventAction item in clickEventActions)
            {
                if (item.FrameworkElement == frameworkElement)
                {
                    item.FrameworkElement.MouseLeave -= item.UIElement_MouseLeave;
                    item.FrameworkElement.MouseLeftButtonUp -= item.UIElement_MouseLeftButtonUp;
                    item.FrameworkElement.MouseLeftButtonDown -= item.UIElement_MouseLeftButtonDown;
                    item.FrameworkElement.MouseEnter -= item.FrameworkElement_MouseEnter;
                }
            }
        }

        public ClickEventAction(FrameworkElement frameworkElement, Action<object> action = null)
        {
            FrameworkElement = frameworkElement;
            Action = action;
            frameworkElement.MouseLeftButtonDown += UIElement_MouseLeftButtonDown;
            frameworkElement.MouseLeftButtonUp += UIElement_MouseLeftButtonUp;
            frameworkElement.MouseLeave += UIElement_MouseLeave;
            frameworkElement.MouseEnter += FrameworkElement_MouseEnter;
        }

        //private Brush background = null;

        public void FrameworkElement_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        private bool isMouseDown = false;

        public void UIElement_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            isMouseDown = false;
        }

        public void UIElement_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (isMouseDown)
                Action?.Invoke(FrameworkElement);
            isMouseDown = false;
        }


        public void UIElement_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isMouseDown = true;
        }

        public Action<object> Action { get; set; }
        public FrameworkElement FrameworkElement { get; set; }
    }
}
