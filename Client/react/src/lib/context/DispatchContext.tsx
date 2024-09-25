import React from "react";
import { HibernumClient, IHibernumClient } from "../db/client/HibernumClient";
import { AuthClient, IAuthClient } from "../db/client/AuthClient";

export interface IDispatchContext {
    auth: IAuthClient;
    hibernum: IHibernumClient;
}

export interface IDispatchProviderProps {
    children: JSX.Element;
}

export const defaultValues: IDispatchContext =  {
    auth: AuthClient(),
    hibernum: HibernumClient(),
}

export const DispatchContext = React.createContext<IDispatchContext>(defaultValues);

export const DispatchProvider = function ({ children }: IDispatchProviderProps): JSX.Element {
    return (
        <DispatchContext.Provider value={defaultValues}>
            {children}
        </DispatchContext.Provider>
    )
}