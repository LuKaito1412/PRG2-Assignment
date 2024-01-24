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
        double preCount = 0;
        double scoopPrice = 0;
        double topPrice = 0;

        foreach (Flavour c in Flavours)
        {
            if (c.Premium)
            {
                preCount += 2.0;
            }
        }

        if (Scoops == 1)
        {
            scoopPrice = 4.0;
        }
        else if (Scoops == 2)
        {
            scoopPrice = 5.5;
        }
        else if (Scoops == 3)
        {
            scoopPrice = 6.5;
        }

        foreach (Topping t in Toppings)
        {
            if (t.Type == "Sprinkles" || t.Type == "Mochi" || t.Type == "Sago" || t.Type == "Oreos")
            {
                topPrice += 1.0;
            }
        }

        return scoopPrice + preCount + topPrice;
    }
}

