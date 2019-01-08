using AutoMapper;
using RV.Model.Entities;
using RV.Model.ViewModels;

namespace RV.Web.Configuration
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg => {});
        }

        private static void AddRvAutoMapperConfiguration(IProfileExpression mapperConfigurationExpression)
        {
            mapperConfigurationExpression.CreateMap<PointViewModel, Point>();
        }
    }
}