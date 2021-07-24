using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESDemo.ESHelper
{
    public interface IEsClientProvider
    {
        ElasticClient GetElasticClient();
    }
}
