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
        [DataMember(Name = "yds")]
        public string Yards { get; set; }
        [DataMember(Name = "tds")]
        public string Tds { get; set; }
        [DataMember(Name = "rec")]
        public string Rec { get; set; }
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
        [DataMember(Name = "yds")]
        public string Yards { get; set; }
        [DataMember(Name = "att")]
        public string Att { get; set; }
        [DataMember(Name = "tds")]
        public string Tds { get; set; }

        [DataMember(Name = "cmp")]
        public string Cmp { get; set; }

        [DataMember(Name = "int")]
        public string IntValue { get; set; }
    }

    [DataContract]
    public class KickingPlayerModel : PlayerModel
    {
        [DataMember(Name = "fld_goals_made")]
        public string FieldGoalsMade { get; set; }

        [DataMember(Name = "fld_goals_att")]
        public string FieldGoalsAttained { get; set; }

        [DataMember(Name = "extra_pt_made")]
        public string ExtraPointMade { get; set; }

        [DataMember(Name = "extra_pt_att")]
        public string ExtraPointAttained { get; set; }
    }
}

