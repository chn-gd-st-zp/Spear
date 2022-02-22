using MessagePack;

namespace Spear.Inf.Core.DTO
{
    [MessagePackObject(true)]
    public class IDTO_GRPCContext : IDTO
    {
        public string Token { get; set; }
    }

    [MessagePackObject(true)]
    public class IDTO_GRPC : IDTO
    {
        public IDTO_GRPCContext GRPCContext { get; set; }
    }

    [MessagePackObject(true)]
    public class IDTO_GRPC<TInput> : IDTO_GRPC
        where TInput : IDTO_Input
    {
        public TInput Param { get; set; }
    }
}
