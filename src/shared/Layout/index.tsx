import LogoIcon from '../../assets/images/Logo.svg?react'
import {Link} from "react-router-dom";
import {urls} from "../../navigate/app.urls.ts";
import styles from "./Layout.module.css"

const Layout = () => {
    return (
        <div className={styles.layout_wrapper}>
            <LogoIcon/>
            <li className={styles.link_list}>
                <Link className={styles.link}
                      to={urls.events}>
                    Мероприятия
                </Link>
                <Link className={styles.link}
                      to={urls.about}>
                    О сервисе
                </Link>
                <Link className={styles.link} to={urls.reviews}>
                    Отзывы
                </Link>
            </li>

            <Link className={styles.link_auth} to={urls.register}>
                Вход / Регистрация
            </Link>
        </div>
    );
};

export default Layout;