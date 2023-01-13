using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Dtos;
public class UpdateFilmeDto
{
    [Required]
    public string Titulo { get; set; } = string.Empty;
    [Required]
    [StringLength(50)]
    public string Genero { get; set; } = string.Empty;
    [Required]
    [Range(10, 1000)]
    public int Duracao { get; set; }
}
