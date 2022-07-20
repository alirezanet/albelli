using Application.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MockQueryable.EntityFrameworkCore;
using NSubstitute;

namespace Application.Test;

public static class AppDbContextMockExtensions
{
    public static IQueryable<TEntity> BuildMock<TEntity>(this IQueryable<TEntity> data) where TEntity : class
    {
        var mock = Substitute.For<IQueryable<TEntity>, IAsyncEnumerable<TEntity>>();
        var enumerable = new TestAsyncEnumerableEfCore<TEntity>(data);
        ((IAsyncEnumerable<TEntity>)mock).ConfigureAsyncEnumerableCalls(enumerable);
        mock.ConfigureQueryableCalls(enumerable, data);
        return mock;
    }

    public static DbSet<TEntity> ToMockDbSet<TEntity>(this IEnumerable<TEntity> data, IAppDbContext? dbContext = null) where TEntity : class => ToMockDbSet(data.AsQueryable(), dbContext);

    public static DbSet<TEntity> ToMockDbSet<TEntity>(this IQueryable<TEntity> data, IAppDbContext? dbContext = null) where TEntity : class
    {
        if (Substitute.For(new[]
            {
             typeof(DbSet<TEntity>),
             typeof(IQueryable<TEntity>),
             typeof(IAsyncEnumerable<TEntity>),
             typeof(IInfrastructure<IServiceProvider>)
          }, null) is not DbSet<TEntity> mock)
            throw new InvalidOperationException("DbSet<TEntity> is not a valid type");

        var enumerable = new TestAsyncEnumerableEfCore<TEntity>(data);

        if (dbContext != null)
            mock.ConfigureDbContext(dbContext);


        ((IAsyncEnumerable<TEntity>)mock).ConfigureAsyncEnumerableCalls(enumerable);
        mock.ConfigureQueryableCalls(enumerable, data);
        mock.ConfigureDbSetCalls(data);

        return mock;
    }

    private static void ConfigureDbContext(this IInfrastructure<IServiceProvider> mock, IAppDbContext dbContext)
    {
        var currentDbContext = Substitute.For<ICurrentDbContext>();
        currentDbContext.Context.Returns(dbContext as DbContext);
        var sp = Substitute.For<IServiceProvider>();
        sp.GetService(typeof(ICurrentDbContext)).Returns(currentDbContext);
        mock.Instance.Returns(sp);
    }

    private static void ConfigureQueryableCalls<TEntity>(
       this IQueryable<TEntity> mock,
       IQueryProvider queryProvider,
       IQueryable<TEntity> data) where TEntity : class
    {
        mock.Provider.Returns(queryProvider);
        mock.Expression.Returns(data?.Expression);
        mock.ElementType.Returns(data?.ElementType);
        mock.GetEnumerator().Returns(info => data?.GetEnumerator());
    }

    private static void ConfigureAsyncEnumerableCalls<TEntity>(
       this IAsyncEnumerable<TEntity> mock,
       IAsyncEnumerable<TEntity> enumerable)
    {
        mock.GetAsyncEnumerator(Arg.Any<CancellationToken>()).Returns(args => enumerable.GetAsyncEnumerator());
    }

    private static void ConfigureDbSetCalls<TEntity>(this DbSet<TEntity> mock, IQueryable<TEntity> data)
       where TEntity : class
    {
        mock.AsQueryable().Returns(data);
        mock.AsAsyncEnumerable().Returns(args => CreateAsyncMock(data));
    }

    private static async IAsyncEnumerable<TEntity> CreateAsyncMock<TEntity>(IEnumerable<TEntity> data)
       where TEntity : class
    {
        foreach (var entity in data) yield return entity;

        await Task.CompletedTask;
    }
}
