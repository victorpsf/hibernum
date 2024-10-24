import {Buffer} from 'buffer'

export const uint8ToBytes = function (buffer: Uint8Array): number[] {
    var bytes: number[] = [];

    for (var i = 0; i < buffer.length; i++)
        bytes.push(buffer[i]);

    return bytes;
}

export const bufferToBytes = function (buffer: ArrayBuffer): number[] {
    return uint8ToBytes(new Uint8Array(buffer));
}

export const base64ToBytes = function (value: string): number[] {
    var buffer = Buffer.from(value, 'base64');
    var bytes: number[] = [];

    buffer.forEach((a, i, j) => {
        bytes.push(a);
    })

    return bytes;
}