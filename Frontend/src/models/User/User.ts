export interface IUser {
  id: string | null
  phone: string | null
  email: string | null
  fio: string | null
  roleTitle: string | null
  description: string | null
  status: string | null
  avatarImage: string | null
  createdEvents: [{}] | null
}
