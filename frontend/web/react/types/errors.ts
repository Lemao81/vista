export class RequestFailedError extends Error {
  status?: number;
  data?: any;

  constructor(message?: string, status?: number, data?: any) {
    super(message || 'Request failed');
    this.status = status;
    this.data = data;
  }
}

export class AssertionError extends Error {}
