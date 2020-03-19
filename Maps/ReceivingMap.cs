using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

using sm_coding_challenge.Models;

namespace sm_coding_challenge.Maps
{
    public class ReceivingMap : IAutoMapperTypeConfigurator
    {
        public void Configure(IMapperConfigurationExpression configuration)
        {
            var map = configuration.CreateMap<ReceivingPlayerModel, PlayerAllAttributesModel>();
        }
    }
}
