using Spear.Inf.Core.Interface;

using MO = MagicOnion;

namespace Spear.MidM.MicoServ.MagicOnion
{
    public interface IMagicOnionContainer : IMicoServContainer
    {
        //
    }

    public interface IMagicOnionContainer<TContainer> : IMagicOnionContainer, IMicoServContainer<TContainer>, MO.IService<TContainer>
    {
        //
    }
}
