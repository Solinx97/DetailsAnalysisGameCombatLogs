﻿using AutoMapper;
using CombatAnalysis.UserBL.DTO;
using CombatAnalysis.Identity.DTO;
using CombatAnalysisIdentity.Models;

namespace CombatAnalysisIdentity.Mapping;

internal class CombatAnalysisIdentityMapper : Profile
{
    public CombatAnalysisIdentityMapper()
    {
        CreateMap<IdentityUserDto, IdentityUserModel>().ReverseMap();
        CreateMap<AppUserDto, AppUserModel>().ReverseMap();
        CreateMap<CustomerDto, CustomerModel>().ReverseMap();
    }
}
