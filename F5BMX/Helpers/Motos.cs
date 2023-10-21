using F5BMX.Core.IO;
using F5BMX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace F5BMX.Helpers;

internal static class Motos
{

    private static int[,] getGateNumbers(int numberOfGates)
    {
        switch (numberOfGates)
        {
            case 8:
                return new int[,] {
                        { 1, 6, 4},
                        { 2, 5, 7},
                        { 3, 8, 6},
                        { 4, 7, 1},
                        { 5, 2, 8},
                        { 6, 1, 3},
                        { 7, 4, 2},
                        { 8, 3, 5}
                    };

            case 6:
                return new int[,] {
                        { 1, 4, 6},
                        { 2, 3, 5},
                        { 3, 6, 1},
                        { 4, 5, 2},
                        { 5, 2, 3},
                        { 6, 1, 4}
                    };

            case 4:
                return new int[,] {
                        { 1, 4, 3},
                        { 2, 3, 1},
                        { 3, 2, 4},
                        { 4, 1, 2}
                    };

            default:
                throw new ArgumentException("Invalid number of Gates");
        }
    }

    public static void Generate(Round round)
    {
        var formulaRaceOrder = round.formulas.OrderBy(x => x.order);
        var gateNumbers = getGateNumbers(round.numberOfGates);

        // ASSIGN RIDERS
        foreach (var formula in formulaRaceOrder)
        {
            // CLEAR IF THERE ARE ANY EXISTING MOTOS
            formula.moto1.Clear();
            formula.moto2.Clear();
            formula.moto3.Clear();

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
            if (numberOfRaces > 1)
            {

            }
        }

        // NUMBER THE RACES
        int raceNumber = 1;
        foreach (var formula in formulaRaceOrder)
        {
            foreach (var race in formula.moto1)
            {
                race.raceNumber = raceNumber;
                raceNumber++;
            }
        }
        foreach (var formula in formulaRaceOrder)
        {
            foreach (var race in formula.moto2)
            {
                race.raceNumber = raceNumber;
                raceNumber++;
            }
        }
        foreach (var formula in formulaRaceOrder)
        {
            foreach (var race in formula.moto3)
            {
                race.raceNumber = raceNumber;
                raceNumber++;
            }
        }
    }

    public static void GenerateMotoListing(Series series, Round round)
    {
        StringBuilder entryList = new StringBuilder();
        entryList.Append(@"
<!DOCTYPE html>
<html>
<head>
    <style>
    html, body {
        width: 210mm;
        font-family: ""Tahoma"";
    }

    h1, h2 {
        text-align: center;
        margin: 0;
    }

    table {
        margin-top: 5mm;
        width: 100%;
        page-break-inside: avoid;
    }

    table thead tr.formulaName td {
        font-size: 1.2em;
        font-weight: bold;
        text-align: center;

        background-color: black;
        color: white;
    }

    table thead tr.tableHeading td {
        font-weight: bold;
        text-align: center;
    }

    table tbody tr td {
        text-align: center;
        border-top: solid 1px black;
    }

    table tbody tr:nth-child(odd) td {
        background-color: lightgrey;
    }
    </style>
</head>

<body>
<h1>");
        entryList.AppendFormat("F5BMX - {0} - {1}", series.year, series.name);
        entryList.Append(@"</h1><h2>");
        entryList.AppendFormat("Round {0} - Race Listings", round.roundNumber);
        entryList.Append(@"</h2>");

        foreach (var formula in round.formulas)
        {
            // SKIP FORMULAS WITH NO RIDERS
            if (formula.riders.Count == 0)
                continue;

            entryList.AppendFormat(@"
<table>
    <thead>
        <tr class=""formulaName"">
            <td colspan=""5"">{0} ~ {1} Riders</td>
        </tr>
        <tr class=""tableHeading"">
            <td width=""20%"">Name</td>
            <td width=""20%"">Number</td>
            <td width=""20%"">Moto 1</td>
            <td width=""20%"">Moto 2</td>
            <td width=""20%"">Moto 3</td>
        </tr>
    </thead>
    <tbody>", formula.name, formula.riders.Count);

            foreach (var rider in formula.riders)
            {
                var riderMoto1 = formula.moto1.Where(x => x.riderList.Contains(rider.id)).FirstOrDefault();
                var riderMoto2 = formula.moto2.Where(x => x.riderList.Contains(rider.id)).FirstOrDefault();
                var riderMoto3 = formula.moto3.Where(x => x.riderList.Contains(rider.id)).FirstOrDefault();

                entryList.AppendLine("<tr>");
                entryList.AppendLine($"<td>{rider.firstName} {rider.lastName}</td>");
                entryList.AppendLine($"<td>{rider.plateNumber}</td>");
                entryList.AppendLine($"<td>Moto {riderMoto1?.raceNumber} -- Gate {riderMoto1?.findRiderGate(rider.id)}</td>");
                entryList.AppendLine($"<td>Moto {riderMoto2?.raceNumber} -- Gate {riderMoto2?.findRiderGate(rider.id)}</td>");
                entryList.AppendLine($"<td>Moto {riderMoto3?.raceNumber} -- Gate {riderMoto3?.findRiderGate(rider.id)}</td>");
                entryList.AppendLine("</tr>");
            }

            entryList.AppendLine(@"
    </tbody>
</table>");
        }

        entryList.AppendLine(@"
</body>
</html>");

        HTML.WriteFile($"round{round.roundNumber}.motolist", entryList.ToString());
    }

}
