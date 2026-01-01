const useDevelopmentMode = true;

export function jsonify(data: any) {
  return JSON.stringify(data, null, 2);
}

export function isDevelopment() {
  return useDevelopmentMode && process.env.NODE_ENV === 'development';
}
