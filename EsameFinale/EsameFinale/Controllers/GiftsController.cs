using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using EsameFinale.DataAccess;
using EsameFinale.Dtos;
using EsameFinale.Models;
using System;

namespace EsameFinale.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class GiftsController : ControllerBase
    {
        private readonly ILogger<GiftsController> _logger;
        private readonly IDbContextFactory<ChristmasDbContext> _dbFactory;

        public GiftsController(
            ILogger<GiftsController> logger,
            IDbContextFactory<ChristmasDbContext> dbFactory) {
            _logger = logger;
            _dbFactory = dbFactory;
        }

        [HttpPost]
        public async Task<GiftAddResultDto> Add(GiftAddDto dto) {
            using var db = _dbFactory.CreateDbContext();
            var model = new Gift {
                Product = dto.Product,
            };
            if (model.Product == null || model.Product == "") {
                var exMessage = "Gift's name is required.";
                _logger.LogError(new InvalidOperationException(), exMessage);
                throw new InvalidOperationException(exMessage);
            } else {
                db.Add(model);
                await db.SaveChangesAsync();
                return new GiftAddResultDto { NewId =  model.Id };
            }
        }
    }
}
