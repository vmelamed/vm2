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

    public async ValueTask ValidateFindableAsync(
        object? context = null,
        CancellationToken ct = default)
        => await new TestEntityFindableValidator().ValidateAndThrowAsync(this, ct);

    class TestEntityFindableValidator : AbstractValidator<TestEntity>
    {
        public TestEntityFindableValidator()
        {
            RuleFor(te => te.Id).NotEmpty();
            RuleFor(te => te.Zone).NotEmpty();
        }
    }
}
