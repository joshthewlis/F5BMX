using F5BMX.Core.IO;
using F5BMX.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace F5BMX.Helpers;

internal static class Registration
{

    public static void GenerateEntryList(Series series, Round round)
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
    </style>
</head>

<body>
<h1>");
        entryList.AppendFormat("F5BMX - {0} - {1}", series.Year, series.Name);
        entryList.Append(@"</h1><h2>");
        entryList.AppendFormat("Round {0} - Entry List", round.RoundNumber);
        entryList.Append(@"</h2>");

        foreach (var formula in round.Formulas)
        {
            // SKIP FORMULAS WITH NO RIDERS
            if (formula.Riders.Count == 0)
                continue;

            entryList.AppendFormat(@"
<table>
    <thead>
        <tr class=""formulaName"">
            <td colspan=""4"">{0} ~ {1} Riders</td>
        </tr>
        <tr class=""tableHeading"">
            <td width=""30%"">Name</td>
            <td width=""30%"">Club / Team</td>
            <td width=""20%"">Number</td>
            <td width=""20%"">Series Points</td>
        </tr>
    </thead>
    <tbody>", formula.Name, formula.Riders.Count);

            foreach (var rider in formula.Riders)
            {
                entryList.AppendLine("<tr>");
                entryList.AppendLine($"<td>{rider.FirstName} {rider.LastName}</td>");
                entryList.AppendLine($"<td>{rider.Club}</td>");
                entryList.AppendLine($"<td>{rider.PlateNumber}</td>");
                entryList.AppendLine($"<td>{series.Riders.FirstOrDefault(x => x.ID == rider.ID)?.SeriesPoints}</td>");
                entryList.AppendLine("</tr>");
            }

            entryList.AppendLine(@"
    </tbody>
</table>");
        }

        entryList.AppendLine(@"
</body>
</html>");

        HTML.WriteFile($"round{round.RoundNumber}.entrylist", entryList.ToString());
    }

}
