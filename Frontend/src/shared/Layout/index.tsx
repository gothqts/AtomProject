import LogoIcon from '../../assets/images/Logo.svg?react';
import {Link, Outlet} from "react-router-dom";
import {urls} from "../../navigate/app.urls.ts";
import styles from "./Layout.module.css";
import {Suspense} from "react";

const Layout = () => {
    return (
        <div className={styles.container}>
            <div className={styles.navbar}>

                <Link to={urls.home}>
                    <LogoIcon/>
                </Link>

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
            <div className={styles.page_content}>
                <Suspense fallback={<p>Loading...</p>}>
                    <Outlet/>
                </Suspense>
            </div>
        </div>
    )

};

export default Layout;

