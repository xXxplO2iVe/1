using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Assignment_2.Models
{
    public enum State
    {
        VIC = 1,
        NSW = 2,
        ACT = 3,
        QLD = 4,
        NT = 5,
        WA = 6,
        SA = 7,
        TAS = 8
    }

    public class Customer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CustomerID { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [StringLength(11), RegularExpression(@"\d{11}",
            ErrorMessage = "Error! Enter a valid 11 digit TFN.")]
        public string TFN { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(40)]
        public string Suburb { get; set; }

        [EnumDataType(typeof(State),
            ErrorMessage = "Error! Enter a valid 2 or 3 letter State/Territory i.e: VIC.")]
        public State? State { get; set; }

        [StringLength(4), RegularExpression(@"\d{4}",
            ErrorMessage = "Error! Enter a valid 4 digit postcode.")]
        public string Postcode { get; set; }

        [StringLength(12), RegularExpression(@"^04\d{2} \d{3} \d{3}", 
            ErrorMessage = "Error! Must be of the format: 04XX XXX XXX")]
        public string Mobile { get; set; }

        public bool Locked { get; set; } = false;

        public virtual List<Account> Accounts { get; set; }

        public void Update(Customer customer)
        {
            Name = customer.Name;
            TFN = customer.TFN;
            Address = customer.Address;
            Suburb = customer.Suburb;
            State = customer.State;
            Postcode = customer.Postcode;
            Mobile = customer.Mobile;
        }
    }
}

