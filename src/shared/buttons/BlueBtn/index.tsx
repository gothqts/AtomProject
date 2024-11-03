import s from './BlueBtn.module.css'
const BlueBtn = ({btn_placeholder}) => {
    return (
        <div>
          <button className={s.btn}>{btn_placeholder}</button>
        </div>
    );
};

export default BlueBtn;