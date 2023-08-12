using MaxWorld.Data.Exercises;
using MaxWorld.Web.Models.Exercises;

namespace MaxWorld.Web.Services
{
    public class ExerciseService : BaseService
    {
        public ExerciseService(BaseServiceArgument baseServiceArgument) : base(baseServiceArgument) { }

        public Task<IEnumerable<ExerciseListDataModel>> GetExercisesListAsync()
        {
            return Repository.QueryAsync<ExerciseListDataModel>($"""
                SELECT [{nameof(Exercise.ExerciseId)}] AS "{nameof(ExerciseListDataModel.ExerciseId)}",
                       [{nameof(Exercise.Name)}] AS "{nameof(ExerciseListDataModel.Name)}",
                       [{nameof(Exercise.Description)}] AS "{nameof(ExerciseListDataModel.Description)}"
                FROM [{nameof(Exercise)}]
                """);
        }

        public Task<bool> ExerciseNameAvaliableAsync(string name, Guid? exerciseId = null)
        {
            return Repository.QueryFirstAsync<bool>($"""
                IF EXISTS (
                    SELECT 1 FROM [Exercise] 
                    WHERE [Name] = @name
                    {(exerciseId.HasValue ? $"AND [{nameof(Exercise.ExerciseId)}] != @exerciseId" : "")}
                )
                    SELECT 0
                ELSE
                    SELECT 1
                """, new { name, exerciseId });
        }

        public Task CreateExerciseAsync(Exercise model)
        {
            return Repository.ExecuteAsync($"""
                INSERT INTO [{nameof(Exercise)}] 
                ([{nameof(Exercise.ExerciseId)}], [{nameof(Exercise.Name)}], [{nameof(Exercise.Description)}], [{nameof(Exercise.Field1Name)}],
                 [{nameof(Exercise.Field1Unit)}], [{nameof(Exercise.Field2Name)}], [{nameof(Exercise.Field2Unit)}], [{nameof(Exercise.ShouldTimeFieldsAsTotal)}])
                VALUES
                (@{nameof(Exercise.ExerciseId)}, @{nameof(Exercise.Name)}, @{nameof(Exercise.Description)}, @{nameof(Exercise.Field1Name)},
                 @{nameof(Exercise.Field1Unit)}, @{nameof(Exercise.Field2Name)}, @{nameof(Exercise.Field2Unit)}, @{nameof(Exercise.ShouldTimeFieldsAsTotal)})
                """, model);
        }

        public Task<Exercise> GetExerciseAsync(Guid exerciseId)
        {
            return Repository.QueryFirstOrDefaultAsync<Exercise>($"""
                SELECT * FROM [{nameof(Exercise)}]
                WHERE [{nameof(Exercise.ExerciseId)}] = @exerciseId
                """, new { exerciseId });
        }

        public Task UpdateExerciseAsync(Exercise exercise)
        {
            return Repository.ExecuteAsync($"""
                UPDATE [{nameof(Exercise)}]
                SET [{nameof(Exercise.Name)}] = @{nameof(Exercise.Name)},
                    [{nameof(Exercise.Description)}] = @{nameof(Exercise.Description)},
                    [{nameof(Exercise.Field1Name)}] = @{nameof(Exercise.Field1Name)},
                    [{nameof(Exercise.Field1Unit)}] = @{nameof(Exercise.Field1Unit)},
                    [{nameof(Exercise.Field2Name)}] = @{nameof(Exercise.Field2Name)},
                    [{nameof(Exercise.Field2Unit)}] = @{nameof(Exercise.Field2Unit)},
                    [{nameof(Exercise.ShouldTimeFieldsAsTotal)}] = @{nameof(Exercise.ShouldTimeFieldsAsTotal)}
                WHERE [{nameof(Exercise.ExerciseId)}] = @{nameof(Exercise.ExerciseId)}
                """, exercise);
        }

        public Task DeleteExerciseAsync(Guid exerciseId)
        {
            return Repository.ExecuteAsync($"""
                DELETE FROM [{nameof(Exercise)}]
                WHERE [{nameof(Exercise.ExerciseId)}] = @exerciseId
                """, new { exerciseId });
        }
    }
}
