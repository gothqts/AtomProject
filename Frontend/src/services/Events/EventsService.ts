import { AxiosResponse } from 'axios'
import { http } from '../http'
import { IBasicEventInfo } from '../../models/Events/response/EventsRequests.ts'

export default class EventsService {
  static async createEvent(): Promise<AxiosResponse<IBasicEventInfo>> {
    return http.post(`/api/my-events`)
  }
}
