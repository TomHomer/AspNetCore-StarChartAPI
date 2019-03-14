using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}",Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.Find(id);
            if(celestialObject == null)
            {
                return NotFound();
            }
            else
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == id).ToList();
                return Ok(celestialObject);
            }
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(e => e.Name == name).ToList();
            if (celestialObjects.Count == 0)
            {
                return NotFound();
            }
            else
            {
                foreach (var obj in celestialObjects)
                {
                    obj.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == obj.Id).ToList();
                }

                return Ok(celestialObjects);
            }
        }

        [HttpGet()]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();
            if (celestialObjects.Count == 0)
            {
                return NotFound();
            }
            else
            {
                foreach (var obj in celestialObjects)
                {
                    obj.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == obj.Id).ToList();
                }

                return Ok(celestialObjects);
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject obj)
        {
            _context.CelestialObjects.Add(obj);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = obj.Id }, obj);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject obj)
        {
            var celestialObject = _context.CelestialObjects.Find(id);
            if (celestialObject == null)
            {
                return NotFound();
            }
            else
            {
                celestialObject.Name = obj.Name;
                celestialObject.OrbitalPeriod = obj.OrbitalPeriod;
                celestialObject.OrbitedObjectId = obj.OrbitedObjectId;

                _context.CelestialObjects.Update(celestialObject);
                _context.SaveChanges();

                return NoContent();
            }
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var celestialObject = _context.CelestialObjects.Find(id);
            if (celestialObject == null)
            {
                return NotFound();
            }
            else
            {
                celestialObject.Name = name;

                _context.CelestialObjects.Update(celestialObject);
                _context.SaveChanges();

                return NoContent();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var celestialObjects = _context.CelestialObjects.Where(e => e.Id == id || e.OrbitedObjectId == id).ToList();
            if (celestialObjects.Count == 0)
            {
                return NotFound();
            }
            else
            {
                _context.CelestialObjects.RemoveRange(celestialObjects);
                _context.SaveChanges();

                return NoContent();
            }
        }
    }
}
