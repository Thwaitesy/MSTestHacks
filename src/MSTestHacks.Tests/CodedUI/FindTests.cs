using Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UITest.Extension;

namespace MSTestHacks.Tests.CodedUI
{
    [CodedUITest]
    [DeploymentItem("CodedUI\\App", "CodedUI\\App")]
    public class FindTests : TestBase
    {
        BrowserWindow _browser;

        [TestInitialize]
        public void Initialize()
        {
            BrowserWindow.CurrentBrowser = "ie";
            this._browser = BrowserWindow.Launch(this.TestContext.DeploymentDirectory +  @"\CodedUI\App\TestPage.html");
        }

        [TestMethod]
        public void Microsoft_SimpleFindId()
        {
            var control = new HtmlControl(this._browser);
            control.SearchProperties[HtmlControl.PropertyNames.Id] = "first-div-id";
            control.Find();

            Assert.IsTrue(control.Exists);
        }

        [TestMethod]
        public void MSTestHacks_SimpleFindId()
        {
            var control = new JHtmlControl(this._browser);
            control.SearchProperties[JHtmlControl.PropertyNames.Selector] = "#first-div-id";
            control.Find();

            Assert.IsTrue(control.Exists);
        }

        [TestMethod]
        public void Microsoft_SimpleFindClass()
        {
            var control = new HtmlControl(this._browser);
            control.SearchProperties[HtmlControl.PropertyNames.Class] = "first-div-class";
            control.Find();

            Assert.IsTrue(control.Exists);
        }

        [TestMethod]
        public void MSTestHacks_SimpleFindClass()
        {
            var control = new JHtmlControl(this._browser);
            control.SearchProperties[JHtmlControl.PropertyNames.Selector] = ".first-div-class";
            control.Find();

            Assert.IsTrue(control.Exists);
        }

        [TestMethod]
        [ExpectedException(typeof(UITestControlNotFoundException))]
        public void Microsoft_ControlNotFoundTimeout()
        {
            var control = new HtmlControl(this._browser);
            control.SearchProperties[HtmlControl.PropertyNames.Class] = "unknown";
            control.Find();
        }

        [TestMethod]
        [ExpectedException(typeof(UITestControlNotFoundException))]
        public void MSTestHacks_ControlNotFoundTimeout()
        {
            var control = new JHtmlControl(this._browser);
            control.SearchProperties[JHtmlControl.PropertyNames.Selector] = ".unknown";
            control.Find();
        }
    }
}