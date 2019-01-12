using Microsoft.AspNetCore.Mvc;
 using RV.Model.Entities;
 using RV.Web.Repository.Road;
 
 namespace RV.Web.Controllers
 {
     [Route("api/road")]
     [ApiController]
     public class RoadController : ControllerBase
     {
         private readonly IRoadRepository _roadRepository;
 
         public RoadController(IRoadRepository roadRepository)
         {
             _roadRepository = roadRepository;
         }
         
         [HttpGet("{id}")]
         public ActionResult<Road> Get(int id)
         {
             return _roadRepository.FindById(id);
         }
     }
 }