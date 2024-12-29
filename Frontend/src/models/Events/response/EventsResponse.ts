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

export interface IFullInfoEventResponse {
  id: string
  isPublic: boolean
  title: string
  bannerImageFilepath: string
  dateStart: string
  dateEnd: string
  isOnline: boolean
  city: string
  address: string
  isSignupOpened: boolean
  Description: string
  organizerContacts: {
    email: string
    fio: string
    phone: string
    telegram: string
  }
  signupWindows: ISignupWindowResponse[]
}

export interface ISignupWindowResponse {
  id: string
  title: string
  dateTime: string
  maxVisitors: number
  ticketsLeft: number
}
