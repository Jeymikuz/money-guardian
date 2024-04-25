using FluentAssertions;
using FluentAssertions.Execution;
using money.guardian.core.common;
using money.guardian.core.common.errors;
using money.guardian.domain.entities;
using money.guardian.infrastructure;

namespace money.guardian.core.requests.expense;

public class EditExpenseHandlerTests : TestBase<EditExpenseHandler>
{
    [Fact]
    public async Task Handle_Should_Edit_Expense_And_Update_Group_When_Group_Provided()
    {
        var dbContext = GetSutMockedDependency<AppDbContext>();

        var userId = "UserId";
        var expense = new Expense()
        {
            Name = "TestName",
            UserId = userId,
            Value = 100m
        };

        var expenseGroup = new ExpenseGroup()
        {
            Name = "TestGroup",
            Icon = "TestIcon",
        };


        await dbContext.Expenses.AddAsync(expense);
        await dbContext.ExpenseGroups.AddAsync(expenseGroup);
        await dbContext.SaveChangesAsync();

        var request = new EditExpenseRequest(expense.Id, "Edited", 10.20m, expenseGroup.Id, expense.UserId);

        var result = await Sut.Handle(request, CancellationToken.None);

        using (new AssertionScope())
        {
            result.IsError.Should().BeFalse();
            result.Value.Id.Should().Be(expense.Id.ToString());
            result.Value.Name.Should().Be(request.Name);
            result.Value.Value.Should().Be(request.Value);
            result.Value.ExpenseGroup.Id.Should().Be(expenseGroup.Id.ToString());
            result.Value.ExpenseGroup.Name.Should().Be(expenseGroup.Name);
            result.Value.ExpenseGroup.Icon.Should().Be(expenseGroup.Icon);
        }
    }

    [Fact]
    public async Task Handle_Should_Return_Not_Found_Error_When_Group_To_Update_Doesnt_Exists()
    {
        var dbContext = GetSutMockedDependency<AppDbContext>();

        var userId = "UserId";
        var randomGroupIp = Guid.NewGuid();
        var expense = new Expense()
        {
            Name = "TestName",
            UserId = userId,
            Value = 100m
        };

        await dbContext.Expenses.AddAsync(expense);
        await dbContext.SaveChangesAsync();

        var request = new EditExpenseRequest(expense.Id, "Edited", 10.20m, randomGroupIp, expense.UserId);

        var result = await Sut.Handle(request, CancellationToken.None);

        using (new AssertionScope())
        {
            result.IsError.Should().BeTrue();
            result.Error.Should().BeOfType<NotFoundError>();
        }
    }
}