using System.ComponentModel.DataAnnotations.Schema;

namespace VerstaTest.Domain;

[Table("orders")]
public class Order
{
    [Column("id")]
    public int Id { get; set; }

    [Column("sender_city")]
    public string SenderCity { get; set; } = null!;

    [Column("sender_address")]
    public string SenderAddress { get; set; } = null!;

    [Column("recipient_city")]
    public string RecipientCity { get; set; } = null!;

    [Column("recipient_address")]
    public string RecipientAddress { get; set; } = null!;

    [Column("weight")]
    public double Weight { get; set; }

    [Column("date_cargo")]
    public DateTime DateCargo { get; set; }

    [Column("customer_id")]
    public int CustomerId { get; set; }

    public Customer Customer { get; set; } = null!;
}