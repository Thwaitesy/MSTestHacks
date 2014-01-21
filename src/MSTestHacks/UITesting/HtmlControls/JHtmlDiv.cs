using Microsoft.VisualStudio.TestTools.UITesting;
using MSTestHacks.UITesting.HtmlControls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.VisualStudio.TestTools.UITesting.HtmlControls
{
    public class JHtmlDiv : HtmlDiv, IJqueryControl
    {
        public JHtmlDiv(UITestControl parent)
            : base(parent)
        { }

        public JHtmlDiv(UITestControl parent, string selector)
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
            return this.FindMatchingControls<JHtmlDiv>().ToCollection();
        }

        public new partial class PropertyNames : HtmlControl.PropertyNames
        {
            public static readonly string Selector = "Selector";
        }
    }
}