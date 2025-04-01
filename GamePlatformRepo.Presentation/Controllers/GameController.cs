using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;
using GamePlatformRepo.Models;
using GamePlatformRepo.Repository;
using GamePlatformRepo.Responses;
using Microsoft.AspNetCore.Mvc;

namespace GamePlatformRepo.Controllers
{
    [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(SystemErrorResponse))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestResponse))]
    [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(NotFoundResponse))]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public class GameController : Controller
    {

        private readonly IValidator<Game> gameValidator;
        private readonly IGameRepository gameRepository;

        public GameController(IGameRepository gameRepository, IValidator<Game> gameValidator)
        {
            this.gameRepository = gameRepository;
            this.gameValidator = gameValidator;
        }

        [HttpPost]
        [Route("CreateGame")]
        public async Task<ActionResult> CreateGame([FromForm]Game game) {
            var validationResult = await this.gameValidator.ValidateAsync(game);

            if(validationResult.IsValid == false) {
                base.TempData["validation_errors"] = JsonSerializer.Serialize(validationResult.Errors.Select(error => {
                    return new ValidationResponse (
                        error.PropertyName,
                        error.ErrorMessage
                    );
                }));
                
                return base.RedirectToAction("Create");
            }
            var id = await gameRepository.Create(game);

            return base.RedirectToAction(nameof(GetGames), "Game");
            
        }

        [HttpGet]
        [Route("Create")]
        public ActionResult Create(){
            if(base.TempData.TryGetValue("validation_errors", out object? validationErrorsObject)) {
                if(validationErrorsObject is string validationErrorsJson) {
                    var validationErrors = JsonSerializer.Deserialize<IEnumerable<ValidationResponse>>(validationErrorsJson);

                    base.ViewData["validation_errors"] = validationErrors;
                }
            }
            return base.View();
        }


        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> UpdateGame([FromBody] Game game)
        {
            if (game == null)
            {
                return NotFound();
            }

            var validationResult = await this.gameValidator.ValidateAsync(game);

            if (!validationResult.IsValid)
            {
                TempData["validation_errors"] = JsonSerializer.Serialize(
                    validationResult.Errors.Select(error => new ValidationResponse(
                        error.PropertyName,
                        error.ErrorMessage
                    )));
            }

            await gameRepository.Update(game);

            return Ok();
        }



        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<ActionResult> DeleteGame(int id) {
            var foundGame = gameRepository.Get(id);
            if(foundGame == null) {
                return base.BadRequest();
            }
            await gameRepository.Delete(id);
            return base.Ok();
        }


        [HttpGet]
        [Route("[controller]/{id}")]
        public async Task<ActionResult<Game>> GetGameById(int id) {
            if(base.TempData.TryGetValue("validation_errors", out object? validationErrorsObject)) {
                if(validationErrorsObject is string validationErrorsJson) {
                    var validationErrors = JsonSerializer.Deserialize<IEnumerable<ValidationResponse>>(validationErrorsJson);

                    base.ViewData["validation_errors"] = validationErrors;
                }
            }
            var foundGame = await gameRepository.Get(id);
            var statusCode = foundGame != null 
                ? HttpStatusCode.OK 
                : HttpStatusCode.NotFound;

            base.HttpContext.Response.StatusCode = (int)statusCode;

            if(foundGame == null) {
                return base.NotFound();
            }

            return base.View(viewName: "Game", model: foundGame);
        }
        [HttpGet]
        [Route("Games")]
        [ProducesResponseType(200, Type = typeof(List<Game>))]
        public async Task<ActionResult<List<Game>>> GetGames() {
            var foundGames = await gameRepository.GetAll();
            var statusCode = foundGames != null 
                ? HttpStatusCode.OK 
                : HttpStatusCode.NotFound;

            base.HttpContext.Response.StatusCode = (int)statusCode;

            if(foundGames == null) {
                return base.NotFound();
            }

            return base.View(viewName: "Games", model: foundGames);
        }
        [HttpGet]
        [Route("/")]
        public ActionResult Index() {
            return base.View();
        }

    }
}