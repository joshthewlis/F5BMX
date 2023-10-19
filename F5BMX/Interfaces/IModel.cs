using F5BMX.Core;

namespace F5BMX.Interfaces;

internal interface IModel
{

    T clone<T>() where T : IModel;

}
