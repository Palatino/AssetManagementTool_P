using AssetManagementTool_API.Data.Repositories.IRepositories;
using AssetManagementTool_Common.Models;
using AssetManagementTool_Common.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AssetManagementTool_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AssetAttachmentController : ControllerBase
    {
        private readonly IAttachmentRepository _attachmentRepo;
        public AssetAttachmentController(IAttachmentRepository attachmentRepo)
        {
            _attachmentRepo = attachmentRepo;
        }

        /// <summary>
        /// Create new image attachment
        /// </summary>
        /// <param name="id"></param>
        /// <response code="201">Create new image</response>
        /// <response code="400">If invalid request</response>
        [HttpPost]
        [Route("/api/images")]
        [Authorize(Policy = Authorization.Policy.AttachmentCreate)]
        [ProducesResponseType(typeof(ImageAttachmentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateImage([FromBody]NewImageAttachmentDTO newImageDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new ErrorModel()
                    {
                        ErrorMessage = $"Provide valid new image object",
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Title = "Error"

                    });
                }

                newImageDTO.AddedBy = User.Claims
                    .SingleOrDefault(c => c.Type == "https://AssetManagementTool.com email").Value;
                var createdImage = await _attachmentRepo.CreateImageAttachment(newImageDTO);
                return Created(createdImage.Id.ToString(), createdImage);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorModel()
                {
                    ErrorMessage = $"Internal server error: {e.Message}",
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Title = "Error"

                });
            }

        }

        /// <summary>
        /// Delete existing asset
        /// </summary>
        /// <response code="204">Image deleted</response>
        /// <response code="400">If invalid request</response>
        /// <response code="404">If asset not found request</response>
        [HttpDelete]
        [Route("/api/images/{id:int}")]
        [Authorize(Policy = Authorization.Policy.AttachmentDelete)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteImage(int? id)
        {
            try
            {
                if (!id.HasValue)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new ErrorModel()
                    {
                        ErrorMessage = $"Provide valid image id",
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Title = "Error"

                    });
                }


                var numberOfOperations = await _attachmentRepo.DeleteImageAttachment(id.Value);
                if (numberOfOperations == 0)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, new ErrorModel()
                    {
                        ErrorMessage = $"No entry with that id found",
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Title = "Error"

                    });
                }
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorModel()
                {
                    ErrorMessage = $"Internal server error: {e.Message}",
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Title = "Error"

                });
            }

        }



        /// <summary>
        /// Create new file attachment
        /// </summary>
        /// <param name="id"></param>
        /// <response code="201">Create new file</response>
        /// <response code="400">If invalid request</response>
        [HttpPost]
        [Authorize(Policy = Authorization.Policy.AttachmentCreate)]
        [Route("/api/files")]
        [ProducesResponseType(typeof(FileAttachmentDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateFile([FromBody] NewFileAttachmentDTO newFileDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new ErrorModel()
                    {
                        ErrorMessage = $"Provide valid new file object",
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Title = "Error"

                    });
                }

                newFileDTO.AddedBy = User.Claims
                .SingleOrDefault(c => c.Type == "https://AssetManagementTool.com email").Value;
                var createdFile = await _attachmentRepo.CreateFileAttachment(newFileDTO);
                return Created(createdFile.Id.ToString(), createdFile);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorModel()
                {
                    ErrorMessage = $"Internal server error: {e.Message}",
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Title = "Error"

                });
            }

        }


        /// <summary>
        /// Get link to file including the access token
        /// </summary>
        /// <param name="id"></param>
        /// <response code="201">Create new file</response>
        /// <response code="400">If invalid request</response>
        [HttpGet]
        [Route("/api/files/{id:int}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFile(int? id)
        {
            try
            {
                if (!id.HasValue)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new ErrorModel()
                    {
                        ErrorMessage = $"Provide valid file id",
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Title = "Error"

                    });
                }


                var link = await _attachmentRepo.GetFileLink(id.Value);
                if (link is null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, new ErrorModel()
                    {
                        ErrorMessage = $"No file with that id found",
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Title = "Error"

                    });
                }
                return Ok(link);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorModel()
                {
                    ErrorMessage = $"Internal server error: {e.Message}",
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Title = "Error"

                });
            }
        }

        /// <summary>
        /// Delete existing asset
        /// </summary>
        /// <response code="204">Image deleted</response>
        /// <response code="400">If invalid request</response>
        /// <response code="404">If asset not found request</response>
        [HttpDelete]
        [Route("/api/files/{id:int}")]
        [Authorize(Policy = Authorization.Policy.AttachmentDelete)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteFile(int? id)
        {
            try
            {
                if (!id.HasValue)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new ErrorModel()
                    {
                        ErrorMessage = $"Provide valid file id",
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Title = "Error"

                    });
                }

                var numberOfOperations = await _attachmentRepo.DeleteFileAttachment(id.Value);
                if (numberOfOperations == 0)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, new ErrorModel()
                    {
                        ErrorMessage = $"No entry with that id found",
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Title = "Error"

                    });
                }
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorModel()
                {
                    ErrorMessage = $"Internal server error: {e.Message}",
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Title = "Error"

                });
            }

        }


        /// <summary>
        /// Create new comment
        /// </summary>
        /// <param name="id"></param>
        /// <response code="201">Create new comment</response>
        /// <response code="400">If invalid request</response>
        [HttpPost]
        [Route("/api/comments")]
        [Authorize(Policy = Authorization.Policy.AttachmentCreate)]
        [ProducesResponseType(typeof(ImageAttachmentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateComment([FromBody]NewCommentAttachmentDTO newCommentDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new ErrorModel()
                    {
                        ErrorMessage = $"Provide valid new file object",
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Title = "Error"

                    });
                }

                newCommentDTO.AddedBy = User.Claims
                .SingleOrDefault(c => c.Type == "https://AssetManagementTool.com email").Value;

                var createdComment = await _attachmentRepo.CreateCommentAttachment(newCommentDTO);
                return Created(createdComment.Id.ToString(), createdComment);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorModel()
                {
                    ErrorMessage = $"Internal server error: {e.Message}",
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Title = "Error"

                });
            }

        }

        /// <summary>
        /// Delete existing asset
        /// </summary>
        /// <response code="204">Image deleted</response>
        /// <response code="400">If invalid request</response>
        /// <response code="404">If asset not found request</response>
        [HttpDelete]
        [Route("/api/comments/{id:int}")]
        [Authorize(Policy = Authorization.Policy.AttachmentDelete)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteComment(int? id)
        {
            try
            {
                if (!id.HasValue)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new ErrorModel()
                    {
                        ErrorMessage = $"Provide valid comment id",
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Title = "Error"

                    });
                }

                var numberOfOperations = await _attachmentRepo.DeleteCommentAttachment(id.Value);
                if (numberOfOperations == 0)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, new ErrorModel()
                    {
                        ErrorMessage = $"No entry with that id found",
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Title = "Error"

                    });
                }
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorModel()
                {
                    ErrorMessage = $"Internal server error: {e.Message}",
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Title = "Error"

                });
            }

        }
    }
}
