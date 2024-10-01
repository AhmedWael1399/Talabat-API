using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities;

namespace TalabatCore.OrderAggregate
{
    public enum OrderStatus
    {
        [EnumMember( Value = "pending" )]
        pending,
        [EnumMember(Value = "Payment Received")]
        PaymentReceived, 
        [EnumMember(Value = "Payment Failed")]
        PaymentFailed
    }
}
