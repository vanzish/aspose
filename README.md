# aspose test assignment

Solution is presented as class libraries and a unit tests class library.

To obtain more flexibility and reusage of code in different parts of the solution, code was separated into multiple projects. Each project contains specific part of solution.

The Architecture of the solution is a prototype of N-tier architecture with an implementation of business logic level with technical services and application level with general entities.
Implemented dependency injection using Autofac to make layers less rigidly connected to each other.
In addition, NUnit framework was used for unit tests.

Logging and caching should be implemented in the future according to data layer implementation.

The solution was prepared using Visual Studio 2017 and .Net Framework 4.7. 
To run Tests open the solution in Visual Studio and build it. NuGet packages should be downloaded automatically.
Open SalaryTests.cs file and Run all tests.
