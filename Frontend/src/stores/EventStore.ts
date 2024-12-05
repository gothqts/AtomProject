import { makeAutoObservable, observable, runInAction } from 'mobx'
import RootStore from './rootStore.ts'
import { useStores } from './rootStoreContext.ts'
import UserInfoService from '../services/UserInfo/UserInfoService.ts'
import rootStore from './rootStore.ts'
import { IUser } from '../models/User/User.ts'
import axios from 'axios'
import { http } from '../services/http'

interface ICity {
  label: string
  value: string
}

type ICities = ICity[]

export default class EventStore {
  rootStore: RootStore
  cities: ICities = []

  constructor(rootStore: RootStore) {
    makeAutoObservable(this, { rootStore: false })
    this.rootStore = rootStore
  }
  async fetchCities(skip: number = 0, take: number = 10) {
    try {
      const response = await http.get(`/api/cities?skip=${skip}&take=${take}`)
      this.cities = response.data.cities.map((city) => ({
        label: city.name, // Ключ label будет равен имени города
        value: city.name, // Ключ value тоже будет равен имени города
      }))
      console.log(response.data)
    } catch (error) {
      console.error('Ошибка при загрузке городов:', error)
    }
  }
}
