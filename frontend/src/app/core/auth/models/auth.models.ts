export type UserRole = 'Candidate' | 'Employer' | 'Admin';

export interface ApiResponse<T> {
  data: T;
  message: string | null;
}

export interface AuthResponse {
  userId: string;
  firstName: string;
  lastName: string;
  email: string;
  role: UserRole;
  accessToken: string;
  accessTokenExpiresAtUtc: string;
  refreshToken: string;
  refreshTokenExpiresAtUtc: string;
}

export interface CurrentUserResponse {
  userId: string;
  firstName: string;
  lastName: string;
  email: string;
  role: UserRole;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  confirmPassword: string;
  role: Exclude<UserRole, 'Admin'>;
}

export interface RefreshTokenRequest {
  refreshToken: string;
}

export interface AuthSession {
  accessToken: string;
  accessTokenExpiresAtUtc: string;
  refreshToken: string;
  refreshTokenExpiresAtUtc: string;
}
