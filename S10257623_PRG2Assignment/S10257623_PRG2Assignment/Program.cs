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
void CreateCustomerOrder(int orderNo)
{
    Dictionary<string, Customer> customers = new Dictionary<string, Customer>();
    using (StreamReader sr = new StreamReader("customers.csv"))
    {
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

    // Flavours
    Dictionary<string, int> flavoursDict = new Dictionary<string, int>();
    using (StreamReader sr = new StreamReader("flavours.csv"))
    {
        string line;
        sr.ReadLine();
        while ((line = sr.ReadLine()) != null)
        {
            string[] info = line.Split(',');
            flavoursDict.Add(info[0], Convert.ToInt32(info[1]));
        }
    }

    Console.Write("Enter name: ");
    string name = Console.ReadLine();
    Customer orderCustomer = null;
    foreach (KeyValuePair<string, Customer> kvp in customers)
    {
        if (kvp.Key == name)
        {
            orderCustomer = kvp.Value;
            Console.WriteLine(orderCustomer.Name);
        }
    }

    Order newOrder = new Order(orderNo, DateTime.Now);

    while (true)
    {

        Console.Write("Cup, cone or waffle? ");
        string option = Console.ReadLine();

        Console.Write("Single, double or triple scoop(s)? Enter in number: ");
        int scoop = Convert.ToInt32(Console.ReadLine());



        Console.WriteLine("Regular flavours: Vanila, Chocolate, Strawberry");
        Console.WriteLine("Premium flavours (+$2 per scoop): Durian, Ube, Sea Salt");
        List<string> flavoursOrdered = new List<string>();
        for (int i = 1; i < scoop+1; i++)
        {
            Console.Write("Flavour {0}: ", i);
            string flavour = Console.ReadLine();
            flavoursOrdered.Add(flavour);
        }

        double price;
        int quantity = 0;
        bool prem = false;
        List<Flavour> orderFlavList = new List<Flavour>();
        foreach (string flavour in flavoursOrdered)
        {
            foreach (string flav in flavoursOrdered)
            {
                if (flavour == flav)
                {
                    // If dublicate found, quantity = 2. If not, quantity = 1.
                    quantity += 1;
                }
            }
            foreach (KeyValuePair<string, int> kvp in flavoursDict)
            {
                if (kvp.Key == flavour)
                {
                    price = kvp.Value;
                    if (price == 2)
                    {
                        prem = true;
                    }
                    break;
                }
            }
            Flavour orderFlav = new Flavour(flavour, prem, quantity);
            orderFlavList.Add(orderFlav);
            //flavoursOrdered.RemoveAll(flavour);
        }

        // Toppings
        string[] toppings = { "sprinkles", "mochi", "sago", "oreos" };
        List<Topping> orderToppList = new List<Topping>();
        Console.Write("Toppings (+$1 each): Sprinkles, Mochi, Sago, Oreos");
        Console.WriteLine("You can have 0-4 toppings each ice cream. Enter 'end' to stop adding flavours.");
        for (int i = 1; i < 5; i++)
        {
            Console.Write("Topping {0}: ", i);
            string topping = Console.ReadLine();
            if (topping == "end")
            {
                break;
            }
            Topping orderTopp = new Topping(topping);
            orderToppList.Add(orderTopp);
        }

        IceCream orderIc;
        option = option.ToLower();
        if (option == "cup")
            orderIc = new Cup(option, scoop, orderFlavList, orderToppList);
        else if (option == "cone")
        {
            Console.Write("Do you want to upgrade your cone to a chocolate-dipped cone (+$2) [Y/N]? ");
            string dip = Console.ReadLine();
            bool dipped = false;
            if (dip == "Y")
                dipped = true;
            orderIc = new Cone(option, scoop, orderFlavList, orderToppList, dipped);
        }
        else if (option == "waffle")
        {
            Console.Write("Choose a waffle flavour:");
            Console.Write("Original (free)");
            Console.Write("Red velvet, Charcoal, Pandan (+$3)");
            string waffleFlav = Console.ReadLine();
            orderIc = new Waffle(option, scoop, orderFlavList, orderToppList, waffleFlav);
        }
        
        orderCustomer.CurrentOrder = newOrder;
        orderCustomer.OrderHistory.Add(newOrder);
        Console.Write("Do you wanna order another ice cream [Y/N]? ");
        string cont = Console.ReadLine();
        if (cont == "N")
            break;  
    }
    Console.WriteLine("Order has been made successfully.");
}


int orderNo = 1;
while (true)
{
    Console.Write("Enter option: ");
    string option = Console.ReadLine();
    if (option == "1")
        ListCustomers();
    if (option == "3")
        NewCutomerRegister();
    if (option == "4")
    {
        CreateCustomerOrder(orderNo);
        orderNo += 1;
    }
}


