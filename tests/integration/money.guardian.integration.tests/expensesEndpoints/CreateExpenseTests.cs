using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using money.guardian.api.models.expense;
using money.guardian.core.requests.expense;
using money.guardian.integration.tests.common;

namespace money.guardian.integration.tests.expensesEndpoints;

[Collection("Shared")]
public class CreateExpenseTests(TestWebApplicationFactory factory) : BaseIntegrationUserTest(factory)
{
    [Fact]
    public async Task CreateExpense_Should_Create_Expense_When_Group_Not_Provided_And_Return_Dto()
    {
        await AuthenticateUser(TestDataConstants.User.Email1, TestDataConstants.User.Password);

        var expense = new AddExpenseModel("TestExpense", 10m, null);

        var response = await HttpClient.PostAsJsonAsync("api/v1/expense", expense);
        var result = await response.Content.ReadFromJsonAsync<ExpenseDto>();

        var validId = Guid.TryParse(result.Id, out _);

        using (new AssertionScope())
        {
            validId.Should().BeTrue();
            result.Name.Should().Be(expense.Name);
            result.Value.Should().Be(expense.Value);
            result.ExpenseGroup.Should().BeNull();
        }
    }

    [Fact]
    public async Task CreateExpense_Should_Create_Expense_When_Group_Provided_And_Return_Dto()
    {
        await AuthenticateUser(TestDataConstants.User.Email1, TestDataConstants.User.Password);

        var expenseGroupId = (await DbContext.ExpenseGroups.FirstOrDefaultAsync()).Id;
        var expense = new AddExpenseModel("TestExpense", 10m, expenseGroupId.ToString());

        var response = await HttpClient.PostAsJsonAsync("api/v1/expense", expense);
        var result = await response.Content.ReadFromJsonAsync<ExpenseDto>();

        var validId = Guid.TryParse(result.Id, out _);

        using (new AssertionScope())
        {
            validId.Should().BeTrue();
            result.Name.Should().Be(expense.Name);
            result.Value.Should().Be(expense.Value);
            result.ExpenseGroup.Id.Should().Be(expenseGroupId.ToString());
        }
    }

    [Fact]
    public async Task CreateExpense_Should_Return_Bad_Request_When_Group_Id_Wrong_Format()
    {
        await AuthenticateUser(TestDataConstants.User.Email1, TestDataConstants.User.Password);

        var expenseGroupId = "INCORRECT-GUID-FORMAT";
        var expense = new AddExpenseModel("TestExpense", 10m, expenseGroupId);

        var response = await HttpClient.PostAsJsonAsync("api/v1/expense", expense);
        var message = await response.Content.ReadFromJsonAsync<string>();

        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            message.Should().Be("Incorrect id format");
        }
    }
    
    [Fact]
    public async Task CreateExpense_Should_Return_Bad_Request_When_Group_Id_Not_Found()
    {
        await AuthenticateUser(TestDataConstants.User.Email1, TestDataConstants.User.Password);

        var expenseGroupId = "144dd2c3-35ca-4693-8a4b-f9020acf24af";
        var expense = new AddExpenseModel("TestExpense", 10m, expenseGroupId);

        var response = await HttpClient.PostAsJsonAsync("api/v1/expense", expense);
        var message = await response.Content.ReadFromJsonAsync<string>();

        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            message.Should().Be($"Expense group with id {expenseGroupId} not found");
        }
    }
    
}