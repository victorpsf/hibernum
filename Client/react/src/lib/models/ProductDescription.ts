import {Product} from "./Product";

export interface ProductDescription {
    id: number;
    value: string;
    product?: Product;
}