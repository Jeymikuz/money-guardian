"use server";
import { fetchWrapper } from "@/lib/fetchWrapper";
import { Expense } from "../types/Expense";

export const getExpenses = async (): Promise<Expense[]> =>
  await fetchWrapper.get("expense");

export const getExpensesByMonth = async (month: number): Promise<Expense[]> =>
  await fetchWrapper.get(`expense/month/${month}`);
