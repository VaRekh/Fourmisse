using System;

namespace Assets.Library.StateMachines
{
    public interface IFactory<TStateCode, TInfo>
        where TStateCode : Enum
    {
        State<TStateCode, TInfo> Get(TStateCode code, Updater<TStateCode, TInfo> updater, TInfo info);
    }
}