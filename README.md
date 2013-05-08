[Overview](https://github.com/Thwaitesy/MSTestHacks#overview)   
[Features](https://github.com/Thwaitesy/MSTestHacks#features)   
[Roadmap](https://github.com/Thwaitesy/MSTestHacks#roadmap)      
[Changelog](https://github.com/Thwaitesy/MSTestHacks#changelog)    
[Licence](https://github.com/Thwaitesy/MSTestHacks#licence)

Overview
==========================================================================
Just a bunch of hacks to get around the deficiencies of MSTest. 

Hopefully for those of us that have to work inside the constrainsts of MSTest, this library should ease our pain. (Just a little) 
Features
==========================================================================
***RuntimeDataSource***

A runtime data driven test as opposed to compile time. Just point your datasource to a property, field or method name that returns IEnumerable<T> and at runtime it will loop through the collection just like normal. (Think NUnit's [TestCaseSource](http://nunit.org/index.php?p=testCaseSource&r=2.5))

Getting Started
==========================================================================
**1)** [Install NuGet](http://docs.nuget.org/docs/start-here/installing-nuget). Then, install MSTestHacks from the package manager console:
```csharp
PM> Install-Package MSTestHacks
``` 

**2)** Inherit your test class from `TestBase`
```csharp
public class UnitTest1 : TestBase
{ }
```

###RuntimeDataSource
**1)** Add the following connection string to your app.config file
```xml
<connectionStrings>
  <add name="RuntimeDataSource" connectionString="Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\RuntimeDataSource\RuntimeDataSource.mdf;Integrated Security=True;Connect Timeout=30" providerName="System.Data.SqlClient" />
</connectionStrings>
```

**2)** Apply `AttachRuntimeDatasources` attribute to your test class, with the type of the class as its parameter. 
```csharp
[AttachRuntimeDataSources(typeof(UnitTest1))]
public class UnitTest1 : TestBase
```

**3)** Create a Property, Field or Method, that returns IEnumerable<T>
```csharp
[AttachRuntimeDataSources(typeof(UnitTest1))]
public class UnitTest1 : TestBase
{
    private IEnumerable<int> Stuff
    {
        get
        {
            //This could do anything, get a dynamic list from anywhere....
            return new List<int> { 1, 2, 3 };
        }
    }
}
```

**4)** Add the `DataSource` attribute to your test method, pointing back to the IEnumerable<T> name above.
```csharp
[DataSource("Stuff")]
public void TestMethod1()
{
    var number = this.TestContext.GetRuntimeDataSourceObject<int>();
    
    Assert.IsNotNull(number);
}
```

Roadmap
==========================================================================
* Use CSV's instead of local db
* Better asserts for exceptions
* Injection of `AttachRuntimeDataSources` attribute at compile time using PostSharp
* Injection of `DataSource` attribute at compile time using PostSharp

Changelog
==========================================================================
*0.0.2*
- Initial release

Licence
==========================================================================
See [LICENCE](https://github.com/Thwaitesy/MSTestHacks/blob/master/LICENCE)
