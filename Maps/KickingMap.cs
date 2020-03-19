using AutoMapper;
using sm_coding_challenge.Models;

namespace sm_coding_challenge.Maps
{
    public class KickingMap : IAutoMapperTypeConfigurator
    {
        public void Configure(IMapperConfigurationExpression configuration)
        {
            var map = configuration.CreateMap<KickingPlayerModel, PlayerAllAttributesModel>();
        }
    }
}