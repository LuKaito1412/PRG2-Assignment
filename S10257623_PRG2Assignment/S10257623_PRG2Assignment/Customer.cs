using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
   
class Customer
{
    public string Name {get; set;}
    public int MemberId {get; set;}
    public DateTime Dob {get; set;}
    public Order CurrentOrder {get; set;}
    public List<Order> OrderHistory {get; set;} = new List<Order>();

    public Customer() {}
    public Customer(string n, int m, DateTime d)
    {
        Name = n;
        MemberId = m;
        Dob = d;
    }

    public override string ToString()
    {
        string line = $"{Name,9}{MemberId,9}{Dob}";
        return line;
    }
}