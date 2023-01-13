using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Dtos;
using FilmesAPI.Models;

namespace FilmesAPI.Business;
public class FilmeBusiness : IFilmeBusiness
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;

    public FilmeBusiness(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public Filme AdicionarFilme(CreateFilmeDto filmeDto)
    {
        var filme = _mapper.Map<Filme>(filmeDto);
        _context.Filmes?.Add(filme);
        _context.SaveChanges();

        return filme;
    }

    public IEnumerable<ReadFilmeDto> RecuperaFilmes(int skip, int take)
    {
        var filme = _context.Filmes?.Distinct().Skip(skip).Take(take).OrderBy(f => f.Titulo);
        return _mapper.Map<List<ReadFilmeDto>>(filme);
    }

    public ReadFilmeDto RecuperaFilmePorId(int id)
    {
        var filme = _context.Filmes?.FirstOrDefault(f => f.Id == id);
        return _mapper.Map<ReadFilmeDto>(filme);
    }

    public void AtualizaFilme(UpdateFilmeDto filmeDto, Filme filme)
    {
        _mapper.Map(filmeDto, filme);
        _context.SaveChanges();
    }

    public void DeletaFilme(Filme filme)
    {
        _context.Remove(filme);
        _context.SaveChanges();
    }

    public Filme? ValidaFilme(int id)
    {
        var filme = _context.Filmes?.FirstOrDefault(f => f.Id == id);
        if (filme == null)
        {
            return null;
        }

        return filme;
    }
}