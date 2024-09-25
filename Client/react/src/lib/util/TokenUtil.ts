import { IPayload } from "../models/Payload";
import { Buffer } from 'buffer';
import { User } from "../models/User";

export class TokenUtil {
    static read(token: string): IPayload {
        const [header, payload, signature] = token.split('.').map(a => Buffer.from(a, 'base64'));
        const json = JSON.parse(payload.toString());

        return {
            aI: parseInt(json.aI),
            cI: parseInt(json.cI),
            nbf: new Date((json.nbf * 1000)),
            exp: new Date((json.exp * 1000)),
            iat: new Date((json.iat * 1000)),
            iss: json.iss
        }
    }

    static getUser(token: string): User {
        var payload = TokenUtil.read(token);

        return {
            authId: payload.aI,
            companyId: payload.cI
        }
    } 
}