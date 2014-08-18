StackApis
=========

[![StackApis Home](https://raw.githubusercontent.com/ServiceStack/Assets/master/img/apps/StackApis/stackapis-home.png)](http://stackapis.servicestack.net/)

[StackApis](http://stackapis.servicestack.net/) is a simple new ServiceStack + AngularJS example project created with [ServiceStackVS AngularJS Template](https://github.com/ServiceStack/ServiceStackVS#servicestackvs) showcasing how quick and easy it is to create responsive feature-rich Single Page Apps with AngularJS and [AutoQuery](https://github.com/ServiceStack/ServiceStack/wiki/Auto-Query). StackApis is powered by a Sqlite database containing [snapshot of ServiceStack questions from StackOverflow APIs](https://github.com/ServiceStackApps/StackApis/blob/master/src/StackApis.Tests/UnitTests.cs#L67) that's [persisted in an sqlite database using OrmLite](https://github.com/ServiceStackApps/StackApis/blob/master/src/StackApis.Tests/UnitTests.cs#L119-L124).

### StackApis AutoQuery Service

The [Home Page](https://github.com/ServiceStackApps/StackApis/blob/master/src/StackApis/default.cshtml) is built with less than **<50 Lines** of JavaScript which thanks to [AutoQuery](https://github.com/ServiceStack/ServiceStack/wiki/Auto-Query) routes all requests to the single AutoQuery Service below:

```csharp
[Route("/questions")]
public class StackOverflowQuery : QueryBase<Question>
{
    public int? ScoreGreaterThan { get; set; }
}
```

> Not even `ScoreGreaterThan` is a required property, it's just an example of a [formalized convention](https://github.com/ServiceStack/ServiceStack/wiki/Auto-Query#advantages-of-well-defined-service-contracts) enabling queries from Typed Service Clients.

Feel free to play around with a deployed version of StackApis at [stackapis.servicestack.net](http://stackapis.servicestack.net/).

You can also use the public `http://stackapis.servicestack.net/` url to test out ServiceStack's new **Add ServiceStack Reference** feature :)
