using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using EsameFinale.DataAccess;
using EsameFinale.Dtos;
using EsameFinale.Models;
using System;
using EsameFinale.Validation;

namespace EsameFinale.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class OperationsController : ControllerBase
    {
        private readonly ILogger<OperationsController> _logger;
        private readonly IDbContextFactory<ChristmasDbContext> _dbFactory;

        public OperationsController(
            ILogger<OperationsController> logger,
            IDbContextFactory<ChristmasDbContext> dbFactory) {
            _logger = logger;
            _dbFactory = dbFactory;
        }

        [HttpPost]
        public async Task Add(GiftOperationAddDto dto) {
            var db = _dbFactory.CreateDbContext();
            var model = new GiftOperation {
                OperationId = dto.OperationId,
                ElfId = dto.ElfId,
                GiftId = dto.GiftId,
                UncleChristmasId = dto.UncleChristmasId,
            };
            var validator = new GiftOperationsValidator();
            await validator.ValidateGiftOperation(model, db);
            if (validator.Result) {
                db.Add(model);
                await db.SaveChangesAsync();
            } else {
                throw new InvalidOperationException(validator.Message);
            }
            db.Dispose();
        }

        

    }
}
