using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESDemo.ESHelper;
using ESDemo.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace ESDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
 {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly ElasticClient _elasticClient;
        public ValuesController(IEsClientProvider esClientProvider)
        {
            _elasticClient = esClientProvider.GetElasticClient();
        }

        [HttpGet("Index/{id}")]
        public string Get(int id)
        {
            var ran = new Random();
            var post = new Post
            {
                Id = id,
                Index=Guid.NewGuid().ToString(),
                Type = Summaries[ran.Next(Summaries.Length)]
            };
           var s= _elasticClient.IndexDocument(post); //默认索引添加数据  index  =EsClientNetDemo

            var re= _elasticClient.Index(post, idx => idx.Index("test_index"));  //指定索引
       
            return re.ApiCall.DebugInformation;
        }


        [HttpGet("Search")]
        public IReadOnlyCollection<Post> Search(string type)
        {
           var defaultIndex=  _elasticClient.Search<Post>(s => s
          // .Index("test_index")  //不指定查询默认索引
           .MatchAll()  ).Documents;

            var re= _elasticClient.Search<Post>(s => s
            .Index("test_index")
                .From(0)
                .Size(10)
                .Query(q => q.Match(m => m.Field(f => f.Type).Query(type)))).Documents;

            return re;


        }

    }
}
