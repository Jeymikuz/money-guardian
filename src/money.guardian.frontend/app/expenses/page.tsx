import { getServerSession } from "next-auth";
import { getExpenses, getExpensesByMonth } from "../actions/expensesActions";
import Expense from "./Expense";
import AddExpense from "./AddExpense";

export default async function ExpensesPage() {
  const session = await getServerSession();

  if (!session) {
    return <div>Unauth</div>;
  }

  const currentMonth = getCurrentMonthName();

  const expenses = await getExpensesByMonth(new Date().getMonth());
  console.log(expenses);

  return (
    <main>
      <div className="flex justify-center my-8">
        <h1 className="text-[60px]">{currentMonth}</h1>
      </div>
      <div className="flex justify-center">
        <AddExpense />
      </div>
      {expenses.map((expense) => (
        <Expense key={expense.id} expense={expense} />
      ))}
    </main>
  );
}

const getCurrentMonthName = () =>
  new Date().toLocaleDateString("en-us", {
    month: "long",
  });
