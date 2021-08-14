using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebAPI.Models
{
    public enum TransactionType
    {
        Deposit = 'D',
        Withdraw = 'W',
        Transfer = 'T',
        [Display(Name = "Service Charge")]
        ServiceCharge = 'S',
        BillPay = 'B',
    }

    public class Transaction
    {
        [Required, Display(Name = "ID")]
        public int TransactionID { get; set; }

        [Required, Display(Name = "Type")]
        public TransactionType TransactionType { get; set; }

        [Required, ForeignKey("Account"), Display(Name = "Acc No.")]
        public int AccountNumber { get; set; }
        public virtual Account Account { get; set; }

        [ForeignKey("DestinationAccount"), Display(Name = "Destination Acc No.")]
        public int? DestinationAccountNumber { get; set; }
        public virtual Account DestinationAccount { get; set; }

        [Required, DataType(DataType.Currency), Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [StringLength(30)]
        public string Comment { get; set; }
        
        [Required, DataType(DataType.DateTime), Display(Name = "Time")]
        public DateTime TransactionTimeUtc { get; set; }
    }
}

