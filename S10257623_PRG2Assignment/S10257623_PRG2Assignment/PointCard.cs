using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
   
class PointCard
{
    public int Points {get; set;}
    public int PunchCard {get; set;}
    public string Tier {get; set;}

    public PointCard() {}
    public PointCard(int p, int pC)
    {
        Points = p;
        PunchCard = pC;
    }
}