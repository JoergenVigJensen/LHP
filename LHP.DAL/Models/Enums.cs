using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHP.DAL.Models
{

    public enum InvoiceType
    {

        DefualtInvoice,
        Creditnote,
        CustomInvoice
    }

    public enum InvoiceLineType
    {
        [Description("Leje")]
        Rent,
        [Description("Depositum/Deposit")]
        Deposit,
        [Description("Kredit/Credit")]
        Credit,
        [Description("Andet/See note")]
        Custom
    }
    
    public enum RoomerActivityType
    {
        Created,
        EnterRoom,
        Pay,
        LeaveRoom,
        CheckOut,
        Refund
    }

    public enum RoomActivityType
    {
        RoomerEnter,
        RoomerLeave,
        Activated,
        Deactivated
    }
 }
