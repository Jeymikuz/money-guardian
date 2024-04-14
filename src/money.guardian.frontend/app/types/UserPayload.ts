export interface UserPayload {
  unique_name: string;
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid": string;
  nbf: number;
  exp: number;
  iat: number;
  iss: string;
  aud: string;
}
