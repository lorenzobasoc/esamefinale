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
            if (model.Name == null || model.Name == "") {
                throw new InvalidOperationException("The uncle christmas's name is required.");
            } else {
                db.Add(model);
                await db.SaveChangesAsync();
                return new UncleChristmasAddResultDto { NewId = model.Id };
            }
        }
    }
}
