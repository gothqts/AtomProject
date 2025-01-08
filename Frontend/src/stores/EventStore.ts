import { makeAutoObservable, runInAction } from 'mobx'
import RootStore from './rootStore.ts'
import { http } from '../services/http'
import EventsService from '../services/Events/EventsService.ts'
import { IUpcomingEvents } from '../screens/Home/types/homeTypes.ts'
import { IBasicEventInfo, MyCreatingEvent } from '../models/Events/response/EventsResponse.ts'
import { IQueryParams } from '../models/Events/request/eventRequests.ts'

interface ICity {
  label: string
  value: string
}

type ICities = ICity[]

export default class EventStore {
  rootStore: RootStore
  cities: ICities = []
  creatingEvent: MyCreatingEvent = {
    id: '',
    creationDate: '',
    isPublic: false,
    title: '',
    bannerImageFilepath: '',
    isOnline: false,
    isSignupOpened: false,
    city: '',
    address: '',
    dateStart: '',
    dateEnd: '',
    description: '',
    signupWindows: [],
    signupForm: {
      isFioRequired: false,
      isEmailRequired: false,
      isPhoneRequired: false,
      dynamicFields: [],
    },
    contacts: [],
  }

  allEvents: IBasicEventInfo[] = []
  upcomingEvents: IUpcomingEvents[] = []
  myEvents: IBasicEventInfo[] = []
  myPastEvents: IBasicEventInfo[] = []
  userActivity: IBasicEventInfo[] = []
  userPastActivity: IBasicEventInfo[] = []

  constructor(rootStore: RootStore) {
    makeAutoObservable(this, { rootStore: false })
    this.rootStore = rootStore
  }

  setCreatingEventData(field: keyof IBasicEventInfo, value: string | boolean) {
    if (this.creatingEvent) {
      this.creatingEvent = { ...this.creatingEvent, [field]: value }
    }
  }

  async FetchUpcomingEvents() {
    try {
      const response = await EventsService.FetchUpcomingEvents()
      runInAction(() => {
        this.upcomingEvents = response.data.events
      })
    } catch (error) {
      console.log(error, 'Ошибка загрузки последних событий')
    }
  }

  async fetchCities(skip: number = 0, take: number = 10) {
    try {
      const response = await http.get(`/api/cities?skip=${skip}&take=${take}`)
      runInAction(() => {
        this.cities = response.data.cities.map((city) => ({
          label: city.name,
          value: city.name,
        }))
      })
    } catch (error) {
      console.error('Ошибка при загрузке городов:', error)
    }
  }

  async CreateEvent() {
    try {
      const response = await EventsService.createEvent()
      runInAction(() => {
        this.creatingEvent.id = response.data.id
      })
    } catch (error) {
      console.log(error, 'Ошибка создания меро')
    }
  }

  async UpdateEvent(data, id) {
    try {
      const response = await EventsService.UpdateEvent(data, id)
      runInAction(() => {
        if (response.status == 200) {
          alert('Изменение сохранено')
        }
      })
    } catch (error) {
      console.log(error, 'Ошибка обноления события')
    }
  }

  async FetchMyEvents() {
    try {
      const response = await EventsService.FetchMyEvents()
      runInAction(() => {
        if (response.status == 200) {
          this.myEvents = response.data.events
        }
      })
    } catch (error) {
      console.log(error, 'Ошибка загрузки моих мероприятий')
    }
  }

  async FetchMyPastEvents() {
    try {
      const response = await EventsService.FetchMyPastEvents()
      runInAction(() => {
        if (response.status == 200) {
          this.myPastEvents = response.data.events
        }
      })
    } catch (error) {
      console.log(error, 'Ошибка загрузки завершенный мероприятий')
    }
  }

  async FetchUserActivity() {
    try {
      const response = await EventsService.FetchUserActivity()
      runInAction(() => {
        if (response.status == 200) {
          this.userActivity = response.data.events
        }
      })
    } catch (error) {
      console.log(error, 'Ошибка загрузки активности пользователя')
    }
  }

  async FetchUserPastActivity() {
    try {
      const response = await EventsService.FetchUserPastActivity()
      runInAction(() => {
        if (response.status == 200) {
          this.userPastActivity = response.data.events
        }
      })
    } catch (error) {
      console.log(error, 'Ошибка загрузки прошедшей активности пользователя')
    }
  }

  async FetchEventInfoById(eventId: string) {
    try {
      const response = await EventsService.FetchFullInfoAboutMyEvent(eventId)
      runInAction(() => {
        if (response.data.title == 'Новое мероприятие') {
          this.creatingEvent = { ...response.data, city: 'Введите город', address: 'Введите адрес' }
        } else {
          this.creatingEvent = response.data
        }
      })
    } catch (err) {
      console.log(err)
    }
  }

  async DeleteEvent(eventId: string) {
    try {
      const response = await EventsService.DeleteEventById(eventId)
      runInAction(() => {
        if (response.status == 200) {
          this.creatingEvent.id = ''
          alert('Мероприятие удалено')
        }
      })
    } catch (error) {
      console.log(error, 'Ошибка удаления мероприятия')
    }
  }

  async UpdateBanner(ImgFile, eventId: string) {
    try {
      const response = await EventsService.UpdateEventBanner(ImgFile, eventId)
      runInAction(() => {
        if (response.status == 200) {
          alert('Баннер обновлен!')
          this.creatingEvent.bannerImageFilepath = response.data.image
        }
      })
    } catch (error) {
      console.log('Ошибка загрузки баннера')
    }
  }

  async FetchFilteredEvents(queryParams: IQueryParams) {
    try {
      const response = await EventsService.FetchEventByFilters(queryParams)
      runInAction(() => {
        if (response.status === 200) {
          this.allEvents = response.data.events
        }
      })
    } catch (error) {
      console.log(error, 'Ошибка получения отфильтрованных событий')
    }
  }

  async CreateRegisterWindow(eventId: string, { title, date, time, maxVisitors, alreadyOccupiedPlaces }) {
    try {
      await EventsService.CreateWindow(eventId, {
        title,
        date,
        time,
        maxVisitors,
        alreadyOccupiedPlaces,
      })
    } catch (err) {
      console.log(err)
    }
  }

  async DeleteRegisterWindow(eventId: string, windowsId: string) {
    try {
      await EventsService.DeleteWindow(eventId, windowsId)
    } catch (err) {
      console.log(err)
    }
  }

  async UpdateRegisterWindow(eventId: string, windowsId: string, { title, date, time, maxVisitors, alreadyOccupiedPlaces }) {
    try {
      await EventsService.UpdateWindow(eventId, windowsId, {
        title,
        date,
        time,
        maxVisitors,
        alreadyOccupiedPlaces,
      })
    } catch (err) {
      console.log(err)
    }
  }
}
