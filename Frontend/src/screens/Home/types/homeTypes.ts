export interface IUpcomingEvents {
  id: string
  isPublic: boolean
  title: string
  bannerImage: string | FormData
  dateStart: string
  dateEnd: string
  isOnline: boolean
  city: string
  address: string
  isSignupOpened: boolean
  description: string
}
