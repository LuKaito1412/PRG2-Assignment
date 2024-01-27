using System;
using System.Linq.Expressions;

/*
string date = "24/01/2024";
DateTime day = Convert.ToDateTime(date).Date;
if (day == DateTime.Now.Date)
    Console.WriteLine(day);
*/

//Option 1 (JieXin)

(List<string>, Dictionary<string, Customer>) readCustomerFile()
{
    List<string> displayCustomers = new List<string>();
    Dictionary<string, Customer> customers = new Dictionary<string, Customer>();
    using (StreamReader sr = new StreamReader("customers.csv"))
    {
        string line;
        string[] header = (sr.ReadLine()).Split(",");
        string printHeader = $"{header[0],-11}{header[1],-11}{header[2],-13}{header[3],-19}{header[4],-19}{header[5]}";
        displayCustomers.Add(printHeader);
        while ((line = sr.ReadLine()) != null)
        {
            string[] info = line.Split(',');
            Customer customer = new Customer(info[0], Convert.ToInt32(info[1]), Convert.ToDateTime(info[2]));
            PointCard pointCard = new PointCard(Convert.ToInt32(info[4]), Convert.ToInt32(info[5]));
            customer.PointCard = pointCard;
            customers.Add(info[1], customer);
            string printLine = "";
            printLine += customer.ToString();
            printLine += $"{info[3],-19}{info[4],-19}{info[5]}";
            displayCustomers.Add(printLine);
        }
    }
    return (displayCustomers, customers);
}

void ListCustomers(List<string> displayCustomers)
{
    foreach (string line in displayCustomers)
    { 
        Console.WriteLine(line);
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


// Option 3 (JieXin)

void NewCutomerRegister()
{
    int idNo = 0;
    DateTime dob = new DateTime();

    Console.Write("Enter name: ");
    string name = Console.ReadLine();

    bool isValid = false;
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

// Option 4 (JieXin)

bool idValidation(string input, Dictionary<string, Customer> customers)
{
    bool isValid = false;
    foreach (KeyValuePair<string, Customer> kvp in customers)
    {
        if (input == kvp.Key)
        {
            isValid = true;
            break;
        }
    }
    if (isValid == false)
    {
        Console.WriteLine("Invalid Member ID.");
    }
    return isValid;
}

bool flavourValidation(string input, Dictionary<string, int> flavoursDict)
{
    bool isValid = false;
    foreach (KeyValuePair<string, int> kvp in flavoursDict)
    {
        if (input.ToLower() == kvp.Key.ToLower())
        {
            isValid = true;
            break;
        }
    }
    if (isValid == false)
    {
        Console.WriteLine("Invalid flavour.");
    }
    return isValid;
}

bool toppingValidation(string input)
{
    string[] toppings = { "sprinkles", "mochi", "sago", "oreos" };
    bool isValid = false;
    foreach (string topping in toppings)
    {
        if (topping == input.ToLower())
        { 
            isValid = true;
            break;
        }
    }
    if (isValid == false)
    {
        Console.WriteLine("Invalid topping.");
    }
    return isValid;
}

void CreateCustomerOrder(int orderNo, Dictionary<string, Customer> customers, Dictionary<string, int> flavoursDict)
{
    string userId = null;
    bool idValid = false;
    while (idValid == false)
    {
        Console.Write("Enter member ID to select customer: ");
        userId = Console.ReadLine();
        idValid = idValidation(userId, customers);
    }
    Customer orderCustomer = null;
    foreach (KeyValuePair<string, Customer> kvp in customers)
    {
        if (kvp.Key == userId)
        {
            orderCustomer = kvp.Value;
        }
    }

    Order newOrder = new Order(orderNo, DateTime.Now);
    orderCustomer.CurrentOrder = newOrder;

    while (true)
    {
        string option = null;
        bool optionValid = false;
        while (optionValid == false)
        {
            Console.Write("Cup, cone or waffle? ");
            option = Console.ReadLine();
            if (option.ToLower() == "cup" || option.ToLower() == "cone" || option.ToLower() == "waffle")
            {
                optionValid = true;
            }
            else
                Console.WriteLine("Invalid option.");
        }

        int scoop = 0;
        bool isValid = false;
        bool scoopValid = false;
        while (isValid == false)
        {
            try
            {
                while (scoopValid == false)
                {
                    Console.Write("Single, double or triple scoop(s)? Enter in number: ");
                    scoop = Convert.ToInt32(Console.ReadLine());
                    if (scoop == 1 || scoop == 2 || scoop == 3)
                    {
                        scoopValid = true;
                    }
                    else
                        Console.WriteLine("Please enter 1, 2 or 3.");
                    isValid = true;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Please enter in numbers (1, 2 or 3).");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Maximum scoops is 3.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid scoop amount.");
            }
        }

        Console.WriteLine("Regular flavours: Vanilla, Chocolate, Strawberry");
        Console.WriteLine("Premium flavours (+$2 per scoop): Durian, Ube, Sea Salt");
        string flavour = null;
        List<string> flavoursOrdered = new List<string>();
        for (int i = 1; i <= scoop; i++)
        {
            bool flavourValid = false;
            while (flavourValid == false)
            {
                Console.Write("Flavour {0}: ", i);
                flavour = Console.ReadLine();
                flavourValid = flavourValidation(flavour, flavoursDict);
            }
            flavoursOrdered.Add(flavour);
        }

        double price;
        int quantity = 0;
        bool prem = false;
        List<Flavour> orderFlavList = new List<Flavour>();
        foreach (string orderedFlavour in flavoursOrdered)
        {
            foreach (string flav in flavoursOrdered)
            {
                if (orderedFlavour == flav)
                {
                    // If dublicate found, quantity = 2. If not, quantity = 1.
                    quantity += 1;
                }
            }
            foreach (KeyValuePair<string, int> kvp in flavoursDict)
            {
                if (kvp.Key == orderedFlavour)
                {
                    price = kvp.Value;
                    if (price == 2)
                    {
                        prem = true;
                    }
                    break;
                }
            }

            // Make sure repeated ordered flavour doesnt get added twice
            foreach (Flavour addedFlav in orderFlavList)
            {
                if (addedFlav.Type == orderedFlavour)
                {
                    break;
                }
                else 
                {
                    Flavour orderFlav = new Flavour(orderedFlavour, prem, quantity);
                    orderFlavList.Add(orderFlav);
                }
            }
        }

        // Toppings
        List<Topping> orderToppList = new List<Topping>();
        Console.WriteLine("Toppings (+$1 each): Sprinkles, Mochi, Sago, Oreos");
        Console.WriteLine("You can have 0-4 toppings each ice cream.");
        
        isValid = false;
        int toppingNo = 0;
        while (isValid == false)
        {
            try
            {
                bool numberValid = false;
                while (numberValid == false)
                {
                    Console.Write("How many topping(s) do you want: ");
                    toppingNo = Convert.ToInt32(Console.ReadLine());
                    if (toppingNo >= 0 && toppingNo <= 4)
                    {
                        numberValid = true;
                    }
                    else
                        Console.WriteLine("Please enter 0, 1, 2, 3 or 4.");
                }
                isValid = true;
            }
            catch (FormatException)
            {
                Console.WriteLine("Please enter in numbers (0, 1, 2, 3 or 4).");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Maximum toppings is 4.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid topping amount.");
            }
        }

        string topping = null;
        for (int i = 1; i <= toppingNo; i++)
        {
            bool toppingValid = false;
            while (toppingValid == false)
            {
                Console.Write("Topping {0}: ", i);
                topping = Console.ReadLine();
                toppingValid = toppingValidation(topping);
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

        Console.WriteLine(orderCustomer.CurrentOrder.ToString());

        Console.Write("Do you wanna order another ice cream [Y/N]? ");
        string cont = Console.ReadLine();
        if (cont == "N")
            break;  
    }
    Console.WriteLine("Order has been made successfully.");
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

int orderNo = 1;
/*
Queue<Order> regularQueue = new Queue<Order>();
Queue<Order> goldQueue = new Queue<Order>();
*/
while (true)
{
    (List<string> displayCustomers, Dictionary<string, Customer> customers) = readCustomerFile();
    Console.Write("Enter option: ");
    string option = Console.ReadLine();
    if (option == "1")
        ListCustomers(displayCustomers);
    if (option == "2")
    {
        List<Order> orders = ReadFromFile();
        DisplayAllOrders(orders);
    }
    if (option == "3")
        NewCutomerRegister();
    if (option == "4")
    {
        ListCustomers(displayCustomers);
        CreateCustomerOrder(orderNo, customers, flavoursDict);
        orderNo += 1;
    }
}


