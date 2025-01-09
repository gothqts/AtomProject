import arrowBottom from '@assets/images/arrowBottom.svg'
import { useOnClickOutside } from './hooks/useOnClickOutside.ts'
import { FC, useRef, useState } from 'react'
import styles from './select.module.css'

interface SelectProps {
  label: string
  options: Array<{ label: string; value: string }>
  onChange: (selectedValue: string) => void
}

const Select: FC<SelectProps> = ({ label, options, onChange }) => {
  const [openMenu, setOpenMenu] = useState(false)
  const [selectedValue, setSelectedValue] = useState<string | null>(null)
  const ref = useRef<HTMLDivElement | null>(null)

  useOnClickOutside(ref, () => {
    setOpenMenu(false)
  })
  const handleOptionClick = (value: string) => {
    console.log(selectedValue === value)
    setSelectedValue(value)
    onChange(value)
  }

  return (
    <div ref={ref} className={styles.container}>
      <div className={styles.option} onClick={() => setOpenMenu(!openMenu)}>
        <label className={styles.label}>{label}</label>
        <img className={openMenu ? styles.arrow_top : styles.arrow_bottom} alt='arrow-bottom' src={arrowBottom} />
      </div>

      <div>
        <ul className={`${styles.menu} ${openMenu ? styles.active : ''}`}>
          {options.map((option) => (
            <li className={styles.menu_element} key={option.label}>
              <label htmlFor={option.value} id={option.value}>
                {option.label}
              </label>
              <input
                id={option.value}
                className={styles.check_input}
                type='radio'
                checked={selectedValue === option.value}
                onChange={() => handleOptionClick(option.value)}
              />
            </li>
          ))}
        </ul>
      </div>
    </div>
  )
}

export default Select
