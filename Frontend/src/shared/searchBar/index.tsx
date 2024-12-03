import styles from './searchBar.module.css'
import Loupe from '../../assets/images/lupa.svg?react'
const SearchBar = () => {
  return (
    <div className={styles.container}>
      <div>
        <input className={styles.search_input} placeholder='Поиск' />
        <Loupe />
      </div>
      <select name='city'>
        <option>1</option>
        <option>2</option>
      </select>
      <select name='date'>
        <option>1</option>
        <option>2</option>
      </select>
      <select name='time'>
        <option>1</option>
        <option>2</option>
      </select>
      <select name='theme'>
        <option>1</option>
        <option>2</option>
      </select>
      <select name='format'>
        <option>1</option>
        <option>2</option>
      </select>
    </div>
  )
}

export default SearchBar
