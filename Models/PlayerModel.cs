using System.Runtime.Serialization;

namespace sm_coding_challenge.Models
{
    [DataContract]
    public class PlayerModel
    {
        [DataMember(Name = "player_id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "position")]
        public string Position { get; set; }

        [DataMember(Name = "entry_id")]
        public string EntryId { get; set; }
        
    }
    [DataContract]
    public class ReceivingPlayerModel : PlayerModel
    {
        /*
         * "yds": "7",
         "tds": "0",
         "rec": "1"
         */
    }

    [DataContract]
    public class RushingPlayerModel : PlayerModel
    {
        [DataMember(Name = "yds")]
        public string Yards { get; set; }
        [DataMember(Name = "att")]
        public string Att { get; set; }
        [DataMember(Name = "tds")]
        public string Tds { get; set; }
        [DataMember(Name = "fum")]
        public string Fum { get; set; }
    }

    [DataContract]
    public class PassingPlayerModel : PlayerModel
    {
        /*
         * "yds": "177",
         "att": "23",
         "tds": "1",
         "cmp": "12",
         "int": "1"
         */
    }

    [DataContract]
    public class KickingPlayerModel : PlayerModel
    {
        /*
         *"fld_goals_made": "1",
         "fld_goals_att": "1",
         "extra_pt_made": "5",
         "extra_pt_att": "5"
         *
         */
    }
}

