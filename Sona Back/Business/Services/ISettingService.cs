using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Base;
using DAL.Models;

namespace Business.Services
{
    public interface ISettingService: IBaseService<Setting>
    {
        Task<Dictionary<string, string>> GetAllSettings();
    }
}