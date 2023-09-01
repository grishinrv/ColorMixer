using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ColorMixer.Application.Presentation
{
    /// <summary>
    /// Extension method for working with Visual Tree.
    /// </summary>
    public static class VisualTreeExtensions
    {
        /// <summary>
        /// Get the closest Ancestor of specified type T of the given element.
        /// </summary>
        /// <typeparam name="T">Type of Ancestor to look for.</typeparam>
        /// <param name="element">position in tree to start search.</param>
        /// <returns></returns>
        public static T? GetVisualParent<T>(this DependencyObject element) 
            where T : DependencyObject
        {
            while (element != null && !(element is T))
                element = VisualTreeHelper.GetParent(element);

            return element as T;
        }

        /// <summary>
        /// Get the closest Child of specified type T of the given parent.
        /// </summary>
        /// <typeparam name="T">Type of Child to look for.</typeparam>
        /// <param name="parent">Position in tree to start search.</param>
        /// <param name="point">Where to check.</param>
        /// <returns></returns>
        public static T? FindChildByTypeAndPoint<T>(this DependencyObject parent, Point point)
            where T : DependencyObject
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child is T typedChild && IsPointInsideElement(typedChild, point))
                {
                    return typedChild;
                }

                DependencyObject? foundChild = FindChildByTypeAndPoint<T>(child, point);
                if (foundChild is T result)
                    return result;
            }

            return null;
        }

        private static bool IsPointInsideElement(DependencyObject element, Point point)
        {
            if (element is UIElement uiElement)
            {
                Point elementPoint = uiElement.TranslatePoint(new Point(0, 0), App.Current.MainWindow);
                Rect elementBounds = new Rect(elementPoint, new Size(uiElement.RenderSize.Width, uiElement.RenderSize.Height));
                return elementBounds.Contains(point);
            }

            return false;
        }

        public static T? GetItemsPanel<T>(this DependencyObject itemsControl)
           where T : DependencyObject
        {
            ItemsPresenter itemsPresenter = GetVisualChild<ItemsPresenter>(itemsControl)!;
            T itemsPanel = VisualTreeHelper.GetChild(itemsPresenter, 0) as T;
            return itemsPanel;
        }

        private static T? GetVisualChild<T>(DependencyObject parent) 
            where T : Visual
        {
            T? child = default(T);

            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
    }
}
