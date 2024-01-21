using System;

// Remember add data validation

//Option 1 
void ListCustomers()
{
    using (StreamReader sr = new StreamReader("customers.csv"))
    {
        string line;
        string[] header = (sr.ReadLine()).Split(",");
        Console.WriteLine("{0,-9}{1,-11}{2}", header[0], header[1], header[2]);
        while ((line = sr.ReadLine()) != null)
        {
            string[] info = line.Split(',');
            Customer customer = new Customer(info[0], Convert.ToInt32(info[1]), Convert.ToDateTime(info[2]));
            Console.WriteLine(customer.ToString());
        }
    }
}

// Option 3
void NewCutomerRegister()
{
    Console.Write("Enter name: ");
    string name = Console.ReadLine();
    Console.Write("Enter id number: ");
    int idNo = Convert.ToInt32(Console.ReadLine());
    Console.Write("Enter date of birth (dd/mm/yyyy): ");
    DateTime dob = Convert.ToDateTime(Console.ReadLine());

    Customer newCustomer = new Customer(name, idNo, dob);
    PointCard newPc = new PointCard(0, 0);
    newCustomer.PointCard = newPc;

    using (StreamWriter sw = new StreamWriter("customers.csv", true))
    {
        string line = $"{name},{idNo},{dob.ToString("dd/MM/yyyy")},{"Ordinary"},{0},{0}";
        sw.WriteLine(line);
    }

    Console.WriteLine("Your membership has registered successfully.");
}

// Option 4
void CreateCustomerOrder()
{
    using (StreamReader sr = new StreamReader("customers.csv"))
    {
        Dictionary<string, Customer> customers = new Dictionary<string, Customer>();
        string line;
        string[] header = (sr.ReadLine()).Split(",");
        Console.WriteLine("{0}", header[0]);
        while ((line = sr.ReadLine()) != null)
        {
            string[] info = line.Split(',');
            Customer customer = new Customer(info[0], Convert.ToInt32(info[1]), Convert.ToDateTime(info[2]));
            PointCard pointCard = new PointCard(Convert.ToInt32(info[4]), Convert.ToInt32(info[5]));
            customer.PointCard = pointCard;
            customers.Add(info[0], customer);
            Console.WriteLine(info[0]);
        }
    }
    Console.Write("Enter name: ");
    string name = Console.ReadLine();
}


while (true)
{
    Console.Write("Enter option: ");
    string option = Console.ReadLine();
    if (option == "1")
        ListCustomers();
    if (option == "3")
        NewCutomerRegister();
    if (option == "4")
        CreateCustomerOrder();
}


