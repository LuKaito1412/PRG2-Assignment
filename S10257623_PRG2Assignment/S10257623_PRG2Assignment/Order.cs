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

    public void ModifyIceCream(int index) 
    { 

    }
    
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
        double totalPrice = 0;
        int count = 0;
        foreach (var iceCream in IceCreamList)
        {
            iC += $"\nOption: {iceCream.Option}, Scoops: {iceCream.Scoops},";

            if (iceCream is Cone cone)
            {
                iC += $" Dipped: {cone.Dipped},";
            }
            else if (iceCream is Waffle waffle)
            {
                iC += $" Waffle Flavour: {waffle.WaffleFlavour},";
            }

            string flavours = " Flavours:";
            if (iceCream.Flavours.Count() != 0)
            {
                foreach (Flavour flavour in iceCream.Flavours)
                {
                    flavours += $" {flavour.Type} x{flavour.Quantity},";
                }
            }
            else
                flavours = " Flavours: -,";
            iC += flavours;

            string toppings = " Toppings:";
            if (iceCream.Toppings.Count() != 0)
            {
                foreach (var topping in iceCream.Toppings)
                {
                    toppings += $" {topping.Type},";
                }
            }
            else
                toppings = " Toppings: -,";
            iC += toppings;
        

            iC += $" Price: ${iceCream.CalculatePrice()}";

            totalPrice += iceCream.CalculatePrice();
            
            line = $"Order ID: {Id}, Order Time: {TimeReceived}, {iC}";

            count += 1;
            if (IceCreamList.Count() == count)
            {
                line += $"\nTotal Price: {totalPrice}";
            }
        }
        return line;
    }
}
