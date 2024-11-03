import s from './BlueBtn.module.css'
const BlueBtn = ({btn_placeholder, onClick}) => {
    return (
        <div>
          <button className={s.btn} onClick={onClick}>{btn_placeholder}</button>
        </div>
    );
};

export default BlueBtn;