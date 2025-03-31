using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GamePlatformRepo.Exceptions;
using GamePlatformRepo.Models;
using GamePlatformRepo.Repository.Base;
using Microsoft.AspNetCore.Mvc;

namespace GamePlatformRepo.Controllers
{

    [Route("[controller]/[action]")]
    public class CommentController : Controller
    {
        private readonly ICommentRepository commentRepository;

        public CommentController(ICommentRepository commentRepository)
        {
            this.commentRepository = commentRepository;
        }

        [HttpPost]
        [Route("/[controller]/[action]/{id}")]
        public async Task<ActionResult> CreateComment(int id, [FromForm]Comment comment) {
            Console.WriteLine($"Game/{id}");
            comment.GameId = id;
            var linga = await commentRepository.Create(comment);
            return base.RedirectToAction($"Game/{id}");
        }



        [HttpPut]
        [ProducesResponseType(200)]
        public async Task<ActionResult> UpdateComment([FromBody]Comment comment) {

            if(comment == null) {
                return base.NotFound();
            }
            await commentRepository.Update(comment);

            return base.Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteComment(int id) {
            try {
                var validationException = new ValidationException();

                var foundUser = commentRepository.Get(id);
                if(foundUser == null) {
                    validationException.validationResponseItems.Add(new ValidationResponse("Id", $"Element does not exist!"));
                }
                await commentRepository.Delete(id);
                return base.Ok();
            }catch(ValidationException validationException) {
                return base.BadRequest(validationException.validationResponseItems);
            }
            
        }

    }
}