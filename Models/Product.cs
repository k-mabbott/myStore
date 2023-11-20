
#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace myStore.Models;


public class Product
{

    [Key]
    public int ProductId { get; set; }

    [Required]
    [Display(Name ="Product Name")]
    public string Name { get; set; }

    [Required]
    public byte[] Image { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    [Display(Name ="Price")]

    public float Price { get; set; }

        // -------------- Nav and FK's

    [Required]
    public int UserId { get; set; }

    public User? Creator { get; set; }
}

