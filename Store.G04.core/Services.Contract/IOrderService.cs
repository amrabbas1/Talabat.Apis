using Store.G04.core.Entities.OdrerEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.core.Services.Contract
{
    public interface IOrderService
    {
        Task<Order> GetOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress);
        Task<IEnumerable<Order>?> GetOrdersForSpecificUserAsync(string buyerEmail);
        Task<Order?> GetOrderByIdForSpecificUserAsync(string buyerEmail, int orderId);

    }
}
