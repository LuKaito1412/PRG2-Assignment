using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Customer
{
    public string Name { get; set; }
    public int MemberId { get; set; }
    public DateTime Dob { get; set; }
    public Order CurrentOrder { get; set; }
    public List<Order> OrderHistory { get; set; } = new List<Order>();

    public PointCard PointCard { get; set; }

    public Customer() { }
    public Customer(string n, int m, DateTime d)
    {
        Name = n;
        MemberId = m;
        Dob = d;
    }

    public Order MakeOrder()
    {
        CurrentOrder = new Order();
        return CurrentOrder;
    }

    public bool IsBirthday()
    {
        bool isBd = false;
        if (Dob == DateTime.Now.Date)
        {
            isBd = true;
        }
        return isBd;
    }

    public override string ToString()
    {
        string line = $"{Name,-11}{MemberId,-9}{Dob.ToString("dd/MM/yyyy")}";
        return line;
    }
}