using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Services;
using DAL.Abstracts;
using DAL.Models;

namespace Business.Implementations
{
    public class SettingRepository : ISettingService
    {
        private readonly ISettingDal _settingRepository;

        public SettingRepository(ISettingDal settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public async Task<List<Setting>> GetAll()
        {
            var data = await _settingRepository.GetAllAsync();
            return data;
        }

        public async Task<Setting> Get(int? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException();
            }

            var data = await _settingRepository.GetAsync(n => n.Id == id);

            if (data is null)
            {
                throw new NullReferenceException();
            }

            return data;
        }

        public async Task Update(Setting entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException();
            }

            await _settingRepository.UpdateAsync(entity);
        }

        public Task Create(Setting entity)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(int? id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Dictionary<string, string>> GetAllSettings()
        {
            return await _settingRepository.GetAllSettingsAsync();
        }
    }
}