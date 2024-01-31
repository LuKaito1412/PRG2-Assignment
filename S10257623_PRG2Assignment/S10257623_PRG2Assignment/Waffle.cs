using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Waffle : IceCream
{
    public string WaffleFlavour { get; set; }
    public Waffle() { }
    public Waffle(string option, int scoops, List<Flavour> flavours, List<Topping> toppings, string waffleFlavour) : base(option, scoops, flavours, toppings)
    {
        WaffleFlavour = waffleFlavour;
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
            price += 7.0;
        }
        else if (Scoops == 2)
        {
            price += 8.5;
        }
        else if (Scoops == 3)
        {
            price += 9.5;
        }

        foreach (Topping t in Toppings)
        {
            price += 1.0;
        }

        if (WaffleFlavour == "red velvet" || WaffleFlavour == "charcoal" || WaffleFlavour == "pandan")
        {
            price += 3.0;
        }

        return price;
    }
}
