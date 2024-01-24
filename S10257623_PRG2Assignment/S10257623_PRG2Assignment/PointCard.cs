using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class PointCard
{
    public int Points { get; set; }
    public int PunchCard { get; set; }
    public string Tier { get; set; }

    public PointCard() { }
    public PointCard(int p, int pC)
    {
        Points = p;
        PunchCard = pC;
    }

    public void AddPoints(int points)
    { 
        Points += points;  
    }

    public void RedeemPoints(int points)
    { 
        Points -= points;
    }

    public void Punch()
    {
        PunchCard += 1;
    }

    public override string ToString()
    {
        string line = $"Points: {Points}, PunchCard:{PunchCard}, Tier:{Tier}";
        return line;
    }
}