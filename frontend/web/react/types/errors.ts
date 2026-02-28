export class RequestFailedError extends Error {
  status?: number;
  data?: unknown;

  constructor(message?: string, status?: number, data?: unknown) {
    super(message || 'Request failed');
    this.status = status;
    this.data = data;
  }
}

export class AssertionError extends Error {}
