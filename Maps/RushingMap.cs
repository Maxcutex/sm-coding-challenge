using AutoMapper;
using sm_coding_challenge.Models;

namespace sm_coding_challenge.Maps
{
    public class RushingMap : IAutoMapperTypeConfigurator
    {
        public void Configure(IMapperConfigurationExpression configuration)
        {
            var map = configuration.CreateMap<RushingPlayerModel, PlayerAllAttributesModel>();
        }
    }
}