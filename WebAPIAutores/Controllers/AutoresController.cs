using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;
using WebAPIAutores.Filtros;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController: ControllerBase
    {
        private readonly AplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public AutoresController (AplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        [HttpGet("configuraciones")]
        public ActionResult<string> ObtenerConfiguraciones()
        {
            return configuration["ConnectionStrings:defaultConnection"];
        }

        [HttpGet] //api/autores
        public async Task<List<AutorDTO>> Get()//async Task<ActionResult<List<Autor>>>
        {
            var autores = await context.Autores.ToListAsync();
            return mapper.Map<List<AutorDTO>>(autores);
        }

        [HttpGet("{id:int}", Name = "ObtenerAutor")]//Api/autores/1
        public async Task<ActionResult<AutorDTOConLibros>> Get(int id)
        {

            var autor = await context.Autores
                .Include(autorDB => autorDB.AutoresLibros)
                .ThenInclude(autorLibroDB => autorLibroDB.Libro)
                .FirstOrDefaultAsync(autorBD => autorBD.id == id);

            if (autor == null)
            {
                return NotFound();
            }

            return mapper.Map<AutorDTOConLibros>(autor);

        }

        [HttpGet("{nombre}")]//Api/autores/nombre
        public async Task<ActionResult<List<AutorDTO>>> Get([FromRoute] string nombre)
        {

            var autores = await context.Autores.Where(autorBD => autorBD.name.Contains(nombre)).ToListAsync();

            return mapper.Map<List<AutorDTO>>(autores);
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacionDTO)
        {

            var existeAutorConElMismoNombre = await context.Autores.AnyAsync(x => x.name == autorCreacionDTO.name);

            if (existeAutorConElMismoNombre)
            {
                return BadRequest($"Ya existe un autor con el nombre {autorCreacionDTO.name} ");
            }

            var autor = mapper.Map<Autor>(autorCreacionDTO);

            context.Add(autor);
            await context.SaveChangesAsync();

            var autorDTO = mapper.Map<AutorDTO>(autor);
            return CreatedAtRoute("ObtenerAutor", new { id = autor.id }, autorDTO); 
        }

        [HttpPut("{id:int}")] //Api/autores/1... n
        public async Task<ActionResult> Put(AutorCreacionDTO autorCreacionDTO, int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.id == id);

            if (!existe)
            {
                return NotFound();
            }
            var autor = mapper.Map <Autor>(autorCreacionDTO);

            autor.id = id;
            context.Update(autor);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")] //APi/Autores/2
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Autor() { id= id });
            await context.SaveChangesAsync();
            return NoContent();
        }

            
    }
}
