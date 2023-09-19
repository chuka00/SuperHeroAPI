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
        public async Task<ActionResult<List<SuperHeroes>>> GetAllSuperHeroes()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConn"));
            IEnumerable<SuperHeroes> heroes = await SelectAllHeroes(connection);
            return Ok(heroes);
        }

        

        [HttpGet("{heroId}")]
        public async Task<ActionResult<SuperHeroes>> GetSuperHero(int heroId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConn"));
            var hero = await connection.QueryFirstAsync<SuperHeroes>("select * from superheroes where id = @Id", 
                new { Id = heroId} );
            return Ok(hero);
        }
        

        [HttpPost]
        public async Task<ActionResult<List<SuperHeroes>>> CreateSuperHero(SuperHeroes hero)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConn"));
            await connection.ExecuteAsync("insert into superheroes(name, firstname, lastname, place) values (@Name, @FirstName, @LastName, @Place)", hero);
            return Ok(await SelectAllHeroes(connection));
        }

        [HttpPut]
        public async Task<ActionResult<List<SuperHeroes>>> UpdateSuperHero(SuperHeroes hero)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConn"));
            await connection.ExecuteAsync("update superheroes set name = @Name, firstname = @FirstName, lastname = @LastName, place = @Place where id = @Id", hero);
            return Ok(await SelectAllHeroes(connection));
        }

        [HttpDelete("{heroId}")]
        public async Task<ActionResult<List<SuperHeroes>>> DeleteSuperHero(int heroId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConn"));
            await connection.ExecuteAsync("delete from superheroes where id = @Id", new {Id = heroId});
            return Ok(await SelectAllHeroes(connection));
        }



        private static async Task<IEnumerable<SuperHeroes>> SelectAllHeroes(SqlConnection connection)
        {
            return await connection.QueryAsync<SuperHeroes>("select * from superheroes");
        }

    }
}
