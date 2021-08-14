using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public record Payee
    {
        [Display(Name = "Payee ID")]
        public int PayeeID { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required, StringLength(50)]
        public string Address { get; set; }

        [Required, StringLength(40)]
        public string Suburb { get; set; }

        [Required, StringLength(3), EnumDataType(typeof(State))]
        public State State { get; set; }

        [Required, StringLength(4), RegularExpression(@"\d{4}",
            ErrorMessage = "Error! Enter a valid 4 digit postcode.")]
        public string Postcode { get; set; }

        [Required, StringLength(14), RegularExpression(@"^\(0\d{1}\) \d{4} \d{4}", 
            ErrorMessage = "Error! Must be of the format: (0X) XXXX XXXX")]
        public string Phone { get; set; }
    }
}

