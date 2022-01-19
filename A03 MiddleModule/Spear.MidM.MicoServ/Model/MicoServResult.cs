namespace Spear.MidM.MicoServ
{
    public abstract class MicoServResult<TData>
    {
        public bool IsSuccess { get; set; }

        public string Code { get; set; }

        public string Msg { get; set; }

        public TData Data { get; set; }

        public string ErrorStackTrace { get; set; }
    }
}
