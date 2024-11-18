import s from './BlueBtn.module.css'
import React from 'react'
interface IProps {
  onClick?: (e: React.MouseEvent<HTMLButtonElement>) => void
  btn_placeholder: string
  type: 'button' | 'submit'
}
const BlueBtn = (props: IProps) => {
  return (
    <div>
      <button type={props.type} className={s.btn} onClick={props.onClick}>
        {props.btn_placeholder}
      </button>
    </div>
  )
}

export default BlueBtn
