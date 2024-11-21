export interface AuthResponse {
  completed: boolean
  status: string
  message: string
  userId: string
  accessToken: string
}

export interface BaseStatusResponse {
  completed: boolean
  status: string
  message: string
}
