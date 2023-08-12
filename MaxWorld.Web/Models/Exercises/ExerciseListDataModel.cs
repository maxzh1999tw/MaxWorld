namespace MaxWorld.Web.Models.Exercises
{
    public class ExerciseListDataModel
    {
        public Guid ExerciseId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
