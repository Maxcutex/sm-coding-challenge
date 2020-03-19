using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using sm_coding_challenge.Models;
using sm_coding_challenge.Services.CacheService;

namespace sm_coding_challenge.Services.DataProvider
{
    public class DataProviderImpl : IDataProvider
    {
        public static TimeSpan Timeout = TimeSpan.FromSeconds(30);
        private readonly IResponseCacheService _iResponseCacheService;
        private readonly IConfiguration _config;
        private Dictionary<string, bool> _tempDictionary;
        private readonly IMapper _mapper;

        public DataProviderImpl(IResponseCacheService iResponseCacheService, IConfiguration config, IMapper mapper)
        {
            _iResponseCacheService = iResponseCacheService;
            _config = config;
            _mapper = mapper;
            _tempDictionary = new Dictionary<string, bool>();
        }

        public async Task<PlayerModel> GetPlayerById(string id)
        {
            var player = new PlayerModel();
            
            var dataResponse = await GetDataFromEndpoint();

            var playerRush = dataResponse.Rushing.FirstOrDefault(x => x.Id == id);
            if (playerRush != null)
            {
                return playerRush;
            }

            var playerPass = dataResponse.Passing.FirstOrDefault(x => x.Id == id);
            if (playerPass != null)
            {
                return playerPass;
            }

            var playerRecieve = dataResponse.Receiving.FirstOrDefault(x => x.Id == id);
            if (playerRecieve != null)
            {
                return playerRecieve;
            }

            var playerKick = dataResponse.Kicking.FirstOrDefault(x => x.Id == id);
            if (playerKick != null)
            {
                return playerKick;
            }
            
            return player;
        }

        private PlayerModel ValidateIdInList(string[] idList,  PlayerModel item)
        {

            if (idList.Contains(item.Id))
            {
                if (!_tempDictionary.ContainsKey(item.Id))
                {
                    _tempDictionary[item.Id] = true;
                    return item;

                }
            }

            return null;

        }

        public async Task<List<PlayerAllAttributesModel>> GetPlayersByIds(string[] listIds)
        {
            var playerList = new List<PlayerAllAttributesModel>();
            var dataResponse = await GetDataFromEndpoint();
            var receivingPlayers = dataResponse.Receiving.Where(x => ValidateIdInList(listIds, x)!=null).ToList();
            var rushingPlayers = dataResponse.Rushing.Where(x => ValidateIdInList(listIds, x) != null).ToList();
            var passingPlayers = dataResponse.Passing.Where(x => ValidateIdInList(listIds, x) != null).ToList();
            var kickingPlayers = dataResponse.Kicking.Where(x => ValidateIdInList(listIds, x) != null).ToList();
            var receivingWithAttributesGeneric = _mapper.Map<List<ReceivingPlayerModel>,List<PlayerAllAttributesModel>>(receivingPlayers);
            var rushingWithAttributesGeneric = _mapper.Map<List<RushingPlayerModel>,List<PlayerAllAttributesModel>>(rushingPlayers);
            var passingWithAttributesGeneric = _mapper.Map<List<PassingPlayerModel>,List<PlayerAllAttributesModel>>(passingPlayers);
            var kickingWithAttributesGeneric = _mapper.Map<List<KickingPlayerModel>,List<PlayerAllAttributesModel>>(kickingPlayers);

            playerList.AddRange(receivingWithAttributesGeneric);
            playerList.AddRange(rushingWithAttributesGeneric);
            playerList.AddRange(passingWithAttributesGeneric);
            playerList.AddRange(kickingWithAttributesGeneric);
            if (playerList.Any())
            {
                return playerList;
            }

            return null;
        }


        public async Task<LatestPlayersViewModel> GetLatestPlayersByIds(string[] listIds)
        {
            var playerList = new LatestPlayersViewModel();
            var dataResponse = await GetDataFromEndpoint();
            var receivingPlayers = dataResponse.Receiving.Where(x => listIds.Contains(x.Id)).ToList();
            var rushingPlayers = dataResponse.Rushing.Where(x => listIds.Contains(x.Id)).ToList();
            var passingPlayers = dataResponse.Passing.Where(x => listIds.Contains(x.Id)).ToList();
            var kickingPlayers = dataResponse.Kicking.Where(x => listIds.Contains(x.Id)).ToList();

            playerList.ReceivingPlayers = receivingPlayers;
            playerList.RushingPlayers = rushingPlayers;
            playerList.PassingPlayers = passingPlayers;
            playerList.KickingPlayers = kickingPlayers;
             
            if (playerList.ReceivingPlayers.Any())
            {
                return playerList;
            }

            return null;
        }
        public class ThrottlingHandler : DelegatingHandler
        {
            private SemaphoreSlim _throttler;

            public ThrottlingHandler(SemaphoreSlim throttler, HttpClientHandler handler) : base(handler)
            {
                _throttler = throttler ?? throw new ArgumentNullException(nameof(throttler));
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                if (request == null) throw new ArgumentNullException(nameof(request));

                // limits the number of requests that can be made on the request
                await _throttler.WaitAsync(cancellationToken);
                try
                {
                    return await base.SendAsync(request, cancellationToken);
                }
                finally
                {
                    _throttler.Release();
                }
            }
        }
        private async Task<DataResponseModel> GetDataFromEndpoint()
        {
            HttpResponseMessage response;
            // the number of parallel request that can be made to the data endpoint.
            int maxParallelism = Convert.ToInt16(_config["maxParallelism"]);
            var dataResponse = new DataResponseModel();

            // data endpoint 
            var dataUrl = _config["DataUrl"];

            // The number of days the response from the data endpoint
            var timeToLiveSeconds = Convert.ToInt16(_config["DataUrlTimeToLive"]);


            var cachedResponse = await _iResponseCacheService.GetCachedResponseAsync(dataUrl);
            if (!string.IsNullOrEmpty(cachedResponse))
            {
                return JsonConvert.DeserializeObject<DataResponseModel>(cachedResponse, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            }
            var handler = new ThrottlingHandler(new SemaphoreSlim(maxParallelism), new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,

            });
            using (var client = new HttpClient(handler))
            {
                client.Timeout = Timeout;
                response = await client.GetAsync(dataUrl);

                // This checks if the request was successfull or times out.
                if (response.IsSuccessStatusCode)
                {
                    var stringData = response.Content.ReadAsStringAsync().Result;

                    dataResponse = JsonConvert.DeserializeObject<DataResponseModel>(stringData, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    await _iResponseCacheService.CacheResponseAsync(dataUrl, dataResponse, TimeSpan.FromSeconds(timeToLiveSeconds));
                }
                else
                {
                    throw new CustomResponseException("Endpoint Could not be reached.");
                }
            }

            return dataResponse;
        }
    }

    // Custom Exception to be raised when the data endpoint cannot be reached
    public class CustomResponseException : Exception
    {
        
        public CustomResponseException(string message) : base(message)
        {

        }
    }
}
