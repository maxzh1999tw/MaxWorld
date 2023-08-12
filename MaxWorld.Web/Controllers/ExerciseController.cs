using MaxWorld.Data.Exercises;
using MaxWorld.Web.Filters;
using MaxWorld.Web.Models.Exercises;
using MaxWorld.Web.Services;
using MaxWorld.Web.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace MaxWorld.Web.Controllers
{
    [CustomAuthorize]
    public class ExerciseController : BaseController
    {
        readonly ExerciseService _exerciseService;

        public ExerciseController(BaseControllerArgument baseControllerArgument,
            ExerciseService exerciseService) : base(baseControllerArgument)
        {
            _exerciseService = exerciseService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        [ApiExceptionFilter]
        [CustomAuthorize(isApi: true)]
        public async Task<IActionResult> GetExercisesList()
        {
            var exercisesList = await _exerciseService.GetExercisesListAsync();
            return ApiSuccess(exercisesList);
        }

        [HttpPost]
        [ApiExceptionFilter]
        [CustomAuthorize(isApi: true)]
        public async Task<IActionResult> Create(ExerciseEditModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return ApiFailed(InvalidModelState);
            }

            if (! await _exerciseService.ExerciseNameAvaliableAsync(createModel.Name))
            {
                return ApiFailed("NameUsed");
            }

            var dbModel = ObjectMapper.MapTo<Exercise>(createModel);
            dbModel.ExerciseId = Guid.NewGuid();
            dbModel.CreatorId = SessionUserInfo.UserId;
            await _exerciseService.CreateExerciseAsync(dbModel);

            return ApiSuccess();
        }

        [ApiExceptionFilter]
        [CustomAuthorize(isApi: true)]
        public async Task<IActionResult> GetExerciseEditModel(Guid id)
        {
            var exercise = await _exerciseService.GetExerciseAsync(id);
            if (exercise == null)
            {
                return ApiFailed("NotFound");
            }

            var editModel = ObjectMapper.MapTo<ExerciseEditModel>(exercise);
            return ApiSuccess(editModel);
        }

        [ApiExceptionFilter]
        [CustomAuthorize(isApi: true)]
        [HttpPost]
        public async Task<IActionResult> Update(ExerciseEditModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return ApiFailed(InvalidModelState);
            }

            if (!await _exerciseService.ExerciseNameAvaliableAsync(updateModel.Name, updateModel.ExerciseId))
            {
                return ApiFailed("NameUsed");
            }

            var dbModel = ObjectMapper.MapTo<Exercise>(updateModel);
            await _exerciseService.UpdateExerciseAsync(dbModel);

            return ApiSuccess();
        }

        [ApiExceptionFilter]
        [CustomAuthorize(isApi: true)]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _exerciseService.DeleteExerciseAsync(id);
            return ApiSuccess();
        }
    }
}
