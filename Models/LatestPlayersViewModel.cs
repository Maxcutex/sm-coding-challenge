using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace sm_coding_challenge.Models
{
    [DataContract]
    public class LatestPlayersViewModel
    {
        [DataMember(Name = "receiving")]
        public List<PlayerModel> ReceivingPlayers { get; set; }

        [DataMember(Name = "rushing")]
        public List<PlayerModel> RushingPlayers { get; set; }

        [DataMember(Name = "passing")]
        public List<PlayerModel> PassingPlayers { get; set; }

        [DataMember(Name = "kicking")]
        public List<PlayerModel> KickingPlayers { get; set; }
    }
}
