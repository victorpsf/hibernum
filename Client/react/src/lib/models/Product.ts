import {ProductType} from "./ProductType";
import {ProductDescription} from "./ProductDescription";
import {ProductGroup} from "./ProductGroup";
import {FileReaderDto} from "./FileReaderDto";
import {FileEntity} from "./File";

export interface Product {
    id?: number;
    name: string;
    size: string;
    file?: FileEntity;
    fileDto?: FileReaderDto;
    group?: ProductGroup;
    types?: ProductType[];
    descriptions?: ProductDescription[];
}