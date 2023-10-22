using F5BMX.Core.IO;
using F5BMX.Models;
using F5BMX.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace F5BMX.Helpers;

internal static class Motos
{

    private static uint[,] getGateNumbers(int numberOfGates)
    {
        switch (numberOfGates)
        {
            case 8:
                return new uint[,] {
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
                return new uint[,] {
                        { 1, 4, 6},
                        { 2, 3, 5},
                        { 3, 6, 1},
                        { 4, 5, 2},
                        { 5, 2, 3},
                        { 6, 1, 4}
                    };

            case 4:
                return new uint[,] {
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
        var gateNumbers = getGateNumbers(round.numberOfGates);
        var formulaRaceOrder = round.formulas.OrderBy(x => x.order);

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

                if (round.numberOfMotos == 3)
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

                if (round.numberOfMotos == 3)
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

    public static void Finalize(List<MotoRaceResult> raceResults)
    {
        foreach(var raceResult in raceResults)
            foreach(var riderResult in raceResult.gates)
                riderResult.Value.rider.motoPositions[raceResult.motoRound-1] = riderResult.Value.result;
    }

    public static void GenerateListing(Series series, Round round)
    {
        StringBuilder html = new StringBuilder();
        html.Append(@"
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

        border-collapse: collapse;
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
        border-bottom: solid 1px black;
    }

    table tbody tr:nth-child(odd) td {
        background-color: lightgrey;
    }
    </style>
</head>

<body>
<h1>");
        html.AppendFormat("F5BMX - {0} - {1}", series.year, series.name);
        html.Append(@"</h1><h2>");
        html.AppendFormat("Round {0} - Moto Listings", round.roundNumber);
        html.Append(@"</h2>");

        foreach (var formula in round.formulas.OrderByDescending(x => x.order))
        {
            // SKIP FORMULAS WITH NO RIDERS
            if (formula.riders.Count == 0)
                continue;

            html.AppendFormat(@"
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

                html.AppendLine("<tr>");
                html.AppendLine($"<td>{rider.firstName} {rider.lastName}</td>");
                html.AppendLine($"<td>{rider.plateNumber}</td>");
                html.AppendLine($"<td>Moto {riderMoto1?.raceNumber} -- Gate {riderMoto1?.findRiderGate(rider.id)}</td>");
                html.AppendLine($"<td>Moto {riderMoto2?.raceNumber} -- Gate {riderMoto2?.findRiderGate(rider.id)}</td>");
                html.AppendLine($"<td>Moto {riderMoto3?.raceNumber} -- Gate {riderMoto3?.findRiderGate(rider.id)}</td>");
                html.AppendLine("</tr>");
            }

            html.AppendLine(@"
    </tbody>
</table>");
        }

        html.AppendLine(@"
</body>
</html>");

        HTML.WriteFile($"round{round.roundNumber}.motolist", html.ToString());
    }

    public static void GenerateCommentary(Round round)
    {
        StringBuilder html = new StringBuilder();
        html.Append(@"
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
        width: 100%;
        page-break-inside: avoid;
    margin-bottom: 10mm;

        border-collapse: collapse;
    }

    table thead tr.formulaName td {
        font-size: 1.8em;
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
        font-size: 1.6em;
        text-align: center;
        border-top: solid 1px black;
        border-bottom: solid 1px black;
        height: 15mm;
    }

    table tbody tr:nth-child(odd) td {
        background-color: lightgrey;
    }
    </style>
</head>

<body>");

        for (int moto = 1; moto <= round.numberOfMotos; moto++)
        {
            foreach (var formula in round.formulas.OrderBy(x => x.order))
            {
                List<Race> races = new List<Race>();

                if (moto == 1)
                    races = formula.moto1;
                else if (moto == 2)
                    races = formula.moto2;
                else if (moto == 3)
                    races = formula.moto3;

                foreach (var race in races.OrderBy(x => x.raceNumber))
                {
                    // SKIP FORMULAS WITH NO RIDERS
                    if (formula.riders.Count == 0)
                        continue;

                    html.AppendFormat(@"
<table>
    <thead>
        <tr class=""formulaName"">
            <td colspan=""4"">Moto {0} - {1}</td>
        </tr>
        <tr class=""tableHeading"">
            <td width=""20%"">Plate</td>
            <td width=""50%"">Name</td>
            <td width=""30%"">Club</td>
        </tr>
    </thead>
    <tbody>", race.raceNumber, formula.name);

                    for (uint gate = 1; gate <= round.numberOfGates; gate++)
                    {
                        RoundRider rider;
                        if (race.gates.ContainsKey(gate))
                            rider = formula.riders.Where(x => x.id == race.gates[gate]).First();
                        else
                            rider = new RoundRider();

                        html.AppendLine("<tr>");
                        html.AppendLine($"<td>{rider.plateNumber}</td>");
                        html.AppendLine($"<td>{rider.firstName} {rider.lastName}</td>");
                        html.AppendLine($"<td>{rider.club}</td>");
                        html.AppendLine("</tr>");
                    }

                    html.AppendLine(@"
    </tbody>
</table>");
                }
            }
        }

        html.AppendLine(@"
</body>
</html>");

        HTML.WriteFile($"round{round.roundNumber}.motocommentary", html.ToString());
    }


    public static void GenerateCallup(Round round)
    {
        StringBuilder html = new StringBuilder();
        html.Append(@"
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
        width: 100%;
        page-break-inside: avoid;
        margin-bottom: 10mm;

        border-collapse: collapse;
    }

    table thead tr.formulaName td {
        font-size: 1.6em;
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
        border-bottom: solid 1px black;
    }

    table tbody tr:nth-child(odd) td {
        background-color: lightgrey;
    }
    </style>
</head>

<body>");

        for (int moto = 1; moto <= round.numberOfMotos; moto++)
        {
            foreach (var formula in round.formulas.OrderBy(x => x.order))
            {
                List<Race> races = new List<Race>();

                if (moto == 1)
                    races = formula.moto1;
                else if (moto == 2)
                    races = formula.moto2;
                else if (moto == 3)
                    races = formula.moto3;

                foreach (var race in races.OrderBy(x => x.raceNumber))
                {
                    // SKIP FORMULAS WITH NO RIDERS
                    if (formula.riders.Count == 0)
                        continue;

                    html.AppendFormat(@"
<table>
    <thead>
        <tr class=""formulaName"">
            <td colspan=""4"">Moto {0} - {1}</td>
        </tr>
        <tr class=""tableHeading"">
            <td width=""15%"">Gate</td>
            <td width=""15%"">Plate</td>
            <td width=""35%"">Name</td>
            <td width=""35%"">Club</td>
        </tr>
    </thead>
    <tbody>", race.raceNumber, formula.name);

                    for (uint gate = 1; gate <= round.numberOfGates; gate++)
                    {
                        RoundRider rider;
                        if (race.gates.ContainsKey(gate))
                            rider = formula.riders.Where(x => x.id == race.gates[gate]).First();
                        else
                            rider = new RoundRider();

                        html.AppendLine("<tr>");
                        html.AppendLine($"<td>{gate}</td>");
                        html.AppendLine($"<td>{rider.plateNumber}</td>");
                        html.AppendLine($"<td>{rider.firstName} {rider.lastName}</td>");
                        html.AppendLine($"<td>{rider.club}</td>");
                        html.AppendLine("</tr>");
                    }

                    html.AppendLine(@"
    </tbody>
</table>");
                }
            }
        }

        html.AppendLine(@"
</body>
</html>");

        HTML.WriteFile($"round{round.roundNumber}.motocallup", html.ToString());
    }

}
