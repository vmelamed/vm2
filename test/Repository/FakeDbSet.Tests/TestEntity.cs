namespace vm2.Repository.FakeDbSet.Tests;

using FluentValidation;

class TestEntity : IFindable<TestEntity>
{
    public Guid Zone { get; set; } = default;

    public Guid Id { get; set; } = default;

    public string Name { get; set; } = "";

    public TestEntity() { }

    public TestEntity(
        string name,
        Guid id,
        Guid zone = default)
    {
        Id   = id;
        Name = name;
        Zone = zone;
    }

    public static Expression<Func<TestEntity, object?>> KeyExpression => te => new { te.Id, te.Zone };

    public ValueTask ValidateFindable(
        object? context = null,
        CancellationToken cancellationToken = default)
    {
        new TestEntityFindableValidator().ValidateAndThrow(this);
        return ValueTask.CompletedTask;
    }

    class TestEntityFindableValidator : AbstractValidator<TestEntity>
    {
        public TestEntityFindableValidator()
        {
            RuleFor(te => te.Id).NotEmpty();
            RuleFor(te => te.Zone).NotEmpty();
        }
    }
}
