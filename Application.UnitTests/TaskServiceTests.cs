using Application.Common.Data;
using Application.Group;
using Application.Task;
using AutoMapper;
using Domain.Group.Repositories;
using Domain.Shared;
using Domain.Task.Models;
using Domain.Task.Models.Dtos;
using Domain.Task.Repositories;
using Domain.User.Repositories;
using Moq;

namespace Application.Tests;
using User = Domain.User.Entities.User;
using Group = Domain.Group.Entities.Group;

public class TaskServiceTests
{
    
    IMapper _mapper;
    MapperConfiguration _config;

    public TaskServiceTests()
    {
        _config = new MapperConfiguration(expression => expression.AddProfiles([new AutoMapperProfile()]));
 
        _mapper = _config.CreateMapper();
    }
    
    [Theory]
    [ClassData(typeof(TaskOperationData))]
    public async System.Threading.Tasks.Task CreateTask_Should_ReturnSuccess(List<User> users, Group group)
    {
        var user = users.First();
        
        var taskRepoMock = new Mock<ITaskRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var groupRepoMock = new Mock<IGroupRepository>();
        var userRepoMock = new Mock<IUserRepository>();
        userRepoMock.Setup(repository => repository.GetById(user.Id)).ReturnsAsync(user);
      
        Domain.Task.Entities.Task createdTask = null;
        taskRepoMock.Setup(repo => repo.Add(It.IsAny<Domain.Task.Entities.Task>()))
            .Callback<Domain.Task.Entities.Task>(task => createdTask = task);

        var taskService = new TaskService(taskRepoMock.Object, groupRepoMock.Object, userRepoMock.Object, unitOfWorkMock.Object, _mapper);
        var createTaskRequest = new CreateTaskDto("test", "nothing", DateTime.UtcNow, DateTime.UtcNow.AddHours(2));
        var result = await taskService.Create(createTaskRequest, user.Id);
        
        Assert.True(result.IsSuccess);
        Assert.NotNull(createdTask);
        Assert.Single(createdTask.AssignedUsers);
        Assert.Contains(user, createdTask.AssignedUsers);
    }
    
    [Theory]
    [ClassData(typeof(TaskOperationData))]
    public async System.Threading.Tasks.Task CreateTask_Should_AssignUsersEqualToGroupMembers(List<User> users, Group group)
    {
        var firstUser = users.First();
        var groupMemberIds = group.Members.Select(member => member.UserId).ToList();

        var groupRepoMock = new Mock<IGroupRepository>();
        groupRepoMock.Setup(repository => repository.ReadGroupByUserId(firstUser.Id)).ReturnsAsync(group);
    
        var userRepoMock = new Mock<IUserRepository>();
        userRepoMock.Setup(repository => repository.GetById(firstUser.Id)).ReturnsAsync(firstUser);
        userRepoMock.Setup(repository => repository.GetByIdList(groupMemberIds)).ReturnsAsync(users.Where(user => groupMemberIds.Contains(user.Id)).ToList());
    
        var taskRepoMock = new Mock<ITaskRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
    
        Domain.Task.Entities.Task createdTask = null;
        taskRepoMock.Setup(repo => repo.Add(It.IsAny<Domain.Task.Entities.Task>()))
            .Callback<Domain.Task.Entities.Task>(task => createdTask = task);

        var taskService = new TaskService(taskRepoMock.Object, groupRepoMock.Object, userRepoMock.Object, unitOfWorkMock.Object, _mapper);
        var createTaskRequest = new CreateTaskDto("test", "nothing", DateTime.UtcNow,DateTime.UtcNow.AddHours(2));
        
        var result = await taskService.CreateForGroup(createTaskRequest, firstUser.Id);

        Assert.True(result.IsSuccess);
        Assert.NotNull(createdTask);
        var usersInGroup = users.Where(u => group.Members.Any(member => member.UserId.Equals(u.Id))).ToList();
        Assert.Equal(usersInGroup.Count, createdTask.AssignedUsers.Count);
        Assert.All(createdTask.AssignedUsers, user => Assert.Contains(user.Id, groupMemberIds));
    }

}

public class TaskOperationData : TheoryData<List<User>, Group>
{ 
    public TaskOperationData()
    {
        var user =User.Create("Jan","Kowalski","j.kowalski@o2.pl","secret");
        var user2 =User.Create("Homer","Simpson","h.simpson@o2.pl","secret");
        var user3 =User.Create("Andre","Dre","a.dre@o2.pl","secret");
        var user4 =User.Create("Filip","Kowalski","j.kowalski@o2.pl","secret");
       
        Assert.NotNull(user);
        Assert.NotNull(user2);
        Assert.NotNull(user3);
        Assert.NotNull(user4);
        
        var group = Group.Create("Kowalscy", user);
        Assert.NotNull(group);
        
        var firstInvitation = group.Invite(user2, user.Id);
        var secondInvitation = group.Invite(user3, user.Id);
        Assert.True(firstInvitation.IsSuccess);
        Assert.NotNull(firstInvitation.Value);
        Assert.True(secondInvitation.IsSuccess);
        Assert.NotNull(secondInvitation.Value);
        var acceptRes = group.AcceptInvitation(firstInvitation.Value);
        var acceptRes2 = group.AcceptInvitation(secondInvitation.Value);
        
        Assert.True(acceptRes.IsSuccess);
        Assert.True(acceptRes2.IsSuccess);

        List<User> users = [user,user2,user3,user4];
        
        Add(users, group);    
    }
        
}