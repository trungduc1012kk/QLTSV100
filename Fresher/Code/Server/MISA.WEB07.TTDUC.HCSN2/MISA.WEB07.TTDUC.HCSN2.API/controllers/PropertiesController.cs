using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;

namespace MISA.WEB07.TTDUC.HCSN2.API.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        [HttpGet]
        public String getName()
        {
            return "TTDuc";
        }
    }
}
