using Core.EFRepository.EFEntityRepository;
using DAL.Abstracts;
using DAL.Data;
using DAL.Models;

namespace DAL.Implementations
{
    public class EfCommentRepository:  EfEntityRepository<Comment,AppDbContext>, ICommentDal
    {
        public EfCommentRepository(AppDbContext context) : base(context)
        {
        }
    }
}