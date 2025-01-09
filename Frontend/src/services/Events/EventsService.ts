import { AxiosResponse } from 'axios'
import { http } from '../http'
import {
  IBasicEventInfo,
  IBasicEventResponse,
  IFullInfoEventResponse,
  ISignupWindowResponse,
  IUpdatedEventBanner,
} from '../../models/Events/response/EventsResponse.ts'
import { BaseStatusResponse } from '../../models/Auth/response/authResponse.ts'
import IUpdateEventParams, { IQueryParams, IWindowsParams } from '../../models/Events/request/eventRequests.ts'

export default class EventsService {
  static async createEvent(): Promise<AxiosResponse<IBasicEventInfo>> {
    return http.post(`/api/my-events`)
  }

  static async UpdateEvent(Params: IUpdateEventParams, id: string): Promise<AxiosResponse<BaseStatusResponse>> {
    return http.put(`/api/my-events/${id}`, Params)
  }

  static async FetchUpcomingEvents(): Promise<AxiosResponse<IBasicEventResponse>> {
    return http.get('/api/events/upcoming/2')
  }

  static async FetchMyEvents(): Promise<AxiosResponse<IBasicEventResponse>> {
    return http.get('/api/my-events?skip=0&take=100&finished=false')
  }

  static async FetchMyPastEvents(): Promise<AxiosResponse<IBasicEventResponse>> {
    return http.get('/api/my-events?skip=0&take=100&finished=true')
  }

  static async FetchUserActivity(): Promise<AxiosResponse<IBasicEventResponse>> {
    return http.get('/api/user/signed-up-events?skip=0&take=100&finished=false')
  }

  static async FetchUserPastActivity(): Promise<AxiosResponse<IBasicEventResponse>> {
    return http.get('/api/user/signed-up-events?skip=0&take=100&finished=true')
  }

  static async FetchEventById(id: string): Promise<AxiosResponse<IBasicEventInfo>> {
    return http.get(`/api/events/${id}`)
  }

  static async FetchFullInfoAboutMyEvent(id: string): Promise<AxiosResponse<IFullInfoEventResponse>> {
    return http.get(`/api/my-events/${id}`)
  }

  static async DeleteEventById(id: string): Promise<AxiosResponse<BaseStatusResponse>> {
    return http.delete(`/api/my-events/${id}`)
  }

  static async UpdateEventBanner(file, id: string): Promise<AxiosResponse<IUpdatedEventBanner>> {
    return http.post(`/api/my-events/${id}/image`, file)
  }

  static async FetchEventByFilters(queryParams: IQueryParams): Promise<AxiosResponse<IBasicEventResponse>> {
    const { city, date, time, subject, online, skip, take, search } = queryParams
    return http.get(`/api/events?city=${city}&date=${date}&time=${time}&subject=${subject}&online=${online}&skip=${skip}&take=${take}&search=${search}`)
  }

  static async CreateWindow(EventId: string, WindowsRequestParams: IWindowsParams): Promise<AxiosResponse<ISignupWindowResponse>> {
    return http.post(`/api/my-events/${EventId}/windows`, WindowsRequestParams)
  }

  static async DeleteWindow(EventId: string, WindowId: string): Promise<AxiosResponse<BaseStatusResponse>> {
    return http.delete(`/api/my-events/${EventId}/windows/${WindowId}`)
  }

  static async UpdateWindow(EventId: string, WindowId: string, WindowsRequestParams: IWindowsParams): Promise<AxiosResponse<ISignupWindowResponse>> {
    return http.put(`/api/my-events/${EventId}/windows/${WindowId}`, WindowsRequestParams)
  }
}
