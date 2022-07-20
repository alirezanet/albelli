using AutoMapper;

namespace Application.Test;

public class MapperFixture<T> where T : Profile
{
    public readonly IMapper Mapper;

    public MapperFixture()
    {
        Mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(T));

        }).CreateMapper();
    }
}

public class MapperFixture<T, T2> where T : Profile where T2 : Profile

{
    public readonly IMapper Mapper;

    public MapperFixture()
    {
        Mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(T));
            cfg.AddProfile(typeof(T2));
        }).CreateMapper();
    }
}

 