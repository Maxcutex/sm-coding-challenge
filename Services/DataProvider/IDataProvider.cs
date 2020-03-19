using System.Collections.Generic;
using System.Threading.Tasks;
using sm_coding_challenge.Models;

namespace sm_coding_challenge.Services.DataProvider
{
    public interface IDataProvider
    {
        // change the methods to async - task methods
        Task<PlayerModel> GetPlayerById(string id);

        // Added the methods for retrieving players by a string array
        Task<List<PlayerAllAttributesModel>> GetPlayersByIds(string[] listIds);
        Task<LatestPlayersViewModel> GetLatestPlayersByIds(string[] idList);
    }
}
