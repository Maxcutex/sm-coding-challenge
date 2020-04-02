using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using sm_coding_challenge.Cache;
using sm_coding_challenge.Models;
using sm_coding_challenge.Services.DataProvider;

namespace sm_coding_challenge.Controllers
{
    public class HomeController : Controller
    {

        private IDataProvider _dataProvider;
        
        public HomeController(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
          
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Cached(600)] // implemented caching for multiple request that do not change over the range of 1 minute
        public async Task<IActionResult> Player(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return BadRequest("Invalid Request Data");
            }
            try
            {
                var player = await _dataProvider.GetPlayerById(id);
                if (player.Id == null)
                {
                    return NotFound($"Player with id {id} not found");
                }
                 
                return Json(player);
            }
            catch (CustomResponseException e)
            {
                // return Gateway error when the endpoint cannot be reached.
                return StatusCode(502, "Data Endpoint cannot be reached");
            }
            catch (HttpRequestException e)
            {
                // return Gateway error when the endpoint cannot be reached due to connection.
                return StatusCode(502);
            }

        }

        [HttpGet]
        [Cached(600)]
        public async Task<IActionResult> Players(string ids)
        {
            
            if (String.IsNullOrEmpty(ids))
            {
                return BadRequest("Invalid Request Data");
            }
            try
            {
                var idList = ids.Split(',').Distinct().ToArray();
                var players = await _dataProvider.GetPlayersByIds(idList);
                if (!players.Any())
                {
                    return NotFound($"Player with set of Ids Not Found");
                }
                return Json(players);
            }
            catch (CustomResponseException e)
            {
                // return Gateway error when the endpoint cannot be reached.
                return StatusCode(502);
            }
            catch (HttpRequestException e)
            {
                // return Gateway error when the endpoint cannot be reached due to connection.
                return StatusCode(502);
            }

        }

        [HttpGet]
        [Cached(600)]
        public async Task<IActionResult> LatestPlayers(string ids)
        {
            if (String.IsNullOrEmpty(ids))
            {
                return BadRequest("Invalid Request Data");
            }
            try
            {
                var idList = ids.Split(',').Distinct().ToArray();
                var players = await _dataProvider.GetLatestPlayersByIds(idList);
                if (players == null)
                {
                    return NotFound($"Player with set of Ids Not Found");
                }
                return Json(players);
            }
            catch (CustomResponseException e)
            {
                // return Gateway error when the endpoint cannot be reached.
                return StatusCode(502);
            }
            catch (HttpRequestException e)
            {
                // return Gateway error when the endpoint cannot be reached due to connection.
                return StatusCode(502);
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
