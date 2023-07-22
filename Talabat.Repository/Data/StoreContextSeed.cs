using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext dbContext)
        {
            if (!dbContext.DeliveryMethod.Any())
            {
                var MethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                var Methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(MethodsData);

                if(Methods.Count() > 0)
                {
                    foreach (var method in Methods)
                        await dbContext.Set<DeliveryMethod>().AddAsync(method);

                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
