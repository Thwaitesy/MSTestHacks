using Microsoft.VisualStudio.TestTools.UITesting;
using MSTestHacks.UITesting.HtmlControls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.VisualStudio.TestTools.UITesting.HtmlControls
{
    public class JHtmlList : HtmlList, IJqueryControl
    {
        public JHtmlList(UITestControl parent)
            : base(parent)
        { }

        public JHtmlList(UITestControl parent, string selector)
            : this(parent)
        {
            this.SearchProperties[PropertyNames.Selector] = selector;
        }

        public override void Find()
        {
            this.JqueryFind();
            base.Find();
        }

        public new UITestControlCollection FindMatchingControls()
        {
            return this.FindMatchingControls<JHtmlList>().ToCollection();
        }

        public new partial class PropertyNames : HtmlControl.PropertyNames
        {
            public static readonly string Selector = "Selector";
        }
    }
}