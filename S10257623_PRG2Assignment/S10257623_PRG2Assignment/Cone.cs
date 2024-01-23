﻿using System;
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

        foreach (Flavour c in Flavours)
        {
            int preCount = 0;
            double scoopPrice = 0;
            int topPrice = 0;
            int dip = 0;
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
                foreach (Topping t in Toppings)
                {
                    topPrice += 1;
                }
            }
            if (Dipped)
            {
                dip += 2;
            }

            return (scoopPrice) + (preCount) + (topPrice) + dip;

        }
        return 0.0; // empty order
    }
}

