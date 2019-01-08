using Microsoft.AspNetCore.Mvc;
using RV.Model;
using RV.Web.Repository;

namespace RV.Web.Controllers
{
    [Route("api/point")]
    [ApiController]
    public class PointController : ControllerBase
    {
        private readonly IPointRepository _pointRepository;

        public PointController(IPointRepository pointRepository)
        {
            _pointRepository = pointRepository;
        }
        
        [HttpGet("{id}")]
        public ActionResult<Point> Get(int id)
        {
            return _pointRepository.FindById(id);
        }
        
        
    }
}