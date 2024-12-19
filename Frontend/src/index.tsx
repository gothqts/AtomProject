import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import { RootStoreContext } from './stores/rootStoreContext.ts'
import RootStore from './stores/rootStore.ts'
import { LocalizationProvider } from '@mui/x-date-pickers'
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFnsV3'

createRoot(document.getElementById('root')!).render(
  <RootStoreContext.Provider value={new RootStore()}>
    <LocalizationProvider dateAdapter={AdapterDateFns}>
      <StrictMode>
        <App />
      </StrictMode>
    </LocalizationProvider>
  </RootStoreContext.Provider>
)
