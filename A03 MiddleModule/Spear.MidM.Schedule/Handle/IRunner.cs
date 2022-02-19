using System.Threading.Tasks;

namespace Spear.MidM.Schedule
{
    public interface IRunner
    {
        Task Run(string runnerName, params string[] args);
    }

    public interface IRunner4Timer : IRunner { }

    public interface IRunner4BGWorker : IRunner { }

    public interface IRegister4Timer
    {
        Task Regis(IRunner4Timer runner, string cron, TimerParam param);
    }
}
