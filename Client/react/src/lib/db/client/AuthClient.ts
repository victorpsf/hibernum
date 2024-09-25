import { Authenticated } from "../../models/Authenticated";
import { Company } from "../../models/Company";
import { Connector, IBaseConnectorResult } from "../Connector"
import { IAuthenticationCompanyParams, ISimpleAuthenticationBody } from "./params/IAuthClient";

export interface IAuthClient {
    Authentication: (body: ISimpleAuthenticationBody) => Promise<IBaseConnectorResult<any, ISimpleAuthenticationBody, Authenticated>>;
    AuthenticationCompanies: () => Promise<IBaseConnectorResult<any, any, Company[]>>;
    AuthenticationCompany: (params: IAuthenticationCompanyParams) => Promise<IBaseConnectorResult<IAuthenticationCompanyParams, any, Authenticated>>;
}

export const AuthClient = function (): IAuthClient {
    const client = Connector({ href: process.env.REACT_APP_AUTH_HREF || '' });

    const Authentication = (body: ISimpleAuthenticationBody): Promise<IBaseConnectorResult<any, ISimpleAuthenticationBody, Authenticated>> => client.send<any, ISimpleAuthenticationBody, Authenticated>({
        path: '/Authentication',
        method: 'post',
        body,
        auth: false
    });

    const AuthenticationCompanies = (): Promise<IBaseConnectorResult<any, any, Company[]>> => client.send<any, any, Company[]>({
        path: '/Authentication/Company',
        method: 'get',
        auth: true
    });

    const AuthenticationCompany = (params: IAuthenticationCompanyParams): Promise<IBaseConnectorResult<IAuthenticationCompanyParams, any, Authenticated>> => client.send<IAuthenticationCompanyParams, any, Authenticated>({
        path: '/Authentication/Company',
        params: params,
        method: 'post',
        auth: true
    });

    return {
        Authentication,
        AuthenticationCompanies,
        AuthenticationCompany
    }
}