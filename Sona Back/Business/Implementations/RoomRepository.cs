using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Services;
using DAL.Abstracts;
using DAL.Models;

namespace Business.Implementations
{
    public class RoomRepository: IRoomService
    {
        private readonly IRoomDal _roomRepository;

        public RoomRepository(IRoomDal roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<List<Room>> GetAll()
        {
            var data = await _roomRepository.GetAllAsync(n => !n.IsDeleted);
            return data;
        }

        public async Task<Room> Get(int? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException();
            }

            var data = await _roomRepository.GetAsync(n => !n.IsDeleted && n.Id == id);
            if (data is null)
            {
                throw new NullReferenceException();
            }

            return data;
        }

        public async Task Update(Room entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException();
            }
            await _roomRepository.UpdateAsync(entity);
        }

        public async Task Create(Room entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException();
            }
            await _roomRepository.AddAsync(entity);
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

            await _roomRepository.DeleteAsync(data);
        }
    }
}