using System;
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
        
    }
}

