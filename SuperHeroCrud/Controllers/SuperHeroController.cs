using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperHeroCrud.Data;
using System.Data.SqlClient;

namespace SuperHeroCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly IConfiguration _config;

        public SuperHeroController(IConfiguration config)
        {
            _config = config;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> GetAllSuperHeroes()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConn"));
            var heroes = await connection.QueryAsync<SuperHero>("select * from superheroes");
            return Ok(heroes);
        }

        [HttpGet("{heroId}")]
        public async Task<ActionResult<SuperHero>> GetSuperHero(int heroId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConn"));
            var hero = await connection.QueryFirstAsync<SuperHero>("select * from superheroes where id = @Id", 
                new { Id = heroId} );
            return Ok(hero);
        }
    }
}
