import styles from "./homeSection.module.css";

const HomeSection = () => {
    return (
        <div className={styles.section_wrapper}>
            <div className={styles.wrapper_title}>Подборка мероприятий</div>
            <img src="src/assets/images/Event.svg" alt="Изображение мероприятия" width="400x" height="400px"/>
            <div className={}>Лекция "Просто о науке"</div>
            <div>Защита частных персональных
                данных является важным аспектом обеспечения конфиденциальности
                и безопасности личности. Законодательство многих стран регулирует обработку таких данных, требуя
                согласия владельца на их использование и установления мер
                по их защите.
            </div>
        </div>
    );
};

export default HomeSection;