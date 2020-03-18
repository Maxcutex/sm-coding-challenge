using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using sm_coding_challenge.Models;

namespace sm_coding_challenge.Services.DataProvider
{
    public class DataProviderImpl : IDataProvider
    {
        public static TimeSpan Timeout = TimeSpan.FromSeconds(30);
        
        public async Task<PlayerModel> GetPlayerById(string id)
        {
            var player = new PlayerModel();
            var playerList = new List<PlayerModel>();
            var dataResponse = await GetDataFromEndpoint();
            playerList.AddRange(dataResponse.Kicking.ToList());
            playerList.AddRange(dataResponse.Passing.ToList());
            playerList.AddRange(dataResponse.Receiving.ToList());
            playerList.AddRange(dataResponse.Rushing.ToList());
            player = playerList.FirstOrDefault(x => x.Id == id);
            return player;
        }

        public async Task<List<PlayerModel>> GetPlayersByIds(string[] listIds)
        {
            var playerList = new List<PlayerModel>();
            var dataResponse = await GetDataFromEndpoint();
            var receivingPlayers = dataResponse.Receiving.Where(x => listIds.Contains(x.Id)).ToList();
            var rushingPlayers = dataResponse.Rushing.Where(x => listIds.Contains(x.Id)).ToList();
            var passingPlayers = dataResponse.Passing.Where(x => listIds.Contains(x.Id)).ToList();
            var kickingPlayers = dataResponse.Kicking.Where(x => listIds.Contains(x.Id)).ToList();

            playerList.AddRange(receivingPlayers);
            playerList.AddRange(rushingPlayers);
            playerList.AddRange(passingPlayers);
            playerList.AddRange(kickingPlayers);
            if (playerList.Any())
            {
                return playerList;
            }

            return null;
        }

        public async Task<LatestPlayersViewModel> GetLatestPlayersByIds(string[] idList)
        {
            throw new NotImplementedException();
        }

        private async Task<DataResponseModel> GetDataFromEndpoint()
        {
            HttpResponseMessage response;
            var dataResponse = new DataResponseModel();
            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            using (var client = new HttpClient(handler))
            {
                client.Timeout = Timeout;
                response = await client.GetAsync("https://gist.githubusercontent.com/RichardD012/a81e0d1730555bc0d8856d1be980c803/raw/3fe73fafadf7e5b699f056e55396282ff45a124b/basic.json");
                if (response.IsSuccessStatusCode)
                {
                    var stringData = response.Content.ReadAsStringAsync().Result;
                    dataResponse = JsonConvert.DeserializeObject<DataResponseModel>(stringData, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
                else
                {
                    throw new CustomResponseException("Endpoint Could not be reached.");
                }
            }

            return dataResponse;
        }
    }

    public class CustomResponseException : Exception
    {
        
        public CustomResponseException(string message) : base(message)
        {

        }
    }
}
