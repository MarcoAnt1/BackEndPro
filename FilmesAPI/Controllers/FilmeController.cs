using AutoMapper;
using FilmesAPI.Business;
using FilmesAPI.Data;
using FilmesAPI.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesAPI.Models;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    private readonly IFilmeBusiness _business;
    private readonly IMapper _mapper;

    public FilmeController(IFilmeBusiness business, IMapper mapper)
    {
        _business = business;
        _mapper = mapper;
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
        var filme = _business.AdicionarFilme(filmeDto);
        return CreatedAtAction(nameof(RecuperaFilmePorId), new { id = filme.Id }, filme);
    }

    [HttpGet]
    public IEnumerable<ReadFilmeDto> RecuperaFilmes([FromQuery] int skip = 0, int take = 10)
    {
        return _business.RecuperaFilmes(skip, take);
    }

    [HttpGet("{id}")]
    public IActionResult RecuperaFilmePorId(int id)
    {
        var filmeDto = _business.ValidaFilme(id);
        if (filmeDto == null)
        {
            return NotFound();
        }

        return Ok(filmeDto);
    }

    [HttpPut("{id}")]
    public IActionResult AtualizaFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
    {
        var filme = _business.ValidaFilme(id);
        if (filme == null)
        {
            return NotFound();
        }

        _business.AtualizaFilme(filmeDto, filme);

        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult AtualizaFilmeParcial(int id, [FromBody] JsonPatchDocument<UpdateFilmeDto> patch)
    {
        var filme = _business.ValidaFilme(id);
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

        _business.AtualizaFilme(filmeParaAtualizar, filme);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeletaFilme(int id)
    {
        var filme = _business.ValidaFilme(id);
        if (filme == null)
        {
            return NotFound();
        }

        _business.DeletaFilme(filme);

        return NoContent();
    }
}