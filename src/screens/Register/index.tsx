import styles from "./Register.module.css";
import AuthForm from "../../shared/AuthForm";

const Register = () => {
    return (
        <div className={styles.container}>
            <AuthForm isLogin={false} />
        </div>
    );
};

export default Register;
