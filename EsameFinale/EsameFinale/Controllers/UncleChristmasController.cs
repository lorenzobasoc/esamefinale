using EsameFinale.DataAccess;
using EsameFinale.Dtos;
using EsameFinale.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EsameFinale.Validation;

namespace EsameFinale.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UncleChristmasController : ControllerBase
    {
        private readonly ILogger<OperationsController> _logger;
        private readonly IDbContextFactory<ChristmasDbContext> _dbFactory;

        public UncleChristmasController(
            ILogger<OperationsController> logger,
            IDbContextFactory<ChristmasDbContext> dbFactory) {
            _logger = logger;
            _dbFactory = dbFactory;
        }

        [HttpPost]
        public async Task<UncleChristmasAddResultDto> Add(UncleChristmasAddDto uncleChristmas) {
            using var db = _dbFactory.CreateDbContext();
            var model = new UncleChristmas {
                Name = uncleChristmas.Name,
            };
            var validator = new UncleChristmasAddValidator();
            await validator.ValidateUncleChristmas(model, db);
            if (!validator.Result) {
                _logger.LogError(new InvalidOperationException(), validator.Message);
                throw new InvalidOperationException(validator.Message);
            } else {
                db.Add(model);
                await db.SaveChangesAsync();
                return new UncleChristmasAddResultDto { NewId = model.Id };
            }
        }

        [HttpGet]
        public async Task<List<string>> GetUncles() {
            using var db = _dbFactory.CreateDbContext();
            var unclesGiftsList = new List<string>();
            var uncles = await db.UncleChristmas.ToListAsync();
            foreach (var u in uncles) {
                var uncleId = u.Id;
                var numGifts = await GetNumGiftsOfUncle(uncleId);
                unclesGiftsList.Add($"{u.Name} has got {numGifts} gifts in his sled.");
            }
            return unclesGiftsList;
        }

        private async Task<string> GetNumGiftsOfUncle(int uncleId) {
            using var db = _dbFactory.CreateDbContext();
            var count = await db.GiftOperations.CountAsync(o => o.UncleChristmasId == uncleId);
            return count.ToString();
        }
    }
}
