import { makeAutoObservable } from 'mobx'
import RootStore from './rootStore.ts'
import { http } from '../services/http'
import EventsService from '../services/Events/EventsService.ts'
import { IUpcomingEvents } from '../screens/Home/types/homeTypes.ts'
import { IBasicEventInfo } from '../models/Events/response/EventsResponse.ts'

interface ICity {
  label: string
  value: string
}

type ICities = ICity[]

export default class EventStore {
  rootStore: RootStore
  cities: ICities = []
  creatingEvent: IBasicEventInfo | null = null
  upcomingEvents: IUpcomingEvents[] = []
  myEvents: IBasicEventInfo[] = []
  myPastEvents: IBasicEventInfo[] = []

  constructor(rootStore: RootStore) {
    makeAutoObservable(this, { rootStore: false })
    this.rootStore = rootStore
  }

  setCreatingEventData(field: keyof IBasicEventInfo, value: string) {
    if (this.creatingEvent) {
      this.creatingEvent = { ...this.creatingEvent, [field]: value }
    }
  }

  async FetchUpcomingEvents() {
    try {
      const response = await EventsService.FetchUpcomingEvents()
      this.upcomingEvents = response.data.events
    } catch (error) {
      console.log(error, 'Ошибка загрузки последних событий')
    }
  }

  async fetchCities(skip: number = 0, take: number = 10) {
    try {
      const response = await http.get(`/api/cities?skip=${skip}&take=${take}`)
      this.cities = response.data.cities.map((city) => ({
        label: city.name,
        value: city.name,
      }))
      console.log(response.data)
    } catch (error) {
      console.error('Ошибка при загрузке городов:', error)
    }
  }

  async CreateEvent() {
    try {
      const response = await EventsService.createEvent()
      this.creatingEvent = response.data
      console.log(this.creatingEvent)
    } catch (error) {
      console.log(error, 'Ошибка создания меро')
    }
  }

  async UpdateEvent(data, id) {
    try {
      const response = await EventsService.UpdateEvent(data, id)
      if (response.status == 200) {
        this.creatingEvent = null
      }
    } catch (error) {
      console.log(error, 'Ошибка обноления события')
    }
  }

  async FetchMyEvents() {
    try {
      const response = await EventsService.FetchMyEvents()
      if (response.status == 200) {
        this.myEvents = response.data.events
      }
    } catch (error) {
      console.log(error, 'Ошибка загрузки моих мероприятий')
    }
  }
  async FetchMyPastEvents() {
    try {
      const response = await EventsService.FetchMyPastEvents()
      if (response.status == 200) {
        this.myPastEvents = response.data.events
      }
    } catch (error) {
      console.log(error, 'Ошибка загрузки завершенный мероприятий')
    }
  }
}
