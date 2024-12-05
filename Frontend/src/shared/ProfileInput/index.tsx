import styles from './ProfileInput.module.css'


const ProfileInput = ({ title, type }) => {
	return(
		<div className={styles.container}>
			<div className={styles.title}>{title}</div>
			<div className={styles.input_container}>
				<input className={styles.input} type={type} />
				<button className={styles.edit}>Изменить</button>
			</div>
		</div>
	)
}

export default ProfileInput