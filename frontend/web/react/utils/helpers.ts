import { validationProblemDetailsSchema } from '@/schemas/problemDetails';
import { AssertionError, RequestFailedError } from '@/types/errors';
import { errorCodes } from '@/utils/constants';

const useDevelopmentMode = true;

export function jsonify(data: unknown) {
  return JSON.stringify(data, null, 2);
}

export function isDevelopment() {
  return useDevelopmentMode && process.env.NODE_ENV === 'development';
}

export function assert(condition: boolean, message?: string): asserts condition {
  if (!condition) {
    throw new AssertionError(message);
  }
}

export function isRequestFailedError(error: Error): error is RequestFailedError {
  return error instanceof RequestFailedError;
}

export function isValidationFailedError(error: Error): error is RequestFailedError {
  if (!isRequestFailedError(error) || error.status !== 400) {
    return false;
  }

  const parseResult = validationProblemDetailsSchema.safeParse(error.data);
  if (!parseResult.success) {
    return false;
  }

  const problemDetails = parseResult.data;

  return problemDetails.errorCode === errorCodes.validationFailed && !!problemDetails.errors;
}

export function uncapitalize(str: string | undefined): string | undefined {
  if (!str?.length) {
    return str;
  }

  if (str.length === 1) {
    return str[0].toLowerCase();
  }

  return str[0].toLowerCase() + str.substring(1);
}

export function getValidationErrors(error: RequestFailedError): [string, string[]][] {
  const parseResult = validationProblemDetailsSchema.safeParse(error.data);
  assert(parseResult.success, 'Response expected to be validation problem details');
  const problemDetails = parseResult.data;

  return Object.entries(problemDetails.errors).map<[string, string[]]>((e) => [
    uncapitalize(e[0]) as string,
    e[1],
  ]);
}
