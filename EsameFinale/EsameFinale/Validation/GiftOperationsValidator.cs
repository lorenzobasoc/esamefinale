using Microsoft.EntityFrameworkCore;
using SantaClausCrm.DataAccess;
using SantaClausCrm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsameFinale.Validation
{
    public class GiftOperationsValidator
    {
        public bool Result { get; set; }
        public string Message { get; private set; }

        public async Task ValidateGiftOperation(GiftOperation model, ChristmasDbContext db) {
            Result = model.OperationId switch {
                1 => await ValidateOp1(model, db),
                2 => await ValidateOp2(model, db),
                3 => await ValidateOp3(model, db),
                _ => false,
            };
        }

        private async Task<bool> ValidateOp1(GiftOperation model, ChristmasDbContext db) {
            var isOp1Duplicate = await db.GiftOperations.AnyAsync(o => o.OperationId == model.OperationId && o.GiftId == model.GiftId);
            if (isOp1Duplicate) {
                Message = "Error: This gift is already built.";
            }
            return !isOp1Duplicate;
        }

        private async Task<bool> ValidateOp2(GiftOperation model, ChristmasDbContext db) {
            var isThereOp1 = await db.GiftOperations.AnyAsync(o => o.OperationId == 1 && o.GiftId == model.GiftId);
            var isOp2Duplicate = await db.GiftOperations.AnyAsync(o => o.OperationId == model.OperationId && o.GiftId == model.GiftId);
            if (isThereOp1 && !isOp2Duplicate) {
                return true;
            } else {
                Message = "Error: This gift is not built yet or is altready packed.";
                return false;
            }
        }

        private async Task<bool> ValidateOp3(GiftOperation model, ChristmasDbContext db) {
            var isThereOp1 = await db.GiftOperations.AnyAsync(o => o.OperationId == 1 && o.GiftId == model.GiftId);
            var isThereOp2 = await db.GiftOperations.AnyAsync(o => o.OperationId == 2 && o.GiftId == model.GiftId);
            var isOp3Duplicate = await db.GiftOperations.AnyAsync(o => o.OperationId == model.OperationId && o.GiftId == model.GiftId);
            if (isThereOp1 && isThereOp2 && !isOp3Duplicate) {
                return true;
            } else {
                Message = "Error: You have to build and package the gitf before to put it in the sled, or maybe this gift is already on it.";
                return false;
            }
        }
    }
}
