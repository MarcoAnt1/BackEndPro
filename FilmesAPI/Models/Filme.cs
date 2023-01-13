using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Models;

public class Filme
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]
    public string Titulo { get; set; } = string.Empty;
    [Required]
    [MaxLength(50)]
    public string Genero { get; set; } = string.Empty;
    [Required]
    [Range(10, 1000)]
    public int Duracao { get; set; }
}