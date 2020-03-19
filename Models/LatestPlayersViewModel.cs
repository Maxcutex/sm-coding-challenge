using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace sm_coding_challenge.Models
{
    [DataContract]
    public class LatestPlayersViewModel
    {
        [DataMember(Name = "receiving")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ReceivingPlayerModel> ReceivingPlayers { get; set; }

        [DataMember(Name = "rushing")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<RushingPlayerModel> RushingPlayers { get; set; }

        [DataMember(Name = "passing")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<PassingPlayerModel> PassingPlayers { get; set; }

        [DataMember(Name = "kicking")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<KickingPlayerModel> KickingPlayers { get; set; }
    }
}
