import { Stack, TextField } from '@mui/material'
import { DateTimePicker } from '@mui/x-date-pickers'
import { useState, useEffect } from 'react'

const MuiPicker = ({ title, value, onChange }) => {
  const [selectedDateTime, setSelectedTime] = useState<Date | null>(value || null)

  useEffect(() => {
    setSelectedTime(value)
  }, [value])

  const changeHandler = (newValue) => {
    setSelectedTime(newValue)
    onChange(newValue)
  }

  return (
    <Stack spacing={4} sx={{ width: '700px' }}>
      <DateTimePicker label={title} renderInput={(params) => <TextField {...params} />} value={selectedDateTime} onChange={changeHandler} />
    </Stack>
  )
}

export default MuiPicker
