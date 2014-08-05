using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QAutoQuery.ServiceModel.Types;
using ServiceStack;

namespace QAutoQuery.ServiceModel
{
    [Route("/questions")]
    public class StackOverflowQuery : QueryBase<QuestionItem>
    {

    }
}
