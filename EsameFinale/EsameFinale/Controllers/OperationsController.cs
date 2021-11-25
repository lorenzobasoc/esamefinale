using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using EsameFinale.DataAccess;
using EsameFinale.Dtos;
using EsameFinale.Models;
using System;
using EsameFinale.Validation;
using System.Collections.Generic;

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
                _logger.LogError(new InvalidOperationException(), validator.Message);
                throw new InvalidOperationException(validator.Message);
            }
            db.Dispose();
        }

        [HttpGet]
        public async Task<List<string>> Get() {
            using var db = _dbFactory.CreateDbContext();
            var operatonsList = new List<string>();
            var operations = await db.GiftOperations.ToListAsync();
            foreach (var o in operations) {
                var giftName = await GetGiftName(o.GiftId);
                var operationTypeName = await GetOperationTypeName(o.OperationId);
                var elfName = await GetElfName(o.ElfId);
                if (o.UncleChristmasId is null) {
                    operatonsList.Add($"Regalo: {giftName}, Operazione: {operationTypeName}, Elfo: {elfName}");
                } else {
                    var uncleName = await GetUncleName(o.UncleChristmasId);
                    operatonsList.Add($"Regalo: {giftName}, Operazione: {operationTypeName} di {uncleName}, Elfo: {elfName}");
                }
            }
            return operatonsList;
        }

        private async Task<string> GetUncleName(int? uncleChristmasId) {
            using var db = _dbFactory.CreateDbContext();
            var uncle = await db.UncleChristmas.FindAsync(uncleChristmasId);
            return uncle.Name;
        }

        private async Task<string> GetElfName(int elfId) {
            using var db = _dbFactory.CreateDbContext();
            var elf = await db.Elves.FindAsync(elfId);
            return elf.NickName;
        }

        private async Task<string> GetOperationTypeName(int operationId) {
            using var db = _dbFactory.CreateDbContext();
            var op = await db.Operations.FindAsync(operationId);
            return op.Name;
        }

        private async Task<string> GetGiftName(int giftId) {
            using var db = _dbFactory.CreateDbContext();
            var gift = await db.Gifts.FindAsync(giftId);
            return gift.Product;
        }
    }
}
