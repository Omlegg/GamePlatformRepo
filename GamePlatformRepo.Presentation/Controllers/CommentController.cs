using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;
using GamePlatformRepo.Exceptions;
using GamePlatformRepo.Models;
using GamePlatformRepo.Repository.Base;
using Microsoft.AspNetCore.Mvc;

namespace GamePlatformRepo.Controllers
{

    [Route("[controller]/[action]")]
    public class CommentController : Controller
    {
        
        private readonly IValidator<Comment> commentValidator;
        private readonly ICommentRepository commentRepository;

        public CommentController(ICommentRepository commentRepository, IValidator<Comment> commentValidator)
        {
            this.commentRepository = commentRepository;
            this.commentValidator = commentValidator;
        }

        [HttpPost]
        [Route("/[controller]/[action]/{id}")]
        public async Task<ActionResult> CreateComment(int id, [FromForm]Comment comment) {
            var validationResult = await this.commentValidator.ValidateAsync(comment);

            if(validationResult.IsValid == false) {
                base.TempData["validation_errors"] = JsonSerializer.Serialize(validationResult.Errors.Select(error => {
                    return new ValidationResponse (
                        error.PropertyName,
                        error.ErrorMessage
                    );
                }));
                return base.Redirect($"/Game/{id}");;
                
            }
            comment.GameId = id;
            var linga = await commentRepository.Create(comment);
            return base.Redirect($"/Game/{id}");
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
            var foundComment = commentRepository.Get(id);
            if(foundComment == null) {
                return base.BadRequest();
            }
            await commentRepository.Delete(id);
            return base.Ok();
        }

    }
}