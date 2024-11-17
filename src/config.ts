interface IConfig {
  API_URL: string
  MODE: 'development' | 'production' | 'test'
}

const config: IConfig = {
  API_URL: import.meta.env.REACT_APP_API_URL || 'http://localhost:8080',
  MODE: import.meta.env.NODE_ENV || 'development',
}

export default config
