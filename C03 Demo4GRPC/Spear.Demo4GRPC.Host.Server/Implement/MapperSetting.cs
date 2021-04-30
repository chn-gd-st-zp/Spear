using AutoMapper;

using Spear.Demo4GRPC.Pub.TestDemo;

namespace Spear.Demo4GRPC.Host.Server.Implement
{
    public class MapperSetting : Profile
    {
        public MapperSetting()
        {
            CreateMap<ImportTestDemo, ODTOTestDemo>();
        }
    }
}
