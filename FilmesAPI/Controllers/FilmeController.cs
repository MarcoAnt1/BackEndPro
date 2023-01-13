using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesAPI.Models;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;

    public FilmeController(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    /// <summary>
    /// Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="filmeDto">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionarFilme([FromBody] CreateFilmeDto filmeDto)
    {
        var filme = _mapper.Map<Filme>(filmeDto);
        _context.Filmes?.Add(filme);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperaFilmePorId), new { id = filme.Id }, filme);
    }

    [HttpGet]
    public IEnumerable<ReadFilmeDto> RecuperaFilmes([FromQuery] int skip = 0, int take = 10)
    {
        return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes?.Distinct().Skip(skip).Take(take).OrderBy(f => f.Titulo))!;
    }

    [HttpGet("{id}")]
    public IActionResult RecuperaFilmePorId(int id)
    {
        var filme = _context.Filmes?.FirstOrDefault(f => f.Id == id);
        if (filme == null)
        {
            return NotFound();
        }

        var filmeDto = _mapper.Map<ReadFilmeDto>(filme);
        return Ok(filmeDto);
    }

    [HttpPut("{id}")]
    public IActionResult AtualizaFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
    {
        var filme = _context.Filmes?.FirstOrDefault(f => f.Id == id);
        if (filme == null)
        {
            return NotFound();
        }

        _mapper.Map(filmeDto, filme);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult AtualizaFilmeParcial(int id, [FromBody] JsonPatchDocument<UpdateFilmeDto> patch)
    {
        var filme = _context.Filmes?.FirstOrDefault(f => f.Id == id);
        if (filme == null)
        {
            return NotFound();
        }

        var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);
        patch.ApplyTo(filmeParaAtualizar, ModelState);

        if (!TryValidateModel(filmeParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(filmeParaAtualizar, filme);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeletaFilme(int id)
    {
        var filme = _context.Filmes?.FirstOrDefault(f => f.Id == id);
        if (filme == null)
        {
            return NotFound();
        }

        _context.Remove(filme);
        _context.SaveChanges();

        return NoContent();
    }
}