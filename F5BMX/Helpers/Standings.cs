using F5BMX.Core.IO;
using F5BMX.Enums;
using F5BMX.Models;
using System;
using System.Linq;
using System.Text;

namespace F5BMX.Helpers;

internal static class Standings
{

    public static void Round(Series series, Round round)
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

<body>
<h1>F5BMX - Round Standings</h1>");
        html.AppendFormat("<h2>{0} - {1} - Round {2} - {3}</h2>", series.year, series.name, round.roundNumber, round.date);

        foreach (var formula in round.formulas.OrderByDescending(x => x.order))
        {
            // SKIP FORMULAS WITH NO RIDERS
            if (formula.riders.Count == 0)
                continue;

            html.AppendFormat(@"
<table>
    <thead>
        <tr class=""formulaName"">
            <td colspan=""4"">{0} ~ {1} Riders</td>
        </tr>
        <tr class=""tableHeading"">
            <td width=""40%"">Name</td>
            <td width=""30%"">Club</td>
            <td width=""10%"">Points</td>
            <td width=""20%"">Remarks</td>
        </tr>
    </thead>
    <tbody>", formula.name, formula.riders.Count);

            foreach(var rider in formula.riders.OrderBy(x => x.finalPosition))
            {
                string promotion = String.Empty;
                if (rider.promotion == PromotionEnum.Up)
                    promotion = "Promoted";
                else if (rider.promotion == PromotionEnum.Down)
                    promotion = "Demoted";

                html.AppendLine("<tr>");
                html.AppendLine($"<td>{rider.firstName} {rider.lastName}</td>");
                html.AppendLine($"<td>{rider.club}</td>");
                html.AppendLine($"<td>{rider.roundPoints}</td>");
                html.AppendLine($"<td>{promotion}</td>");
                html.AppendLine("</tr>");
            }

            html.AppendLine(@"
    </tbody>
</table>");
        }

        html.AppendLine(@"
</body>
</html>");

        HTML.WriteFile($"round{round.roundNumber}.standings", html.ToString());
    }

    public static void Series(Series series)
    {

    }

}
