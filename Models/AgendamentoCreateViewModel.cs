using System.ComponentModel.DataAnnotations;

public class AgendamentoCreateViewModel
{
    [Required]
    [Display(Name = "Cliente")]
    public int ClienteId { get; set; }

    [Display(Name = "Veículo")]
    public string VeiculoId { get; set; } 
    [Required]
    [Display(Name = "Serviço")]
    public int ServicosId { get; set; }

    [Required]
    [Display(Name = "Data")]
    public DateTime Data { get; set; }

    // Campos do cadastro pra veiculo novo
    [Display(Name = "Modelo")]
    public string NovoVeiculoModelo { get; set; }

    [Display(Name = "Placa")]
    public string NovoVeiculoPlaca { get; set; }

    [Display(Name = "Marca")]
    public string NovoVeiculoMarca { get; set; }

    [Display(Name = "Cor")]
    public string NovoVeiculoCor { get; set; }

    [Display(Name = "Ano")]
    public int? NovoVeiculoAno { get; set; }
}