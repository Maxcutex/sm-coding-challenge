using AutoMapper;
using sm_coding_challenge.Models;

namespace sm_coding_challenge.Maps
{
    public class PassingMap : IAutoMapperTypeConfigurator
    {
        public void Configure(IMapperConfigurationExpression configuration)
        {
            var map = configuration.CreateMap<PassingPlayerModel, PlayerAllAttributesModel>()
                .ForMember(dest => dest.Tds, opts => opts.MapFrom(src => src.Tds));
        }
    }
}