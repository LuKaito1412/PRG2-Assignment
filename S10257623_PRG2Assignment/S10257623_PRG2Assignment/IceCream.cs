using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
   
abstract class IceCream
{
    public string Option { get; set; }
    public int Scoops { get; set; }
    
    public List<Flavour> Flavours { get; set; } = new List<Flavour>();
    public List<Topping> Toppings { get; set; } = new List<Topping>();

    public IceCream() { }
    public IceCream(string option, int scoops, List<Flavour> flavours, List<Topping> toppings)
    {
        Option = option;
        Scoops = scoops;
        Flavours = flavours;
        Toppings = toppings;
    }
    public abstract double CalculatePrice();


    // to string method not fully done yet

    public override string ToString()
    {
        return $"Option: {Option} Scoops: {Scoops}";
    }
}

