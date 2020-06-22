using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;

namespace Labor.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        /// <summary>
        /// 默认的get
        /// </summary>
        /// <returns></returns>
        // GET: api/Default
        [Authorize]
        [HttpGet]
        public IActionResult Get(string callback)
        {
            IPHostEntry myHost = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            string domain = myHost.HostName;
            string domainAccount =JsonSerializer.Serialize(HttpContext.User.Identity.Name);
            return Ok($"{callback}({domainAccount})");
        }

        // GET: api/Default/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        [Authorize]
        // POST: api/Default
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Default/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }



    }
}
