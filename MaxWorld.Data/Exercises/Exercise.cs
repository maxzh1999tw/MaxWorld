namespace MaxWorld.Data.Exercises
{
    public class Exercise
    {
        public Guid ExerciseId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Field1Name { get; set; } = string.Empty;

        public string Field1Unit { get; set; } = string.Empty;

        public string Field2Name { get; set; } = string.Empty;

        public string Field2Unit { get; set; } = string.Empty;

        public bool ShouldTimeFieldsAsTotal { get; set; }

        public Guid? CreatorId { get; set; }
    }
}
