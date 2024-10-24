import {Connector, IBaseConnectorResult} from "../Connector"
import {GetProductsParams} from "./params/GetProductsParams";
import {Product} from "../../models/Product";
import {GetProductDecriptionParams} from "./params/GetProductDecriptionParams";
import {ProductDescription} from "../../models/ProductDescription";
import {GetProductTypeParams} from "./params/GetProductTypeParams";
import {ProductType} from "../../models/ProductType";
import {GetProductGroupParams} from "./params/GetProductGroupParams";
import {ProductGroup} from "../../models/ProductGroup";
import {FileReaderDto} from "../../models/FileReaderDto";
import {FileEntity} from "../../models/File";
import {SaveProductParams} from "./params/SaveProductParams";

export interface IHibernumClient {
    GetProducts: (params: GetProductsParams) => Promise<IBaseConnectorResult<GetProductsParams, any, Product[]>>;
    GetProductDescription: (params: GetProductDecriptionParams) => Promise<IBaseConnectorResult<GetProductDecriptionParams, any, ProductDescription[]>>;
    GetProductType: (params: GetProductTypeParams) => Promise<IBaseConnectorResult<GetProductTypeParams, any, ProductType[]>>;
    GetProductGroup: (params: GetProductGroupParams) => Promise<IBaseConnectorResult<GetProductGroupParams, any, ProductGroup[]>>;
    SaveFile: (body: FileReaderDto) => Promise<IBaseConnectorResult<any, FileReaderDto, FileEntity>>;
    RemoveFile: (params: { id: number }) => Promise<IBaseConnectorResult<{ id: number }, any, FileEntity>>;
    SaveProduct: (body: SaveProductParams) => Promise<IBaseConnectorResult<any, SaveProductParams, Product>>;
}

export const HibernumClient = function (): IHibernumClient {
    const client = Connector({ href: process.env.REACT_APP_HIBERNUM_HREF || '' });

    const GetProducts = (params: GetProductsParams): Promise<IBaseConnectorResult<GetProductsParams, any, Product[]>> => client.send<GetProductsParams, any, Product[]>({
        path: '/Product',
        method: 'get',
        params,
        auth: true
    });

    const GetProductDescription = (params: GetProductDecriptionParams): Promise<IBaseConnectorResult<GetProductDecriptionParams, any, ProductDescription[]>> => client.send<GetProductDecriptionParams, any, ProductDescription[]>({
        path: '/ProductDescription',
        method: 'get',
        params,
        auth: true
    });

    const GetProductType = (params: GetProductTypeParams): Promise<IBaseConnectorResult<GetProductTypeParams, any, ProductType[]>> => client.send<GetProductTypeParams, any, ProductType[]>({
        path: '/ProductType',
        method: 'get',
        params,
        auth: true
    });

    const GetProductGroup = (params: GetProductGroupParams): Promise<IBaseConnectorResult<GetProductGroupParams, any, ProductGroup[]>> => client.send<GetProductGroupParams, any, ProductGroup[]>({
        path: '/ProductGroup',
        method: 'get',
        params,
        auth: true
    });

    const SaveFile = (body: FileReaderDto): Promise<IBaseConnectorResult<any, FileReaderDto, FileEntity>> => client.send<any, FileReaderDto, FileEntity>({
        path: '/File',
        method: 'post',
        body,
        auth: true
    });

    const RemoveFile = (params: { id: number }): Promise<IBaseConnectorResult<{ id: number }, any, FileEntity>> => client.send<{ id: number }, any, FileEntity>({
        path: '/File',
        method: 'delete',
        params,
        auth: true
    });

    const SaveProduct = (body: SaveProductParams): Promise<IBaseConnectorResult<any, SaveProductParams, Product>> => client.send<any, SaveProductParams, Product>({
        path: '/Product',
        method: 'post',
        body,
        auth: true
    });

    return {
        GetProducts,
        GetProductDescription,
        GetProductType,
        GetProductGroup,
        SaveFile,
        RemoveFile,
        SaveProduct
    };
}