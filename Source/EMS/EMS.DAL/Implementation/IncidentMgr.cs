using AutoMapper;
using ClosedXML.Excel;
using CloudinaryDotNet.Actions;
using DocumentFormat.OpenXml.Spreadsheet;
using EMS.DAL.Interface;
using EMS.DataAccess.Models;
using EMS.DTO;
using EMS.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Utilities.Net;

namespace EMS.DAL.Implementation
{
    public class IncidentMgr : IIncidentMgr
    {
        private readonly EmsDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<IncidentMgr> _logger;
        private readonly IEmailMgr _emailMgr;

        public IncidentMgr(EmsDbContext dbContext, IMapper mapper, ILogger<IncidentMgr> logger, IEmailMgr emailMgr)
        {
            _context = dbContext;
            _mapper = mapper;
            _logger = logger;
            _emailMgr = emailMgr;
        }

        public List<RequestTypeDTO> GetRequestType()
        {
            try
            {
                var requestTypes = _context.RequestTypes
                    .Select(it => new RequestTypeDTO
                    {
                        Id = it.Id,
                        Name = it.Name
                    }).ToList();

                return requestTypes;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving request type: {ex.Message}");
                throw;
            }
        }

        public List<CompanyTypeDTO> GetCompanyType()
        {
            try
            {
                var companyTypes = _context.CompanyTypes
                    .Select(it => new CompanyTypeDTO
                    {
                        Id = it.Id,
                        Name = it.Name
                    }).ToList();
                return companyTypes;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving company type: {ex.Message}");
                throw;
            }
        }


        public List<SubjectDTO> GetSubjectList()
        {
            try
            {
                var subject = _context.Subjects
                    .Select(it => new SubjectDTO
                    {
                        Id = it.Id,
                        Name = it.Name
                    }).ToList();

                return subject;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving subject lists: {ex.Message}");
                throw;
            }
        }

        public List<IncidentStatusDTO> GetIncidentStatuses()
        {
            try
            {
                var status = _context.IncidentStatuses
                    .Select(it => new IncidentStatusDTO
                    {
                        Id = it.Id,
                        Name = it.Name
                    }).ToList();

                return status;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving status lists: {ex.Message}");
                throw;

            }
        }


        public List<IncidentCategoryDTO> GetIncidentCategory()
        {
            try
            {
                var incidentCategory = _context.IncidentCategories
                    .Select(it => new IncidentCategoryDTO
                    {
                        Id = it.Id,
                        Name = it.Name
                    }).ToList();

                return incidentCategory;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving incident category: {ex.Message}");
                throw;
            }
        }

        public List<EngineerDTO> GetEngineerList()
        {
            try
            {
                // Get users who have the role of engineer (RoleId = 3)
                var engineers = _context.Users
                    .Where(u => u.UserRoleMappers.Any(ur => ur.RoleId == 3)) // Assuming UserRoles is the join table for the many-to-many relationship
                    .Select(u => new EngineerDTO
                    {
                        Id = u.Id,
                        Name = u.Name
                    }).ToList();

                return engineers;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving engineers: {ex.Message}");
                throw;
            }
        }

        public List<AdminDTO> GetAdminList()
        {
            try
            {
                var admin = _context.Users
                    .Where(u => u.UserRoleMappers.Any(ur => ur.RoleId == 1))
                    .Select(u => new AdminDTO
                    {
                        Id = u.Id,
                        Name = u.Name
                    }).ToList();

                return admin;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving admin: {ex.Message}");
                throw;
            }
        }


        public async Task<IncidentDTOListResult> AddIncidentAsync(AddIncidentDTO incidentDetails)
        {
            IncidentDTOListResult oResult = new IncidentDTOListResult
            {
                ResultCode = 0,
                ResultDescription = "Incident added successfully!"
            };

            if (string.IsNullOrEmpty(incidentDetails.Company))
            {
                oResult.ResultCode = 1;
                oResult.ResultDescription = "Failed to add incident. Provided Company Name is empty!";
                return oResult;
            }
            if (string.IsNullOrEmpty(incidentDetails.IpAddress))    
            {
                incidentDetails.IpAddress = null;
            }

            if (string.IsNullOrEmpty(incidentDetails.WorkOrderNo))
            {
                incidentDetails.WorkOrderNo = null;
            }

            try
            {
                // Create and save the new Incident record
                Incident newIncident = new Incident
                {
                    RequestTypeId = incidentDetails.RequestTypeId,
                    Company = incidentDetails.Company,
                    CompanyTypeId = incidentDetails.CompanyTypeId,
                    SubItem = incidentDetails.SubItem,
                    Customer = incidentDetails.Customer,
                    CustomerPhone = incidentDetails.CustomerPhone,
                    SubjectId = incidentDetails.SubjectId,
                    IncidentCategoryId = incidentDetails.IncidentCategoryId,
                    Description = incidentDetails.Description,
                    Address = incidentDetails.Address,
                    IpAddress = incidentDetails.IpAddress,
                    AdminId = incidentDetails.AdminId,
                    WorkOrderNo = incidentDetails.WorkOrderNo,
                    ResponseDateTime = incidentDetails.ResponseDateTime?.ToLocalTime(),
                    IncidentStatusId = 1,
                    IncidentCreatedDateTime = DateTime.Now,
                };

                // Assign engineers to the incident
                foreach (int engineerId in incidentDetails.EngineerId)
                {
                    var user = await _context.Users.Include(x => x.UserRoleMappers)
                                        .FirstOrDefaultAsync(x => x.Id == engineerId);

                    if (user == null || !user.UserRoleMappers.Any(x => x.RoleId == (int)RoleEnum.Engineer))
                    {
                        //Provided user is not an engineer
                        continue;
                    }

                    newIncident.Engineers.Add(user);
                }

                _context.Incidents.Add(newIncident);
                await _context.SaveChangesAsync();

                // Send email notifications to all assigned engineers
                var engineers = await _context.Users
                    .Where(u => incidentDetails.EngineerId.Contains(u.Id))
                    .ToListAsync();

                foreach (var engineer in engineers)
                {
                    if (!string.IsNullOrEmpty(engineer.EmailAddress))
                    {
                        EmailDto mailData = new EmailDto
                        {
                            To = engineer.EmailAddress,
                            Subject = $"Fault Call Notification",
                            Body = $@"
Dear {engineer.Name},<br/><br/>

You are assigned a new incident.<br/><br/>

The details of the incident are as follows:<br/><br/>

<u>Incident Details</u></br>
Ref Num: {newIncident.RefNum}<br/>
Company: {newIncident.Company}<br/>
SubItem: {newIncident.SubItem}<br/> 
Address: {newIncident.Address}<br/><br/>  

For more information, please log in to the Employee Management System (EMS).<br/><br/>

Thank you.<br/><br/>

Best regards,<br/>
Incident Management Team"
                        };

                        _emailMgr.SendEmail(mailData);
                    }
                }

                return oResult;
            }
            catch (Exception ex)
            {
                oResult.ResultCode = 1;
                oResult.ResultDescription = "Internal Server Error! Please contact the Server Administrator!";
                _logger.LogError(ex, ex.StackTrace);
                return oResult;
            }
        }


        public List<AllIncidentDTO> GetOpenIncidentList()
        {
            return _context.Incidents
                .Where(incident => incident.IncidentStatusId == 1)
                .Select(incident => new AllIncidentDTO
                {
                    Id = incident.Id,
                    RefNum = incident.RefNum,
                    CompanyName = incident.Company,
                    SubItem = incident.SubItem,
                    StatusName = _context.IncidentStatuses.FirstOrDefault(status => status.Id == incident.IncidentStatusId)!.Name,
                    SubjectName = _context.Subjects.FirstOrDefault(subject => subject.Id == incident.SubjectId)!.Name,
                    IpAddress = incident.IpAddress,
                    EngineerName = String.Join(", ", incident.Engineers.Select(x => x.Name).ToList()),
                    IncidentCreatedDateTime = incident.IncidentCreatedDateTime
                })
                .ToList();
        }

        public List<AllIncidentDTO> GetCloseIncidentList()
        {
            return _context.Incidents
                .Where(incident => incident.IncidentStatusId == 2)
                .Select(incident => new AllIncidentDTO
                {
                    Id = incident.Id,
                    RefNum = incident.RefNum,
                    CompanyName = incident.Company,
                    SubItem = incident.SubItem,
                    StatusName = _context.IncidentStatuses.FirstOrDefault(status => status.Id == incident.IncidentStatusId)!.Name,
                    SubjectName = _context.Subjects.FirstOrDefault(subject => subject.Id == incident.SubjectId)!.Name,
                    IpAddress = incident.IpAddress,
                    EngineerName = String.Join(", ", incident.Engineers.Select(x => x.Name).ToList()),
                    IncidentCreatedDateTime = incident.IncidentCreatedDateTime
                })
                .ToList();
        }


        public async Task<IncidentDetailsDTO> GetIncidentDetailsByIdAsync(int Id)
        {
            try
            {
                // Fetch incident details
                var incident = await _context.Incidents
                    .Include(i => i.IncidentStatus)
                    .Include(i => i.RequestType)
                    .Include(i => i.IncidentCategory)
                    .Include(i => i.Subject)
                    .Include(i => i.CompanyType)
                    .Include(i => i.Replacements)
                    .Include(i => i.Engineers)
                    .FirstOrDefaultAsync(i => i.Id == Id);

                if (incident == null)
                {
                    _logger.LogWarning($"Incident with ID {Id} not found.");
                    return null;
                }

                // Map incident details to DTO
                var incidentDetails = new IncidentDetailsDTO
                {
                    RefNum = incident.RefNum,
                    StatusId = incident.IncidentStatusId,
                    RequestTypeId = (int)incident.RequestTypeId,
                    Company = incident.Company,
                    CompanyTypeId = incident.CompanyTypeId,
                    SubItem = incident.SubItem,
                    Customer = incident.Customer,
                    CustomerPhone = incident.CustomerPhone,
                    SubjectId = incident.SubjectId,
                    CategoryId = incident.IncidentCategoryId,
                    Description = incident.Description,
                    Address = incident.Address,
                    IpAddress = incident.IpAddress,
                    EngineerId = incident.Engineers.Select(x => x.Id).ToList(),
                    AdminId = incident.AdminId,
                    WorkOrderNo = incident.WorkOrderNo,
                    ResponseDateTime = incident.ResponseDateTime,
                    //CompletedDateTime = incident.CompletedDateTime,
                    //ArrivalDateTime = incident.ArrivalDateTime,
                    //Solution = incident.Solution,

                    /*
                    // Map replacements
                    Replacements = incident.Replacements.Select(r => new ReplacementDTO
                    {
                        Model = r.Model,
                        OldSerialNo = r.OldSerialNo,
                        NewSerialNo = r.NewSerialNo,
                        Remarks = r.Remarks
                    }).ToList()
                    */
                };

                return incidentDetails;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving incident details for ID {Id}: {ex.Message}");
                throw;
            }
        }


        public async Task<IncidentDTOListResult> EditIncidentAsync(EditIncidentDTO incidentDetails)
        {
            var result = new IncidentDTOListResult();

            try
            {
                var incident = await _context.Incidents
                    .Include(i => i.Engineers)
                    .FirstOrDefaultAsync(i => i.Id == incidentDetails.Id);

                if (incident == null)
                {
                    result.ResultCode = 1;
                    result.ResultDescription = "Incident not found.";
                    return result;
                }

                // Update Incident fields
                incident.IncidentStatusId = incidentDetails.StatusId;
                incident.RequestTypeId = incidentDetails.RequestTypeId;
                incident.CompanyTypeId = incidentDetails.CompanyTypeId;
                incident.SubjectId = incidentDetails.SubjectId;
                incident.IncidentCategoryId = incidentDetails.CategoryId;

                incident.AdminId = incidentDetails.AdminId;
                incident.Company = incidentDetails.Company;
                incident.SubItem = incidentDetails.SubItem;
                incident.Customer = incidentDetails.Customer;
                incident.CustomerPhone = incidentDetails.CustomerPhone;
                incident.Description = incidentDetails.Description;
                incident.Address = incidentDetails.Address;
                incident.IpAddress = incidentDetails.IpAddress;
                incident.WorkOrderNo = incidentDetails.WorkOrderNo;
                incident.ResponseDateTime = incidentDetails.ResponseDateTime?.ToLocalTime();

                // Update Engineer assignments
                if (incidentDetails.EngineerId != null)
                {
                    // Track newly assigned engineers
                    var newEngineerIds = incidentDetails.EngineerId.Except(incident.Engineers.Select(e => e.Id)).ToList();

                    // Remove existing engineers
                    incident.Engineers.Clear();

                    // Add new engineers
                    foreach (var engineerId in incidentDetails.EngineerId)
                    {
                        var engineer = await _context.Users.FindAsync(engineerId);
                        if (engineer != null)
                        {
                            incident.Engineers.Add(engineer);
                        }
                    }

                    // Send email notifications to newly assigned engineers
                    var newEngineers = await _context.Users
                        .Where(u => newEngineerIds.Contains(u.Id))
                        .ToListAsync();

                    foreach (var engineer in newEngineers)
                    {
                        if (!string.IsNullOrEmpty(engineer.EmailAddress))
                        {
                            EmailDto mailData = new EmailDto
                            {
                                To = engineer.EmailAddress,
                                Subject = $"Fault Call Notification",
                                Body = $@"
Dear {engineer.Name},<br/><br/>

You are assigned a new incident.<br/><br/>

The details of the incident are as follows:<br/><br/>

<u>Incident Details</u></br>
Ref Num: {incident.RefNum}<br/>
Company: {incident.Company}<br/>
SubItem: {incident.SubItem}<br/> 
Address: {incident.Address}<br/><br/>  

For more information, please log in to the Employee Management System (EMS).<br/><br/>

Thank you.<br/><br/>

Best regards,<br/>
Incident Management Team"
                            };

                            _emailMgr.SendEmail(mailData);
                        }
                    }
                }

                await _context.SaveChangesAsync();

                result.ResultCode = 0;
                result.ResultDescription = "Incident updated successfully.";
            }
            catch (Exception ex)
            {
                result.ResultCode = 1;
                result.ResultDescription = "An error occurred while updating the incident.";
                _logger.LogError($"Error updating incident ID {incidentDetails.Id}: {ex.Message}");
            }

            return result;
        }

        public async Task<IncidentDTOListResult> DeleteIncidentAsync(int Id)
        {
            var result = new IncidentDTOListResult();

            try
            {
                // Retrieve the incident along with its engineers
                var incident = await _context.Incidents
                    .Include(i => i.Engineers) // Include the related engineers
                    .FirstOrDefaultAsync(i => i.Id == Id);

                if (incident == null)
                {
                    result.ResultCode = 1;
                    result.ResultDescription = "Incident not found.";
                    return result;
                }

                // Clear the relationship in the resolving table
                incident.Engineers.Clear();

                // Remove the incident itself
                _context.Incidents.Remove(incident);

                await _context.SaveChangesAsync();

                result.ResultCode = 0;
                result.ResultDescription = "Incident and related mappings deleted successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting incident with ID {Id}: {ex.Message}");
                result.ResultCode = 1;
                result.ResultDescription = "An error occurred while deleting the incident.";
            }

            return result;
        }

        public async Task<List<EngineerIncidentDTO>> GetOpenIncidentsByEngineerId(int engineerId)
        {
            try
            {
                // Query the database to retrieve incidents associated with the given engineer
                var incidents = await _context.Incidents
                    .Where(incident => incident.Engineers.Any(engineer => engineer.Id == engineerId)
                                       && incident.IncidentStatusId == 1)
                    .Select(incident => new EngineerIncidentDTO
                    {
                        Id = incident.Id,
                        RefNum = incident.RefNum,
                        Company = incident.Company,
                        IncidentCreatedDateTime = incident.IncidentCreatedDateTime
                    })
                    .ToListAsync();

                return incidents;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching incidents for engineer {engineerId}: {ex.Message}");
                throw;
            }
        }

        public async Task<List<EngineerIncidentDTO>> GetCloseIncidentsByEngineerId(int engineerId)
        {
            try
            {
                // Query the database to retrieve incidents assigned to the specified engineer
                var incidents = await _context.Incidents
                    .Where(incident => incident.Engineers.Any(engineer => engineer.Id == engineerId)
                                       && incident.IncidentStatusId == 2)
                    .Select(incident => new EngineerIncidentDTO

                    {
                        Id = incident.Id,
                        RefNum = incident.RefNum,
                        Company = incident.Company,
                        IncidentCreatedDateTime = incident.IncidentCreatedDateTime,

                    })
                    .ToListAsync();

                return incidents;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching incidents for engineer {engineerId}: {ex.Message}");
                throw;
            }
        }

        public async Task<EngineerIncidentDetailsDTO> EngineerGetIncidentDetails(int Id)
        {
            try
            {
                // Fetch incident details
                var incident = await _context.Incidents
                    .Include(i => i.IncidentStatus)
                    .Include(i => i.RequestType)
                    .Include(i => i.IncidentCategory)
                    .Include(i => i.Subject)
                    .Include(i => i.CompanyType)
                    .Include(i => i.Replacements)
                    .Include(i => i.Engineers)
                    .FirstOrDefaultAsync(i => i.Id == Id);

                if (incident == null)
                {
                    _logger.LogWarning($"Incident with ID {Id} not found.");
                    return null;
                }

                // Map incident details to DTO
                var incidentDetails = new EngineerIncidentDetailsDTO
                {
                    RefNum = incident.RefNum,
                    StatusId = incident.IncidentStatusId,
                    RequestTypeId = (int)incident.RequestTypeId,
                    Company = incident.Company,
                    CompanyTypeId = incident.CompanyTypeId,
                    SubItem = incident.SubItem,
                    Customer = incident.Customer,
                    CustomerPhone = incident.CustomerPhone,
                    SubjectId = incident.SubjectId,
                    CategoryId = incident.IncidentCategoryId,
                    Description = incident.Description,
                    Address = incident.Address,
                    IpAddress = incident.IpAddress,
                    EngineerId = incident.Engineers.Select(x => x.Id).ToList(),
                    AdminId = incident.AdminId,
                    WorkOrderNo = incident.WorkOrderNo,
                    ResponseDateTime = incident.ResponseDateTime,
                    CompletedDateTime = incident.CompletedDateTime,
                    ArrivalDateTime = incident.ArrivalDateTime,
                    Solution = incident.Solution,

                    // Map replacements
                    Replacements = incident.Replacements.Select(r => new ReplacementDTO
                    {
                        Model = r.Model,
                        OldSerialNo = r.OldSerialNo,
                        NewSerialNo = r.NewSerialNo,
                        Remarks = r.Remarks
                    }).ToList()
                };

                return incidentDetails;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving incident details for ID {Id}: {ex.Message}");
                throw;
            }
        }

        public async Task<IncidentDTOListResult> EngineerEditIncident(EngineerEditIncidentDTO incidentDetails)
        {
            var result = new IncidentDTOListResult();

            try
            {
                // Fetch the incident to edit
                var incident = await _context.Incidents
                    .Include(i => i.Engineers)
                    .Include(i => i.Replacements)
                    .FirstOrDefaultAsync(i => i.Id == incidentDetails.Id);

                if (incident == null)
                {
                    result.ResultCode = 1;
                    result.ResultDescription = "Incident not found.";
                    return result;
                }

                // Update Incident fields
                //incident.IncidentStatusId = incidentDetails.StatusId;
                incident.RequestTypeId = incidentDetails.RequestTypeId;
                incident.CompanyTypeId = incidentDetails.CompanyTypeId;
                incident.SubjectId = incidentDetails.SubjectId;
                incident.IncidentCategoryId = incidentDetails.CategoryId;

                incident.Company = incidentDetails.Company;
                incident.SubItem = incidentDetails.SubItem;
                incident.Customer = incidentDetails.Customer;
                incident.CustomerPhone = incidentDetails.CustomerPhone;
                incident.Description = incidentDetails.Description;
                incident.Address = incidentDetails.Address;
                incident.IpAddress = incidentDetails.IpAddress;
                incident.WorkOrderNo = incidentDetails.WorkOrderNo;
                incident.Solution = incidentDetails.Solution;
                incident.ResponseDateTime = incidentDetails.ResponseDateTime?.ToLocalTime();
                incident.CompletedDateTime = incidentDetails.CompletedDateTime?.ToLocalTime();
                incident.ArrivalDateTime = incidentDetails.ArrivalDateTime?.ToLocalTime();

                /*
                // Update Engineer assignments
                if (incidentDetails.EngineerId != null && incidentDetails.EngineerId.Any())
                {
                    // Remove current engineers
                    incident.Engineers.Clear();

                    // Add new engineers
                    foreach (var engineerId in incidentDetails.EngineerId)
                    {
                        var engineer = await _context.Users
                            .Include(u => u.UserRoleMappers)
                            .FirstOrDefaultAsync(u => u.Id == engineerId &&
                                u.UserRoleMappers.Any(r => r.RoleId == (int)RoleEnum.Engineer));

                        if (engineer != null)
                        {
                            incident.Engineers.Add(engineer);
                        }
                    }
                }
                */

                // Update Replacements
                _context.Replacements.RemoveRange(incident.Replacements);

                if (incidentDetails.Replacements != null && incidentDetails.Replacements.Any())
                {
                    foreach (var replacement in incidentDetails.Replacements)
                    {
                        await _context.Replacements.AddAsync(new Replacement
                        {
                            IncidentId = incident.Id,
                            Model = replacement.Model,
                            OldSerialNo = replacement.OldSerialNo,
                            NewSerialNo = replacement.NewSerialNo,
                            Remarks = replacement.Remarks
                        });
                    }
                }

                // Save changes
                _context.Incidents.Update(incident);
                await _context.SaveChangesAsync();

                result.ResultCode = 0;
                result.ResultDescription = "Incident updated successfully.";
            }
            catch (Exception ex)
            {
                result.ResultCode = 1;
                result.ResultDescription = "Internal Server Error. Please contact the administrator.";
                _logger.LogError(ex, $"Error updating incident with RefNum {incidentDetails.Id}");
            }

            return result;
        }
        
        
        public async Task<List<FilterIncidentDTO>> GetFilteredIncidentsAsync(DateTime? fromDate, DateTime? toDate, int? statusId, List<int> engineerIds)
          {
            
            var query = _context.Incidents
                .Include(i => i.IncidentStatus) 
                .Include(i => i.Engineers)     
                .AsQueryable();

           // 7 Jan 3 am vs 7 Jan 12 am  
            if (fromDate.HasValue)
            { 
                query = query.Where(i => i.IncidentCreatedDateTime >= fromDate.Value); 
            }

            
            if (toDate.HasValue)
            {
                query = query.Where(i => i.IncidentCreatedDateTime < toDate.Value.AddDays(1));
            }


            // Apply status filter
            if (statusId.HasValue)
            {
                query = query.Where(i => i.IncidentStatusId == statusId.Value);
            }

            // Apply engineer filter
            if (engineerIds != null && engineerIds.Any())
            {
                query = query.Where(i => i.Engineers.Any(e => engineerIds.Contains(e.Id)));
            }

            // Project the filtered incidents into FilterIncidentDTO
            
            var result = await query.Select(i => new FilterIncidentDTO
            {
                RefNum = i.RefNum,
                Company = i.Company,
                SubItem = i.SubItem,
                Status = i.IncidentStatus.Name,
                IpAddress = i.IpAddress,
                //Engineers = string.Join(", ", i.Engineers.Select(e => e.Name)), // Combine multiple engineers into a string
                Engineer = i.Engineers.Select(e => e.Name).ToList(), // Map engineers to their names
                IncidentCreatedDateTime = i.IncidentCreatedDateTime,
                Description = i.Description,
                Solution = i.Solution
            }).ToListAsync();

            return result;
        }
        

       
        public async Task<MemoryStream> GetIncidentReportExcelAsync(List<FilterIncidentDTO> rows)
        {
            XLWorkbook wb = new XLWorkbook(); // Creating a workbook
            IXLWorksheet ws = wb.Worksheets.Add(); // Creating a worksheet

            var companyAddress = ws.Cell(1, 1).SetValue("EVANTEK PTE LTD\r\nBLK 16 KALLANG PLACE #07-35\r\nKALLANG BASIN INDUSTRIAL ESTATE\r\nSINGAPORE 339156\r\nTEL : (65) 6295 8033 FAX (65) 6295 8022");

            var imagePath = @"C:\Users\22015024\Downloads\evanteklogo.png";

            var image = ws.AddPicture(imagePath)
                .MoveTo(ws.Cell(1, 9))
                .Scale(0.7);

            var reportType = ws.Cell(3, 1).SetValue("FAULT CALLS REPORT");

            // Fill headers
            var headers = new List<string> { "Ref Num", "Company", "Sub Item", "Status", "IP Address", "Engineer", "Date created", "Description", "Solution" };
            var headerCell = ws.Row(4).FirstCell();

            foreach (var header in headers)
            {
                headerCell.SetValue(header);
                headerCell = headerCell.CellRight();
            }

            // Fill data
            var dataStartRow = 5;
            var currentRow = dataStartRow;

            foreach (var data in rows)
            {
                var dataCell = ws.Row(currentRow).FirstCell();

                dataCell.SetValue(data.RefNum.ToString())
                    .CellRight().SetValue(data.Company)
                    .CellRight().SetValue(data.SubItem)
                    .CellRight().SetValue(data.Status)
                    .CellRight().SetValue(data.IpAddress)
                    //.CellRight().SetValue(data.Engineer)
                    .CellRight().SetValue(string.Join(", ", data.Engineer))
                    .CellRight().SetValue(data.IncidentCreatedDateTime.ToString())
                    .CellRight().SetValue(data.Description)
                    .CellRight().SetValue(data.Solution);

                currentRow++;
            }

            // Define table range dynamically
            var tableEndRow = currentRow - 1;
            var tableRange = ws.Range($"A4:I{tableEndRow}");
            var table = tableRange.CreateTable();
            table.Theme = XLTableTheme.TableStyleLight16;

            ws.Columns().AdjustToContents();

            // Save the workbook to a memory stream and return it
            MemoryStream fs = new MemoryStream();
            wb.SaveAs(fs);
            fs.Position = 0;
            return fs;
        }
       





        /*
        public List<AllIncidentDTO> GetIncidentList()
        {
            try
            {
                // Retrieve all incidents and join them with the User (engineer) table
                var incidents = (from incident in _context.Incidents
                                 join user in _context.Users
                                 on incident.EngineerId equals user.Id // Join on EngineerId in Incident and Id in User
                                 select new AllIncidentDTO
                                 {
                                     Id = incident.Id,
                                     RefNum = incident.RefNum,
                                     CompanyName = incident.Company,
                                     SubItem = incident.SubItem,
                                     IpAddress = incident.IpAddress,
                                     IncidentCreatedDateTime = incident.IncidentCreatedDateTime,


                                     // Get subject name from the related Subject table
                                     SubjectName = incident.Subject.Name,

                                     // Get status name from the related Status table
                                     StatusName = incident.IncidentStatus.Name,

                                     // Get the engineer name from the User table
                                     EngineerName = user.Name // Assuming 'Name' is the property in the User table for the engineer's name
                                 }).ToList();

                return incidents;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving incident list: {ex.Message}");
                throw;
            }
        }
        */
        /*
        */

        public async Task<bool> SaveSignatureAsync(int Id, string signature)
        {
            try
            {
                var incident = await _context.Incidents.FindAsync(Id);
                if (incident == null)
                    return false;

                incident.Signature = signature;
                _context.Incidents.Update(incident);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new Exception("An error occurred while saving the signature.", ex);
            }
        }


        public async Task<string?> GetSignatureByIncidentIdAsync(int Id)
        {
            var signature = await _context.Incidents
                .Where(i => i.Id == Id)
                .Select(i => i.Signature)
                .FirstOrDefaultAsync();

            return signature;
        }

        /*
        public async Task<List<FilterIncidentDTO>> GetFilteredIncidentsAsync(DateTime? fromDate, DateTime? toDate, int? statusId, List<int> engineerId)
        {
            // Start building the query
            var query = _context.Incidents
                .Include(i => i.IncidentStatus) // Include IncidentStatus for status name
                .Include(i => i.Engineer)          // Include User for engineer name
                .AsQueryable();

            // Apply date filters
            if (fromDate.HasValue)
            {
                query = query.Where(i => i.IncidentCreatedDateTime >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(i => i.IncidentCreatedDateTime <= toDate.Value);
            }

            // Apply status filter
            if (statusId.HasValue)
            {
                query = query.Where(i => i.IncidentStatusId == statusId.Value);
            }

            // Apply engineer filter
            if (engineerId.HasValue)
            {
                query = query.Where(i => i.EngineerId == engineerId.Value);
            }

            // Project the filtered incidents into FilterIncidentDTO
            var result = await query.Select(i => new FilterIncidentDTO
            {

                RefNum = i.RefNum,
                Company = i.Company,
                SubItem = i.SubItem,
                Status = i.IncidentStatus.Name,
                IpAddress = i.IpAddress,
                Engineer = i.Engineer.Name,
                IncidentCreatedDateTime = i.IncidentCreatedDateTime,
                Description = i.Description,
                Solution = i.Solution
            }).ToListAsync();

            return result;
        }
        */



















    }
}