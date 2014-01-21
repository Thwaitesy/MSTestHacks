using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTestHacks.UITesting.HtmlControls
{
    internal static class Helpers
    {
        internal static string GenerateAbsoluteSelector(this UITestControl control)
        {
            var absoluteSelector = "";
            while (control != null)
            {
                if (control.SearchProperties.Contains(JHtmlControl.PropertyNames.Selector) &&
                   !string.IsNullOrWhiteSpace(control.SearchProperties[JHtmlControl.PropertyNames.Selector]))
                {
                    var selector = control.SearchProperties[JHtmlControl.PropertyNames.Selector];
                    if (string.IsNullOrEmpty(absoluteSelector))
                    {
                        absoluteSelector = selector;
                    }
                    else
                    {
                        absoluteSelector = selector + " " + absoluteSelector;
                    }

                    //A selector control has a modified parent, so get its original one to build up the absolute selector.
                    control = control.Container;
                }
                else
                {
                    //Not a jquery control so the container tree is still in tact.
                    control = control.Container;
                }
            }

            if (string.IsNullOrEmpty(absoluteSelector))
                return "body";

            return absoluteSelector;
        }

        internal static BrowserWindow FindBrowserWindow(this UITestControl control)
        {
            while (!(control is BrowserWindow))
            {
                control = control.Container;

                if (control == null)
                    throw new Exception("Traversed all parents and no BrowserWindow found. Make sure the browser window is at the top level.");
            }

            return (BrowserWindow)control;
        }

        internal static IEnumerable<string> FindControlsBySelector(this UITestControl uiTestControl)
        {
            var absoluteSelector = uiTestControl.GenerateAbsoluteSelector();
            var browserWindow = uiTestControl.FindBrowserWindow();

            //Add jquery if it doesn't exist
            var isJqueryUndefined = new Func<bool>(() => (bool)browserWindow.ExecuteScript("return (typeof $ === 'undefined')"));
            if (isJqueryUndefined())
            {
                browserWindow.ExecuteScript(@"
                    var scheme =  window.location.protocol;
                    if(scheme != 'https:')
                        scheme = 'http:';

                    var script = document.createElement('script');
                    script.type = 'text/javascript';
                    script.src = scheme + '//code.jquery.com/jquery-1.10.1.min.js'; 
                    document.getElementsByTagName('head')[0].appendChild(script);
                ");

                //Loop until jquery is inserted and takes effect
                while (isJqueryUndefined())
                {
                    System.Threading.Thread.Sleep(100);
                }
            }

            // make a script from the selector... special handling for / which we use to divide iframes
            var selectorParts = absoluteSelector.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            //var selectorScript = "$(\"" + selectorParts[0] + ":visible\")";
            var selectorScript = "$(\"" + selectorParts[0] + "\")";
            for (var i = 1; i < selectorParts.Count(); i++)
            {
                //selectorScript += ".contents().find(\"" + selectorParts[i] + ":visible\")";
                selectorScript += ".contents().find(\"" + selectorParts[i] + "\")";
            }

            //Assign and return ids of elements that match the jquery selector
            var script = @"  

                    if(typeof window.automationUniqueId === 'undefined') {
                        window.automationUniqueId = '0'; 
                    }
                    
                    var automationIds = [];                    
                    " + selectorScript + @".map(function() { 
                        var obj = $(this);
                       
                        obj.stop().css('outline', '100px solid rgba(255, 0, 0, .7)').animate({ 'outline-width': '1px' }, 50);

                        if(!obj.attr('id') || obj.attr('id') == '') 
                        {
                            obj.attr('id', 'automationSelectorId-' + ++window.automationUniqueId);  
                        }
                        automationIds.push(obj.attr('id'));
                    });
                    
                    return automationIds;
                    ";

            var list = ((List<object>)browserWindow.ExecuteScript(script)).Cast<string>().ToList();
            Console.WriteLine(absoluteSelector + " = " + selectorScript + " = " + list.FirstOrDefault() ?? "");
            return list;
        }

        internal static string FindControlBySelector(this UITestControl uiTestControl)
        {
            return FindControlsBySelector(uiTestControl).FirstOrDefault();
        }

        internal static void JqueryFind(this UITestControl control)
        {
            var id = control.FindControlBySelector();

            //Only update the control if the id is found.
            if (!string.IsNullOrEmpty(id))
            {
                var htmlControl = new HtmlControl(control.Container);
                htmlControl.SearchProperties["id"] = id;
                control.CopyFrom(htmlControl);
            }
        }

        internal static string GetSelector(this UITestControl uiTestControl)
        {
            if (uiTestControl.SearchProperties.Contains(JHtmlControl.PropertyNames.Selector) &&
                !string.IsNullOrWhiteSpace(uiTestControl.SearchProperties[JHtmlControl.PropertyNames.Selector]))
            {
                return uiTestControl.SearchProperties[JHtmlControl.PropertyNames.Selector];
            }
            else
            {
                return string.Empty;
            }
        }

        internal static UITestControlCollection ToCollection<T>(this IEnumerable<T> controls) where T : UITestControl, IJqueryControl
        {
            var collection = new UITestControlCollection();
            controls.ToList().ForEach(x => collection.Add(x));
            return collection;
        }
    }
}
