import React from "react";
import { User } from "../models/User";
import { TokenUtil } from "../util/TokenUtil";
import AppStorage from "../AppStorage";

export interface IAuthContext {
    user?: User;
    token?: string;
    tokenType?: string;
    logged: boolean;

    signIn: (token: string, tokenType: string) => void;
    signOut: () => void;
}

export interface IAuthProvider {
    children: JSX.Element;
}

export const AuthContext = React.createContext<IAuthContext>({
    logged: false,

    signIn: (token: string, tokenType: string) => {},
    signOut: () => {}
});

export const AuthProvider = function ({ children }: IAuthProvider): JSX.Element {
    const [user, setUser] = React.useState<User>();
    const [token, setToken] = React.useState<string>();
    const [tokenType, setTokenType] = React.useState<string>();
    const [logged, setLogged] = React.useState<boolean>(false);

    const signIn = function (token: string, tokenType: string): void {
        var _user = TokenUtil.getUser(token);

        setLogged(true);
        setToken(token);
        setTokenType(tokenType);
        setUser(_user);

        AppStorage.set(token, 'TOKEN');
        AppStorage.set(tokenType, 'TOKEN_TYPE');
    }

    const signOut = function (): void {
        setLogged(false);
        setToken(undefined);
        setTokenType(undefined);
        setUser(undefined);

        AppStorage.unset('TOKEN');
        AppStorage.unset('TOKEN_TYPE');
    }

    React.useEffect(() => {
        const [token, type] = [AppStorage.get<string>('TOKEN'), AppStorage.get<string>('TOKEN_TYPE')];

        if (token && type)
            signIn(token, type);
    }, []);

    return (
        <AuthContext.Provider value={{ user, logged, signIn, signOut }}>
            {children}
        </AuthContext.Provider>
    )
}