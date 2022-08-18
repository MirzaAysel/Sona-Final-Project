using Core.EFRepository.EFEntityRepository;
using DAL.Abstracts;
using DAL.Data;
using DAL.Models;

namespace DAL.Implementations
{
    public class EfBlogRepository: EfEntityRepository<Blog,AppDbContext>, IBlogDal
    {
        public EfBlogRepository(AppDbContext context):base(context)
        {
            
        }
    }
}