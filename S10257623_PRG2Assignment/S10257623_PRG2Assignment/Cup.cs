﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Cup: IceCream
{
    public Cup() { }
    public Cup(string option, int scoops, List<Flavour> flavours, List<Topping> toppings) :base(option, scoops, flavours, toppings)
    {

    }
    public override double CalculatePrice()
    {
        
        foreach (Flavour c in Flavours)
        {
            int preCount = 0;
            double scoopPrice = 0;
            int topPrice = 0;
            if (c.Premium)
            {
                
                preCount = c.Quantity * 2;
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
            if (Toppings != null)
            {
                foreach(Topping t in Toppings)
                {
                    topPrice += 1;
                }
            }


            return (scoopPrice) + (preCount) + (topPrice);

        }
        return 0.0; // empty order
    }
}

