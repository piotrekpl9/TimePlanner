
using Application.Common.Data;
using Application.Group;
using Domain.Group.Errors;
using Domain.Group.Repositories;
using Domain.Shared;
using Domain.Task.Repositories;
using Domain.User.Repositories;
using Domain.User.ValueObjects;
using Moq;

namespace Application.Tests;

public class GroupServiceTests
{
 
    [Theory]
    [ClassData(typeof(GroupCreateUserAlreadyInGroup))]
    public async System.Threading.Tasks.Task CreateGroup_Should_ReturnFailure_When_UserAlreadyInOtherGroup(Domain.User.Entities.User user)
    {
        var group = Domain.Group.Entities.Group.Create("Rodmanowie", user.Id);
        var groupRepoMock = new Mock<IGroupRepository>();
        groupRepoMock.Setup(repository => repository.ReadGroupByUserId(user.Id)).ReturnsAsync(group);
        var userRepoMock = new Mock<IUserRepository>();
        userRepoMock.Setup(repository => repository.GetById(user.Id)).ReturnsAsync(user);
        
        var taskRepoMock = new Mock<ITaskRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        
        var groupService = new GroupService(groupRepoMock.Object,userRepoMock.Object, taskRepoMock.Object, unitOfWorkMock.Object);

        var result = await groupService.CreateGroup("Kowalscy", user.Id);
        
        Assert.True(result.IsFailure);
        Assert.NotStrictEqual(Result<Domain.Group.Entities.Group>.Failure(GroupError.UserAlreadyInOtherGroup), result);
    }

}

public class GroupCreateUserAlreadyInGroup : TheoryData<Domain.User.Entities.User>
{
    public GroupCreateUserAlreadyInGroup()
    { 
        var userResult = Domain.User.Entities.User.Create("Jan","Kowalski","j.kowalski@o2.pl","secret");
        var user = userResult.Value;  
    
        Assert.NotNull(user);
        
        Add(user);
    }
}