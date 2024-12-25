import { Stack } from '@mui/material'
import { DateTimePicker, renderTimeViewClock } from '@mui/x-date-pickers'

const MuiPicker = ({ title, value, onChange }) => {
  return (
    <Stack spacing={4} sx={{ width: '700px' }}>
      <DateTimePicker
        format='DD - MM - YYYY'
        ampm={false}
        viewRenderers={{
          hours: renderTimeViewClock,
          minutes: renderTimeViewClock,
        }}
        label={title}
        slotProps={{ textField: { variant: 'outlined' } }}
        value={value}
        onChange={(newValue) => {
          onChange(newValue)
        }}
      />
    </Stack>
  )
}

export default MuiPicker
