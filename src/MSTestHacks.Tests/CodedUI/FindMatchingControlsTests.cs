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
    public class FindMatchingControlsTests : TestBase
    {
        BrowserWindow _browser;

        [TestInitialize]
        public void Initialize()
        {
            BrowserWindow.CurrentBrowser = "ie";
            this._browser = BrowserWindow.Launch(this.TestContext.DeploymentDirectory + @"\CodedUI\App\TestPage.html");
        }

        [TestMethod]
        public void Microsoft_FindsAllRepeatingItems()
        {
            var control = new HtmlControl(this._browser);
            control.SearchProperties[HtmlControl.PropertyNames.Class] = "repeatingItem";
            var allControls = control.FindMatchingControls();

            Assert.AreEqual(3, allControls.Count);
        }

        [TestMethod]
        public void MSTestHacks_FindsAllRepeatingItems()
        {
            var control = new JHtmlControl(this._browser);
            control.SearchProperties[JHtmlControl.PropertyNames.Selector] = ".repeatingItem";
            var allControls = control.FindMatchingControls();

            Assert.AreEqual(3, allControls.Count);
        }

        [TestMethod]
        public void Microsoft_FindsAllRepeatingItemsAfter1IsRemoved()
        {
            var control = new HtmlControl(this._browser);
            control.SearchProperties[HtmlControl.PropertyNames.Class] = "repeatingItem";
            var allControls = control.FindMatchingControls();
            Assert.AreEqual(3, allControls.Count);
            
            Thread.Sleep(11000);
            allControls = control.FindMatchingControls();
            Assert.AreEqual(2, allControls.Count);
        }

        [TestMethod]
        public void MSTestHacks_FindsAllRepeatingItemsAfter1IsRemoved()
        {
            var control = new JHtmlControl(this._browser);
            control.SearchProperties[JHtmlControl.PropertyNames.Selector] = ".repeatingItem";
            var allControls = control.FindMatchingControls();
            Assert.AreEqual(3, allControls.Count);

            Thread.Sleep(11000);
            allControls = control.FindMatchingControls();
            Assert.AreEqual(2, allControls.Count);
        }
    }
}