import axios, { AxiosResponse } from 'axios';
import { ProblemDetails } from '@/requests/types';

export type SignUpUserRequest = {
  userName: string;
  email: string;
  password: string;
  passwordRepeat: string;
};

export type SignUpUserResponse = null | ProblemDetails;

export async function signUpUser(request: SignUpUserRequest): Promise<null | ProblemDetails> {
  const response = await axios.post<SignUpUserRequest, AxiosResponse<SignUpUserResponse>>(
    `${process.env.NEXT_PUBLIC_BACKEND_URL}/api/auth/signUp`,
    request
  );

  return response.data;
}
