import axios, { AxiosError, AxiosInstance, AxiosRequestConfig, AxiosResponse } from "axios";
import AppStorage from "../AppStorage";

export type IConnectorHttpMethods = keyof AxiosInstance;

export interface IJSON {
    [key: string]: IJSON | any | null;
}

export interface IBaseConnectorCalled<T, B> {
    href: string;
    path: string;
    method: keyof AxiosInstance;
    params?: T;
    body?: B | null;
    headers?: IJSON;
}

export interface IBaseConnectorResultData<R> {
    code: number;
    message: string;
    data: R | null;
    failed: boolean;
}

export interface IBaseConnectorResult<T, B, R> {
    called: IBaseConnectorCalled<T, B>;
    result: IBaseConnectorResultData<R>;
    redirect?: string;
}

export interface IBaseConnectorGetParams<T> {
    url: string;
    params?: T;
    auth: boolean;
}

export interface IBaseConnectorPostParams<T, C> {
    url: string;
    params?: T;
    body?: C;
    auth: boolean;
}

export interface IBaseConnectorProperties {
    get: <P, R>(param: IBaseConnectorGetParams<P>) => Promise<IBaseConnectorResult<P, any, R>>;
    post: <B, R>(param: IBaseConnectorPostParams<any, B>) => Promise<IBaseConnectorResult<any, B, R>>;
    postWithParams: <P, B, R>(param: IBaseConnectorPostParams<P, B>) => Promise<IBaseConnectorResult<P, B, R>>;
    put?: <B, R>(param: IBaseConnectorPostParams<any, B>) => Promise<IBaseConnectorResult<any, B, R>>;
    putWithParams?: <P, B, R>(param: IBaseConnectorPostParams<P, B>) => Promise<IBaseConnectorResult<P, B, R>>;
    delete?: <P, R>(param: IBaseConnectorGetParams<P>) => Promise<IBaseConnectorResult<P, any, R>>;
}

export interface IBaseConnectorConstructorParams {
    href: string;
    params?: IJSON;
}

export type IAxiosSimpleCaller = <T = any, R = AxiosResponse<T>>(path: string, config: AxiosRequestConfig) => Promise<R>;
export type IAxiosCaller = <T = any, R = AxiosResponse<T>>(path: string, body: any, config: AxiosRequestConfig) => Promise<R>;

export interface ISendParams<P, B> {
    path: string;
    method: keyof AxiosInstance;
    params?: P;
    body?: B;
    headers?: IJSON;
    auth?: boolean;
}

export interface IBaseConnector {
    send: <P, B, R>(param: ISendParams<P, B>) => Promise<IBaseConnectorResult<P, B, R>>;
}

export const Connector = function ({ href, params }: IBaseConnectorConstructorParams): IBaseConnector {
    const client = axios.create({
        baseURL: href
    });

    const getToken = (): string | null => AppStorage.get<string>('TOKEN');
    const getTokenType = (): string | null => AppStorage.get<string>('TOKEN_TYPE');

    const defaultHeaders = (auth: boolean, headers?: IJSON): any => {
        const _headers_: IJSON = headers ?? {};
        const token = getToken();
        const type = getTokenType();

        if (auth && token)
            _headers_['Authorization'] = `${type} ${token}`;

        return _headers_;
    }

    const defaultParams = <T>(param?: T): any => {
        const _param_: IJSON = param ?? {};

        if (params) for(const key in params)
            _param_[key] = params[key];

        return _param_;
    }

    const SimpleCaller = (method: IConnectorHttpMethods) => client[method] as IAxiosSimpleCaller;
    const CompleteCaller = (method: IConnectorHttpMethods) => client[method] as IAxiosCaller;

    const send = async function <P, B, R>({ path, method, params, body, headers, auth = false }: ISendParams<P, B>): Promise<IBaseConnectorResult<P, B, R>> {
        let promise: Promise<AxiosResponse<R>> | null = null;
        const _params_ = defaultParams(params);
        const _headers_ = defaultHeaders(auth, headers);

        const called = {
            href,
            path,
            params: _params_,
            method,
            body: body ?? null,
            headers: _headers_
        };
        
        if (['put', 'post'].includes(method))
            promise = CompleteCaller(method)<R>(
                path,
                body ?? null,
                { params: _params_, headers: _headers_ }
            );
        else if (['delete', 'get'])
            promise = SimpleCaller(method)<R>(
                path, { params: _params_, headers: _headers_ }
            );
        
        if (promise)
            try {
                const { status, statusText, data } = await promise;

                return {
                    called,
                    result: {
                        code: status,
                        message: statusText,
                        data: data,
                        failed: !(status >= 200 && status < 300)
                    }
                };
            }

            catch (ex: AxiosError | Error | any) {
                if ('isAxiosError' in ex && ex.isAxiosError) {
                    const code = (ex as AxiosError).response?.status || (parseInt((ex as AxiosError).code ?? "") ?? 500);
                    return {
                        called,
                        result: {
                            code: code,
                            message: (ex as AxiosError).response?.statusText ?? "INTERNAL SERVER ERROR",
                            data: (ex as AxiosError).response?.data as R,
                            failed: !(code >= 200 && code < 300)
                        }
                    };
                }

                return {
                    called,
                    result: {
                        code: 500,
                        message: "INTERNAL SERVER ERROR",
                        data: null,
                        failed: true
                    }
                };
            }

        else return {
            called,
            result: {
                code: 500,
                message: 'INTERNAL SERVER ERROR',
                data: null,
                failed: true
            }
        }
    }

    return { send };
}