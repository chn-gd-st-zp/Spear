using System.Threading.Tasks;

namespace Spear.Inf.Core.Interface
{
    public interface ISpearAppLifeTime
    {
        Task Starting();

        Task Started();

        Task Stopping();

        Task stopped();
    }
}
