using RV.Model;

namespace RV.Web.Repository
{
    public interface IRepository<out T> where T : BaseEntity
    {
        T FindById(int id);
    }
}