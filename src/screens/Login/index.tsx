import styles from "./Login.module.css";
import AuthForm from "../../shared/AuthForm";

const Login = () => {
    return (
        <div className={styles.container}>
            <AuthForm isLogin={true} />
        </div>
    );
};

export default Login;
