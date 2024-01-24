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

        foreach (Flavour c in Flavours)
        {
            int preCount = 0;
            double scoopPrice = 0;
            int topPrice = 0;
            int wFlavour = 0;
            if (c.Premium)
            {

                preCount += 2;
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
                if (t.Type == "Sprinkles")
                {
                    topPrice += 1;
                }
                else if (t.Type == "Mochi")
                {
                    topPrice += 1;
                }
                else if (t.Type == "Sago")
                {
                    topPrice += 1;
                }
                else if (t.Type == "Oreos")
                {
                    topPrice += 1;
                }
            }
            if (WaffleFlavour != null)
            {
                wFlavour = 3;
            }

            return (scoopPrice) + (preCount) + (topPrice) + wFlavour;

        }
        return 0.0; // empty order
    }
}
