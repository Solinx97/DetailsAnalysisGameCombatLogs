﻿using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.CombatParserAPI.Models;

namespace CombatAnalysis.CombatParserAPI.Mapping
{
    public class ApiMapper : Profile
    {
        public ApiMapper()
        {
            CreateMap<CombatLogDto, CombatLogModel>().ReverseMap();
            CreateMap<CombatDto, CombatModel>().ReverseMap();
            CreateMap<CombatPlayerDataDto, CombatPlayerDataModel>().ReverseMap();
            CreateMap<DamageDoneDto, DamageDoneModel>().ReverseMap();
            CreateMap<DamageDoneGeneralDto, DamageDoneGeneralModel>().ReverseMap();
            CreateMap<HealDoneDto, HealDoneModel>().ReverseMap();
            CreateMap<HealDoneGeneralDto, HealDoneGeneralModel>().ReverseMap();
            CreateMap<DamageTakenDto, DamageTakenModel>().ReverseMap();
            CreateMap<DamageTakenGeneralDto, DamageTakenGeneralModel>().ReverseMap();
            CreateMap<ResourceRecoveryDto, ResourceRecoveryModel>().ReverseMap();
            CreateMap<ResourceRecoveryGeneralDto, ResourceRecoveryGeneralModel>().ReverseMap();
        }
    }
}