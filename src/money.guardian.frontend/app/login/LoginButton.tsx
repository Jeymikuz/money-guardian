"use client";
import { Button } from "@/components/ui/button";
import { signIn, signOut, useSession } from "next-auth/react";

export default function LoginButton() {
  const { data: session } = useSession();

  const loginBtn = (
    <Button variant="default" onClick={() => signIn()}>
      Login
    </Button>
  );

  const logoutBtn = (
    <>
      <h2>Hello: {session?.user.username}</h2>
      <Button variant="default" onClick={() => signOut()}>
        Logout
      </Button>
    </>
  );

  return session?.user ? logoutBtn : loginBtn;
}
