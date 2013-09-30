@echo off

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" src\MSTestHacks.sln /t:rebuild

"%VS110COMNTOOLS%..\IDE\mstest.exe"  /testcontainer:src\MSTestHacks.Tests\bin\Debug\MSTestHacks.Tests.dll /resultsfile:TestResult.trx