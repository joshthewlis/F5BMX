using F5BMX.Core.IO;
using F5BMX.Interfaces;
using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace F5BMX.Core;

internal class ModelBase : IModel
{

    public T Clone<T>() where T : IModel
    {
        return (T)this.MemberwiseClone();
    }

    public virtual void Save(string? fileName = null)
    {
        if (fileName == null)
            fileName = this.GetType().Name;

        JSON.WriteFile(fileName, this);
    }

}
