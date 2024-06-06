using Application.Authentication.Model;
using AutoMapper;
using Domain.Group.Models.Dtos;
using Domain.Task.Models.Dtos;
using Domain.User.Models.Dtos;
using Presentation.Model;
using Presentation.Model.Requests;
using Presentation.Model.Responses;
using LoginRequest = Presentation.Model.Requests.LoginRequest;

namespace Presentation;

public class MapperPresentationProfile : Profile
{
    public MapperPresentationProfile()
    {
        CreateMap<CreateGroupRequest, CreateGroupDto>();
        CreateMap<CreateTaskRequest, CreateTaskDto>();
        CreateMap<InviteUserRequest, CreateInvitationDto>();
        CreateMap<LoginRequest, LoginUserDto>();
        CreateMap<PasswordUpdateRequest, UpdatePasswordDto>();
        CreateMap<RegisterRequest, RegisterUserDto>();
        CreateMap<UpdateTaskRequest, UpdateTaskDto>();
        CreateMap<LoginResultDto, LoginResponse>();
        CreateMap<RegisterResultDto, RegisterResponse>();
    }
}