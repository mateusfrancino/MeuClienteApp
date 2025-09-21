namespace MeuClienteApp.Models;

public class Customer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Address { get; set; } = string.Empty;
}
