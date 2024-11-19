import styles from "./LastEvents.module.css";
const Index = ({events}) => {
    return (
        <>
            {events.map(event => (
                event.id % 2 !== 0 ? (
                    <div key={event.id} className={styles.wrapper_odd}>
                        <img src={event.image} alt="Изображение мероприятия"/>
                        <div className={styles.event_content}>
                            <div className={styles.event_header}>
                                {event.title}
                            </div>
                            <div className={styles.event_description}>
                                {event.description}
                            </div>
                        </div>
                    </div>
                ) : (
                    <div key={event.id} className={styles.wrapper_even}>
                        <div className={styles.event_content}>
                            <div className={styles.event_header}>
                                {event.title}
                            </div>
                            <div className={styles.event_description}>
                                {event.description}
                            </div>
                        </div>
                        <img src={event.image} alt="Изображение мероприятия"/>

                    </div>
                )
            ))}
        </>
    );
};

export default Index;
