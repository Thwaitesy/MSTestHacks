MSTestHacks
===========

A bunch of hacks to get around the deficiencies of MSTest.

How To
======

1. Install nuget package 
 
`Install-Package MSTestHacks`

2. Add the following connection string
```xml
<connectionStrings>
  <add name="RuntimeDataSource" connectionString="Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\RuntimeDataSource\RuntimeDataSource.mdf;Integrated Security=True;Connect Timeout=30" providerName="System.Data.SqlClient" />
</connectionStrings>
```

3.1. Inherit from TestBase
```csharp
public class UnitTest1 : TestBase
```

3.2. Apply AttachRuntimeDatasources Attribute to class
```csharp
[AttachRuntimeDataSources(typeof(UnitTest1))]
public class UnitTest1 : TestBase
```

3.3. Create field, method, or property that implements IEnumerable<object>
```csharp
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
}
```

3.4. Add Datasource attribute to method, pointing back to the property name of 3.3
```csharp
[DataSource("Stuff")]
public void TestMethod1()
{
    var number = this.TestContext.GetRuntimeDataSourceObject<int>();

    Assert.IsNotNull(number);
}
```

Example
------------
```csharp
[TestClass]
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
}
```
