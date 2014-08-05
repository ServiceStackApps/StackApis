using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QAutoQuery.ServiceModel.Types;
using ServiceStack;
using QAutoQuery.ServiceModel;

namespace QAutoQuery.ServiceInterface
{
    public class MyServices : Service
    {
        public IAutoQuery AutoQuery { get; set; }

        public object Any(StackOverflowQuery request)
        {
            var q = AutoQuery.CreateQuery(request, Request.GetRequestParams());
            return AutoQuery.Execute(request, q);
        }
    }
}