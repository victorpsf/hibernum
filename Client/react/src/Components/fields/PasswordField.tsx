import React from "react";
import { FaEye } from "react-icons/fa";
import { FaEyeSlash } from "react-icons/fa6";

export interface IPasswordFieldProps {
    label: string;
    value?: string;
    onTextChange: (value: string) => void;
}

const PasswordField = function ({ label, value, onTextChange }: IPasswordFieldProps): JSX.Element {
    const [view, setView] = React.useState(false);

    const getLabelClass = function (): string {
        return value ? 'transition-all ease-in-out absolute left-2 top-1 text-xs text-gray-600': 'transition-all ease-in-out absolute top-4 left-4 text-base';
    }

    return (
        <div className={'relative w-full'}>
            <div className={getLabelClass()}>{ label }</div>
            <div className={'w-full rounded border border-gray-400 p-4 flex'}>
                <input
                    className={'text-base w-[calc(100%-40px)] outline-0'}
                    type={view ? 'text' : 'password'}
                    value={value}
                    onChange={(event) => onTextChange(event.target.value || '')}
                />

                <div className={'w-[40px] flex justify-center items-center cursor-pointer hover:opacity-60'} onClick={() => setView(!view)}>
                    {!view && <FaEye size={25} />}
                    {view && <FaEyeSlash size={25} />}
                </div>
            </div>

        </div>
    )
};

export default PasswordField;