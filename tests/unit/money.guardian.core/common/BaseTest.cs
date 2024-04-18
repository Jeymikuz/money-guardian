using Microsoft.EntityFrameworkCore;
using money.guardian.infrastructure;
using NSubstitute;

namespace money.guardian.core.common;

public abstract class BaseTest<TSubject> where TSubject : class, new()
{
    protected readonly AppDbContext DbContext;
    protected readonly TSubject Subject;
    private readonly IEnumerable<object> _dependencies;

    protected BaseTest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        DbContext = new AppDbContext(options);

        var type = typeof(TSubject);
        var constructors = type.GetConstructors();

        var dependencies = new List<object>();

        foreach (var constructor in constructors)
        {
            var parameters = constructor.GetParameters();

            foreach (var parameter in parameters)
            {
                if (parameter.ParameterType == typeof(AppDbContext))
                {
                    dependencies.Add(DbContext);
                    continue;
                }

                var parameterType = parameter.ParameterType;
                var dependency = Substitute.For(new[] { parameterType }, Array.Empty<object>());
                dependencies.Add(dependency);
            }

            _dependencies = dependencies;
        }

        Subject = (TSubject)Activator.CreateInstance(typeof(TSubject), dependencies.ToArray())!;
    }

    protected T GetSubjectDependency<T>() => (T)_dependencies.FirstOrDefault(x => x is T);
}