using System;
using System.Linq.Expressions;

//Option 1 (Liu JieXin)

// Read customer.csv, returns a dictionary and list containing information on Customer.
(Dictionary<string, Customer>, string printHeader) readCustomerFile()
{
    Dictionary<string, Customer> customers = new Dictionary<string, Customer>();
    string printHeader = null;
    using (StreamReader sr = new StreamReader("customers.csv"))
    {
        string line;
        string[] header = (sr.ReadLine()).Split(",");
        printHeader = $"\n{header[0],-11}{header[1],-11}{header[2],-13}{header[3],-19}{header[4],-19}{header[5]}";
        while ((line = sr.ReadLine()) != null)
        {
            string[] info = line.Split(',');
            // Create customer object.
            Customer customer = new Customer(info[0], Convert.ToInt32(info[1]), Convert.ToDateTime(info[2]));
            PointCard pointCard = new PointCard(Convert.ToInt32(info[4]), Convert.ToInt32(info[5]));
            // Create pointcard for customer object.
            customer.PointCard = pointCard;
            customer.PointCard.Tier = info[3];
            // Add to dictionary that is used to display data.
            customers.Add(info[1], customer);
        }
    }
    return (customers, printHeader);
}

// To print out a table containing the information of the customers
void ListCustomers1(Dictionary<string, Customer> customers, string header)
{
    Console.WriteLine(header);
    foreach (KeyValuePair<string, Customer> kvp in customers)
    {
        Console.WriteLine(kvp.Value.ToString() + $"{kvp.Value.PointCard.Tier,-19}{kvp.Value.PointCard.Points,-19}{kvp.Value.PointCard.PunchCard}");
    }
}

// ---------- Option 1 ends here ----------

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

// Option 3 (Liu JieXin)

// Takes in name, member ID and Dob to create a Customer object and pointcard.
// Then the info about new customer are written to the customers.csv.
void NewCutomerRegister(Dictionary<string, Customer> customers)
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
            // To keep it within 6 digits
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
    newCustomer.PointCard.Tier = "Ordinary";

    customers.Add(Convert.ToString(idNo),newCustomer);

    using (StreamWriter sw = new StreamWriter("customers.csv", true))
    {
        string line = $"{name},{idNo},{dob.ToString("dd/MM/yyyy")},{"Ordinary"},{0},{0}";
        sw.WriteLine(line);
    }

    Console.WriteLine("Your membership has registered successfully.");
}

// ---------- Option 3 ends here ----------

// Option 4 (Liu JieXin)

// To check if the entered member is valid.
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

// Read flavours.csv to have a dictionary of the valid flavours.
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

// To check if the entered flavour is valid.
bool flavourValidation(string input, Dictionary<string, int> flavoursDict)
{
    bool isValid = false;
    foreach (KeyValuePair<string, int> kvp in flavoursDict)
    {
        // ToLower() is used to make user input verification easier.
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

// Used by toppings and waffle flavours to check if the customer entry is valid by comparing with a set of array.
bool inputValidation(string type, string input, string[] array)
{
    bool isValid = false;
    foreach (string item in array)
    {
        // ToLower() is used to make user input verification easier.
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

// Takes in member id to select customer and then creates ice cream order.
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
    // Search for customer using member id
    Customer orderCustomer = null;
    foreach (KeyValuePair<string, Customer> kvp in customers)
    {
        if (kvp.Key == userId)
        {
            orderCustomer = kvp.Value;
        }
    }

    // Create order object and linking order to ordering customer.
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
            // ToLower() is used to make user input verification easier.
            if (option.ToLower() == "cup" || option.ToLower() == "cone" || option.ToLower() == "waffle")
            {
                optionValid = true;
            }
            else
                Console.WriteLine("Invalid option.");
        }

        // Input validation and error detection.
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

        // Select Flavours
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

        // Increases quantity of ice cream accordingly and check if it is a premium flavour.
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


            // Make sure ordered flavours that are repeated doesnt get added twice
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
        // Select toppings
        List<Topping> orderToppList = new List<Topping>();
        Console.WriteLine("Toppings (+$1 each): Sprinkles, Mochi, Sago, Oreos");
        Console.WriteLine("You can have 0-4 toppings each ice cream.");

        // Input validation and error detection.
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

        // Input validation for toppings.
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

        // Create ice cream objects based on previous user inputs.
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
                Console.Write("Choose a waffle flavour: ");
                waffleFlav = Console.ReadLine();
                isValid = inputValidation("waffle flavour", waffleFlav, waffleFlavours);
            }
            orderIc = new Waffle(option, scoop, orderFlavList, orderToppList, waffleFlav);
        }

        newOrder.AddIceCream(orderIc);

        // Loop if customer wants another ice cream in same order.
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
        {
            orderCustomerDict.Add(newOrder, orderCustomer);
            break;
        }
    }

    // Queue according to tier.
    if (orderCustomer.PointCard.Tier == "Gold")
    {
        goldQueue.Enqueue(newOrder);
    }
    else
    {
        regularQueue.Enqueue(newOrder);
    }
    Console.WriteLine("Order has been made successfully.");
    return orderCustomerDict;
}


// ---------- Option 4 ends here ----------

//option 5 (Julian)

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

// Advanced option (a) (Liu JieXin)
void processCheckout(Queue<Order> regularQueue, Queue<Order> goldQueue, Dictionary<Order, Customer> orderCustomerDict, Dictionary<string, Customer> customers)
{
    // Check if queue empty.
    if (goldQueue.Count() == 0 && regularQueue.Count() == 0)
    {
        Console.WriteLine("There is no order in the queues to process.");
    }
    else
    {
        // Priortize gold queue.
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
        
        // Calculate and store raw price.
        int count = 0;
        Dictionary<IceCream, double> priceDict = new Dictionary<IceCream, double>();
        foreach (IceCream iC in processOrder.IceCreamList)
        {
            double price = iC.CalculatePrice();
            priceDict.Add(iC, price);
            count += 1;
        }

        // Search for the customer of the order.
        string orderCustomer1 = null;
        foreach (KeyValuePair<Order, Customer> kvp in orderCustomerDict)
        {
            if (kvp.Key == processOrder)
            {
                orderCustomer1 = Convert.ToString(kvp.Value.MemberId);
            }
        }

        // Search for the customer in the dictionary that is used for displaying.
        // Assign order customer to the kvp.Value, so that when calling the ListCustomers1 method, data is updated.
        Customer orderCustomer = null;
        foreach (KeyValuePair<string, Customer> kvp in customers)
        {
            if (orderCustomer1 == kvp.Key)
            {
                orderCustomer = kvp.Value;
            }
        }

        // If birthday today, most expansive ice cream in order is free.
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

        // When punchcard is 10, the current order 
        if (orderCustomer.PointCard.PunchCard == 10)
        {
            IceCream punchCardOrder = priceDict.First().Key;
            priceDict[punchCardOrder] = 0;
            orderCustomer.PointCard.PunchCard = 0;
            // Add to punchcard according to number of ice cream ordered - 1 (free ice cream).
            orderCustomer.PointCard.PunchCard += count - 1;
            if (orderCustomer.PointCard.PunchCard > 10)
            {
                orderCustomer.PointCard.PunchCard = 10;
            }
        }
        else 
        {
            orderCustomer.PointCard.PunchCard += count;
            if (orderCustomer.PointCard.PunchCard > 10)
            {
                orderCustomer.PointCard.PunchCard = 10;
            }
        }

        // Ask if want to redeem points.
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

        // To make sure customer doesnt enter more points than what he has.
        int pointsRedeem = orderCustomer.PointCard.Points + 1;
        double costRedeem = 0;
        if (redeem.ToLower() == "y")
        {
            Console.WriteLine($"You currently have {orderCustomer.PointCard.Points}.");
            while (pointsRedeem > orderCustomer.PointCard.Points)
            {
                Console.Write("How many points do you want to redeem? ");
                pointsRedeem = Convert.ToInt32(Console.ReadLine());
                if (pointsRedeem > orderCustomer.PointCard.Points)
                {
                    Console.WriteLine("You don't have that many points.");
                }
            }
            orderCustomer.PointCard.RedeemPoints(pointsRedeem);
            costRedeem = 0.02 * pointsRedeem;
        }

        // Calculate final price after point redeem and birthday discount.
        double finalPrice = 0;
        foreach (KeyValuePair<IceCream, double> kvp in priceDict)
        {
            finalPrice += kvp.Value;
            finalPrice -= costRedeem;
        }
        Console.WriteLine($"The final price is ${finalPrice}.");

        Console.Write("Type in your Bank ID: ");
        string bankId = Console.ReadLine();

        // Add points based on amount spent.
        int pointsEarned = (int)Math.Floor(finalPrice * 0.72);
        orderCustomer.PointCard.Points += (int)Math.Floor(finalPrice * 0.72);

        // Check if customer has reached a new status for pointcard.
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

        // Formatting data to write to orders.csv.
        foreach (IceCream iC in processOrder.IceCreamList)
        {
            string line = null;
            line += $"{processOrder.Id},{orderCustomer.MemberId},{processOrder.TimeReceived},{processOrder.TimeFulfuilled},{iC.Option},{iC.Scoops},";
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
            count = 0;
            foreach (Flavour f in iC.Flavours)
            {
                flavours += $"{f.Type},";
                count += 1;
            }
            line += flavours;
            for (int i = 1; i <= (3 - count); i++)
            {
                line += ",";
            }
            string toppings = null;
            count = 0;
            foreach (Topping t in iC.Toppings)
            {
                toppings += $"{t.Type},";
                count += 1;
            }
            line += toppings;
            for (int i = 1; i <= (3 - count); i++)
            {
                line += ",";
            }
            using (StreamWriter sw = new StreamWriter("orders.csv", true))
            {
                sw.WriteLine(line);
            }
        }

        orderCustomer.OrderHistory.Add(processOrder);

        // Clear order history.
        orderCustomer.CurrentOrder = null;
    }
}

// Liu JieXin
// To retrieve the last order number + 1.
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

// Liu JieXin
Dictionary<Order, Customer> orderCustomerDict = new Dictionary<Order, Customer>();

Queue<Order> regularQueue = new Queue<Order>();
Queue<Order> goldQueue = new Queue<Order>();

(Dictionary<string, Customer> customers, string header) = readCustomerFile();

string OptionsPrint()
{
    Console.WriteLine("\nOptions");
    Console.WriteLine("------------------------------------------");
    Console.WriteLine("[1]: List All customers.");
    Console.WriteLine("[2]: List all current orders.");
    Console.WriteLine("[3]: Register a new customer.");
    Console.WriteLine("[4]: Create a customer's order.");
    Console.WriteLine("[5]: Display order details of a customer.");
    Console.WriteLine("[6]: Modify order details.");
    Console.WriteLine("[7]: Process an order and checkpoint.");
    Console.WriteLine("[0]: Quit");
    Console.Write("Enter option: ");
    string option = Console.ReadLine();
    return option;
}

while (true)
{
    string option = OptionsPrint();
    if (option == "1")
        ListCustomers1(customers, header);
    else if (option == "2")
    {
        Dictionary<int, List<Order>> cusOrderDict = ReadFromFile(customerList);
        DisplayAllOrders(cusOrderDict);
    }
    else if (option == "3")
        NewCutomerRegister(customers);
    else if (option == "4")
    {
        ListCustomers1(customers, header);
        CreateCustomerOrder(orderNo, customers, flavoursDict, orderCustomerDict, regularQueue, goldQueue);
        orderNo += 1;
    }
    else if (option == "5")
    {
        ListCustomers(customerList);
        Dictionary<int, List<Order>> cusOrderDict = ReadFromFile(customerList);
        DisplayOrderDetails(cusOrderDict, customerList);
    }
    else if (option == "6")
    {
        int choice = OrderDetailsMenu();
        if (choice == 1)
        {
            ListCustomers(customerList);
            Dictionary<int, List<Order>> cusOrderDict = ReadFromFile(customerList);
            ModifyOrderDetails(customerList, cusOrderDict);

        }
        if (choice == 2)
        {
            Console.WriteLine("Havent implemented");
        }
        if (choice == 3)
        {
            ListCustomers(customerList);
            DelIceCream(customerList);
        }
    }
    else if (option == "7")
    {
        processCheckout(regularQueue, goldQueue, orderCustomerDict, customers);
    }
    else if (option == "0")
    {
        Console.WriteLine("Bye!");
        break;
    }
    else 
    {
        Console.WriteLine("Invalid option.");
    }
}