import styles from "./LastEvents.module.css";
import Circle from "../../assets/images/circle_vector.svg?react";
import Square from "../../assets/images/square.svg?react"

const LastEvents = ({events}) => {
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

export default LastEvents;
