using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Services;
using DAL.Abstracts;
using DAL.Models;

namespace Business.Implementations
{
    public class BlogRepository : IBlogService
    {
        private readonly IBlogDal _blogRepository;

        public BlogRepository(IBlogDal blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<List<Blog>> GetAll()
        {
            var data = await _blogRepository.GetAllAsync(n => !n.IsDeleted);
            return data;
        }

        public async Task<Blog> Get(int? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException();
            }

            var data = await _blogRepository.GetAsync(n => !n.IsDeleted && n.Id == id);
            if (data is null)
            {
                throw new NullReferenceException();
            }

            return data;
        }

        public async Task Update(Blog entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException();
            }

            await _blogRepository.UpdateAsync(entity);
        }

        public async Task Create(Blog entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException();
            }

            await _blogRepository.AddAsync(entity);
        }

        public async Task Delete(int? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException();
            }

            var data = await Get(id);

            if (data is null)
            {
                throw new NullReferenceException();
            }

            await _blogRepository.DeleteAsync(data);
        }
    }
}