import React from "react";
import { useNavigate } from "react-router";
import { IRoutePath } from "../../Pages/Routes";

export interface IRouteContext {
    current: () => string;
    query: <T>() => T;
    navigate: (path: IRoutePath, params?: { [key: string]: any; }) => void;
    goBack: () => void;
}

export interface IRouteProviderProps {
    children: JSX.Element;
}

export const RouteContext = React.createContext<IRouteContext>({
    current: () => '',
    goBack: () => {},
    navigate: (path: IRoutePath, query?: { [key: string]: string; }) => {},
    query: function <T>(): T { return {} as T; }
});

export const RouteProvider = function ({ children }: IRouteProviderProps): JSX.Element {
    const nav = useNavigate();

    const createPath = function (path: string, params?: { [key: string]: any; }): string {
        if (!params)
            return path;

        const parts = [];
        for (const key in params) try {
            parts.push(`${encodeURIComponent(key)}=${encodeURIComponent(params[key])}`)
        }

        catch (ex) {
            console.error(ex);
        }

        return (parts.length > 0) ? `${path}?${parts.join('&')}`: path;
    }

    const current = function (): string {
        return window.location.pathname;
    }

    const query = function <T>(): T {
        const json: { [key: string]: any; } = {};

        for (const parts of window.location.search.substring(1).split("&")) {
            const [key, ...value] = parts.split('=');
            json[decodeURIComponent(key)] = value.map(a => {
                if (a)
                    return decodeURIComponent(a);

                return '='
            }).join('');
        }

        return json as T;
    }

    const navigate = (path: IRoutePath, params?: { [key: string]: any; }) => {
        nav(createPath(path, (params || {})));
    }

    const goBack = (): void => {
        if (window.history.length > 0)
            window.history.back();
        else
            nav('/');

        return;
    }

    return (
        <RouteContext.Provider value={{ current, query, navigate, goBack }}>
            {children}
        </RouteContext.Provider>
    )
}