using System.Threading.Tasks;

namespace Spear.Inf.Core.Interface
{
    public interface ISpearAppLifeTime
    {
        Task Started(params object[] args);

        Task Stopping(params object[] args);

        Task Stopped(params object[] args);
    }
}
