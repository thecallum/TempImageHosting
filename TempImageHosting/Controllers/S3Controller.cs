using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TempImageHosting.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class S3Controller : ControllerBase
    {
        private readonly S3 _s3;

        public S3Controller()
        {
            _s3 = new S3();
        }

        public ActionResult<string> Get(string fileName)
        {
            string uploadUrl = _s3.GenerateUploadUrl(fileName);

            return uploadUrl;
        }
    }
}
