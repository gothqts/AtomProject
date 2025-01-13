import styles from './BooleanToogle.module.css'

interface BooleanToggleProps {
  label: string
  value: boolean
  onChange: (newValue: boolean) => void
}

const BooleanToggle: React.FC<BooleanToggleProps> = ({ label, value, onChange }) => {
  return (
    <div className={styles.container}>
      <div className={styles.label}>{label}</div>
      <button onClick={() => onChange(true)} className={value ? styles.button_active : styles.button}>
        Да
      </button>
      <button onClick={() => onChange(false)} className={!value ? styles.button_active : styles.button}>
        Нет
      </button>
    </div>
  )
}

export default BooleanToggle
