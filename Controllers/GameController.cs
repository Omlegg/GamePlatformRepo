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
    [Route("[controller]/")]
    [ProducesResponseType(400, Type = typeof(List<ValidationResponse>))]
    [ProducesResponseType(500)]
    public class GameController : ControllerBase
    {
        private readonly IGameRepository gameRepository;

        public GameController(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(200)]
        public ActionResult CreateGame([FromBody]Game game) {
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

                var id = gameRepository.CreateGame(game);

                return base.Ok(id);
            }
            catch(ValidationException validationException) {
                return base.BadRequest(validationException.validationResponseItems);
            }
        }

        [HttpPut]
        [Route("[action]")]
        [ProducesResponseType(200)]
        public ActionResult UpdateGame([FromBody]Game game) {
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

                gameRepository.UpdateGame(game);

                return base.Ok();
            }
            catch(ValidationException validationException) {
                return base.BadRequest(validationException.validationResponseItems);
            }
        }

        [HttpDelete]
        [Route("[action]")]
        [ProducesResponseType(200)]
        public ActionResult DeleteGame([FromBody]int id) {
            try {
                var validationException = new ValidationException();

                var foundUser = gameRepository.GetGame(id);
                gameRepository.DeleteGame(id);
                if(foundUser == null) {
                    validationException.validationResponseItems.Add(new ValidationResponse("Id", $"Element does not exist!"));
                }
                return base.Ok();
            }catch(ValidationException validationException) {
                return base.BadRequest(validationException.validationResponseItems);
            }
            
        }


        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(200, Type = typeof(Game))]
        public ActionResult<Game> GetGameById(int id) {
            var foundGame = gameRepository.GetGame(id);
            var statusCode = foundGame != null 
                ? HttpStatusCode.OK 
                : HttpStatusCode.NotFound;

            base.HttpContext.Response.StatusCode = (int)statusCode;

            if(foundGame == null) {
                return base.NotFound();
            }

            return base.Ok(foundGame);
        }
    }
}