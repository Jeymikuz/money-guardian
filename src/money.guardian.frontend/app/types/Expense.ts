export interface ExpenseGroup {
  id: string;
  name: string;
  icon: string;
  createdAt: string; // This could be a Date type if you want to parse it as a Date object
}

export interface Expense {
  id: string;
  name: string;
  value: number;
  expenseGroup?: ExpenseGroup;
  createdAt: string; // This could be a Date type if you want to parse it as a Date object
}
