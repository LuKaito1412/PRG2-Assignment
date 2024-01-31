using System;
using System.Linq.Expressions;

/*
string date = "24/01/2024";
DateTime day = Convert.ToDateTime(date).Date;
if (day == DateTime.Now.Date)
    Console.WriteLine(day);
*/

//Option 1 (JieXin)

// Reads customer.csv, prints out the information and returns a dictionary and list.
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
            customer.PointCard.Tier = info[3];
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

//Julian
List<Customer> customerList = new List<Customer>();
void ListCustomers(List<Customer> customerList)
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
            PointCard pointCard = new PointCard(Convert.ToInt32(info[4]), Convert.ToInt32(info[5]));
            customer.PointCard = pointCard;
            customer.PointCard.Tier = info[3];
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

Dictionary<int, List<Order>> ReadFromFile(List<Customer> customerList)
{
    List<Order> orders = new List<Order>();
    Dictionary<int, List<Order>> cusOrderDict = new Dictionary<int, List<Order>>();
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

        IceCream iceCream = null;

        if (option == "Cup")
        {
            iceCream = new Cup(option, scoops, flavours, toppings);
        }
        else if (option == "Cone")
        {
            if (infoLines[6] != null)
            {
                bool dipped = infoLines[6] == "TRUE";
                iceCream = new Cone(option, scoops, flavours, toppings, dipped);
            }
        }
        else if (option == "Waffle")
        {
            if (infoLines[7] != null)
            {
                string waffleFlavour = infoLines[7];
                iceCream = new Waffle(option, scoops, flavours, toppings, waffleFlavour);
            }
        }

        Order order = new Order(orderId, timeRec);

        // Find the customer associated with the memberId
        Customer customer = customerList.FirstOrDefault(c => c.MemberId == memId);

        if (customer != null)
        {
            customer.OrderHistory.Add(order);
            order.AddIceCream(iceCream);
            orders.Add(order);

            // Check if the memId is already in the dictionary
            if (cusOrderDict.ContainsKey(memId))
            {
                // If yes, add the order to the existing list of orders
                cusOrderDict[memId].Add(order);
            }
            else
            {
                // If no, create a new list and add the order to it
                List<Order> orderList = new List<Order> { order };
                cusOrderDict.Add(memId, orderList);
            }
        }
        else
        {
            Console.Write("Doesn't work");
        }
    }

    return cusOrderDict;
}


void DisplayAllOrders(Dictionary<int, List<Order>> cusOrderDict)
{
    foreach (var kvp in cusOrderDict)
    {
        int memId = kvp.Key;
        List<Order> orders = kvp.Value;

        Console.WriteLine($"Orders for MemberId: {memId}");

        foreach (Order order in orders)
        {
            Console.WriteLine(order.ToString());
            Console.WriteLine();

        }

        Console.WriteLine();
    }
}

//need add Queue




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
            while (idNo > 999999 || idNo < 100000)
            {
                Console.Write("Enter id number: ");
                idNo = Convert.ToInt32(Console.ReadLine());
                if (idNo > 999999 || idNo < 100000)
                {
                    Console.WriteLine("Please enter a number between 100000 - 999999 for member ID.");
                }
            }
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

bool inputValidation(string type, string input, string[] array)
{
    bool isValid = false;
    foreach (string item in array)
    {
        if (item == input.ToLower())
        {
            isValid = true;
            break;
        }
    }
    if (isValid == false)
    {
        Console.WriteLine($"Invalid {type}.");
    }
    return isValid;
}


Dictionary<Order, Customer> CreateCustomerOrder(int orderNo, Dictionary<string, Customer> customers, Dictionary<string, int> flavoursDict, Dictionary<Order, Customer> orderCustomerDict, Queue<Order> regularQueue, Queue<Order> goldQueue)
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

    if (orderCustomer.CurrentOrder != null)
        Console.WriteLine("Pay for your current order before ordering another one.");
    else
    {
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
                flavoursOrdered.Add(flavour.ToLower());
            }

            double price;
            bool prem = false;
            List<Flavour> orderFlavList = new List<Flavour>();
            foreach (string orderedFlavour in flavoursOrdered)
            {
                int quantity = 0;
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
                    if (kvp.Key.ToLower() == orderedFlavour)
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
                if (orderFlavList.Count() == 0)
                {
                    Flavour orderFlav = new Flavour(orderedFlavour, prem, quantity);
                    orderFlavList.Add(orderFlav);
                }
                else
                {
                    bool noDub = true;
                    foreach (Flavour addedFlav in orderFlavList)
                    {
                        if (addedFlav.Type == orderedFlavour)
                        {
                            noDub = false;
                            break;
                        }
                    }
                    if (noDub == true)
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
                string[] toppings = { "sprinkles", "mochi", "sago", "oreos" };
                while (toppingValid == false)
                {
                    Console.Write("Topping {0}: ", i);
                    topping = Console.ReadLine();
                    toppingValid = inputValidation("topping", topping, toppings);
                }
                Topping orderTopp = new Topping(topping.ToLower());
                orderToppList.Add(orderTopp);
            }

            isValid = false;
            IceCream orderIc = null;
            option = option.ToLower();
            if (option == "cup")
                orderIc = new Cup(option, scoop, orderFlavList, orderToppList);
            else if (option == "cone")
            {
                string dip = null;
                while (isValid == false)
                {
                    Console.Write("Do you want to upgrade your cone to a chocolate-dipped cone (+$2) [Y/N]? ");
                    dip = Console.ReadLine();
                    if (dip.ToLower() == "y" || dip.ToLower() == "n")
                    {
                        isValid = true;
                    }
                }
                bool dipped = false;
                if (dip.ToLower() == "y")
                    dipped = true;
                orderIc = new Cone(option, scoop, orderFlavList, orderToppList, dipped);
            }
            else if (option == "waffle")
            {
                string[] waffleFlavours = { "original", "red velvet", "charcoal", "pandan" };
                string waffleFlav = null;
                Console.WriteLine("Waffle flavours:");
                Console.WriteLine("Original (free)");
                Console.WriteLine("Red velvet, Charcoal, Pandan (+$3)");
                while (isValid == false)
                {
                    Console.WriteLine("Choose a waffle flavour:");
                    waffleFlav = Console.ReadLine();
                    isValid = inputValidation("waffle flavour", waffleFlav, waffleFlavours);
                }
                orderIc = new Waffle(option, scoop, orderFlavList, orderToppList, waffleFlav);
            }

            newOrder.AddIceCream(orderIc);

            string cont = null;
            isValid = false;
            while (isValid == false)
            {
                Console.Write("Do you wanna order another ice cream [Y/N]? ");
                cont = Console.ReadLine();
                if (cont.ToLower() == "y" || cont.ToLower() == "n")
                {
                    isValid = true;
                }
            }
            if (cont.ToLower() == "n")
                orderCustomerDict.Add(newOrder, orderCustomer);
            break;
        }

        //2,245718,27 / 10 / 2023 13:50,27 / 10 / 2023 13:59,Cone,2, FALSE,, Chocolate, Sea Salt,, Sprinkles, Mochi, Sago, Oreos
        foreach (IceCream iC in newOrder.IceCreamList)
        {
            string line = null;
            using (StreamWriter sw = new StreamWriter("customers.csv", true))
            {
                line += $"{orderNo},{orderCustomer.MemberId},{newOrder.TimeReceived},{newOrder.TimeFulfuilled},{iC.Option},{iC.Scoops},";
                if (iC.Option == "cone")
                {
                    Cone c = (Cone)iC;
                    line += $"{c.Dipped},,";
                }
                else if (iC.Option == "waffle")
                {
                    Waffle w = (Waffle)iC;
                    line += $",{w.WaffleFlavour},";
                }
                else
                {
                    line += ",,";
                }
                string flavours = null;
                int count = 0;
                foreach (Flavour f in iC.Flavours)
                {
                    flavours += $"{f},";
                    count += 1;
                }
                line += flavours;
                for (int i = 1; i <= (3 - count); i++)
                {
                    line += ",";
                }
                foreach (Topping t in iC.Toppings)
                { 
                    
                }    
            }
        }
        if (orderCustomer.PointCard.Tier == "Gold")
        {
            goldQueue.Enqueue(newOrder);
        }
        else
        {
            regularQueue.Enqueue(newOrder);
        }
        Console.WriteLine("Order has been made successfully.");
    }
    return orderCustomerDict;
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
//option 5

static void DisplayOrderDetails(Dictionary<int, List<Order>> cusOrderDict, List<Customer> customerList)
{
    Customer selectedCustomer = ObtainCustomer(customerList);

    if (selectedCustomer != null && cusOrderDict.ContainsKey(selectedCustomer.MemberId))
    {
        List<Order> orders = cusOrderDict[selectedCustomer.MemberId];

        Console.WriteLine($"Displaying order details for {selectedCustomer.Name} (MemberId: {selectedCustomer.MemberId})");

        foreach (Order order in orders)
        {
            Console.WriteLine(order.ToString());



            Console.WriteLine();
        }
    }
    else
    {
        Console.WriteLine("Customer not found or no orders available.");
    }
}



//Function to obtain the customer
static Customer? ObtainCustomer(List<Customer> customerList)
{
    while (true)
    {
        try
        {
            Console.Write("Enter Customer Name: ");
            string? cusName = Console.ReadLine();

            foreach (Customer c in customerList)
            {
                if (c.Name == cusName)
                {
                    //Customer found
                    return c;
                }
            }

            throw new InvalidOperationException("Customer not found. Please enter a valid customer name.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

//option 6
static int OrderDetailsMenu()
{
    Console.WriteLine("[1] Choose an existing Ice Cream object to modify.");
    Console.WriteLine("[2] Add an ENTIRELY new Ice Cream object to the order.");
    Console.WriteLine("[3] Choose an existing Ice Cream object to delete from the Order.");
    Console.WriteLine("Enter your option: ");
    int options = Convert.ToInt32(Console.ReadLine());
    return options;
}



static void ModifyOrderDetails(List<Customer> customerList, Dictionary<int, List<Order>> cusOrderDict)
{
    Customer selectedCustomer = ObtainCustomer(customerList);
    if (selectedCustomer != null)
    {
        // Display order history for the selected customer
        Console.WriteLine("Order History:");
        for (int i = 0; i < selectedCustomer.OrderHistory.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {selectedCustomer.OrderHistory[i]}");
        }

        Console.WriteLine("Enter order to modify: ");
        int orderIndex = Convert.ToInt32(Console.ReadLine());

        // Validate the order index
        if (orderIndex >= 1 && orderIndex <= selectedCustomer.OrderHistory.Count)
        {
            Order orderMod = selectedCustomer.OrderHistory[orderIndex - 1];

            Console.WriteLine("Enter option (Cup, Cone, Waffle): ");
            string opt = Console.ReadLine();

            if (opt == "Cup" || opt == "Cone" || opt == "Waffle")
            {
                // Instead of individually changing, clear list and create a new order
                orderMod.IceCreamList.Clear();

                // Add new ice cream based on the selected option
                IceCream newIceCream = CreateIceCream(opt);
                orderMod.AddIceCream(newIceCream);

                // Update cusOrderDict with the modified order list
                cusOrderDict[selectedCustomer.MemberId] = selectedCustomer.OrderHistory;

                Console.WriteLine("Order modified successfully.");
            }
            else
            {
                Console.WriteLine("Invalid ice cream option.");
            }
        }
        else
        {
            Console.WriteLine("Invalid order index.");
        }
    }
}


static IceCream CreateIceCream(string option)
{
    
    Console.WriteLine($"Enter Scoops for {option}: ");
    int scoops = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine($"Enter number of toppings for {option}: ");
    int topCount = Convert.ToInt32(Console.ReadLine());

    List<Flavour> flavours = new List<Flavour>();
    for (int x = 0; x < scoops; x++)
    {
        Console.WriteLine("Enter Flavour: ");
        string? fla = Console.ReadLine();
        bool flaPre = isPremium(fla);
        flavours.Add(new Flavour(fla, flaPre, 1));
    }

    List<Topping> toppings = new List<Topping>();
    for (int x = 0; x < topCount; x++)
    {
        Console.WriteLine("Enter Topping: ");
        string? top = Console.ReadLine();
        toppings.Add(new Topping(top));
    }

    if (option == "Cone")
    {
        Console.WriteLine("Is it dipped? (true/false): ");
        bool dipped = Convert.ToBoolean(Console.ReadLine());
        return new Cone(option, scoops, flavours, toppings, dipped);
    }
    else if (option == "Waffle")
    {
        Console.WriteLine("Enter Waffle Flavour: ");
        string? waffleFlavour = Console.ReadLine();
        return new Waffle(option, scoops, flavours, toppings, waffleFlavour);
    }
    else
    {
        return new Cup(option, scoops, flavours, toppings);
    }
}


static void DelIceCream(List<Customer> customerList)
{
    Customer selectedCustomer = ObtainCustomer(customerList);
    if (selectedCustomer != null)
    {
        for (int i = 0; i < selectedCustomer.OrderHistory.Count(); i++)
        {
            Console.WriteLine($"{i + 1}. {selectedCustomer.OrderHistory[i]}");
        }
        Console.WriteLine("Enter number to delete: ");
        int numDel = Convert.ToInt32(Console.ReadLine());
        Order orderDel = selectedCustomer.OrderHistory[numDel - 1];

        Console.WriteLine("Removed");
    }

}

// Advanced option a
// IF not order to process add exception
void processCheckout(Queue<Order> regularQueue, Queue<Order> goldQueue, Dictionary<Order, Customer> orderCustomerDict)
{
    if (goldQueue.Count() == 0 && regularQueue.Count() == 0)
    {
        Console.WriteLine("There is no order in the queues to process.");
    }
    else
    {
        Order processOrder = null;
        if (goldQueue.Count() == 0)
        {
            processOrder = regularQueue.Dequeue();
        }
        else
        {
            processOrder = goldQueue.Dequeue();
        }

        Console.WriteLine(processOrder.ToString());

        int count = 0;
        Dictionary<IceCream, double> priceDict = new Dictionary<IceCream, double>();
        foreach (IceCream iC in processOrder.IceCreamList)
        {
            double price = iC.CalculatePrice();
            priceDict.Add(iC, price);
            count += 1;
        }


        Customer orderCustomer = null;
        foreach (KeyValuePair<Order, Customer> kvp in orderCustomerDict)
        {
            if (kvp.Key == processOrder)
            {
                orderCustomer = kvp.Value;
            }
        }

        if (orderCustomer.IsBirthday() == true)
        {
            IceCream mostEx = null;
            double price = 0;
            foreach (KeyValuePair<IceCream, double> kvp in priceDict)
            {
                if (kvp.Value > price)
                {
                    price = kvp.Value;
                    mostEx = kvp.Key;
                }
            }
            priceDict[mostEx] = 0;
        }

        if (orderCustomer.PointCard.PunchCard == 11)
        {
            IceCream punchCardOrder = priceDict.First().Key;
            priceDict[punchCardOrder] = 0;
            orderCustomer.PointCard.Punch();
        }

        string redeem = null;
        bool isValid = false;
        if (orderCustomer.PointCard.Tier == "Silver" || orderCustomer.PointCard.Tier == "Gold")
        {
            while (isValid == false)
            {
                Console.Write("Do you want to redeem points [Y/N]? ");
                redeem = Console.ReadLine();
                if (redeem.ToLower() == "y" || redeem.ToLower() == "n")
                {
                    isValid = true;
                }
            }
        }
        else
        {
            redeem = "n";
        }

        int pointsRedeem = 0;
        double costRedeem = 0;
        if (redeem.ToLower() == "y")
        {
            Console.WriteLine($"You currently have {orderCustomer.PointCard.Points}.");
            Console.Write("How many points do you want to redeem? ");
            pointsRedeem = Convert.ToInt32(Console.ReadLine());
            if (pointsRedeem < orderCustomer.PointCard.Points)
            {
                orderCustomer.PointCard.RedeemPoints(pointsRedeem);
                costRedeem = 0.02 * pointsRedeem;
            }
        }
        double finalPrice = 0;
        foreach (KeyValuePair<IceCream, double> kvp in priceDict)
        {
            finalPrice += kvp.Value;
        }
        Console.WriteLine($"The final price is {finalPrice}.");

        Console.Write("Type in your Bank ID:");
        string bankId = Console.ReadLine();

        orderCustomer.PointCard.PunchCard += count;
        if (orderCustomer.PointCard.PunchCard > 10)
        {
            orderCustomer.PointCard.PunchCard = 10;
        }

        int pointsEarned = (int)Math.Floor(finalPrice * 0.72);
        orderCustomer.PointCard.Points += pointsEarned;

        if (orderCustomer.PointCard.Tier == "Ordinary")
        {
            if (orderCustomer.PointCard.Points >= 50)
            {
                orderCustomer.PointCard.Tier = "Silver";
            }
        }
        else if (orderCustomer.PointCard.Tier == "Ordinary" || orderCustomer.PointCard.Tier == "Silver")
        {
            if (orderCustomer.PointCard.Points >= 100)
            {
                orderCustomer.PointCard.Tier = "Gold";
            }
        }

        processOrder.TimeFulfuilled = DateTime.Now;

        orderCustomer.OrderHistory.Add(processOrder);

        orderCustomer.CurrentOrder = null;
    }
}


int orderNo = 0;
using (StreamReader sr = new StreamReader("orders.csv"))
{
    string line;
    sr.ReadLine();
    while ((line = sr.ReadLine()) != null)
    {
        string[] info = line.Split(",");
        orderNo = Convert.ToInt32(info[0]) + 1;
    }
}

Dictionary<Order, Customer> orderCustomerDict = new Dictionary<Order, Customer>();

Queue<Order> regularQueue = new Queue<Order>();
Queue<Order> goldQueue = new Queue<Order>();

while (true)
{
    /*
    foreach (KeyValuePair<Order, Customer> kvp in orderCustomerDict)
    { 
        Console.WriteLine(kvp.Value);
    }
    */
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
        CreateCustomerOrder(orderNo, customers, flavoursDict, orderCustomerDict, regularQueue, goldQueue);
        orderNo += 1;
    }
    if (option == "7")
    {
        processCheckout(regularQueue, goldQueue, orderCustomerDict);
    }
}


//--------------------

// my Order class has an extra MemberId 

/** public Order(int i, int memberId, DateTime t)
        {
            Id = i;
            MemberId = memberId;
            TimeReceived = t;
        }
**/

//This is my option 1 

/**
List<Customer> customerList = new List<Customer>();
void ListCustomers(List<Customer> customerList)
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
**/

// This is my option 2 
/**
 * static List<Order> ReadFromFile(List<Customer> customerList)
{
    List<Order> orders = new List<Order>();
    string[] lines = File.ReadAllLines("orders.csv");
    for (int i = 1; i < lines.Length; i++)
    {
        string[] infoLines = lines[i].Split(',');
        int orderId = Convert.ToInt32(infoLines[0]);
        int memId = Convert.ToInt32(infoLines[1]);
        // Added


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

        IceCream iceCream = null;
        if (option == "Cup")
        {
            iceCream = new Cup(option, scoops, flavours, toppings);
            
            

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
                    
                }
                else
                {
                    dipped = false;
                    iceCream = new Cone(option, scoops, flavours, toppings, dipped);
                    
                }
            }

        }
        else if (option == "Waffle")
        {
            if (infoLines[7] != null)
            {
                string waffleFlavour = infoLines[7];
                iceCream = new Waffle(option, scoops, flavours, toppings, waffleFlavour);
                
            }

        }
        
        Order order = new Order(orderId, memId, timeRec);

        //Associating Customer to current order based on unique memberId
        Customer customer = customerList.FirstOrDefault(c => c.MemberId == memId);

        if (customer != null)
        {
            customer.OrderHistory.Add(order);
            order.AddIceCream(iceCream);
            orders.Add(order);
        }
        else
        {
            Console.Write("Doenst work");
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
**/

//This is my option 5 

/**
 * static void DisplayOrderDetails(List<Customer> customerList)
{
    

    Customer selectedCustomer = ObtainCustomer(customerList);

    if (selectedCustomer != null)
    {
        Console.WriteLine($"Displaying order details for {selectedCustomer.Name}");

        foreach (Order order in selectedCustomer.OrderHistory)
        {

            
            Console.WriteLine($"Order ID: {order.Id}");
            Console.WriteLine($"Time Received: {order.TimeReceived}");
            Console.WriteLine($"Time Fulfilled: {order.TimeFulfuilled}");
            
            
            
            //Console.WriteLine(order.ToString());
            
            foreach (IceCream iceCream in order.IceCreamList)
            {
                Console.WriteLine($"Ice Cream Option: {iceCream.Option}");
                Console.WriteLine($"Scoops: {iceCream.Scoops}");

                foreach (Flavour flavour in iceCream.Flavours)
                {
                    Console.WriteLine($"Flavour: {flavour.Type} (Premium: {flavour.Premium})");
                }

                foreach (Topping topping in iceCream.Toppings)
                {
                    Console.WriteLine($"Topping: {topping.Type}");
                }

                // Display additional properties for specific ice cream types (Cone, Cup, Waffle)
                if (iceCream is Cone cone)
                {
                    Console.WriteLine($"Dipped: {cone.Dipped}");
                }
                else if (iceCream is Waffle waffle)
                {
                    Console.WriteLine($"Waffle Flavour: {waffle.WaffleFlavour}");
                }

                // Calculate and display the price
                double price = iceCream.CalculatePrice();
                Console.WriteLine($"Total Price: {price}");

                Console.WriteLine(); 
            }
            
            Console.WriteLine(); 
        }
    }
}


//Function to obtain the customer
static Customer? ObtainCustomer(List<Customer> customerList)
{
    while (true)
    {
        try
        {
            Console.Write("Enter Customer Name: ");
            string? cusName = Console.ReadLine();

            foreach (Customer c in customerList)
            {
                if (c.Name == cusName)
                {
                    //Customer found
                    return c;
                }
            }

            throw new InvalidOperationException("Customer not found. Please enter a valid customer name.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
**/

//This is my Option 6 

/**
 * static int OrderDetailsMenu()
{
    Console.WriteLine("[1] Choose an existing Ice Cream object to modify.");
    Console.WriteLine("[2] Add an ENTIRELY new Ice Cream object to the order.");
    Console.WriteLine("[3] Choose an existing Ice Cream object to delete from the Order.");
    Console.WriteLine("Enter your option: ");
    int options = Convert.ToInt32(Console.ReadLine());
    return options;
}

static void ModifyOrderDetails(List<Customer> customerList)
{
    Customer selectedCustomer = ObtainCustomer(customerList);
    if (selectedCustomer != null)
    {
        // Display order history for the selected customer
        Console.WriteLine("Order History:");
        for (int i = 0; i < selectedCustomer.OrderHistory.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {selectedCustomer.OrderHistory[i]}");
        }

        Console.WriteLine("Enter order to modify: ");
        int orderIndex = Convert.ToInt32(Console.ReadLine());

        // Validate the order index
        if (orderIndex >= 1 && orderIndex <= selectedCustomer.OrderHistory.Count)
        {
            Order orderMod = selectedCustomer.OrderHistory[orderIndex - 1];

            Console.WriteLine("Enter option (Cup, Cone, Waffle): ");
            string opt = Console.ReadLine();

            if (opt == "Cup" || opt == "Cone" || opt == "Waffle")
            {
                // Instead of individually changing, clear list and create a new order
                orderMod.IceCreamList.Clear();

                // Add new ice cream based on the selected option
                IceCream newIceCream = CreateIceCream(opt);
                orderMod.AddIceCream(newIceCream);

                Console.WriteLine("Order modified successfully.");
            }
            else
            {
                Console.WriteLine("Invalid ice cream option.");
            }
        }
        else
        {
            Console.WriteLine("Invalid order index.");
        }
    }
}


static IceCream CreateIceCream(string option)
{
    
    Console.WriteLine($"Enter Scoops for {option}: ");
    int scoops = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine($"Enter number of toppings for {option}: ");
    int topCount = Convert.ToInt32(Console.ReadLine());

    List<Flavour> flavours = new List<Flavour>();
    for (int x = 0; x < scoops; x++)
    {
        Console.WriteLine("Enter Flavour: ");
        string? fla = Console.ReadLine();
        bool flaPre = isPremium(fla);
        flavours.Add(new Flavour(fla, flaPre, 1));
    }

    List<Topping> toppings = new List<Topping>();
    for (int x = 0; x < topCount; x++)
    {
        Console.WriteLine("Enter Topping: ");
        string? top = Console.ReadLine();
        toppings.Add(new Topping(top));
    }

    if (option == "Cone")
    {
        Console.WriteLine("Is it dipped? (true/false): ");
        bool dipped = Convert.ToBoolean(Console.ReadLine());
        return new Cone(option, scoops, flavours, toppings, dipped);
    }
    else if (option == "Waffle")
    {
        Console.WriteLine("Enter Waffle Flavour: ");
        string? waffleFlavour = Console.ReadLine();
        return new Waffle(option, scoops, flavours, toppings, waffleFlavour);
    }
    else
    {
        return new Cup(option, scoops, flavours, toppings);
    }
}
**/