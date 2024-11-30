import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import { RootStoreContext } from './stores/rootStoreContext.ts'
import RootStore from './stores/rootStore.ts'

createRoot(document.getElementById('root')!).render(
  <RootStoreContext.Provider value={new RootStore()}>
    <StrictMode>
      <App />
    </StrictMode>
  </RootStoreContext.Provider>
)
