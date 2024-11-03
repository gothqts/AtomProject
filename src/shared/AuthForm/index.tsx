import {useState} from "react";
import styles from "./AuthForm.module.css"
import BlueBtn from "../../shared/buttons/BlueBtn";
import {Link} from "react-router-dom";
import {urls} from "../../navigate/app.urls.ts";

const AuthForm = ({isLogin}) => {
    const [showSecondForm, setShowSecondForm] = useState<boolean>(false);

    const handleNext = (e) => {
        e.preventDefault();
        setShowSecondForm(true);
    };

    return (
        <form className={styles.register_form}>
            <div className={styles.form_title}>{isLogin ? "Вход" : "Регистрация"}</div>

            {!showSecondForm ? (
                <>
                    {!isLogin && (
                        <input className={styles.form_input} placeholder="ФИО" type="text"/>
                    )}
                    <input className={styles.form_input} placeholder="E-mail" type="email"/>
                    <input className={styles.form_input} placeholder="Пароль" type="password"/>
                    {!isLogin && (
                        <input className={styles.form_input} placeholder="Повторите пароль" type="password"/>
                    )}
                    {!isLogin && (
                        <Link to={urls.login} className={styles.form_paragraph}>Уже есть аккаунт?</Link>
                    )}
                    <BlueBtn btn_placeholder={isLogin ? "Войти" : "Далее"} onClick={handleNext}/>
                </>
            ) : (
                <>
                    <input className={styles.form_input} placeholder="Город" type="text"/>
                    <input className={styles.form_input} placeholder="Статус" type="text"/>
                    <BlueBtn btn_placeholder={isLogin ? "Войти" : "Зарегистрироваться"}/>
                </>
            )}
        </form>
    );
};

export default AuthForm;
