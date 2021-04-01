using System;
using System.Threading.Tasks;

using MagicOnion.Server;

using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;

using MS = MagicOnion.Server;
using ServiceContext = Spear.Inf.Core.ServGeneric.ServiceContext;

namespace Spear.Inf.Core.Attr
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class GRPCFilterAttribute : MagicOnionFilterAttribute
    {
        public override async ValueTask Invoke(MS.ServiceContext context, Func<MS.ServiceContext, ValueTask> next)
        {
            await new GRPCFilterRunner(context, next).Run();
        }
    }

    public class GRPCFilterRunner
    {
        private readonly MS.ServiceContext _context;
        private readonly Func<MS.ServiceContext, ValueTask> _next;
        private readonly IRequestFilterHandle _filters;

        public GRPCFilterRunner(MS.ServiceContext context, Func<MS.ServiceContext, ValueTask> next)
        {
            _context = context;
            _next = next;
            _filters = ServiceContext.ResolveServByKeyed<IRequestFilterHandle>(Enum_FilterType.GRPC);
        }

        public async Task<bool> Run()
        {
            try
            {
                foreach (var filter in _filters.FilterItems)
                    filter.OnExecuting(_context);

                await _next(_context);
            }
            catch (Exception ex)
            {
                foreach (var filter in _filters.FilterItems)
                    filter.OnException(_context, ex);
            }

            foreach (var filter in _filters.FilterItems)
                filter.OnExecuted(_context);

            foreach (var filter in _filters.FilterItems)
                filter.OnExit(_context);

            return true;
        }
    }
}
