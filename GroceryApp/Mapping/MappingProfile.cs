using AutoMapper;
using GroceryApp.Models;
using GroceryApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryApp.Extensions
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerForCreateUpdateViewModel>();
            CreateMap<Customer, CustomerForListViewModel>().ReverseMap();
            CreateMap<CustomerForCreateUpdateViewModel, Customer>();
            CreateMap<TransactionForSellViewModel, CustomerTransaction>().ReverseMap();
            CreateMap<TransactionForReceiveViewModel, CustomerTransaction>().ReverseMap();
            CreateMap<TransactionForCreateUpdateViewModel, CustomerTransaction>().ReverseMap(); 
            //CreateMap<UserTask, TaskForListDto>().ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName));
        }
    }
}
