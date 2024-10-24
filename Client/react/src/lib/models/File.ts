export interface FileData {
    encode: 'BASE64' | 'HEX' | 'BINARY';
    data: string;
    size: number;
}

export interface FileEntity {
    id: number;
    name: string;
    mime: string;
    lastModified: string;
    data: FileData;
}