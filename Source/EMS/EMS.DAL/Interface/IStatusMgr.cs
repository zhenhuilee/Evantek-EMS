using EMS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DAL.Interface
{
    public interface IStatusMgr
    {
        List<AllStatusesDTO> GetStatusList();
        Task<StatusDTOListResult> AddStatusAsync(StatusAddDTO statusDetails, int loggedUserId);
        Task<ResultDTO> EditStatus(StatusEditDTO statusEditDTO, int loggedUserId);
        Task<ResultDTO> DeleteStatusAsync(int statusId, int loggedUserId); 

        
    }
}
