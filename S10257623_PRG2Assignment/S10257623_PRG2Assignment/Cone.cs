using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

class Cone : IceCream
{
    public bool Dipped { get; set; }
    public Cone() { }
    public Cone(string option, int scoops, List<Flavour> flavours, List<Topping> toppings, bool dipped) : base(option, scoops, flavours, toppings)
    {
        Dipped = dipped;
    }

    public override double CalculatePrice()
    {
        double preCount = 0;
        double scoopPrice = 0;
        double topPrice = 0;
        double dip = 0;

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

        if (Dipped == true)
        {
            dip += 2.0;
        }

        return scoopPrice + preCount + topPrice + dip;
    }
}

