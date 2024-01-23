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

    Console.Write("Cup, cone or waffle? ");
    string option = Console.ReadLine();
    
    Console.Write("Single, double or triple scoop(s)? Enter in number: ");
    int scoop = Convert.ToInt32(Console.ReadLine());

    using (StreamReader sr = new StreamReader("flavours.csv"))
    {
        Dictionary<string, int> flavoursDict = new Dictionary<string, int>();
        string line;
        sr.ReadLine();
        while ((line = sr.ReadLine()) != null)
        {
            string[] info = line.Split(',');
            flavoursDict.Add(info[0], Convert.ToInt32(info[1]));
        }
    }

    Console.WriteLine("Regular flavours: Vanila, Chocolate, Strawberry");
    Console.WriteLine("Premium flavours (+$2 per scoop): Durian, Ube, Sea Salt");
    Console.WriteLine("You can have 0-4 flavours each ice cream. Enter 'end' to stop adding flavours.");
    List<Flavour> flavours = new List<Flavour>();
    for (int i = 1; i < 5; i++)
    {
        Console.Write("Flavour {0}: ", i);
        string flavour = Console.ReadLine();
        if (flavour == "end")
        {
            break;
        }
        //flavours.Add(flavour);
    }
   
    Console.Write("Toppings (+$1 each): Sprinkles, Mochi, Sago, Oreos");
    Console.WriteLine("You can have 0-3 toppings each ice cream. Enter 'end' to stop adding flavours.");
    for (int i = 1; i < 4; i++)
    {
        Console.Write("Topping {0}: ", i);
        string topping = Console.ReadLine();
        if (topping == "end")
        {
            break;
        }
    }
    Order newOrder = new Order();
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


