using F5BMX.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace F5BMX.Helpers;

internal static class Motos
{

    private static int[,] gateNumbers =
    {
        {1, 6, 4},
        {2, 5, 7},
        {3, 8, 6},
        {4, 7, 1},
        {5, 2, 8},
        {6, 1, 3},
        {7, 4, 2},
        {8, 3, 5}
    };


    public static void Generate(Round round)
    {
        foreach (var formula in round.formulas.OrderBy(x => x.order))
        {
            // GENERATE THE RACE CLASSES
            int numberOfRaces = (int)Math.Ceiling((double)formula.riders.Count / round.numberOfGates);

            for (int i = 0; i < numberOfRaces; i++)
            {
                formula.moto1.Add(new Race());
                formula.moto2.Add(new Race());
                formula.moto3.Add(new Race());
            }

            // FILL RACE CLASSES WITH RIDERS
            Random rng = new Random();
            int fillIndex = 0;
            int gateNumber = 0;
            var randomRiderOrder = formula.riders.OrderBy(x => rng.Next()).ToList();
            foreach (var rider in randomRiderOrder)
            {
                formula.moto1[fillIndex].setGateRider(gateNumbers[gateNumber, 0], rider.id);
                formula.moto2[fillIndex].setGateRider(gateNumbers[gateNumber, 1], rider.id);
                formula.moto3[fillIndex].setGateRider(gateNumbers[gateNumber, 2], rider.id);

                fillIndex++;
                if (fillIndex >= numberOfRaces)
                {
                    fillIndex = 0;
                    gateNumber++;
                }
            }
            // SWAP RIDERS IN RACES
        }
    }

    public static void GenerateEntryList(Round round)
    {

    }

}
