﻿using F5BMX.Core.IO;
using F5BMX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace F5BMX.Helpers;

internal static class Finals
{

    public static void Generate(Round round)
    {

        var formulaRaceOrder = round.formulas.OrderBy(x => x.order);

        // ASSIGN RIDERS
        foreach (var formula in formulaRaceOrder)
        {
            // CLEAR IF THERE ARE ANY EXISTING FINALS
            formula.final.Clear();

            // GENERATE THE RACE CLASSES
            int numberOfRaces = (int)Math.Ceiling((double)formula.riders.Count / round.numberOfGates);

            for (int i = 0; i < numberOfRaces; i++)
                formula.final.Add(new Race() { finalNumber = i });

            // FILL THE RACES
            var riders = formula.riders.OrderBy(x => x.motoPositions.Sum(x => (int)x)).ThenBy(x => x.motoPositions[2]).ThenBy(x => x.motoPositions[1]).ThenBy(x => x.motoPositions[0]).ToList();
            int riderRace = 0;
            uint riderGate = 1;
            foreach (var rider in riders)
            {
                formula.final[riderRace].gates[riderGate] = rider.id;
                riderGate++;
                if (riderGate > round.numberOfGates)
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
            foreach (var race in formula.final.OrderByDescending(x => x.finalNumber))
            {
                race.raceNumber = raceNumber;
                raceNumber++;
            }
        }
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
        html.AppendFormat("F5BMX - {0} - {1}", series.year, series.name);
        html.Append(@"</h1><h2>");
        html.AppendFormat("Round {0} - Final Listings", round.roundNumber);
        html.Append(@"</h2>");

        foreach (var formula in round.formulas.OrderBy(x => x.order))
        {
            foreach (var race in formula.final.OrderBy(x => x.raceNumber))
            {
                // SKIP FORMULAS WITH NO RIDERS
                if (formula.riders.Count == 0)
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
    <tbody>", race.raceNumber, formula.name, (char)(65 + race.finalNumber));

                for (uint gate = 1; gate <= round.numberOfGates; gate++)
                {
                    if (race.gates.ContainsKey(gate) == false)
                        continue;

                    var rider = formula.riders.Where(x => x.id == race.gates[gate]).First();

                    html.AppendLine("<tr>");
                    html.AppendLine($"<td>{gate}</td>");
                    html.AppendLine($"<td>{rider.plateNumber}</td>");
                    html.AppendLine($"<td>{rider.firstName} {rider.lastName}</td>");
                    html.AppendLine($"<td>{rider.motoPositions[0]} - {rider.motoPositions[1]} - {rider.motoPositions[2]} ({rider.motoPositions.Sum(x => (int)x)})</td>");
                    html.AppendLine($"<td>{rider.club}</td>");
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

        HTML.WriteFile($"round{round.roundNumber}.finallist", html.ToString());
    }

}