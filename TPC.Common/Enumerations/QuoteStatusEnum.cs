using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPC.Common.Enumerations
{
    public enum QuoteStatusEnum
    {
        Open = 1,
        InActive = 2,
        Web = 3,
        HoldRepresentative = 4,
        HoldCredit = 5,
        HoldProfileIncomplete = 6,
        Transferred = 7,
        Shipped = 8,
        CallTagIssued = 9,
        Invoiced = 10,
        Sent = 11,
        HoldPO = 13,
        Hold2previews = 15,
        Holdstickrate = 16,
        HoldAudit = 17,
        CTPending = 18,
        CTStatus = 19
    }
}
