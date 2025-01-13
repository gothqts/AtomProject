import exp = require('constants')

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

export interface MyCreatingEvent extends IFullInfoEventResponse {}

export interface IFullInfoEventResponse {
  id: string
  creationDate: string
  isPublic: boolean
  title: string
  bannerImageFilepath: string
  isOnline: boolean
  isSignupOpened: boolean
  city: string
  address: string
  dateStart: string
  dateEnd: string
  description: string
  signupWindows: ISignupWindowResponse[]
  signupForm: {
    isFioRequired: boolean
    isEmailRequired: boolean
    isPhoneRequired: boolean
    dynamicFields: dynamicFields[]
  }
  contacts: contacts[]
}

export interface ISignupWindowResponse {
  id: string
  title: string
  dateTime: string
  maxVisitors: number
  ticketsLeft: number
}

export interface dynamicFields {
  id: string
  title: string
  isRequired: boolean
  type: string
  maxSymbols: number
  minValue: string
  maxValue: string
}

export interface contacts {
  id: string
  email: string
  fio: string
  phone: string
  telegram: string
}

export interface IUpdatedEventBanner {
  completed: boolean
  status: string
  message: string
  image: string
}

export interface IUpcomingEvent {
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

export interface IWindowsList {
  signupWindows: ISignupWindowResponse[]
}

export interface ISubscribeForm {
  isFioRequired: boolean
  isEmailRequired: boolean
  isPhoneRequired: boolean
}
export interface ISignUpResponse {
  entryId: string
  eventId: string
  phone: string
  fio: string
  email: string
  dateTime: string
  dynamicFields: dynamicFields[]
}
