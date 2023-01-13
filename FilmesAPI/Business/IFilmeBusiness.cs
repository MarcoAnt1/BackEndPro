using FilmesAPI.Dtos;
using FilmesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace FilmesAPI.Business;
public interface IFilmeBusiness
{
    Filme AdicionarFilme(CreateFilmeDto filmeDto);
    IEnumerable<ReadFilmeDto> RecuperaFilmes(int skip, int take);
    ReadFilmeDto RecuperaFilmePorId(int id);
    void AtualizaFilme(UpdateFilmeDto filmeDto, Filme filme);
    void DeletaFilme(Filme filme);
    Filme? ValidaFilme(int id);
}
