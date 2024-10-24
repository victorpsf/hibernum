import React from "react";
import {UUID} from "../../lib/util/Rand";

export interface ISelectFieldProps<T> {
    label: string;
    value?: T;
    options: T[];
    handleSelectedElement: (value?: T) => JSX.Element;
    handleOptionElement: (value: T) => JSX.Element;
    onChange: (value?: T) => void;
}

const SelectField = function <T>({
    value,
    options,
    label,
    onChange,
    handleSelectedElement,
    handleOptionElement
}: ISelectFieldProps<T>
): JSX.Element {
    const [open, setOpen] = React.useState(false);

    const getLabelClass = function (): string {
        return value ? 'transition-all ease-in-out absolute left-2 top-1 text-xs text-gray-600': 'transition-all ease-in-out absolute top-4 left-4 text-base';
    }

    return (
        <div className={'relative w-full'}>
            <div className={getLabelClass()}>{ label }</div>
            <div
                className={'w-full rounded border border-gray-400 w-full p-4 text-base outline-0 min-h-[58px] cursor-pointer hover:opacity-60'}
                onClick={() => setOpen(!open)}
            >
                {handleSelectedElement(value)}
            </div>

            {open && (
                <div className={'w-full p-2 border border-gray-400 my-1 rounded p-2'}>
                    {
                        options.map(
                            (option, index) => (
                                <div
                                    key={UUID().toString()}
                                    className={(index === 0 ? '': 'border-t').concat('cursor-pointer hover:opacity-60')}
                                    onClick={() => onChange(option)}
                                >
                                    {handleOptionElement(option)}
                                </div>
                            )
                        )
                    }
                </div>
            )}
            {/*<input  value={value} onChange={(event) => handleOnTextChange(event.target.value || '')} />*/}
        </div>
    )
}

export default SelectField