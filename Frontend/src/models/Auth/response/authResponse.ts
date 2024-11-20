export interface AuthResponse {
  completed: boolean
  status: string
  message: string
  userId: string
  accessToken: any
}

export interface BaseStatusResponse {
  completed: boolean
  status: string
  message: string
}
