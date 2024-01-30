using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

class Cup : IceCream
{
    public Cup() { }
    public Cup(string option, int scoops, List<Flavour> flavours, List<Topping> toppings) : base(option, scoops, flavours, toppings)
    {

    }
    public override double CalculatePrice()
    {
        double price = 0;

        foreach (Flavour c in Flavours)
        {
            if (c.Premium == true)
            {
                price += 2.0 * c.Quantity;
            }
        }

        if (Scoops == 1)
        {
            price += 4.0;
        }
        else if (Scoops == 2)
        {
            price += 5.5;
        }
        else if (Scoops == 3)
        {
            price += 6.5;
        }

        foreach (Topping t in Toppings)
        {
            price += 1.0;
        }

        return price;
    }
}

