import { Stack, TextField } from '@mui/material'
import { DateTimePicker } from '@mui/x-date-pickers'
import { useState } from 'react'
import { useStores } from '../../stores/rootStoreContext.ts'

const MuiPicker = ({ title }) => {
  const [selectedDateTime, setSelectedTime] = useState<Date | null>(null)
  const { eventStore } = useStores()

  const changeHandler = (newValue) => {
    setSelectedTime(newValue)
    eventStore.setDateStart(newValue)
  }
  return (
    <Stack spacing={4} sx={{ width: '700px' }}>
      <DateTimePicker label={title} renderInput={(params) => <TextField {...params} />} value={selectedDateTime} onChange={changeHandler} />
    </Stack>
  )
}

export default MuiPicker
