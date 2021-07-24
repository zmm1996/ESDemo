using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESDemo.ESHelper
{
    public class EsClientProvider : IEsClientProvider
    {
        private readonly IConfiguration _configuration;
        private ElasticClient _elasticClient;
        private static readonly object lockHelper = new object();
        public EsClientProvider(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public ElasticClient GetElasticClient()
        {
            //if (_elasticClient != null)
            //    return _elasticClient;
            //InitElasticClient();
            //return _elasticClient;

            //双重检查锁定单例
            if(_elasticClient==null)
            {
                lock(lockHelper)
                {
                    if (_elasticClient == null)
                        InitElasticClient();
                }
            }
            return _elasticClient;

            
        }

        private void InitElasticClient()
        {
            var node =new Uri(_configuration["ESUrl"]);
            _elasticClient = new ElasticClient(new ConnectionSettings(node).DefaultIndex("es-client-netdemo"));
        }
    }
}
