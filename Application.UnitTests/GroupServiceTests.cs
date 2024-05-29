
using Application.Common.Data;
using Application.Group;
using AutoMapper;
using Domain.Group.Errors;
using Domain.Group.Models.Dtos;
using Domain.Group.Repositories;
using Domain.Shared;
using Domain.Task.Repositories;
using Domain.User.Repositories;
using Moq;
namespace Application.Tests;
using Group = Domain.Group.Entities.Group;
using Task = System.Threading.Tasks.Task;
using User = Domain.User.Entities.User;
public class GroupServiceTests
{
    IMapper _mapper;
    MapperConfiguration _config;

    public GroupServiceTests()
    {
        _config = new MapperConfiguration(expression => expression.AddProfiles([new AutoMapperProfile()]));

        _mapper = _config.CreateMapper();
    }
    
    [Theory]
    [ClassData(typeof(GroupCreateUserAlreadyInGroup))]
    public async Task CreateGroup_Should_ReturnFailure_When_UserAlreadyInOtherGroup(User user)
    {
        var group = Group.Create("Rodmanowie", user.Id);
        var groupRepoMock = new Mock<IGroupRepository>();
        groupRepoMock.Setup(repository => repository.ReadGroupByUserId(user.Id)).ReturnsAsync(group);
        var userRepoMock = new Mock<IUserRepository>();
        userRepoMock.Setup(repository => repository.GetById(user.Id)).ReturnsAsync(user);
        
        var taskRepoMock = new Mock<ITaskRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        
        var groupService = new GroupService(groupRepoMock.Object,userRepoMock.Object, taskRepoMock.Object, unitOfWorkMock.Object, _mapper);

        var result = await groupService.CreateGroup("Kowalscy", user.Id);
        
        Assert.True(result.IsFailure);
        Assert.NotStrictEqual(Result<Group>.Failure(GroupError.UserAlreadyInOtherGroup), result);
    }

    [Theory]
    [ClassData(typeof(GroupCreateUserAlreadyInGroup))]
    public async Task InviteUserByEmail_Should_ReturnFailure_When_UserInvitingHimself(User user)
    {
        var group = Group.Create("Rodmanowie", user.Id);
        var groupRepoMock = new Mock<IGroupRepository>();
        groupRepoMock.Setup(repository => repository.ReadGroupByUserId(user.Id)).ReturnsAsync(group);
        var userRepoMock = new Mock<IUserRepository>();
        userRepoMock.Setup(repository => repository.GetById(user.Id)).ReturnsAsync(user);
        userRepoMock.Setup(repository => repository.GetByEmail(user.Email)).ReturnsAsync(user);
        
        var taskRepoMock = new Mock<ITaskRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        
        var groupService = new GroupService(groupRepoMock.Object,userRepoMock.Object, taskRepoMock.Object, unitOfWorkMock.Object, _mapper);

        var result = await groupService.InviteUserByEmail(user.Email,group.Id, user.Id);
        
        Assert.True(result.IsFailure);
        Assert.NotStrictEqual(Result<InvitationDto>.Failure(GroupError.UserInvitingItself), result);
    }
}

public class GroupCreateUserAlreadyInGroup : TheoryData<User>
{
    public GroupCreateUserAlreadyInGroup()
    { 
        var userResult = User.Create("Jan","Kowalski","j.kowalski@o2.pl","secret");
        var user = userResult.Value;  
    
        Assert.NotNull(user);
        
        Add(user);
    }
}