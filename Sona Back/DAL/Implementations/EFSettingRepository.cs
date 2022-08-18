using System.Collections.Generic;
using System.Threading.Tasks;
using Core.EFRepository.EFEntityRepository;
using DAL.Abstracts;
using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Implementations
{
    public class EFSettingRepository: EfEntityRepository<Setting,AppDbContext>, ISettingDal
    {
        private readonly AppDbContext _context;
        public EFSettingRepository(AppDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, string>> GetAllSettingsAsync()
        {
            var data = await _context.Settings.ToDictionaryAsync(n => n.Key, n => n.Value);
            return data;
        }
    }
}