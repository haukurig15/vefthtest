using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace gittest.Controllers
{
    [Route("api/[controller]")]
    public class CoursesController : Controller
    {
        // GET api/courses
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/courses/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/courses
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/courses/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/courses/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
