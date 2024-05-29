using AutoMapper;
using Domain.Group.Entities;
using Domain.Group.Models.Dtos;
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
        CreateMap<Group, GroupDto>();
        CreateMap<Invitation, InvitationDto>();
        CreateMap<Member, MemberDto>();
        CreateMap<User, UserDto>();
        CreateMap<Task, TaskDto>();
    }
}