import React from "react";

export interface INumericFieldProps {
    label: string;
    value?: string;
    float?: boolean;
    floatLength?: number;
    onTextChange: (value?: string) => void;
}

const NumericField = function ({ label, value, onTextChange, float, floatLength }: INumericFieldProps): JSX.Element {
    const getLabelClass = function (): string {
        return value ? 'transition-all ease-in-out absolute left-2 top-1 text-xs text-gray-600': 'transition-all ease-in-out absolute top-4 left-4 text-base';
    }

    const handleOnTextChange = function (value: string): void {
        const _value = !value ? undefined: value;

        if (!_value)
            return onTextChange(_value);

        const [number, ...floatArray] = _value.replace(/\./g, ',').split('').filter(a => float ? /\d|\,/g.test(a): /\d/g.test(a)).join('').split(',');

        if (floatArray.length == 0)
            return onTextChange(number);

        let floatNumbers = floatArray.join('');
        if (floatLength) floatNumbers = floatNumbers.substring(0, floatLength);

        return onTextChange(`${number},${floatNumbers}`);
    }

    return (
        <div className={'relative w-full'}>
            <div className={getLabelClass()}>{ label }</div>
            <input className={'rounded border border-gray-400 w-full p-4 text-base outline-0'} value={value} onChange={(event) => handleOnTextChange(event.target.value || '')} />
        </div>
    )
}

export default NumericField