import { useStores } from '../../../../stores/rootStoreContext.ts'
import styles from './SubscribeForm.module.css'
import { useState } from 'react'
import { ISignUpParams } from '../../../../models/Events/request/eventRequests.ts'

const SubscribeForm = ({ selectedSignWindow, eventId }) => {
  const { eventStore } = useStores()
  const { isEmailRequired, isPhoneRequired, isFioRequired } = eventStore.currentSubscribeForm

  const generateInputValues = () => ({
    signupWindowId: selectedSignWindow?.id,
    fio: '',
    phone: '',
    email: '',
  })

  const [inputValues, setInputValues] = useState<ISignUpParams>(generateInputValues())

  const handleClick = async (e) => {
    e.preventDefault()
    await eventStore.SignUp(eventId, { ...inputValues })
  }

  return (
    <form className={styles.form} onSubmit={handleClick}>
      {isFioRequired && (
        <div>
          <input
            className={styles.input}
            type='text'
            id='fio'
            name='fio'
            required
            placeholder='Ваше ФИО'
            value={inputValues.fio}
            onChange={(e) => setInputValues({ ...inputValues, fio: e.target.value })}
          />
        </div>
      )}
      {isEmailRequired && (
        <div>
          <input
            className={styles.input}
            type='email'
            id='email'
            name='email'
            required
            placeholder='Ваш email'
            value={inputValues.email}
            onChange={(e) => setInputValues({ ...inputValues, email: e.target.value })}
          />
        </div>
      )}
      {isPhoneRequired && (
        <div>
          <input
            className={styles.input}
            type='tel'
            id='phone'
            name='phone'
            required
            placeholder='Ваш номер телефона'
            value={inputValues.phone}
            onChange={(e) => setInputValues({ ...inputValues, phone: e.target.value })}
          />
        </div>
      )}
      <button className={styles.btn} type='submit'>
        Записаться
      </button>
    </form>
  )
}

export default SubscribeForm
