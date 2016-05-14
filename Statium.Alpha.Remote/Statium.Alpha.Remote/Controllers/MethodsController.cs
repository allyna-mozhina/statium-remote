using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Statium.Alpha.Remote.Data;
using Statium.Alpha.Remote.Data.Models;
using Statium.Alpha.Remote.Data.Repositories;

namespace Statium.Alpha.Remote.Controllers
{
    public class MethodsController : ApiController
    {
        private readonly IRepository<Method> _repository = new MethodDbRepository(); 

        // GET: api/Methods
        public IQueryable<Method> GetMethods()
        {
            return _repository.GetAll();
        }

        /*// GET: api/Methods/5
        [ResponseType(typeof(Method))]
        public async Task<IHttpActionResult> GetMethod(int id)
        {
            Method method = await _db.Methods.FindAsync(id);
            if (method == null)
            {
                return NotFound();
            }

            return Ok(method);
        }

        // PUT: api/Methods/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMethod(int id, Method method)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != method.Id)
            {
                return BadRequest();
            }

            _db.Entry(method).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MethodExists(id))
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

        // POST: api/Methods
        [ResponseType(typeof(Method))]
        public async Task<IHttpActionResult> PostMethod(Method method)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Methods.Add(method);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = method.Id }, method);
        }

        // DELETE: api/Methods/5
        [ResponseType(typeof(Method))]
        public async Task<IHttpActionResult> DeleteMethod(int id)
        {
            Method method = await _db.Methods.FindAsync(id);
            if (method == null)
            {
                return NotFound();
            }

            _db.Methods.Remove(method);
            await _db.SaveChangesAsync();

            return Ok(method);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MethodExists(int id)
        {
            return _db.Methods.Count(e => e.Id == id) > 0;
        }*/
    }
}