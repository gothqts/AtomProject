export interface IUser {
  id: string | null
  phone: string | null
  email: string | null
  fio: string | null
  city: string | null
  roleTitle: string | null
  description: string | null
  status: string | null
  avatarImage: string
  createdEvents: [{}] | null
}
