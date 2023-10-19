using F5BMX.Interfaces;

namespace F5BMX.Core;

internal class ModelBase : IModel
{

    public T clone<T>() where T : IModel
    {
        return (T)this.MemberwiseClone();
    }

}
