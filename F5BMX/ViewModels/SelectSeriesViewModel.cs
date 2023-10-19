using F5BMX.Core;
using F5BMX.Core.IO;
using F5BMX.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace F5BMX.ViewModels;

internal class SelectSeriesViewModel : ViewModelBase
{

    public SelectSeriesViewModel()
    {
        Directories.LoadSeries().ForEach((x) =>
        {
            series.Add(new Series(x));
        });
    }

    public ObservableCollection<Series> series { get; init; } = new ObservableCollection<Series>();

    public Series? selectedSeries { get; set; }

}
