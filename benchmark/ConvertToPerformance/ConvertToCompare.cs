﻿using System;
using System.Collections.Generic;
using AutoMapper;
using BenchmarkDotNet.Attributes;
using FlyTiger;
namespace ConvertToPerformance
{
    public class ConvertToCompare
    {
        private readonly IMapper _mapper;
        public ConvertToCompare()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserInfo, UserDto>());
            _mapper = config.CreateMapper();
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
    }
}
