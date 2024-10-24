import { Buffer } from 'buffer'

export const RandomNumber = function (min: number, max: number): number {
    return Math.floor(Math.random() * (max - min + 1) + min);
}

interface IUUID {
    buffer: () => Buffer;
    toString: () => string;
}

export const UUID = function (): IUUID {
    const parts = [
        [0,0,0,0].map(a => RandomNumber(0,255)),
        [0,0].map(a => RandomNumber(0,255)),
        [0,0].map(a => RandomNumber(0,255)),
        [0,0].map(a => RandomNumber(0,255)),
        [0,0,0,0,0,0].map(a => RandomNumber(0,255)),
    ];

    const toArrayBytes = function (value: string) {
        const bytes = [];

        for (let x = 0; x < value.length; x++)
            bytes.push(value.charCodeAt(x));

        return bytes;
    }

    return {
        buffer: function (): Buffer {
            const [v1, v2, v3, v4, v5] = parts.map(a => a.map(b => `000${b.toString(16)}`.slice(-2)).join(''))

            return Buffer.from(
                toArrayBytes(v1)
                    .concat(toArrayBytes('-'))
                    .concat(toArrayBytes(v2))
                    .concat(toArrayBytes('-'))
                    .concat(toArrayBytes(v3))
                    .concat(toArrayBytes('-'))
                    .concat(toArrayBytes(v4))
                    .concat(toArrayBytes('-'))
                    .concat(toArrayBytes(v5))
            );
        },
        toString: function (): string { return this.buffer().toString('utf-8'); }
    };
}