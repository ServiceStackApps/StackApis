using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using NUnit.Framework;
using QAutoQuery.ServiceModel.Types;
using ServiceStack;
using ServiceStack.Testing;
using QAutoQuery.ServiceModel;
using QAutoQuery.ServiceInterface;
using ServiceStack.Web;

namespace QAutoQuery.Tests
{
    [TestFixture]
    public class UnitTests
    {
        private readonly ServiceStackHost appHost;

        public UnitTests()
        {
            appHost = new BasicAppHost(typeof(MyServices).Assembly)
            {
                ConfigureContainer = container =>
                {
                    //Add your IoC dependencies here
                    
                }
            }
            .Init();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            appHost.Dispose();
        }


        [Test]
        public void TestMethod1()
        {
            
        }
    }
}
