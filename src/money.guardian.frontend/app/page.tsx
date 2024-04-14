import { ModeToggle } from "@/components/ui/ModeToggle";
import { Button } from "@/components/ui/button";
import Image from "next/image";
import Link from "next/link";

export default function Home() {
  return (
    <main className="mx-4">
      <div className="flex flex-col justify-center items-center">
        <h1 className="text-4xl">Money Guardian</h1>
        <Link className="my-4" href="/expenses">
          <Button>Go to Expenses</Button>
        </Link>
      </div>
    </main>
  );
}
