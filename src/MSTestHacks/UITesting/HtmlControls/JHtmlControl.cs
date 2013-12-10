using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTestHacks.UITesting.HtmlControls
{
    public class JHtmlControl : HtmlControl
    {
        public JHtmlControl(UITestControl parent)
            : base(parent)
        { }

        public JHtmlControl(UITestControl parent, string selector)
            : base(parent)
        {
            this.SearchProperties[PropertyNames.Selector] = selector;
        }

        public partial class PropertyNames : HtmlControl.PropertyNames
        {
            public static readonly string Selector = "Selector";
        }
    }
}
