using EMS.DAL.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using EMS.DTO;
using Microsoft.AspNetCore.Authorization;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisplayController : ControllerBase
    {
        private readonly IDisplayMgr _displayMgr;
        private readonly ILogger<DisplayController> _logger;

        public DisplayController(IDisplayMgr displayMgr, ILogger<DisplayController> logger)
        {
            _displayMgr = displayMgr;
            _logger = logger;
        }


        [HttpGet("GetDisplayList")]
        public ActionResult<List<DisplayDTO>> GetDisplayList()
        {
            try
            {
                var displayList = _displayMgr.GetDisplayList();
                return Ok(displayList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving display list");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
