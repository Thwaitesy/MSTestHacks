using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTestHacks.Tests.CodedUI
{
    [CodedUITest]
    [DeploymentItem("CodedUI\\App", "CodedUI\\App")]
    public class WaitTests : TestBase
    {
        BrowserWindow _browser;

        [TestInitialize]
        public void Initialize()
        {
            BrowserWindow.CurrentBrowser = "ie";
            this._browser = BrowserWindow.Launch(this.TestContext.DeploymentDirectory + @"\CodedUI\App\TestPage.html");
        }

        [TestMethod]
        public void Microsoft_WaitForControlToExist()
        {
            var control = new HtmlControl(this._browser);
            control.SearchProperties[HtmlControl.PropertyNames.Class] = "add-me";
            Assert.IsTrue(control.WaitForControlExist());
        }

        [TestMethod]
        public void MSTestHacks_WaitForControlToExist()
        {
            var control = new JHtmlControl(this._browser);
            control.SearchProperties[JHtmlControl.PropertyNames.Selector] = ".add-me";
            Assert.IsTrue(control.WaitForControlExist());
        }

        [TestMethod]
        public void Microsoft_WaitForControlToNotExist()
        {
            var control = new HtmlControl(this._browser);
            control.SearchProperties[HtmlControl.PropertyNames.Class] = "removed-me";
            Assert.IsTrue(control.WaitForControlNotExist());
        }

        [TestMethod]
        public void MSTestHacks_WaitForControlToNotExist()
        {
            var control = new JHtmlControl(this._browser);
            control.SearchProperties[JHtmlControl.PropertyNames.Selector] = ".removed-me";
            Assert.IsTrue(control.WaitForControlNotExist());
        }
    }
}