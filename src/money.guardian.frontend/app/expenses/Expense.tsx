"use client";
import {
  Collapsible,
  CollapsibleContent,
  CollapsibleTrigger,
} from "@radix-ui/react-collapsible";
import { Expense } from "../types/Expense";

interface Props {
  expense: Expense;
}

function Expenses({ expense }: Props) {
  return (
    <Collapsible className="flex flex-col">
      <CollapsibleTrigger className="border m-3 p-2 rounded-md flex justify-between hover:bg-gray-600 hover:cursor-pointer">
        <p>{expense.expenseGroup?.icon}</p>
        <p>{expense.name}</p>
        {resolvePrice(expense.value)}
      </CollapsibleTrigger>
      <CollapsibleContent>
        {new Date("2024-03-25T21:59:56.956881+00:00").toLocaleString()}
      </CollapsibleContent>
    </Collapsible>
  );
}

const resolvePrice = (value: number) => {
  if (value < 0) {
    return <p className="text-red-700">- {value.toFixed(2)}</p>;
  } else {
    return <p className="text-green-600">+ {value.toFixed(2)}</p>;
  }
};

export default Expenses;
