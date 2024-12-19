export const formatDate = (isoDate: string): string => {
  const date = new Date(isoDate)
  const months = ['января', 'февраля', 'марта', 'апреля', 'мая', 'июня', 'июля', 'августа', 'сентября', 'октября', 'ноября', 'декабря']
  const day = date.getDate()
  const month = months[date.getMonth()]
  const hours = date.getHours().toString().padStart(2, '0')
  const minutes = date.getMinutes().toString().padStart(2, '0')
  return `${day} ${month} ${hours}:${minutes}`
}
