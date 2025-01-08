import React, { useState, useRef } from 'react'
import arrowBottom from '@assets/images/arrowBottom.svg'
import styles from './selectDate.module.css'
import { useOnClickOutside } from '../searchBar/Select/hooks/useOnClickOutside.ts'

interface SelectDateProps {
  label: string
  inputType: 'date' | 'time'
  onChange: (value: string) => void
}

const SelectDate: React.FC<SelectDateProps> = ({ label, inputType, onChange }) => {
  const [openMenu, setOpenMenu] = useState<boolean>(false)
  const [selectedValue, setSelectedValue] = useState('')
  const ref = useRef<HTMLDivElement | null>(null)

  useOnClickOutside(ref, () => {
    setOpenMenu(false)
  })

  const handleChange = (event) => {
    const value = event.target.value
    setSelectedValue(value)
    onChange(value)
  }

  return (
    <div ref={ref} className={styles.container}>
      <div className={styles.option} onClick={() => setOpenMenu(!openMenu)}>
        <label className={styles.label}>{label}</label>
        <img className={openMenu ? styles.arrow_top : styles.arrow_bottom} alt='arrow-bottom' src={arrowBottom} />
      </div>
      {openMenu && (
        <div className={`${styles.dropdown} ${openMenu ? styles.active : ''}`}>
          <input type={inputType} value={selectedValue} onChange={handleChange} className={styles.input} />
        </div>
      )}
    </div>
  )
}

export default SelectDate
