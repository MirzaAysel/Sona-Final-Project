using System.Collections.Generic;
using System.Threading.Tasks;
using Core.EFRepository;
using DAL.Models;

namespace DAL.Abstracts
{
    public interface ISettingDal: IRepositoryBase<Setting>
    {
        Task<Dictionary<string, string>> GetAllSettingsAsync();
    }
}