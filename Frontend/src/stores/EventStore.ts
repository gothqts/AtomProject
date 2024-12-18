import { makeAutoObservable, observable, runInAction } from 'mobx'
import RootStore from './rootStore.ts'
import { useStores } from './rootStoreContext.ts'
import UserInfoService from '../services/UserInfo/UserInfoService.ts'
import rootStore from './rootStore.ts'
import { IUser } from '../models/User/response/User.ts'
import axios from 'axios'
import { http } from '../services/http'
import EventsService from '../services/Events/EventsService.ts'

interface ICity {
  label: string
  value: string
}
interface IEvent {
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
}
type ICities = ICity[]

export default class EventStore {
  rootStore: RootStore
  cities: ICities = []
  creatingEvent: null | IEvent
  constructor(rootStore: RootStore) {
    makeAutoObservable(this, { rootStore: false })
    this.rootStore = rootStore
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
    } catch (error) {
      console.log(error, 'Ошибка создания меро')
    }
  }
}
