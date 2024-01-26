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

    public void ModifyIceCream(int index) { }
    
    public void AddIceCream(IceCream icecream)
    {
        IceCreamList.Add(icecream);
    }

    public void DeleteIceCream(int index)
    { 
        IceCreamList.Remove(IceCreamList[index]);
    }

    public double CalculateTotal()
    {
        double total = 0;
        foreach (IceCream iC in IceCreamList)
        {
            total += total + iC.CalculatePrice();
        }
        return total;
    }

    public override string ToString()
    {
        string iC = null;
        string line = null;
        foreach (var iceCream in IceCreamList)
        {
            iC += $"Ice Cream Option: {iceCream.Option}, Scoops: {iceCream.Scoops}";

            if (iceCream is Cone cone)
            {
                iC += $" Dipped: {cone.Dipped}";
            }
            else if (iceCream is Waffle waffle)
            {
                iC += $" Waffle Flavour: {waffle.WaffleFlavour}";
            }


            foreach (var flavour in iceCream.Flavours)
            {
                iC += $" Flavour: {flavour.Type}, Premium: {flavour.Premium}";
            }


            foreach (var topping in iceCream.Toppings)
            {
                iC += $" Topping: {topping.Type}";
            }

            iC += $" Total Price: ${iceCream.CalculatePrice()}\n";
            
            line = $"Order ID: {Id}, Order Time: {TimeReceived}, Order Fulfilled Time: {TimeFulfuilled}, Order: {iC}";
        }
        return line;
    }
}
