using Core.EFRepository.EFEntityRepository;
using DAL.Abstracts;
using DAL.Data;
using DAL.Models;

namespace DAL.Implementations
{
    public class EFRoomRepository: EfEntityRepository<Room,AppDbContext>, IRoomDal
    {
        public EFRoomRepository(AppDbContext context): base(context)
        {
            
        }
    }
}