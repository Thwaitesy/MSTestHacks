using Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;
using MSTestHacks.UITesting.HtmlControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTestHacks.Tests.CodedUI
{
    [CodedUITest]
    [DeploymentItem("CodedUI\\App", "CodedUI\\App")]
    public class Find : TestBase
    {
        BrowserWindow _browser;

        [TestInitialize]
        public void Initialize()
        {
            BrowserWindow.CurrentBrowser = "ie";
            this._browser = BrowserWindow.Launch(this.TestContext.DeploymentDirectory +  @"\CodedUI\App\TestPage.html");
        }

        [TestMethod]
        public void Microsoft_SimpleFind()
        {
            var control = new HtmlControl(this._browser);
            control.SearchProperties[HtmlControl.PropertyNames.Id] = "first-div";
            control.Find();

            Assert.IsTrue(control.Exists);
        }

        [TestMethod]
        public void MSTestHacks_SimpleFind()
        {
            var control = new JHtmlControl(this._browser);
            control.SearchProperties[JHtmlControl.PropertyNames.Selector] = "#first-div";
            control.Find();

            Assert.IsTrue(control.Exists);
        }
    }
}