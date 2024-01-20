using System;

//Option 1 
void ListCustomers()
{
    using (StreamReader sr = new StreamReader("./customers.csv"))
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

ListCustomers();


