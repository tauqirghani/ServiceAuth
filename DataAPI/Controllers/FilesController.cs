using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataAPI.Controllers
{
    [Route("data/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class FilesController : ControllerBase
    {
        private readonly FileContext _context;
        public FilesController(FileContext context)
        {
            _context = context;

        }
        private static readonly IReadOnlyCollection<string> ClaimsToSendBack = new HashSet<string>
        {
            "iat", "nbf", "exp", "appid", "appidacr", "e_exp",
            "http://schemas.microsoft.com/identity/claims/objectidentifier",
            "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
        };

        [HttpGet("{fileid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<File> GetByfileId(int fileid)
        {
            var file = _context.Files.Where(f => f.fileId == fileid).Single<File>();
            //return User.Claims.Where(c => ClaimsToSendBack.Contains(c.Type)).ToDictionary(c => c.Type, c => c.Value);
            return (file);
        }
    }
}