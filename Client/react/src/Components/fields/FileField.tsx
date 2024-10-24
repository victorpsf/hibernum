import React, {MouseEvent} from "react";
import {FileReaderDto} from "../../lib/models/FileReaderDto";
import {bufferToBytes} from "../../lib/util/BinaryManager";
import {UUID} from "../../lib/util/Rand";
import { FaTrash } from "react-icons/fa";

export interface IFileFieldProps {
    label: string;
    multiple?: boolean;
    filter?: string;
    value?: FileReaderDto[];
    onFileChange: (value: FileReaderDto[]) => void;
}

const FileField = function ({ label, multiple, filter, value, onFileChange }: IFileFieldProps): JSX.Element {
    const id = React.useRef<string>(UUID().toString());

    const getLabelClass = function (): string {
        return value && value.length ? 'transition-all ease-in-out absolute left-2 top-1 text-xs text-gray-600': 'transition-all ease-in-out absolute top-4 left-4 text-base';
    }

    const readFiles = function(files: FileList): File[] {
        var _files: File[] = []
        for (var i = 0; i < files.length; i++)
            _files.push(files[i]);
        return _files;
    }

    const ReadFile = (file: File): Promise<FileReaderDto | null> => new Promise((resolve: (dto: FileReaderDto | null) => void) => {
        try {
            const reader = new FileReader();

            reader.onerror = () => resolve(null);
            reader.onloadend = (e) => {
                resolve({
                    name: file.name,
                    mime: file.type,
                    lastModified: new Date(file.lastModified),
                    size: file.size,
                    bytes: bufferToBytes(e?.target?.result as ArrayBuffer || new ArrayBuffer(0))
                })
            }

            reader.readAsArrayBuffer(file);
        }

        catch (e)
        { resolve(null); }
    })

    const onFileInput = async function (event: Event): Promise<void> {
        var target = event.target as HTMLInputElement;

        if (!target || !target.files) {
            onFileChange([]);
            return;
        }

        const files = await Promise.all(
            readFiles(target.files)
                .map(a => ReadFile(a))
        );

        onFileChange(files.filter(a => a != null) as FileReaderDto[]);
    }

    const inputClick = function (event: MouseEvent): void {
        var target = event.target as HTMLDivElement;

        if (target.id !== id.current)
            return;

        var input: HTMLInputElement = document.createElement('input');

        input.type = 'file';
        input.multiple = multiple || false;
        input.accept = filter || '*/*';

        input.oninput = (e) => onFileInput(e);
        input.click();
    }

    const removeFile = function (index: number): void {
        value?.splice(index, 1);
        onFileChange(value || []);
    }

    return (
        <div className={'relative w-full'}>
            <div className={getLabelClass()}>{ label }</div>
            <div
                id={id.current}
                className={'w-full rounded border border-gray-400 w-full p-2 pt-6 text-base outline-0 min-h-[58px] cursor-pointer hover:opacity-60 flex'}
                onClick={(event: MouseEvent<HTMLDivElement>) => inputClick(event)}
            >
                {value && value.length > 0 && (
                    value.map((a, i) => (
                        <div key={UUID().toString()} className={'bg-gray-500 text-white p-2 rounded flex justify-center items-center'} onClick={(e) => {
                            e.preventDefault();
                            removeFile(i)
                        }}>
                            <p>{a.name}</p>
                            <div className={'mx-2'}><FaTrash size={14}/></div>
                        </div>
                    ))
                )}
            </div>
        </div>
    )
};

export default FileField;