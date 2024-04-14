interface LoginResponse {
  token: string | null;
  isSuccessful: boolean;
  errorMessage: string | null;
}
