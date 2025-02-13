using DocumentFormat.OpenXml.Drawing.Charts;
using EMS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DAL.Interface
{
    public interface IIncidentMgr
    {
        List<RequestTypeDTO> GetRequestType();

        List<CompanyTypeDTO> GetCompanyType();

        List<SubjectDTO> GetSubjectList();

        List<IncidentCategoryDTO> GetIncidentCategory();

        List<EngineerDTO> GetEngineerList();

        List<AdminDTO> GetAdminList();

        List<IncidentStatusDTO> GetIncidentStatuses();


        public Task<IncidentDTOListResult> AddIncidentAsync(AddIncidentDTO incidentDetails);

        public Task<IncidentDetailsDTO> GetIncidentDetailsByIdAsync(int Id);

        List<AllIncidentDTO> GetOpenIncidentList();

        List<AllIncidentDTO> GetCloseIncidentList();

        public Task<IncidentDTOListResult> EditIncidentAsync(EditIncidentDTO incidentDetails);

        public Task<IncidentDTOListResult> DeleteIncidentAsync(int Id);

        // List<AllIncidentDTO> GetIncidentList();

        public Task<List<EngineerIncidentDTO>> GetOpenIncidentsByEngineerId(int engineerId);

        public Task<List<EngineerIncidentDTO>> GetCloseIncidentsByEngineerId(int engineerId);

        public Task<EngineerIncidentDetailsDTO> EngineerGetIncidentDetails(int Id);

        public Task<IncidentDTOListResult> EngineerEditIncident(EngineerEditIncidentDTO incidentDetails);

        public Task<bool> SaveSignatureAsync(int Id, string signature);
        public Task<string?> GetSignatureByIncidentIdAsync(int Id);

        public Task<MemoryStream> GetIncidentReportExcelAsync(List<FilterIncidentDTO> rows);
        public Task<List<FilterIncidentDTO>> GetFilteredIncidentsAsync(DateTime? fromDate, DateTime? toDate, int? statusId, List<int> engineerId);
    }
}