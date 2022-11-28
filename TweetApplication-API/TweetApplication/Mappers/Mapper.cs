using AutoMapper;
using com.tweetapp.Models;
using com.tweetapp.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.tweetapp.Mappers
{
    public class Mapper: Profile
    {
        public Mapper()
        {
            CreateMap<ViewUserDto, User>().ReverseMap();

            CreateMap<PostNewTweetDto, Tweet>();
        }
    }
}
