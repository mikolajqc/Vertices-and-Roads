using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RV.Model.Entities;
using RV.Web.Repository.Road;
using RV.Web.Requests;

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

        [HttpPost, Route("get-view-roads")]
        public ActionResult<IEnumerable<Road>> GetViewRoads([FromBody] GetPathWithRoadsWithViewRequest request)
        {
            return Ok(_roadRepository.GetViewRoads(
                Mapper.Map<Requests.Common.Point, Point>(request.SourcePoint, opt =>
                    opt.AfterMap((src, dest) => dest.Id = 0)),
                Mapper.Map<Requests.Common.Point, Point>(request.TargetPoint, opt =>
                    opt.AfterMap((src, dest) => dest.Id = 0)),
                request.MinimalLengthOfViewRoads
            ));
        }
    }
}