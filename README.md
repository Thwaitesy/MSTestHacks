MSTestHacks
===========

A bunch of hacks to get around the deficiencies of MSTest.

How To
======

1. Install nuget package: `Install-Package MSTestHacks`

2. Add the following connection string:

   <connectionStrings>
      <add name="RuntimeDataSource" connectionString="Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\RuntimeDataSource\RuntimeDataSource.mdf;Integrated Security=True;Connect Timeout=30" providerName="System.Data.SqlClient" />
   </connectionStrings>

3. Implement your class like this:

    `[TestClass]
    [AttachRuntimeDataSources(typeof(UnitTest1))]
    public class UnitTest1 : TestBase
    {
        private IEnumerable<int> Stuff
        {
            get
            {
                return new List<int> { 1, 2, 3 };
            }
        }

        [TestMethod]
        [DataSource("Stuff")]
        public void TestMethod1()
        {
            var number = this.TestContext.GetRuntimeDataSourceObject<int>();

            Assert.IsNotNull(number);
        }
    }`
