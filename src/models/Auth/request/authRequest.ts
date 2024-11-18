export interface RegisterRequest {
  fio: string
  email: string
  password: string
  city: string
  status: string
}
export interface LoginRequest {
  email: string
  password: string
}
