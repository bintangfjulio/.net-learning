using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IPortofolioRepository _portofolioRepo;

        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepo, IPortofolioRepository portofolioRepo)
        {
            _userManager = userManager;
            _stockRepo = stockRepo;
            _portofolioRepo = portofolioRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();

            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null)
            {
                return Unauthorized("User not found");
            }

            var userPortfolio = await _portofolioRepo.GetUserPortfolio(appUser);
            return Ok(userPortfolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var username = User.GetUsername();

            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null)
            {
                return Unauthorized("User not found");
            }

            var stock = await _stockRepo.GetBySymbolAsync(symbol);
            if (stock == null)
            {
                return BadRequest("Stock does not exist.");
            }

            var userPortfolio = await _portofolioRepo.GetUserPortfolio(appUser);
            if (userPortfolio.Any(x => x.Symbol.ToLower() == symbol.ToLower()))
            {
                return BadRequest("Stock already exists in portfolio.");
            }

            var portfolioModel = new Portfolio
            {
                AppUserId = appUser.Id,
                StockId = stock.Id
            };

            var result = await _portofolioRepo.CreateAsync(portfolioModel);
            if (result == null)
            {
                return StatusCode(500, "Could not create");
            }
            else
            {
                return Created();
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null)
            {
                return Unauthorized("User not found");
            }

            var userPortfolio = await _portofolioRepo.GetUserPortfolio(appUser);
            var filteredStock = userPortfolio.Where(x => x.Symbol.ToLower() == symbol.ToLower()).ToList();
            if (filteredStock.Count == 1)
            {
                await _portofolioRepo.DeleteAsync(appUser, symbol);
                return Ok();
            }
            else
            {
                return BadRequest("Stock does not exist in portfolio.");
            }
        }
    }
}