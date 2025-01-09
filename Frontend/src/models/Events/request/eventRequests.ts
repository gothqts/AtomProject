export default interface IUpdateEventParams {
  isPublic: boolean
  title: string
  isOnline: boolean
  dateStart: string
  dateEnd: string
  city: string
  address: string
  isSignupOpened: boolean
  description: string
}

export interface IQueryParams {
  city: string
  date: string
  time: string
  subject: string
  online: boolean | string
  skip: number
  take: number
  search: string
}

export interface IWindowsParams {
  title: string
  date: string
  time: string
  maxVisitors: number
  alreadyOccupiedPlaces: number
}
