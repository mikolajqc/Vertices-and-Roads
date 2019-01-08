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

        [HttpPost, Route("shortest-path")]
        public ActionResult<List<PointViewModel>> GetShortestPath(GetShortestPathRequest request)
        {
            return new ActionResult<List<PointViewModel>>(new List<PointViewModel>
            {
                new PointViewModel {Id = 1,Latitude = 10,Longitude = 10},
                new PointViewModel {Id = 2,Latitude = 20,Longitude = 20}
            });
        }
    }
}