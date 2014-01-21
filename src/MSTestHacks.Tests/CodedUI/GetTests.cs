using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSTestHacks.Tests.CodedUI
{
    [CodedUITest]
    [DeploymentItem("CodedUI\\App", "CodedUI\\App")]
    public class GetTests : TestBase
    {
        BrowserWindow _browser;

        [TestInitialize]
        public void Initialize()
        {
            BrowserWindow.CurrentBrowser = "ie";
            this._browser = BrowserWindow.Launch(this.TestContext.DeploymentDirectory + @"\CodedUI\App\TestPage.html");
        }

        [TestMethod]
        public void Microsoft_CanGetText()
        {
            var control = new HtmlControl(this._browser);
            control.SearchProperties[HtmlControl.PropertyNames.Class] = "contentx";
            Assert.AreEqual("Standard text Content", control.InnerText);
        }

        [TestMethod]
        public void MSTestHacks_CanGetText()
        {
            var control = new JHtmlControl(this._browser);
            control.SearchProperties[JHtmlControl.PropertyNames.Selector] = ".contentx";
            Assert.AreEqual("Standard text Content", control.InnerText);
        }

        [TestMethod]
        public void Microsoft_CanGetTextAfterChange()
        {
            var control = new HtmlControl(this._browser);
            control.SearchProperties[HtmlControl.PropertyNames.Class] = "contentx";
            Assert.AreEqual("Standard text Content", control.InnerText);
            Thread.Sleep(11000);
            Assert.AreEqual("Standard changed Content", control.InnerText);
        }

        [TestMethod]
        public void MSTestHacks_CanGetTextAfterChange()
        {
            var control = new JHtmlControl(this._browser);
            control.SearchProperties[JHtmlControl.PropertyNames.Selector] = ".contentx";
            Assert.AreEqual("Standard text Content", control.InnerText);
            Thread.Sleep(11000);
            Assert.AreEqual("Standard changed Content", control.InnerText);
        }
    }
}