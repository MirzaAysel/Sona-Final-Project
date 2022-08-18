using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Services;
using DAL.Abstracts;
using DAL.Models;

namespace Business.Implementations
{
    public class CommentRepository : ICommentService
    {
        private readonly ICommentDal _commentRepository;

        public CommentRepository(ICommentDal commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<List<Comment>> GetAll()
        {
            var data = await _commentRepository.GetAllAsync(n => !n.IsDeleted);
            return data;
        }

        public async Task<Comment> Get(int? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException();
            }

            var data = await _commentRepository.GetAsync(n => !n.IsDeleted && n.Id == id);
            if (data is null)
            {
                throw new NullReferenceException();
            }

            return data;
        }

        public async Task Update(Comment entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException();
            }

            await _commentRepository.UpdateAsync(entity);
        }

        public async Task Create(Comment entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException();
            }

            await _commentRepository.AddAsync(entity);
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

            await _commentRepository.DeleteAsync(data);
        }
    }
}