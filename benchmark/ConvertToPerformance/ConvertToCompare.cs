using System;
using System.Collections.Generic;
using AutoMapper;
using BenchmarkDotNet.Attributes;
using FlyTiger;
using System.Text.Json;
namespace ConvertToPerformance
{
    public class ConvertToCompare
    {
        private readonly IMapper _mapper;
        public ConvertToCompare()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserInfo, UserDto>();
                cfg.CreateMap<RoleInfo, RoleDto>();
                cfg.CreateMap<AddressInfo, AddressDto>();
            });
            _mapper = config.CreateMapper();

            userInfo10 =JsonSerializer.Deserialize<UserInfo[]>(JsonSerializer.Serialize(Enumerable.Repeat(userInfo,10)));
            userInfo100 =JsonSerializer.Deserialize<UserInfo[]>(JsonSerializer.Serialize(Enumerable.Repeat(userInfo,100)));
            userInfo1000 =JsonSerializer.Deserialize<UserInfo[]>(JsonSerializer.Serialize(Enumerable.Repeat(userInfo,1000)));
        }
        UserInfo userInfo = new UserInfo
        {
            Name = "zhangsan",
            Birthday = DateTime.Parse("2000-11-11"),
            Role = new RoleInfo { Name = "admin" },
            Addresses = new List<AddressInfo> {
                 new AddressInfo {
                     Province = "shan'xi",
                     City ="xi'an",
                     Street ="da zhai lu",
                     Tel = "13666666666"
                 }
             }
        };

        UserInfo[] userInfo10;
        UserInfo[] userInfo100;

        UserInfo[] userInfo1000;
        [Benchmark]
        public void MapSingleUseFlyTiger()
        {
            _ = userInfo.To<UserDto>();
        }

        [Benchmark]
        public void MapSingleUseAutoMapper()
        {
            _ = _mapper.Map<UserDto>(userInfo);
        }

        [Benchmark]
        public void Map10ObjectUseFlyTiger()
        {
            _ = userInfo10.To<UserDto>().ToArray();
        }
        [Benchmark]
        public void Map10ObjectUseAutoMapper()
        {
            _ = _mapper.Map<UserInfo[], IEnumerable<UserDto>>(userInfo10).ToArray();
        }

        [Benchmark]
        public void Map100ObjectUseFlyTiger()
        {
            _ = userInfo100.To<UserDto>().ToArray();
        }
        [Benchmark]
        public void Map100ObjectUseAutoMapper()
        {
            _ = _mapper.Map<UserInfo[], IEnumerable<UserDto>>(userInfo100).ToArray();
        }

        [Benchmark]
        public void Map1000ObjectUseFlyTiger()
        {
            _ = userInfo1000.To<UserDto>().ToArray();
        }
        [Benchmark]
        public void Map1000ObjectUseAutoMapper()
        {
            _ = _mapper.Map<UserInfo[], IEnumerable<UserDto>>(userInfo1000).ToArray();
        }
    }
}
