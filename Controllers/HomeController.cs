using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        [Cached(600)]
        public IActionResult Player(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return BadRequest("Invalid Request Data");
            }
            try
            {
                var player = _dataProvider.GetPlayerById(id);
                if (player == null)
                {
                    return NotFound($"Player with id {id} not found");
                }
                return Json(player);
            }
            catch (CustomResponseException e)
            {
                // return Gateway error when the endpoint cannot be reached.
                return StatusCode(502);
            }
            
        }

        [HttpGet]
        public IActionResult Players(string ids)
        {
            
            if (String.IsNullOrEmpty(ids))
            {
                return BadRequest("Invalid Request Data");
            }
            try
            {
                var idList = ids.Split(',').Distinct().ToArray();
                var players = _dataProvider.GetPlayersByIds(idList);
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
            
        }

        [HttpGet]
        public IActionResult LatestPlayers(string ids)
        {
            if (String.IsNullOrEmpty(ids))
            {
                return BadRequest("Invalid Request Data");
            }
            try
            {
                var idList = ids.Split(',').Distinct().ToArray();
                var players = _dataProvider.GetLatestPlayersByIds(idList);
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
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
