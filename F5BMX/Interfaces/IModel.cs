using F5BMX.Core;

namespace F5BMX.Interfaces;

internal interface IModel
{

    T Clone<T>() where T : IModel;
    void Save(string fileName);

}
