using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlbumAPI.DAL;
using AlbumAPI.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlbumAPI.Controllers
{
    [Route("api/album")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumRepository repository;

        public AlbumController(IAlbumRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public IEnumerable<Model.Album> List()
        {
            var list = repository.ListAlbums();

            return list;

        }

        [HttpGet("{owner}")]
        public IEnumerable<Model.Album> ListByName(string owner)
        {
            var list = repository.ListAlbumsOfOwner(owner);
            return list;

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Model.Album> Create([FromBody] Model.Album value)
        {
            try
            {
                var created = repository.Create(value);
                return CreatedAtAction(nameof(GetDetails), new { name = created.Name, owner = created.Owner }, created);
            } catch (ArgumentException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpDelete("{owner}/{name}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(string owner, string name)
        {
            var deleted = repository.Delete(owner, name);
            if (deleted == null)
            {
                return NotFound();
            }
            else
            {
                return NoContent();
            }
        }

        

        [HttpGet("{owner}/{name}/details")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Model.AlbumDetails> GetDetails(string owner, string name)
        {
            var details = repository.FindDetailsByNameAndOwner(name, owner);
            if (details == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(details);
            }

        }

        [HttpPatch("{owner}/{name}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult AddPicture([FromBody] string link, string name, string owner)
        {
            var succes = repository.AddPicture(owner, name, link);
            if (!succes)
            {
                return NotFound();
            }
            else
            {
                return Ok();
            }
            
        }

        [HttpDelete("{owner}/{name}/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeletePicture(string owner, string name, string id)
        {
            var result = repository.DeletePictureFromAlbum(owner, name, id);
            if (result)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }


    }
    /*
     * Album create --
     * Album details --
     * Album delete --
     * Album delete picture --
     * Album add picture --
     */
}