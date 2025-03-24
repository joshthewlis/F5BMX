using F5BMX.Core.IO;
using F5BMX.Models;
using F5BMX.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace F5BMX.Helpers;

internal static class Finals
{

    private static uint[] pointsAllocation =
    {
        90,
        86,
        82,
        78,
        75,
        73,
        71,
        69,
        67,
        65,
        63,
        61,
        59,
        57,
        55,
        53,
        51,
        49,
        47,
        45,
        43,
        41,
        39,
        37,
        35,
        34,
        33,
        32,
        31,
        30,
        29,
        28,
        27,
        26,
        25,
        24,
        23,
        22,
        21,
        20,
        19,
        18,
        17,
        16,
        15,
        14,
        13,
        12,
        11,
        10,
        9,
        8,
        7,
        6,
        5,
        4,
        3
    };

    public static void Generate(Round round)
    {

        var formulaRaceOrder = round.formulas.OrderBy(x => x.Order);

        // ASSIGN RIDERS
        foreach (var formula in formulaRaceOrder)
        {
            // CLEAR IF THERE ARE ANY EXISTING FINALS
            formula.Final.Clear();

            // GENERATE THE RACE CLASSES
            int numberOfRaces = (int)Math.Ceiling((double)formula.Riders.Count / round.NumberOfGates);

            for (uint i = 0; i < numberOfRaces; i++)
                formula.Final.Add(new Race() { FinalNumber = i });

            // FILL THE RACES
            var riders = formula.Riders.OrderBy(x => x.MotoPositions.Sum(x => (int)x)).ThenBy(x => x.MotoPositions[2]).ThenBy(x => x.MotoPositions[1]).ThenBy(x => x.MotoPositions[0]).ToList();
            int riderRace = 0;
            uint riderGate = 1;
            foreach (var rider in riders)
            {
                formula.Final[riderRace].Gates[riderGate] = rider.ID;
                riderGate++;
                if (riderGate > round.NumberOfGates)
                {
                    riderGate = 1;
                    riderRace++;
                }
            }
        }

        // NUMBER THE RACES
        int raceNumber = 1;
        foreach (var formula in formulaRaceOrder)
        {
            foreach (var race in formula.Final.OrderByDescending(x => x.FinalNumber))
            {
                race.RaceNumber = raceNumber;
                raceNumber++;
            }
        }
    }

    public static void Finalize(Series series, Round round, List<RaceResult> raceResults)
    {
        foreach (var formula in round.formulas)
        {
            // IGNORE FORMULAS WITH NO RIDERS
            if (formula.Riders.Count == 0)
                continue;

            // ADD SERIES POINTS
            foreach (var race in formula.Final)
            {
                var raceResult = raceResults.Where(x => x.raceNumber == race.RaceNumber).First();

                var startingPosition = race.FinalNumber * round.NumberOfGates;

                foreach (var riderResult in raceResult.gates.Values)
                {
                    var seriesRider = series.Riders.Where(x => x.id == riderResult.rider.id).First();

                    var position = startingPosition + riderResult.result;
                    if (position > pointsAllocation.Length - 1)
                        position = (uint)pointsAllocation.Length - 1; // ENSURES EVERYONE GETS MINIMUM 3 POINTS

                    riderResult.rider.finalPosition = position;
                    riderResult.rider.roundPoints = pointsAllocation[position - 1];
                    seriesRider.seriesPoints += pointsAllocation[position - 1];
                }
            }

            // TRY MOVE FORMULAS
            if (round.FinalRound == false)
            {
                var seriesFormula = series.Formulas.Where(x => x.id == formula.ID).First();
                if (seriesFormula.promotion == true)
                {
                    // ATTEMPT TO MOVE POSITION 1 RIDER UP A FORMULA
                    var firstRider = formula.Riders.Where(x => x.FinalPosition == 1).First();
                    var nextFormula = series.Formulas.Where(x => x.order == formula.Order + 1).FirstOrDefault();
                    if (nextFormula != null)
                    {
                        if (nextFormula.promotion == true)
                        {
                            var seriesRider = series.Riders.Where(x => x.id == firstRider.ID).First();

                            firstRider.Promotion = Enums.PromotionEnum.Up;
                            seriesRider.formulaID = nextFormula.id;
                        }
                    }

                    // ATTEMPT TO MOVE LAST POSITION DOWN A FORMULA
                    var lastRider = formula.Riders.Where(x => x.FinalPosition == formula.Riders.Count).First();
                    var prevFormula = series.Formulas.Where(x => x.order == formula.Order - 1).FirstOrDefault();
                    if (prevFormula != null)
                    {
                        if (prevFormula.promotion == true)
                        {
                            var seriesRider = series.Riders.Where(x => x.id == lastRider.ID).First();

                            lastRider.Promotion = Enums.PromotionEnum.Down;
                            seriesRider.formulaID = prevFormula.id;
                        }
                    }
                }
            }

            // DASH FOR CASH
            if(series.DashForCash == true)
            {

            }
        }

        // Save Changes to Series Riders
        JSON.WriteFile("riders", series.Riders);
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

<body><h1>");
        html.AppendFormat("F5BMX - {0} - {1}", series.Year, series.Name);
        html.Append(@"</h1><h2>");
        html.AppendFormat("Round {0} - Final Listings", round.RoundNumber);
        html.Append(@"</h2>");

        foreach (var formula in round.formulas.OrderBy(x => x.Order))
        {
            foreach (var race in formula.Final.OrderBy(x => x.RaceNumber))
            {
                // SKIP FORMULAS WITH NO RIDERS
                if (formula.Riders.Count == 0)
                    continue;

                html.AppendFormat(@"
<table>
    <thead>
        <tr class=""formulaName"">
            <td colspan=""5"">Final {0} - {1} - {2} Final</td>
        </tr>
        <tr class=""tableHeading"">
            <td width=""12%"">Pick</td>
            <td width=""12%"">Plate</td>
            <td width=""36%"">Name</td>
            <td width=""20%"">Results</td>
            <td width=""20%"">Club</td>
        </tr>
    </thead>
    <tbody>", race.RaceNumber, formula.Name, (char)(65 + race.FinalNumber));

                for (uint gate = 1; gate <= round.NumberOfGates; gate++)
                {
                    if (race.Gates.ContainsKey(gate) == false)
                        continue;

                    var rider = formula.Riders.Where(x => x.ID == race.Gates[gate]).First();

                    html.AppendLine("<tr>");
                    html.AppendLine($"<td>{gate}</td>");
                    html.AppendLine($"<td>{rider.PlateNumber}</td>");
                    html.AppendLine($"<td>{rider.FirstName} {rider.LastName}</td>");
                    html.AppendLine($"<td>{rider.MotoPositions[0]} - {rider.MotoPositions[1]} - {rider.MotoPositions[2]} ({rider.MotoPositions.Sum(x => (int)x)})</td>");
                    html.AppendLine($"<td>{rider.Club}</td>");
                    html.AppendLine("</tr>");
                }

                html.AppendLine(@"
    </tbody>
</table>");
            }
        }

        html.AppendLine(@"
</body>
</html>");

        HTML.WriteFile($"round{round.RoundNumber}.finallist", html.ToString());
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

        foreach (var formula in round.formulas.OrderBy(x => x.Order))
        {
            foreach (var race in formula.Final.OrderByDescending(x => x.FinalNumber))
            {
                // SKIP FORMULAS WITH NO RIDERS
                if (formula.Riders.Count == 0)
                    continue;

                html.AppendFormat(@"
<table>
    <thead>
        <tr class=""formulaName"">
            <td colspan=""4"">Final {0} - {1} - {2} Final</td>
        </tr>
        <tr class=""tableHeading"">
            <td width=""20%"">Plate</td>
            <td width=""50%"">Name</td>
            <td width=""30%"">Club</td>
        </tr>
    </thead>
    <tbody>", race.RaceNumber, formula.Name, (char)(65 + race.FinalNumber));

                for (uint gate = 1; gate <= round.NumberOfGates; gate++)
                {
                    RoundRider rider;
                    if (race.Gates.ContainsKey(gate))
                        rider = formula.Riders.Where(x => x.ID == race.Gates[gate]).First();
                    else
                        rider = new RoundRider();

                    html.AppendLine("<tr>");
                    html.AppendLine($"<td>{rider.PlateNumber}</td>");
                    html.AppendLine($"<td>{rider.FirstName} {rider.LastName}</td>");
                    html.AppendLine($"<td>{rider.Club}</td>");
                    html.AppendLine("</tr>");
                }

                html.AppendLine(@"
    </tbody>
</table>");
            }
        }

        html.AppendLine(@"
</body>
</html>");

        HTML.WriteFile($"round{round.RoundNumber}.finalcommentary", html.ToString());
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

        foreach (var formula in round.formulas.OrderBy(x => x.Order))
        {
            foreach (var race in formula.Final.OrderByDescending(x => x.FinalNumber))
            {
                // SKIP FORMULAS WITH NO RIDERS
                if (formula.Riders.Count == 0)
                    continue;

                html.AppendFormat(@"
<table>
    <thead>
        <tr class=""formulaName"">
            <td colspan=""4"">Final {0} - {1} - {2} Final</td>
        </tr>
        <tr class=""tableHeading"">
            <td width=""15%"">Pick</td>
            <td width=""15%"">Plate</td>
            <td width=""35%"">Name</td>
            <td width=""35%"">Club</td>
        </tr>
    </thead>
    <tbody>", race.RaceNumber, formula.Name, (char)(65 + race.FinalNumber));

                for (uint gate = 1; gate <= round.NumberOfGates; gate++)
                {
                    RoundRider rider;
                    if (race.Gates.ContainsKey(gate))
                        rider = formula.Riders.Where(x => x.ID == race.Gates[gate]).First();
                    else
                        rider = new RoundRider();

                    html.AppendLine("<tr>");
                    html.AppendLine($"<td>{gate}</td>");
                    html.AppendLine($"<td>{rider.PlateNumber}</td>");
                    html.AppendLine($"<td>{rider.FirstName} {rider.LastName}</td>");
                    html.AppendLine($"<td>{rider.Club}</td>");
                    html.AppendLine("</tr>");
                }

                html.AppendLine(@"
    </tbody>
</table>");
            }
        }

        html.AppendLine(@"
</body>
</html>");

        HTML.WriteFile($"round{round.RoundNumber}.finalcallup", html.ToString());
    }

}