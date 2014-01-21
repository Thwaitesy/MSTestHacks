using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTestHacks.Tests.CodedUI.App
{
    public class RepeatingItem : JHtmlControl
    {
        private HtmlControl _partA;
        private HtmlControl _partB;

        public string PartAText { get { return this._partA.InnerText; } }
        public string PartBText { get { return this._partB.InnerText; } }

        public RepeatingItem(UITestControl control, string selector)
            : base(control, selector)
        {
            this._partA = new JHtmlControl(this, ".partA");
            this._partB = new JHtmlControl(this, ".partB");
        }
    }
}