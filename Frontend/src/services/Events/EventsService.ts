import { AxiosResponse } from 'axios'
import { http } from '../http'
import { IBasicEventInfo, IBasicEventResponse } from '../../models/Events/response/EventsResponse.ts'
import { BaseStatusResponse } from '../../models/Auth/response/authResponse.ts'
import IUpdateEventParams from '../../models/Events/request/eventRequests.ts'

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
    return http.get('/api/my-events?skip=0&take=10')
  }
  static async FetchMyPastEvents(): Promise<AxiosResponse<IBasicEventResponse>> {
    return http.get('/api/my-events?skip=0&take=10&finished=true')
  }
}