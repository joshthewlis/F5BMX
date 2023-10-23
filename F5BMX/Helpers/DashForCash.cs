using F5BMX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Documents;

namespace F5BMX.Helpers;

internal static class DashForCash
{

    public static Guid RandomDashForCashFormula(Series series)
    {
        var formulasWithDashForCashEnabled = series.formulas.Where(x => x.dashForCash == true).ToList();
        Dictionary<Guid, int> formulaDashCount = new Dictionary<Guid, int>();

        foreach (var item in formulasWithDashForCashEnabled)
            formulaDashCount.Add(item.id, 0);

        foreach (var item in series.rounds)
            if (item.dashForCashFormulaID != null)
                formulaDashCount[item.dashForCashFormulaID.Value]++;

        var formulasToPickFrom = formulaDashCount.Where(x => x.Value == formulaDashCount.Min(x => x.Value)).ToList();

        Random rng = new Random();
        var randomFormula = formulasToPickFrom.OrderBy(x => rng.Next()).First().Key;

        return randomFormula;
    }

}
