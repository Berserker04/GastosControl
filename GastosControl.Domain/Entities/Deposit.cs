using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GastosControl.Domain.Entities
{
    public class Deposit
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public int MonetaryFundId { get; set; }
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Debe ingresar un monto positivo.")]
        public decimal Amount { get; set; }
    }
}
