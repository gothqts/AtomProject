export interface IUser {
  id: string
  phone: string
  email: string
  fio: string
  city: string
  roleTitle: string
  description: string
  status: string
  avatarImage: string
  createdEvents: [{}] | null
  CurrentPassword: string
  NewPassword: string
}

export interface IAvatarUpdateResponse {
  completed: boolean
  status: string
  message: string
  image: string
}
