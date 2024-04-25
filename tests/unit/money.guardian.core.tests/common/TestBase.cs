using Microsoft.EntityFrameworkCore;
using money.guardian.infrastructure;
using NSubstitute;

namespace money.guardian.core.common;

public abstract class TestBase<TSubject> where TSubject : class
{
    protected readonly TSubject Sut;
    private readonly List<object> _dependencies = new();

    protected TestBase()
    {
        var type = typeof(TSubject);
        var constructors = type.GetConstructors();

        foreach (var constructor in constructors)
        {
            var parameters = constructor.GetParameters();

            foreach (var parameter in parameters)
            {
                if (parameter.ParameterType == typeof(AppDbContext))
                {
                    var options = new DbContextOptionsBuilder<AppDbContext>()
                        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                        .Options;

                    var dbContext = new AppDbContext(options);
                    _dependencies.Add(dbContext);
                    continue;
                }

                var parameterType = parameter.ParameterType;
                var dependency = Substitute.For(new[] { parameterType }, Array.Empty<object>());
                _dependencies.Add(dependency);
            }
        }

        Sut = (TSubject)Activator.CreateInstance(typeof(TSubject), _dependencies.ToArray());
    }

    protected T GetSutMockedDependency<T>() => (T)_dependencies.FirstOrDefault(x => x is T);
}