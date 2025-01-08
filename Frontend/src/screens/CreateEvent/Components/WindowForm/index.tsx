import styles from './WindowForm.module.css'
import { useState } from 'react'
import { useStores } from '../../../../stores/rootStoreContext.ts'
import { observer } from 'mobx-react-lite'

interface IWindowValues {
  title: string
  date: string
  time: string
  maxVisitors: number
  alreadyOccupiedPlaces: number
}

const WindowForm: React.FC = ({ eventId }) => {
  const { eventStore } = useStores()
  const generateInputValues = (): IWindowValues => ({
    title: '',
    date: '',
    time: '',
    maxVisitors: 0,
    alreadyOccupiedPlaces: 0,
  })

  const [values, setValues] = useState(generateInputValues())

  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.target

    setValues((prevValues) => ({
      ...prevValues,
      [name]: value,
    }))
  }

  const handleSubmit = async (event) => {
    event.preventDefault()
    const formattedTime = values.time.replace(':', '-')

    const [year, month, day] = values.date.split('-')
    const formattedDate = `${day}-${month}-${year}`

    await eventStore.CreateRegisterWindow(eventId, {
      ...values,
      time: formattedTime,
      date: formattedDate,
    })
    await eventStore.FetchEventInfoById(eventId)
  }

  return (
    <form onSubmit={handleSubmit} className={styles.form}>
      <input className={styles.input} name='title' value={values.title} onChange={handleChange} placeholder='Введите название окна' />
      <input className={styles.input} name='date' type='date' value={values.date} onChange={handleChange} placeholder='Введите дату' />
      <input className={styles.input} name='time' type='time' value={values.time} onChange={handleChange} placeholder='Введите время' />
      <input
        className={styles.input}
        name='maxVisitors'
        type='text'
        value={values.maxVisitors}
        onChange={handleChange}
        placeholder='Максимальное количество мест'
      />
      <input
        className={styles.input}
        name='alreadyOccupiedPlaces'
        type='text'
        value={values.alreadyOccupiedPlaces}
        onChange={handleChange}
        placeholder='Занятые места'
      />
      <button type='submit' className={styles.btn}>
        Сохранить
      </button>
    </form>
  )
}

export default observer(WindowForm)
