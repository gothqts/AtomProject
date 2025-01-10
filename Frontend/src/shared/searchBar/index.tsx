import styles from './searchBar.module.css'
import Lupe from '../../assets/images/lupa.svg?react'
import { useEffect, useState } from 'react'
import { useStores } from '../../stores/rootStoreContext.ts'
import { observer } from 'mobx-react-lite'
import Select from './Select'
import SelectDate from '../SelectDate'
import { IQueryParams } from '../../models/Events/request/eventRequests.ts'
import { formatInputTime, formatInputDate } from '../../utils/formatingData/formatDate.ts'

const SearchBar = observer(({ setParams }) => {
  const { eventStore } = useStores()
  const [value, setValue] = useState<string>('')
  const [selectedFilters, setSelectedFilters] = useState<IQueryParams>({
    date: '',
    time: '',
    city: '',
    online: '',
    take: 6,
    skip: 0,
    subject: '',
    search: '',
  })

  const formatOptions = [
    {
      label: 'Онлайн',
      value: 'true',
    },
    {
      label: 'Оффлайн',
      value: 'false',
    },
    {
      label: 'Все',
      value: '',
    },
  ]

  const handleSearch = (e) => {
    e.preventDefault()
    const formattedDate = formatInputDate(selectedFilters.date)
    const formattedTime = formatInputTime(selectedFilters.time)

    setParams({ ...selectedFilters, name: value, date: formattedDate, time: formattedTime })
  }

  useEffect(() => {
    eventStore.fetchCities()
  }, [eventStore])

  return (
    <div className={styles.container}>
      <form className={styles.form} onSubmit={handleSearch}>
        <div className={styles.input_container}>
          <input
            className={styles.search_input}
            placeholder='Поиск'
            value={value}
            onChange={(e) => {
              const newValue = e.target.value
              setValue(newValue)
              setSelectedFilters((prevFilters) => ({ ...prevFilters, search: newValue }))
            }}
          />
          <button className={styles.search_button} type='submit'>
            <Lupe className={styles.icon} />
          </button>
        </div>
        <div className={styles.select_list}>
          <Select label='Город' options={eventStore.cities} onChange={(selectedValue) => setSelectedFilters({ ...selectedFilters, city: selectedValue })} />
          <SelectDate label='Дата записи' inputType='date' onChange={(selectedValue) => setSelectedFilters({ ...selectedFilters, date: selectedValue })} />
          <SelectDate label='Время записи ' inputType='time' onChange={(selectedValue) => setSelectedFilters({ ...selectedFilters, time: selectedValue })} />
          <Select label='Формат' options={formatOptions} onChange={(selectedValue) => setSelectedFilters({ ...selectedFilters, online: selectedValue })} />
        </div>
      </form>
    </div>
  )
})

export default SearchBar
