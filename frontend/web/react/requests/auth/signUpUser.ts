import axios, { AxiosResponse } from 'axios';
import { RequestFailedError } from '@/types/errors';

export type SignUpUserRequest = {
  userName: string;
  email: string;
  password: string;
  passwordRepeat: string;
};

export type SignUpUserResponse = '';

export async function signUpUser(request: SignUpUserRequest): Promise<''> {
  try {
    const response = await axios.post<SignUpUserRequest, AxiosResponse<SignUpUserResponse>>(
      `${process.env.NEXT_PUBLIC_BACKEND_URL}/api/auth/signUp`,
      request
    );

    return response.data;
  } catch (error: any) {
    const requestError = new RequestFailedError(error?.message);
    if (axios.isAxiosError(error)) {
      requestError.status = error.status;
      requestError.data = error.response?.data;
    }

    throw requestError;
  }
}
