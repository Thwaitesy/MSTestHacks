using Microsoft.VisualStudio.TestTools.UITesting;
using MSTestHacks.UITesting.HtmlControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.TestTools.UITesting.HtmlControls
{
    public static class Extenstions
    {
        public static bool IsVisible(this UITestControl control)
        {
            var xCordinate = control.BoundingRectangle.X;
            var yCordinate = control.BoundingRectangle.Y;
            var height = control.BoundingRectangle.Height;
            var width = control.BoundingRectangle.Width;

            return (xCordinate > -1 && yCordinate > -1 && height > 0 && width > 0);
        }
        
        public static IEnumerable<T> FindMatchingControls<T>(this UITestControl uiTestControl) where T : UITestControl, IJqueryControl
        {
            var ctrls = new List<T>();
            var ids = uiTestControl.FindControlsBySelector();

            for (int i = 0; i < ids.Count(); i++)
            {
                var selector = uiTestControl.GetSelector() + ":eq(" + i + ")"; //So that we can locate it next time.
                var ctrl = (T)Activator.CreateInstance(typeof(T), uiTestControl.Container, selector); //Must have selector constructor or will break.
                ctrl.SearchProperties[HtmlControl.PropertyNames.Id] = ids.ElementAt(i);
                ctrls.Add(ctrl);
            }

            return ctrls.ToList();
        }
    }
}