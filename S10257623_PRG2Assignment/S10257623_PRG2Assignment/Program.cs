using System;
using System.Linq.Expressions;

//Option 1 
List<Customer> customerList = new List<Customer>();
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
            customerList.Add(customer);
            Console.WriteLine(customer.ToString());
        }
    }
}

// Option 2 (Julian) 

static bool isPremium(string flavourName)
{
    string[] lines = File.ReadAllLines("flavours.csv");
    for (int i = 1; i < lines.Length; i++)
    {
        string[] premLines = lines[i].Split(",");
        if (premLines[0] == flavourName)
        {
            if (Convert.ToInt32(premLines[1]) == 2)
            {
                return true;
            }


        }

    }
    return false;
}
static List<Order> ReadFromFile()
{
    List<Order> orders = new List<Order>();
    string[] lines = File.ReadAllLines("orders.csv");
    for (int i = 1; i < lines.Length; i++)
    {
        string[] infoLines = lines[i].Split(',');
        int orderId = Convert.ToInt32(infoLines[0]);
        int memId = Convert.ToInt32(infoLines[1]);
        DateTime timeRec = Convert.ToDateTime(infoLines[2]);
        DateTime? timeFul = Convert.ToDateTime(infoLines[3]);
        string option = infoLines[4];
        int scoops = Convert.ToInt32(infoLines[5]);

        List<Flavour> flavours = new List<Flavour>
        {
        new Flavour(infoLines[8], isPremium(infoLines[8]), 1),
        new Flavour(infoLines[9], isPremium(infoLines[9]), 1),
        new Flavour(infoLines[10], isPremium(infoLines[10]), 1)
        };

        List<Topping> toppings = new List<Topping>
        {
        new Topping(infoLines[11]),
        new Topping(infoLines[12]),
        new Topping(infoLines[13]),
        new Topping(infoLines[14])
        };

        IceCream iceCream;
        if (option == "Cup")
        {
            iceCream = new Cup(option, scoops, flavours, toppings);
            Order order = new Order(orderId, timeRec);
            orders.Add(order);
            order.AddIceCream(iceCream);
        }
        else if (option == "Cone")
        {
            if (infoLines[6] != null)
            {
                bool dipped;
                if (infoLines[6] == "TRUE")
                {
                    dipped = true;
                    iceCream = new Cone(option, scoops, flavours, toppings, dipped);
                    Order order = new Order(orderId, timeRec);
                    orders.Add(order);
                    order.AddIceCream(iceCream);
                }
                else
                {
                    dipped = false;
                    iceCream = new Cone(option, scoops, flavours, toppings, dipped);
                    Order order = new Order(orderId, timeRec);
                    orders.Add(order);
                    order.AddIceCream(iceCream);
                }
            }

        }
        else if (option == "Waffle")
        {
            if (infoLines[7] != null)
            {
                string waffleFlavour = infoLines[7];
                iceCream = new Waffle(option, scoops, flavours, toppings, waffleFlavour);
                Order order = new Order(orderId, timeRec);
                orders.Add(order);
                order.AddIceCream(iceCream);
            }

        }
    }
    return orders;

}
static void DisplayAllOrders(List<Order> orders)
{
    foreach (var order in orders)
    {
        Console.WriteLine($"Order ID: {order.Id}");
        Console.WriteLine($"Time Received: {order.TimeReceived}");

        //Can Possibly make it neater by printing flavour once if there is only one flavour instead of 3 times
        // Same goes to toppings. Any suggestions?

        foreach (var iceCream in order.IceCreamList)
        {
            Console.WriteLine($"Ice Cream Option: {iceCream.Option}, Scoops: {iceCream.Scoops}");


            if (iceCream is Cone cone)
            {
                Console.WriteLine($"Dipped: {cone.Dipped}");
            }
            else if (iceCream is Waffle waffle)
            {
                Console.WriteLine($"Waffle Flavour: {waffle.WaffleFlavour}");
            }


            foreach (var flavour in iceCream.Flavours)
            {
                Console.WriteLine($"Flavour: {flavour.Type}, Premium: {flavour.Premium}");
            }


            foreach (var topping in iceCream.Toppings)
            {
                Console.WriteLine($"Topping: {topping.Type}");
            }

            Console.WriteLine($"Total Price: ${iceCream.CalculatePrice()}\n");
        }
    }
}





List<Order> orders = ReadFromFile();
DisplayAllOrders(orders);


//----------------------

//-----------------------




// Option 3
void NewCutomerRegister()
{
    string name = null;
    int idNo = 0;
    DateTime dob = new DateTime();
    
    bool isValid = false;
    while (isValid == false)
    {
        try
        {
            Console.Write("Enter name: ");
            name = Console.ReadLine();
            isValid = true;
        }
        catch (Exception)
        {
            Console.WriteLine("Invalid name.");
        }
    }

    isValid = false;
    while (isValid == false)
    {
        try
        {
            Console.Write("Enter id number: ");
            idNo = Convert.ToInt32(Console.ReadLine());
            isValid = true;
        }
        catch (FormatException)
        {
            Console.WriteLine("Numbers only.");
        }
        catch (OverflowException)
        {
            Console.WriteLine("ID entered is too large.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Invalid ID.");
        }
    }

    isValid = false;
    while (isValid == false)
    {
        try
        {
            Console.Write("Enter date of birth (dd/mm/yyyy): ");
            dob = Convert.ToDateTime(Console.ReadLine());
            isValid = true;
        }
        catch (Exception)
        {
            Console.WriteLine("Invalid date.");
        }
    }

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

/*
bool isValid = false;
while (isValid == false)
{
    try
    {

    }
    catch (Exception)
    {
        Console.WriteLine("Invalid name.");
    }
}
*/

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

    bool isValid = false;
    while (isValid == false)
    {
        try
        {
            Console.Write("Enter name: ");
            string name = Console.ReadLine();
        }
        catch (Exception)
        {
            Console.WriteLine("Invalid name.");
        }
    }
    Customer orderCustomer = null;
    foreach (KeyValuePair<string, Customer> kvp in customers)
    {
        if (kvp.Key == name)
        {
            orderCustomer = kvp.Value;
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

        IceCream orderIc = null;
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

        newOrder.AddIceCream(orderIc);
        orderCustomer.CurrentOrder = newOrder;

        //if (orderCustomer.)

        Console.Write("Do you wanna order another ice cream [Y/N]? ");
        string cont = Console.ReadLine();
        if (cont == "N")
            break;  
    }
    Console.WriteLine("Order has been made successfully.");
}



int orderNo = 1;
/*
Queue<Order> regularQueue = new Queue<Order>();
Queue<Order> goldQueue = new Queue<Order>();
*/
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


