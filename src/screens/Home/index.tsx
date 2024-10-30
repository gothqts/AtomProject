import Layout from "../../shared/Layout/index.tsx";
import styles from "./home.module.css";
import RegisterBtn from "../../shared/buttons/register";
import HomeSection from "../../shared/homeSection";


const Home = () => {
    return (
        <div className={styles.home_container}>
            <Layout/>
            <div className={styles.main_wrapper}>

                <div className={styles.title}>
                    Интересные<br/>мероприятия<br/>для вас
                </div>

                <div className={styles.btn}>
                    <RegisterBtn/>
                </div>

            </div>

            <div className={styles.section_about_event}>
                <HomeSection/>
            </div>



        </div>
    );
};

export default Home;