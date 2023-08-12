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
        private readonly ExerciseService _exerciseService;

        public ExerciseController(BaseControllerArgument baseControllerArgument,
            ExerciseService exerciseService) : base(baseControllerArgument)
        {
            _exerciseService = exerciseService;
        }

        #region === 回傳 View ===

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

        #endregion

        /// <summary>
        /// 取得運動項目管理的列表頁資訊
        /// </summary>
        [ApiExceptionFilter]
        [CustomAuthorize(isApi: true)]
        public async Task<IActionResult> GetExercisesList()
        {
            var exercisesList = await _exerciseService.GetExercisesListAsync();
            return ApiSuccess(exercisesList);
        }

        /// <summary>
        /// 新增運動項目
        /// </summary>
        [HttpPost]
        [ApiExceptionFilter]
        [CustomAuthorize(isApi: true)]
        public async Task<IActionResult> Create(ExerciseEditModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return ApiFailed(InvalidModelState);
            }

            if (!await _exerciseService.ExerciseNameAvaliableAsync(createModel.Name))
            {
                return ApiFailed("NameUsed");
            }

            var dbModel = ObjectMapper.MapTo<Exercise>(createModel);
            dbModel.ExerciseId = Guid.NewGuid();
            dbModel.CreatorId = SessionUserInfo.UserId;
            await _exerciseService.CreateExerciseAsync(dbModel);

            return ApiSuccess();
        }

        /// <summary>
        /// 取得運動項目資料
        /// </summary>
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

        /// <summary>
        /// 更新運動項目資料
        /// </summary>
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

        /// <summary>
        /// 刪除運動項目
        /// </summary>
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
