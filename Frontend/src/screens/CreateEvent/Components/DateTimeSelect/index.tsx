import styles from './DateTimeSelect.module.css'

const DateTimeInput: React.FC<{ value: string; onChange: (newValue: string) => void }> = ({ value, onChange }) => {
  const formattedValue = value ? new Date(value).toISOString().slice(0, 16) : ''

  return (
    <div>
      <input
        className={styles.input}
        id='datetime-input'
        type='datetime-local'
        value={formattedValue}
        onChange={(e) => {
          const newValue = new Date(e.target.value + ':00.000Z').toISOString()
          onChange(newValue)
        }}
      />
    </div>
  )
}
export default DateTimeInput
