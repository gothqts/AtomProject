import styles from "./home.module.css";
import HomeSection from "../../shared/LastEvents";
import Arrow from "../../assets/images/arrow_down.svg?react"
import BlueBtn from "../../shared/buttons/BlueBtn";


const Home = () => {

    const events = [
        {
            id: 1,
            image: "src/assets/images/Event.svg",
            title: `Лекция "Просто о науке"`,
            description: "Защита частных персональных данных является важным аспектом обеспечения конфиденциальности\n" +
                "и безопасности личности. Законодательство многих стран регулирует обработку таких данных, требуя согласия владельца на их использование и установления мер\n" +
                "по их защите."
        },
        {
            id: 2,
            image: "src/assets/images/Event.svg",
            title: `Лекция "Разработка сайтов"`,
            description: "Защита частных персональных данных является важным аспектом обеспечения конфиденциальности\n" +
                "и безопасности личности. Законодательство многих стран регулирует обработку таких данных, требуя согласия владельца на их использование и установления мер\n" +
                "по их защите."
        }
    ]
    return (
        <div className={styles.home_container}>
            <div className={styles.main_wrapper}>
                <div className={styles.title}>
                    Интересные<br/>мероприятия<br/>для вас
                </div>

                <div className={styles.btn}>
                    <BlueBtn btn_placeholder={"Зарегестрироваться"}/>
                </div>

            </div>

            <div className={styles.selection_of_events}>
                <div className={styles.selection_of_events_title}>Подборка мероприятий</div>
                <HomeSection events={events}/>
            </div>
            <div className={styles.footer_link}>
                Интересные факты о нас
                <Arrow/>
            </div>

        </div>
    );
};

export default Home;