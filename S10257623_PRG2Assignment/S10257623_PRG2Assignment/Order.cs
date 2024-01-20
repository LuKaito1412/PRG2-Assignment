using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Order
{
    public int Id { get; set; }
    public DateTime TimeReceived { get; set; }
    public DateTime? TimeFulfuilled { get; set; }
    public List<IceCream> IceCreamList { get; set; } = new List<IceCream>();

    public Order() { }
    public Order(int i, DateTime t)
    {
        Id = i;
        TimeReceived = t;
    }

    //just added
    public void AddIceCream(IceCream icecream)
    {
        IceCreamList.Add(icecream);
    }
}
