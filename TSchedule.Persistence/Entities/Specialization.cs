using System.ComponentModel.DataAnnotations;

namespace TSchedule.Persistence.Entities
{
    class Specialization
    {
        [Key] public string Code { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false)]
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false)]
        [StringLength(20, MinimumLength = 3)]
        public string Reduction { get; set; } = string.Empty;
    }
}
