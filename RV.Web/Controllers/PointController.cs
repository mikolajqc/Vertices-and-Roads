using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RV.Model.Entities;
using RV.Model.ViewModels;
using RV.Web.Repository;
using RV.Web.Requests;

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
        public ActionResult<PointViewModel> Get(int id)
        {
            return Mapper.Map<PointViewModel>(_pointRepository.FindById(id));
        }

        [HttpPost, Route("shortest-path-dijkstra")]
        public ActionResult<IEnumerable<PointViewModel>> GetShortestPathUsingDijkstra([FromBody]GetShortestPathRequest request)
        {
            return Ok(_pointRepository.GetPointsOnShortestPathUsingDijkstra(
                Mapper.Map<Requests.Common.Point,Point>(request.SourcePoint, opt =>
                opt.AfterMap((src, dest) => dest.Id = 0)),
                Mapper.Map<Requests.Common.Point,Point>(request.TargetPoint, opt =>
                    opt.AfterMap((src, dest) => dest.Id = 0))
                ));
        }
        
        [HttpPost, Route("shortest-path-astar")]
        public ActionResult<IEnumerable<PointViewModel>> GetShortestPathUsingAStar([FromBody]GetShortestPathRequest request)
        {
            return Ok(_pointRepository.GetPointsOnShortestPathUsingAStar(
                Mapper.Map<Requests.Common.Point,Point>(request.SourcePoint, opt =>
                    opt.AfterMap((src, dest) => dest.Id = 0)),
                Mapper.Map<Requests.Common.Point,Point>(request.TargetPoint, opt =>
                    opt.AfterMap((src, dest) => dest.Id = 0))
            ));
        }
    }
}