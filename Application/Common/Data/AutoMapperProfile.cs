using AutoMapper;
using Domain.Group.Entities;
using Domain.Group.Models.Dtos;
using Domain.Group.Models.Enums;
using Domain.Group.Models.ValueObjects;
using Domain.Task.Models.Dtos;
using Domain.Task.Models.ValueObjects;
using Domain.User.Models.Dtos;

namespace Application.Common.Data;
using Group = Domain.Group.Entities.Group;
using Task = Domain.Task.Entities.Task;
using User = Domain.User.Entities.User;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<GroupId, Guid>();
        CreateMap<Group, GroupDto>()
            .ForMember(dto => dto.GroupId,opt => opt.MapFrom(group => group.Id.Value)).ForMember(dto => dto.Creator,opt => opt.MapFrom(group => group.Members.FirstOrDefault(member => member.Role == Role.Admin)));
        CreateMap<Invitation, InvitationDto>().ForMember(dto => dto.TargetEmail,opt => opt.MapFrom(invitation => invitation.User.Email))
            .ForMember(dto => dto.InvitationId,opt => opt.MapFrom(invitation => invitation.Id.Value));
        CreateMap<Member, MemberDto>()
            .ForMember(
                dto => dto.Name,
                opt => opt.MapFrom(member => member.User.Name))
            .ForMember(
                dto => dto.Surname,
                opt => opt.MapFrom(member => member.User.Surname));
        CreateMap<User, UserDto>();
        CreateMap<Task, TaskDto>();
    }
}