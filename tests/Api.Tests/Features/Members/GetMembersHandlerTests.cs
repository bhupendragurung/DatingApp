using Api.Features.Members.GetMembers;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Time.Testing;

namespace Api.Tests.Features.Members;

public sealed class GetMembersHandlerTests
{
    private static readonly DateTimeOffset Now = new(2026,1,15,12,0,0, TimeSpan.Zero);
    private static readonly DateOnly Today = new(2026,1,15);
    private static AppDbContext CreateDb()
    {
        var options= new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }
      private static AppUser CreateUser(string name, string gender, int age = 30) => new()
    {
        Id = Guid.NewGuid(),
        UserName = name.ToLower(),
        KnownAs = name,
        Gender = gender,
        DateOfBirth = Today.AddYears(-age),
        City = "London",
        Country = "UK"
    };

    private static GetMembersHandler CreateHandler(AppDbContext db) =>
        new(db, new FakeTimeProvider(Now));

    [Fact]
    public async Task Handle_ReturnOppositeGender_WhenGenderIsNotGiven()
    {
        await using var db= CreateDb();
         var currentUser = CreateUser("Tom", "male");
        db.Users.AddRange(
            currentUser,
            CreateUser("Lisa", "female"),
            CreateUser("Karen", "female"),
            CreateUser("Bob", "male"));
        await db.SaveChangesAsync();
         var query = new GetMembersQuery { CurrentUserId = currentUser.Id };
        var result = await CreateHandler(db).Handle(query, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.TotalCount);
        Assert.All(result.Value.Items, m => Assert.Equal("female", m.Gender));
    
    }
[Fact]
public async Task Handle_ExcludesCurrentUser_WhenTheirOwnGenderIsRequested()
{
    await using var db = CreateDb();
    var currentUser = CreateUser("Tom", "male");
    db.Users.AddRange(currentUser, CreateUser("Bob", "male"));
    await db.SaveChangesAsync();

    var query = new GetMembersQuery { CurrentUserId = currentUser.Id, Gender = "male" };
    var result = await CreateHandler(db).Handle(query, CancellationToken.None);

    Assert.True(result.IsSuccess);
    var member = Assert.Single(result.Value.Items);
    Assert.Equal("Bob", member.KnownAs);
}
 [Fact]
public async Task Handle_ReturnsOnlyMembersBetweenMinAgeAndMaxAge()
    {
    await using var db = CreateDb();
    var currentUser = CreateUser("Tom", "male");
    db.Users.AddRange(
        currentUser,
        CreateUser("Amy", "female", age: 24),
        CreateUser("Beth", "female", age: 25),
        CreateUser("Cara", "female", age: 30),
        CreateUser("Dana", "female", age: 31));
    await db.SaveChangesAsync();

    var query = new GetMembersQuery { CurrentUserId = currentUser.Id, MinAge = 25, MaxAge = 30 };
    var result = await CreateHandler(db).Handle(query, CancellationToken.None);

    Assert.True(result.IsSuccess);
    var ages = result.Value.Items.Select(m => m.Age).Order();
    Assert.Equal(new[] { 25, 30 }, ages);
}
    [Fact]
public async Task Handle_ReturnsRequestedPage_WithTotalsForAllMatches()
{
    await using var db = CreateDb();
    var currentUser = CreateUser("Tom", "male");
    db.Users.AddRange(
        currentUser,
        CreateUser("Dana", "female"),
        CreateUser("Amy", "female"),
        CreateUser("Emma", "female"),
        CreateUser("Cara", "female"),
        CreateUser("Beth", "female"));
    await db.SaveChangesAsync();

    var query = new GetMembersQuery
    {
        CurrentUserId = currentUser.Id,
        OrderBy = "name",
        Page = 2,
        PageSize = 2
    };
    var result = await CreateHandler(db).Handle(query, CancellationToken.None);

    Assert.True(result.IsSuccess);
    Assert.Equal(new[] { "Cara", "Dana" }, result.Value.Items.Select(m => m.KnownAs));
    Assert.Equal(5, result.Value.TotalCount);
    Assert.Equal(3, result.Value.TotalPages);
}
}

