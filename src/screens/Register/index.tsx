import styles from "./Register.module.css";
import BlueBtn from "../../shared/buttons/BlueBtn";
import { useState } from "react";

const Register = () => {
    const [showSecondForm, setShowSecondForm] = useState<boolean>(false);

    const handleNext = (e) => {
        e.preventDefault();
        setShowSecondForm(true);
    };

    return (
        <div className={styles.container}>
            <form className={styles.register_form}>
                <div className={styles.form_title}>Регистрация</div>

                {!showSecondForm ? (
                    <>
                        <input className={styles.form_input} placeholder="ФИО" type="text" />
                        <input className={styles.form_input} placeholder="E-mail" type="email" />
                        <input className={styles.form_input} placeholder="Пароль" type="password" />
                        <input className={styles.form_input} placeholder="Повторите пароль" type="password" />
                        <BlueBtn btn_placeholder={"Далее"} onClick={handleNext} />
                    </>
                ) : (
                    <>
                        <input className={styles.form_input} placeholder="Город" type="text" />
                        <input className={styles.form_input} placeholder="Статус" type="text" />
                        <BlueBtn btn_placeholder={"Зарегистрироваться"} />
                    </>
                )}
            </form>
        </div>
    );
};

export default Register;
