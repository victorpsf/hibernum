import { Buffer } from 'buffer'

const { STORAGE_PREFIX } = {
    STORAGE_PREFIX: '@HIBERNUM@'
}

export default class AppStorage {
    constructor() { }

    static getConstants () {
        return { STORAGE_PREFIX };
    }

    static absoluteKey (...key: string[]): string {
        return Buffer.from([STORAGE_PREFIX].concat(key).join(':')).toString('base64');
    }

    static get <T>(...keys: string[]): T | null {
        const value = window.localStorage.getItem(this.absoluteKey.apply(null, keys));
        if (!value) return null;
        return JSON.parse(value);
    }

    static unset (...keys: string[]): void {
        window.localStorage.removeItem(this.absoluteKey.apply(null, keys));
    }

    static set (value: any, ...keys: string[]) {
        window.localStorage.setItem(this.absoluteKey.apply(null, keys), JSON.stringify(value));
    }
}
