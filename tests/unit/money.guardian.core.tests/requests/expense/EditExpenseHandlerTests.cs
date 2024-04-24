using FluentAssertions;
using FluentAssertions.Execution;
using money.guardian.core.common;
using money.guardian.domain.entities;
using money.guardian.infrastructure;

namespace money.guardian.core.requests.expense;

public class EditExpenseHandlerTests : TestBase<EditExpenseHandler>
{
    [Fact]
    public async Task Should_Edit_Expense_And_Update_Group_When_Group_Provided()
    {
        var dbContext = GetSutMockedDependency<AppDbContext>();

        var userId = "UserId";
        var expense = new Expense()
        {
            Name = "TestName",
            Group = new ExpenseGroup()
            {
                Name = "TestGroup",
                Icon = "Icon",
                UserId = userId
            },
            UserId = userId,
            Value = 100m
        };

        await dbContext.Expenses.AddAsync(expense);
        await dbContext.SaveChangesAsync();

        var request = new EditExpenseRequest(expense.Id, "Edited", 10.20m, expense.GroupId, expense.UserId);

        var result = await Sut.Handle(request, CancellationToken.None);

        using (new AssertionScope())
        {
            result.IsError.Should().BeFalse();
            result.Value.Name.Should().Be(request.Name);
            result.Value.Value.Should().Be(request.Value);
        }
    }
}