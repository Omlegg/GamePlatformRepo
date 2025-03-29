using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GamePlatformRepo.Exceptions;
using GamePlatformRepo.Models;
using GamePlatformRepo.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GamePlatformRepo.Controllers
{
    [ProducesResponseType(400, Type = typeof(List<ValidationResponse>))]
    [ProducesResponseType(500)]
    [ProducesResponseType(200)]
    public class GameController : Controller
    {
        private readonly IGameRepository gameRepository;

        public GameController(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }

        [HttpPost]
        [Route("CreateGame")]
        public async Task<ActionResult> CreateGame([FromForm]Game game) {
            try {
                var validationException = new ValidationException();

                if(string.IsNullOrWhiteSpace(game.Name)) {
                    validationException.validationResponseItems.Add(new ValidationResponse(nameof(game.Name), $"{nameof(game.Name)} can not be empty!"));
                }

                if(game.Price < 0) {
                    validationException.validationResponseItems.Add(new ValidationResponse(nameof(game.Price), $"{nameof(game.Price)} can not be less than 0!"));
                }

                if(game.Views < 0) {
                    validationException.validationResponseItems.Add(new ValidationResponse(nameof(game.Views), $"{nameof(game.Views)} can not be less than 0!"));
                }

                if(validationException.validationResponseItems.Any()) {
                    throw validationException;
                }

                var id = await gameRepository.Create(game);

                return base.RedirectToAction("Create");

            }
            catch(ValidationException validationException) {
                return base.BadRequest(validationException.validationResponseItems);
            }
        }

        [HttpGet]
        [Route("Create")]
        public ActionResult Create(){
            return base.View();
        }


        [HttpPut]
        [Route("[action]")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> UpdateGame([FromBody]Game game) {

            if(game == null) {
                return base.NotFound();
            }
            try {
                var validationException = new ValidationException();

                if(string.IsNullOrWhiteSpace(game.Name)) {
                    validationException.validationResponseItems.Add(new ValidationResponse(nameof(game.Name), $"{nameof(game.Name)} can not be empty!"));
                }

                if(game.Price < 0) {
                    validationException.validationResponseItems.Add(new ValidationResponse(nameof(game.Price), $"{nameof(game.Price)} can not be less than 0!"));
                }

                if(game.Views < 0) {
                    validationException.validationResponseItems.Add(new ValidationResponse(nameof(game.Views), $"{nameof(game.Views)} can not be less than 0!"));
                }

                if(validationException.validationResponseItems.Any()) {
                    throw validationException;
                }

                await gameRepository.Update(game);

                return base.Ok();
            }
            catch(ValidationException validationException) {
                return base.BadRequest(validationException.validationResponseItems);
            }
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> DeleteGame(int id) {
            try {
                var validationException = new ValidationException();

                var foundUser = gameRepository.Get(id);
                await gameRepository.Delete(id);
                if(foundUser == null) {
                    validationException.validationResponseItems.Add(new ValidationResponse("Id", $"Element does not exist!"));
                }
                return base.Ok();
            }catch(ValidationException validationException) {
                return base.BadRequest(validationException.validationResponseItems);
            }
            
        }


        [HttpGet]
        [Route("[controller]/{id}")]
        [ProducesResponseType(200, Type = typeof(Game))]
        public async Task<ActionResult<Game>> GetGameById(int id) {
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