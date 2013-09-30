Overview
==========================================================================
Just a bunch of hacks to get around the deficiencies of MSTest. 

Hopefully for those of us that have to work inside the constrainsts of MSTest, this library should ease our pain. (Just a little) 

Check out the tests project for samples

Features
==========================================================================
***RuntimeDataSource***

A runtime data driven test as opposed to compile time. Just point your datasource to a property, field or method name that returns an IEnumerable and at runtime it will loop through the collection just like normal. (Think NUnit's [TestCaseSource](http://nunit.org/index.php?p=testCaseSource&r=2.5))

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
**1)** Apply `AttachRuntimeDatasources` attribute to your test class, with the type of the class as its parameter. 
```csharp
[AttachRuntimeDataSources(typeof(UnitTest1))]
public class UnitTest1 : TestBase
```

**2)** Create a Property, Field or Method, that returns IEnumerable
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

**3)** Add the `DataSource` attribute to your test method, pointing back to the IEnumerable<T> name above. This needs to be fully qualified to create uniqueness.
```csharp
[DataSource("Namespace.UnitTest1.Stuff")]
public void TestMethod1()
{
    var number = this.TestContext.GetRuntimeDataSourceObject<int>();
    
    Assert.IsNotNull(number);
}
```

Roadmap
==========================================================================
* Better asserts for exceptions
* Injection of `AttachRuntimeDataSources` attribute at compile time using PostSharp
* Injection of `DataSource` attribute at compile time using PostSharp

Changelog
==========================================================================
*1.1.0 - (BREAKING CHANGE)*
- Creating a datasource file per datasource, simplifies life
- All datasources now have to be fully qualified, pointing to the IEnumerable<T>. This creates a unique datasource that was required in some instances. 

*1.0.2*
- Inject the ConnectionString automatically
- Add a couple more tests

*1.0.1*
- Fixes the issue with stale iterations. Each run was not getting deleted from the xml file and was building up. 

*1.0.0*
- Replaced the database backend with an xml one
- Production Ready

*0.0.2*
- Simplified NuGet and added project links etc

*0.0.1*
- Initial release

Contributors
==========================================================================
Sam Thwaites    
Corey Warner

Licence
==========================================================================
See [LICENCE](https://github.com/Thwaitesy/MSTestHacks/blob/master/LICENCE)
