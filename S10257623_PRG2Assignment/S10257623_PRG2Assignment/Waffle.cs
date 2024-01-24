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
        double preCount = 0;
        double scoopPrice = 0;
        double topPrice = 0;
        double wFlavour = 0;

        foreach (Flavour c in Flavours)
        {
            if (c.Premium)
            {
                preCount += 2.0;
            }
        }

        if (Scoops == 1)
        {
            scoopPrice = 7.0;
        }
        else if (Scoops == 2)
        {
            scoopPrice = 8.5;
        }
        else if (Scoops == 3)
        {
            scoopPrice = 9.5;
        }

        foreach (Topping t in Toppings)
        {
            if (t.Type == "Sprinkles" || t.Type == "Mochi" || t.Type == "Sago" || t.Type == "Oreos")
            {
                topPrice += 1.0;
            }
        }

        if (WaffleFlavour != null)
        {
            if (WaffleFlavour == "Original")
            {
                wFlavour = 0;
            }
            else
            {
                wFlavour = 3.0;
            }
        }

        return scoopPrice + preCount + topPrice + wFlavour;
    }
}
