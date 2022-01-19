using Spear.Inf.Core.Interface;

using MO = MagicOnion;

namespace Spear.MidM.MicoServ.MagicOnion
{
    public interface IMagicOnionContainer<TType> : IMicoServContainer, MO.IService<TType>
    {
        //
    }
}
