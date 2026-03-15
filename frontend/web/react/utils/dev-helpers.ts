const useDevelopmentMode = true;

export function isDevelopment() {
  return useDevelopmentMode && process.env.NODE_ENV === 'development';
}

export function getSignUpFormDevDefault() {
  return {
    userName: 'test',
    email: 'test@test.com',
    password: 'Test01!',
    passwordRepeat: 'Test01!',
    acceptTerms: true,
  };
}
