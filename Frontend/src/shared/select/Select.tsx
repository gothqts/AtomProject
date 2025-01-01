import { SelectHTMLAttributes } from 'react'
import { useSelect, UseSelectParams } from './useSelect'
import { useSelectOptions, UseSelectOptionsParams } from './useSelectOptions'

type SelectProps = Pick<SelectHTMLAttributes<HTMLSelectElement>, 'name'>

export function Select<Option>({
  selectedOption,
  options,
  onChange,
  getLabel,
  ...props
}: UseSelectParams<Option> & UseSelectOptionsParams<Option> & SelectProps) {
  const selectProps = useSelect({ selectedOption, options, onChange })
  const selectOptions = useSelectOptions({ options, getLabel })

  return (
    <select {...props} {...selectProps}>
      {selectOptions}
    </select>
  )
}
