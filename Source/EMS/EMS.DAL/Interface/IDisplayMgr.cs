using System.Collections.Generic;
using EMS.DTO;

namespace EMS.DAL.Interface
{
    public interface IDisplayMgr
    {
        List<DisplayDTO> GetDisplayList();
    }
}