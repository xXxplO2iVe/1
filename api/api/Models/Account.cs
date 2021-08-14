using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public enum AccountType
    {
        Checking = 1,
        Saving = 2
    }
    public class Account
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), Display(Name = "Account Number")]
        public int AccountNumber { get; set; }

        [Required, Display(Name = "Account Type")]
        public AccountType AccountType { get; set; }

        [Required, ForeignKey("Customer")]
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }

        [DataType(DataType.Currency), Column(TypeName = "money")]
        public decimal Balance { get; set; }
        [InverseProperty("Account")]
        public virtual List<Transaction> Transactions { get; set; }

        public virtual List<BillPay> BillPays { get; set; }
    }
}
