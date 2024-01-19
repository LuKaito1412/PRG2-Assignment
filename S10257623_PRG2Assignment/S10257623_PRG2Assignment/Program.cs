// See https://aka.ms/new-console-template for more information

//option 1 
static void ListCustomers()
{
    string[] lines = File.ReadAllLines("customers.csv");
    for (int i = 1; i < lines.Length; i++)
    {
        string[] infoLines = lines[i].Split(',');
        Customer customer = new Customer(infoLines[0], Convert.ToInt32(infoLines[1]), Convert.ToDateTime(infoLines[2]));
        Console.WriteLine(customer.ToString());
    }
}

ListCustomers();


