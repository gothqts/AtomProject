import styles from "./SelectionField.module.css";
import Search_Input_Icon from '../../assets/images/Search_Input_Icon.svg?react';


const SelectionField = () => {
	return(
		<div className={styles.main}>
			
			<div className={styles.search_container}>
				<input type="text" className={styles.search_input} placeholder="Поиск"/>
				<Search_Input_Icon/>
			</div>

			{/* Заглушка фильтров */}
			<div className={styles.filters_container}>
				<div className={styles.filter}>Город</div>
				<div className={styles.filter}>Дата начала</div>
				<div className={styles.filter}>Тематика</div>
				<div className={styles.filter}>Формат</div>
				<div className={styles.filter}>Тематика</div>
			</div>
		</div>
	)
};

export default SelectionField