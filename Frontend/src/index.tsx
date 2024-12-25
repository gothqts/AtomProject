import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import { RootStoreContext } from './stores/rootStoreContext.ts'
import RootStore from './stores/rootStore.ts'
import { LocalizationProvider } from '@mui/x-date-pickers'
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs'

createRoot(document.getElementById('root')!).render(
  <RootStoreContext.Provider value={new RootStore()}>
    <LocalizationProvider dateAdapter={AdapterDayjs}>
      <StrictMode>
        <App />
      </StrictMode>
    </LocalizationProvider>
  </RootStoreContext.Provider>
)
