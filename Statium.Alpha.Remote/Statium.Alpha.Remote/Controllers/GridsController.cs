using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using Statium.Alpha.Remote.Data;
using Statium.Alpha.Remote.Data.Models;
using Statium.Alpha.Remote.Data.Repositories;

namespace Statium.Alpha.Remote.Controllers
{
    //[RoutePrefix("api/Grids")]
    public class GridsController : ApiController
    {
        private readonly IRepository<Grid> _repository = new GridHardcodeRepository();

        // GET: api/Grids
        //[Authorize]
        public IQueryable<Grid> GetGrids()
        {
            var userId = User.Identity.GetUserId<int>();
            return _repository.GetWhere(g => g.UserId == userId);
        }

        // POST: api/Grids
        [Authorize]
        [ResponseType(typeof(Grid))]
        public IHttpActionResult PostGrid(Grid grid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repository.Add(grid);

            return CreatedAtRoute("DefaultApi", new { id = grid.Id }, grid);
        }

        // POST: api/Grids/upload
        //[Authorize]
        [Route("api/Grids/upload")]
        public async Task<IHttpActionResult> PostGrid()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var targetDirectory = HttpContext.Current.Server.MapPath("~/App_Data/Grids");
            var provider = new MultipartFormDataStreamProvider(targetDirectory);

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                var grid = provider.FileData.FirstOrDefault();
                var gridId = provider.FormData.GetValues("id")?[0];

                if (grid != null & gridId != null)
                {
                    var originalFilename = grid.Headers.ContentDisposition.FileName;
                    File.Move(grid.LocalFileName, Path.Combine(targetDirectory, 
                                                               $"{gridId}{Path.GetExtension(originalFilename ?? ".csv")}"));
                }
                
                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
        }

#region ToBeImplemented
        /*// GET: api/Grids/5
        [ResponseType(typeof(Grid))]
        public async Task<IHttpActionResult> GetGrid(int id)
        {
            Grid grid = await db.Grids.FindAsync(id);
            if (grid == null)
            {
                return NotFound();
            }

            return Ok(grid);
        }

        // PUT: api/Grids/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutGrid(int id, Grid grid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != grid.Id)
            {
                return BadRequest();
            }

            db.Entry(grid).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GridExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        

        // DELETE: api/Grids/5
        [ResponseType(typeof(Grid))]
        public async Task<IHttpActionResult> DeleteGrid(int id)
        {
            Grid grid = await db.Grids.FindAsync(id);
            if (grid == null)
            {
                return NotFound();
            }

            db.Grids.Remove(grid);
            await db.SaveChangesAsync();

            return Ok(grid);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GridExists(int id)
        {
            return db.Grids.Count(e => e.Id == id) > 0;
        }*/
#endregion
    }
}