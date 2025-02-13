using EMS.DAL.Implementation;
using EMS.DAL.Interface;
using EMS.DTO;
using EMS.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentsController : ControllerBase
    {
        private readonly IIncidentMgr _incidentMgr;
        private readonly ILogger<IncidentsController> _logger;
        private readonly IModuleMgr _moduleMgr;

        public IncidentsController(IIncidentMgr incidentMgr, IModuleMgr moduleMgr, ILogger<IncidentsController> logger)
        {
            _incidentMgr = incidentMgr;
            _moduleMgr = moduleMgr;
            _logger = logger;
        }


        [HttpGet("GetRequestType")]
        public ActionResult<List<RequestTypeDTO>> GetRequestType()
        {
            try
            {
                var requestTypes = _incidentMgr.GetRequestType();
                if (requestTypes == null || !requestTypes.Any())
                {
                    return NotFound("No incident type found.");
                }
                return Ok(requestTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetRequestType: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving request type.");
            }
        }


        [HttpGet("GetCompanyType")]
        public ActionResult<List<CompanyTypeDTO>> GetCompanyType()
        {
            try
            {
                var companyTypes = _incidentMgr.GetCompanyType();
                if (companyTypes == null || !companyTypes.Any())
                {
                    return NotFound("No company type found.");
                }
                return Ok(companyTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetCompanyType: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving company type.");
            }
        }


        [HttpGet("GetSubjectList")]
        public ActionResult<List<SubjectDTO>> GetSubjectList()
        {
            try
            {
                var subject = _incidentMgr.GetSubjectList();
                if (subject == null || !subject.Any())
                {
                    return NotFound("No subjects found.");
                }
                return Ok(subject);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetSubjectList: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving subjects.");
            }
        }

        [HttpGet("GetIncidentCategory")]
        public ActionResult<List<IncidentCategoryDTO>> GetIncidentCategory()
        {
            try
            {
                var incidentCategory = _incidentMgr.GetIncidentCategory();
                if (incidentCategory == null || !incidentCategory.Any())
                {
                    return NotFound("No incident category found.");
                }
                return Ok(incidentCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetIncidentCategoryList: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving incident category.");
            }
        }

        [HttpGet("GetEngineerList")]
        public ActionResult<List<EngineerDTO>> GetEngineerList()
        {
            try
            {
                var engineers = _incidentMgr.GetEngineerList();
                if (engineers == null || !engineers.Any())
                {
                    return NotFound("No engineers found.");
                }
                return Ok(engineers);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetEngineerList: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving engineers.");
            }
        }

        [HttpGet("GeAdminList")]
        public ActionResult<List<AdminDTO>> GetAdminList()
        {
            try
            {
                var admin = _incidentMgr.GetAdminList();
                if (admin == null || !admin.Any())
                {
                    return NotFound("No admin found.");
                }
                return Ok(admin);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAdminList: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving admin.");
            }
        }


        [HttpPost]
        public async Task<ActionResult<List<IncidentDTO>>> AddIncident(AddIncidentDTO incidentDetails)
        {
            try
            {
                var incidentList = await _incidentMgr.AddIncidentAsync(incidentDetails);

                return Ok(incidentList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogInformation(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("open-incidents")]
        public ActionResult<List<AllIncidentDTO>> GetOpenIncidents()
        {
            try
            {
                var openIncidents = _incidentMgr.GetOpenIncidentList();
                if (openIncidents == null || !openIncidents.Any())
                {
                    return NotFound("No open incidents found.");
                }
                return Ok(openIncidents);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("close-incidents")]
        public ActionResult<List<AllIncidentDTO>> GetCloseIncidents()
        {
            try
            {
                var closeIncidents = _incidentMgr.GetCloseIncidentList();
                if (closeIncidents == null || !closeIncidents.Any())
                {
                    return NotFound("No close incidents found.");
                }
                return Ok(closeIncidents);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetIncidentStatus")]
        public ActionResult<List<IncidentStatusDTO>> GetIncidentStatus()
        {
            try
            {
                var incidentStatus = _incidentMgr.GetIncidentStatuses();
                if (incidentStatus == null || !incidentStatus.Any())
                {
                    return NotFound("No incident status found.");
                }
                return Ok(incidentStatus);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetIncidentStatus: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving incident category.");
            }
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteIncident(int Id)
        {
            try
            {
                var result = await _incidentMgr.DeleteIncidentAsync(Id);
                if (result.ResultCode == 1)
                {
                    return NotFound(result.ResultDescription);
                }

                return Ok(result.ResultDescription);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteIncident: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the incident.");
            }
        }



        [HttpGet("{Id}")]
        public async Task<IActionResult> GetIncidentDetails(int Id)
        {
            try
            {
                var incidentDetails = await _incidentMgr.GetIncidentDetailsByIdAsync(Id);
                if (incidentDetails == null)
                {
                    return NotFound($"Incident with ID {Id} not found.");
                }
                return Ok(incidentDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error. " + ex.Message);
            }
        }



        [HttpPut("edit")]
        public async Task<ActionResult<IncidentDTOListResult>> EditIncident([FromBody] EditIncidentDTO incidentDetails)
        {
            if (incidentDetails == null || incidentDetails.Id <= 0)
            {
                return BadRequest(new { message = "Invalid incident details provided." });
            }

            try
            {
                var result = await _incidentMgr.EditIncidentAsync(incidentDetails);
                if (result.ResultCode == 0)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error editing incident with ID {incidentDetails.Id}: {ex.Message}");
                return StatusCode(500, "An error occurred while editing the incident.");
            }
        }


        [HttpPut("engineeredit")]
        public async Task<ActionResult<IncidentDTOListResult>> EngineerEditIncident([FromBody] EngineerEditIncidentDTO incidentDetails)
        {
            if (incidentDetails == null || incidentDetails.Id <= 0)
            {
                return BadRequest(new { message = "Invalid incident details provided." });
            }

            try
            {
                var result = await _incidentMgr.EngineerEditIncident(incidentDetails);
                if (result.ResultCode == 0)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error editing incident with ID {incidentDetails.Id}: {ex.Message}");
                return StatusCode(500, "An error occurred while editing the incident.");
            }
        }



        [HttpGet("GetOpenIncidentsByEngineer")]
        public async Task<ActionResult<List<EngineerIncidentDTO>>> GetOpenIncidentsByEngineer()
        {
            try
            {

                var userIdClaim = HttpContext.User?.FindFirst(ClaimTypes.PrimarySid);

                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var engineerId) || engineerId <= 0)
                {
                    return Unauthorized("Invalid engineer id.");
                }

                var incidents = await _incidentMgr.GetOpenIncidentsByEngineerId(engineerId);

                if (incidents == null || !incidents.Any())
                {
                    return Ok("The engineer had successfully closed all incident.");
                }

                return Ok(incidents);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetCloseIncidentsByEngineer")]
        public async Task<ActionResult<List<EngineerIncidentDTO>>> GetCloseIncidentsByEngineer()
        {
            try
            {

                var userIdClaim = HttpContext.User?.FindFirst(ClaimTypes.PrimarySid);

                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var engineerId) || engineerId <= 0)
                {
                    return Unauthorized("Invalid engineer id.");
                }

                var incidents = await _incidentMgr.GetCloseIncidentsByEngineerId(engineerId);

                if (incidents == null || !incidents.Any())
                {
                    return Ok("The engineer had successfully closed all incident.");
                }

                return Ok(incidents);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("EngineerGetIncidentDetails/{Id}")]
        public async Task<IActionResult> EngineerGetIncidentDetails(int Id)
        {
            try
            {
                var incidentDetails = await _incidentMgr.EngineerGetIncidentDetails(Id);
                if (incidentDetails == null)
                {
                    return NotFound($"Incident with ID {Id} not found.");
                }
                return Ok(incidentDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error. " + ex.Message);
            }
        }

        [HttpPost("SaveSignature")]
        public async Task<IActionResult> SaveSignature([FromBody] SaveSignatureRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Signature))
            {
                return BadRequest("Invalid request data. Signature cannot be empty.");
            }

            var result = await _incidentMgr.SaveSignatureAsync(request.Id, request.Signature);

            if (result)
            {
                return Ok(new { Message = "Signature saved successfully." });
            }
            else
            {
                return NotFound(new { Message = "Incident not found or unable to save signature." });
            }
        }


        [HttpPost("GetFilteredIncidents")]
        public async Task<IActionResult> GetFilteredIncidents([FromBody] FilterIncidentsRequestDTO request)
        {
            try
            {
                var incidents = await _incidentMgr.GetFilteredIncidentsAsync(
                    request.FromDate,
                    request.ToDate,
                    request.StatusId,
                    request.EngineerIds);
                return Ok(incidents);
            }
            catch (Exception ex)
            {
                // Log the exception (using ILogger)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        /*
        [HttpPost("GetFilteredIncidents")]
        public async Task<IActionResult> GetFilteredIncidents([FromBody] FilterIncidentsRequestDTO request)
        {
            try
            {
                var incidents = await _incidentMgr.GetFilteredIncidentsAsync(
                    request.FromDate,
                    request.ToDate,
                    request.StatusId,
                    request.EngineerIds);
                return Ok(incidents);
            }
            catch (Exception ex)
            {
                // Log the exception (using ILogger)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        */


        
        [HttpPost("GetIncReportExcel")]
        public async Task<FileResult> GetIncidentReportExcel(List<FilterIncidentDTO> data)
        {

            var stream = await _incidentMgr.GetIncidentReportExcelAsync(data);

            const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            string fileName = $"IncidentReport.xlsx";

            HttpContext.Response.ContentType = contentType;

            HttpContext.Response.Headers.Append("Access-Control-Expose-Headers", "Content-Disposition");

            var fileContentResult = new FileContentResult(stream.ToArray(), contentType)
            {
                FileDownloadName = fileName
            };

            return fileContentResult;
        }
        



        /*
       
   
        [Authorize]
        [HttpGet("GetIncidentList")]
        public async Task<ActionResult<List<AllIncidentDTO>>> GetIncidentList()
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    var userIdClaim = identity.FindFirst(ClaimTypes.PrimarySid)?.Value;
                    if (int.TryParse(userIdClaim, out int userId))
                    {
                        // Check if the current user is an admin
                        var isAdmin = await _moduleMgr.IsAdminAsync(userId);
                        if (!isAdmin)
                        {
                            return StatusCode(StatusCodes.Status403Forbidden, "Invalid user permissions");
                        }

                        // Proceed to get the user list if the user is an admin
                        var users = _incidentMgr.GetIncidentList(); // Make sure to await this if it's an async method
                        return Ok(users);
                    }
                }

                return BadRequest("Invalid user ID");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error! Please contact the Server Administrator!");
            }
        }
      






        [HttpGet("GetCloseIncidentsByEngineer")]
        public async Task<ActionResult<List<EngineerIncidentDTO>>> GetCloseIncidentsByEngineer()
        {
            try
            {
                var userIdClaim = HttpContext.User?.FindFirst(ClaimTypes.PrimarySid);

                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var engineerId) || engineerId <= 0)
                {
                    return Unauthorized("Invalid engineer id");
                }

                var incidents = await _incidentMgr.GetCloseIncidentsByEngineerId(engineerId);
                if (incidents == null || !incidents.Any())
                {
                    return Ok("The engineer did not closed any incident.");
                }

                return Ok(incidents);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);

            }

        }
        */

        [HttpGet("signature/{Id}")]
        public async Task<IActionResult> GetSignatureByIncidentId(int Id)
        {
            var signature = await _incidentMgr.GetSignatureByIncidentIdAsync(Id);

            if (string.IsNullOrEmpty(signature))
            {
                return NotFound(new { Message = "Signature not found for the given incident ID." });
            }

            return Ok(signature);
        }
    }

    public class SaveSignatureRequest
    {
        public int Id { get; set; }
        public string Signature { get; set; }
    }





}