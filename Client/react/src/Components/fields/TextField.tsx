import React from "react";

export interface ITextFieldProps {
    label: string;
    value?: string;
    onTextChange: (value: string) => void;
}

const TextField = function ({ label, value, onTextChange }: ITextFieldProps): JSX.Element {
    const getLabelClass = function (): string {
        return value ? 'transition-all ease-in-out absolute left-2 top-1 text-xs text-gray-600': 'transition-all ease-in-out absolute top-4 left-4 text-base';
    }

    return (
        <div className={'relative w-full'}>
            <div className={getLabelClass()}>{ label }</div>
            <input className={'rounded border border-gray-400 w-full p-4 text-base outline-0'} value={value} onChange={(event) => onTextChange(event.target.value || '')} />
        </div>
    )
};

export default TextField;