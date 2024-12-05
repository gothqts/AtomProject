import styles from './searchBar.module.css'
import Lupe from '../../assets/images/lupa.svg?react'
import { useEffect, useState } from 'react'
import { useStores } from '../../stores/rootStoreContext.ts'
import { observer } from 'mobx-react-lite'
import Select from './Select'

const SearchBar = observer(() => {
  const { userStore } = useStores()
  const [value, setValue] = useState('')
  const [selectedFilters, setSelectedFilters] = useState({})
  const [params, setParams] = useState({})
  const handleSearch = (e) => {
    e.preventDefault()
    setParams({ ...selectedFilters, name: value })
  }

  useEffect(() => {
    userStore.fetchCities()
  }, [userStore])
  return (
    <div className={styles.container}>
      <div className={styles.input_container}>
        <input className={styles.search_input} placeholder='Поиск' />
        <button className={styles.search_button}>
          <Lupe className={styles.icon} />
        </button>
      </div>
      <form onSubmit={handleSearch}>
        <div className={styles.select_list}>
          <Select label='Город' options={userStore.cities} onChange={(selectedValue) => setSelectedFilters({ ...selectedFilters, city: selectedValue })} />
          <Select
            label='Дата начала'
            options={userStore.cities}
            onChange={(selectedValue) => setSelectedFilters({ ...selectedFilters, city: selectedValue })}
          />
          <Select label='Время' options={userStore.cities} onChange={(selectedValue) => setSelectedFilters({ ...selectedFilters, city: selectedValue })} />
          <Select label='Формат' options={userStore.cities} onChange={(selectedValue) => setSelectedFilters({ ...selectedFilters, city: selectedValue })} />
          <Select label='Тематика' options={userStore.cities} onChange={(selectedValue) => setSelectedFilters({ ...selectedFilters, city: selectedValue })} />
        </div>
      </form>
    </div>
  )
})

export default SearchBar
