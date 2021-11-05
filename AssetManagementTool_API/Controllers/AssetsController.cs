using AssetManagementTool_API.Data.Repositories;
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
using System.Security.Claims;
using System.Threading.Tasks;

namespace AssetManagementTool_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetRepository _repo;
        public AssetsController(IAssetRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Retrieve asset by ID
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Returns asset with the matching Id</response>
        /// <response code="400">If the id is not valid</response>  
        /// <response code="404">If no entries with provided id</response>  
        [HttpGet]
        [Route("/api/assets/{id:int}")]
        [ProducesResponseType(typeof(AssetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int? id)
        {
            try
            {
                if (!id.HasValue)
                {
                    return BadRequest(new ErrorModel()
                    {
                        Title = "Error",
                        ErrorMessage = "Provide valid id",
                        StatusCode = (int)HttpStatusCode.BadRequest,
                    });
                }
                var assetInDB = await _repo.GetAsset(id.Value);

                if (assetInDB is null)
                {
                    return NotFound(new ErrorModel()
                    {
                        Title = "Error",
                        ErrorMessage = $"Asset with id {id.Value} not found",
                        StatusCode = (int)HttpStatusCode.NotFound,
                    });
                }

                return Ok(assetInDB);
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
        /// Retrieve all assets
        /// </summary>
        /// <response code="200">Returnsall assets basic info</response> 
        [HttpGet]
        [Route("/api/assets")]
        [ProducesResponseType(typeof(IEnumerable<AssetDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {

            try
            {

                var assetsInDB = await _repo.GetAssets();
                return Ok(assetsInDB);
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
        /// Retrieve the names of all the assets in the DB
        /// </summary>
        /// <response code="200">Returns asset with the matching Id</response>
        [HttpGet]
        [Route("/api/assets/names")]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNames()
        {
            try
            {

                var assetNames = await _repo.GetAssetNames();
                return Ok(assetNames);
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
        /// Create new asset
        /// </summary>
        /// <response code="201">Create new asset</response>
        /// <response code="400">If invalid request</response>
        [HttpPost]
        [Route("/api/assets")]
        [Authorize(Policy = Authorization.Policy.AssetCreate)]
        [RequestSizeLimit(100000000)]
        [DisableRequestSizeLimit]
        [ProducesResponseType(typeof(AssetDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] NewAssetDTO newAssetDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new ErrorModel()
                    {
                        ErrorMessage = $"Provide valid asset object",
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Title = "Error"

                    });
                }

                newAssetDTO.AddedBy = User.Claims
                    .SingleOrDefault(c => c.Type == "https://AssetManagementTool.com email").Value;
                var createdAsset = await _repo.CreateAsset(newAssetDTO);
                return Created(createdAsset.Id.ToString(), createdAsset);
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
        /// <response code="204">Asset deleted</response>
        /// <response code="400">If invalid request</response>
        /// <response code="404">If asset not found request</response>
        [HttpDelete]
        [Route("/api/assets/{id:int}")]
        [Authorize(Policy = Authorization.Policy.AssetDelete)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (!id.HasValue)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new ErrorModel()
                    {
                        ErrorMessage = $"Provide valid id",
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Title = "Error"

                    });
                }

                var numberOfOperations = await _repo.DeleteAsset(id.Value);
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
