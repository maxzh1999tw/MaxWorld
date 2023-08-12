using System.ComponentModel.DataAnnotations;

namespace MaxWorld.Web.Models.Exercises
{
    public class ExerciseEditModel
    {
        public Guid ExerciseId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; } = string.Empty;

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Description { get; set; } = string.Empty;

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Field1Name { get; set; } = string.Empty;

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Field1Unit { get; set; } = string.Empty;

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Field2Name { get; set; } = string.Empty;

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Field2Unit { get; set; } = string.Empty;

        public bool ShouldTimeFieldsAsTotal { get; set; }
    }
}
