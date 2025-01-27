using Store.G04.core.Entities;
using Store.G04.core.Entities.OdrerEntities;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.core.Services.Contract
{
    public interface IPaymentService
    {
        Task<CustomerBasket> CreateOrUpdatePaymentIntentIdAsync(string basketId);
        Task<Order> UpdatePaymentIntentForSucceedOrFailed(string paymentIntentId, bool flag);
    }
}
