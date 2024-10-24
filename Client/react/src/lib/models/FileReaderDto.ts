import {FileEntity} from "./File";
import {base64ToBytes} from '../util/BinaryManager'

export interface FileReaderDto {
    id?: number;
    name: string;
    mime: string;
    lastModified: Date;
    size: number;
    bytes: number[];
}

export const toFileDto = function (file?: FileEntity): FileReaderDto | undefined {
    if (!file)
        return undefined;

    return {
        id: file.id,
        name: file.name,
        mime: file.mime,
        lastModified: new Date(file.lastModified),
        size: file.data.size,
        bytes: base64ToBytes(file.data.data)
    }
}