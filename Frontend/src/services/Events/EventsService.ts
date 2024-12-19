import { AxiosResponse } from 'axios'
import { http } from '../http'
import { IBasicEventInfo, IBasicEventResponse } from '../../models/Events/response/EventsResponse.ts'
import { BaseStatusResponse } from '../../models/Auth/response/authResponse.ts'
import IUpdateEventParams from '../../models/Events/request/eventRequests.ts'

export default class EventsService {
  static async createEvent(): Promise<AxiosResponse<IBasicEventInfo>> {
    return http.post(`/api/my-events`)
  }
  static async UpdateEvent(Params: IUpdateEventParams): Promise<AxiosResponse<BaseStatusResponse>> {
    return http.put('/api/my-events/${eventId:guid}', Params)
  }
  static async FetchUpcomingEvents(): Promise<AxiosResponse<IBasicEventResponse>> {
    return http.get('/api/events/upcoming/2')
  }
}
