using AutoMapper;
using RV.Model.Entities;
using RV.Model.ViewModels;

namespace RV.Web.Configuration
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg => { AddRvAutoMapperConfiguration(cfg);});
        }

        private static void AddRvAutoMapperConfiguration(IProfileExpression mapperConfigurationExpression)
        {
            mapperConfigurationExpression.CreateMap<PointViewModel, Point>();
            mapperConfigurationExpression.CreateMap<Point, PointViewModel>();
            mapperConfigurationExpression.CreateMap<Point, Requests.Common.Point>();
            mapperConfigurationExpression.CreateMap<Requests.Common.Point, Point>()
                .ForMember(dest=>dest.Id, opt=>opt.Ignore());
        }
    }
}