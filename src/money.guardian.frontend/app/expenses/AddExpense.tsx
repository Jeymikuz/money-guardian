"use client";
import React, { useState } from "react";
import { PlusCircledIcon } from "@radix-ui/react-icons";
import { z } from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";

const formSchema = z.object({
  name: z.string().min(1, "You have to provide name"),
  value: z.coerce.number().default(0),
  groupdId: z.string().optional(),
});

function AddExpense() {
  const [addNew, setAddNew] = useState(false);

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      name: "",
      value: 0,
      groupdId: undefined,
    },
  });

  async function onSubmit(values: z.infer<typeof formSchema>) {
    console.log(values);
  }

  if (!addNew)
    return (
      <PlusCircledIcon
        onClick={() => setAddNew(!addNew)}
        className="hover:cursor-pointer"
        fontSize={20}
      />
    );

  return (
    <Form {...form}>
      <form
        onSubmit={form.handleSubmit(onSubmit)}
        className="border m-3 p-2 rounded-md flex justify-between space-x-2"
      >
        <FormField
          control={form.control}
          name="groupdId"
          render={({ field }) => (
            <FormItem>
              <FormControl>
                <Input placeholder="Group" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="name"
          render={({ field }) => (
            <FormItem>
              <FormControl>
                <Input placeholder="Name" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="value"
          render={({ field }) => (
            <FormItem>
              <FormControl>
                <Input
                  type="number"
                  placeholder="Amount"
                  {...field}
                  step="0.01"
                  lang="en"
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <Button type="submit">Submit</Button>
      </form>
    </Form>
  );
}

export default AddExpense;
