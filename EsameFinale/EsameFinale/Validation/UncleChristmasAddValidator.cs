using EsameFinale.DataAccess;
using EsameFinale.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EsameFinale.Validation
{
    public class UncleChristmasAddValidator
    {
        public bool Result { get; set; }
        public string Message { get; set; }

        public async Task ValidateUncleChristmas(UncleChristmas model, ChristmasDbContext db) {
            var isNameNull = model.Name == null;
            var isNameEmpty = model.Name == "";
            var isNameDuplicated = await db.UncleChristmas.AnyAsync(u => u.Name == model.Name);
            if (isNameNull || isNameEmpty) {
                Message = "Error: You have to insert uncle christmas's name.";
                Result = false;
            } else if (isNameDuplicated) {
                Message = "Error: This name is already used.";
                Result = false;
            } else {
                Result = true;
            }
        }
    }
}
