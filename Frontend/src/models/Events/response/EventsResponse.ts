export interface IBasicEventInfo {
  id: string
  isPublic: boolean
  title: string
  bannerImage: string
  dateStart: string
  dateEnd: string
  isOnline: boolean
  city: string
  address: string
  isSignupOpened: boolean
  description: string
}
export interface IBasicEventResponse {
  events: IBasicEventInfo[]
}
