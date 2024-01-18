using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Waffle: IceCream
{
    public string WaffleFlavour { get; set; }


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

                preCount = c.Quantity * 2;
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
            if (Toppings != null)
            {
                foreach (Topping t in Toppings)
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
