using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HealthUp.Models
{
    public partial class Ginasio
    {
        [Key]
        public int Id { get; set; }
        public string NumAdmin { get; set; }
        [Required]
        [StringLength(30)]
        public string Nome { get; set; }
        [Required]
        [StringLength(200)]
        public string Endereco { get; set; }
        [Required]
        [StringLength(20)]
        public string Email { get; set; }
        [Required]
        [StringLength(13)]
        public string Telemovel { get; set; }
        [Required]
        [StringLength(200)]
        public string LocalizacaoGps { get; set; }

        [ForeignKey(nameof(NumAdmin))]
        [InverseProperty(nameof(Admin.Ginasio))]
        public virtual Admin NumAdminNavigation { get; set; }
    }
}
