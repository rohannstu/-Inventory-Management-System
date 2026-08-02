using InventoryManagementSystem.Domain.ValueObjects;

var sku = Sku.Create("wh-1001");
Console.WriteLine(sku); // WH-1001

var price1 = Money.Create(19.99m);
var price2 = Money.Create(19.99m);
Console.WriteLine(price1 == price2); // True — value equality, no Id involved

try
{
    Money.Create(-5m);
}
catch (ArgumentException ex)
{
    Console.WriteLine(ex.Message); // Amount cannot be negative. (Parameter 'amount')
}